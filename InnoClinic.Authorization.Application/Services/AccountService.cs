using System.Security.Claims;
using AutoMapper;
using InnoClinic.Authorization.Core.Dto;
using InnoClinic.Authorization.Core.Exceptions;
using InnoClinic.Authorization.Core.Models;
using InnoClinic.Authorization.DataAccess.Repositories;
using InnoClinic.Authorization.Infrastructure.Jwt;
using InnoClinic.Authorization.Infrastructure.RabbitMQ;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace InnoClinic.Authorization.Application.Services
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IJwtTokenService _jwtTokenService;
        private readonly JwtOptions _jwtOptions;
        private readonly IValidationService _validationService;
        private readonly IEmailVerificationService _emailVerificationService;
        private readonly IMapper _mapper;
        private readonly IRabbitMQService _rabbitmqService;


        public AccountService(IAccountRepository accountRepository, IJwtTokenService jwtTokenService, IOptions<JwtOptions> jwtOptions, IValidationService validationService, IEmailVerificationService emailVerificationService, IMapper mapper, IRabbitMQService rabbitmqService)
        {
            _accountRepository = accountRepository;
            _jwtTokenService = jwtTokenService;
            _jwtOptions = jwtOptions.Value;
            _validationService = validationService;
            _emailVerificationService = emailVerificationService;
            _mapper = mapper;
            _rabbitmqService = rabbitmqService;
        }

        public async Task<(string accessToken, string refreshToken, string message)> CreateAccountAsync(string email, string password, IUrlHelper urlHelper)
        {
            var account = new AccountModel 
            { 
                Id = Guid.NewGuid(),
                Email = email, 
                Password = password, 
                CreateBy = DateTime.UtcNow,
                CreateAt = DateTime.UtcNow 
            };

            //validation account
            var validationErrors = _validationService.Validation(account);
            if(validationErrors.Count != 0)
            {
                throw new ValidationException(validationErrors);
            }

            //creating and sending emails for email verification
            string message = $"Для подтверждения почты проверьте электронную почту и перейдите по ссылке, указанной в письме. ";
            await _emailVerificationService.SendVerificationEmailAsync(account, urlHelper);

            //create tokens(access and refresh token)
            var claims = GetClaimsForAccount(account);
            var accessToken = _jwtTokenService.GenerateAccessToken(claims);
            account.RefreshToken = _jwtTokenService.GenerateRefreshToken();
            account.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(_jwtOptions.RefreshTokenExpirationDays);
            var expiration = DateTime.UtcNow.AddMinutes(_jwtOptions.AccessTokenExpirationMinutes);

            //create account
            await _accountRepository.CreateAsync(account);

            //send to rabbitMQ
            var accountDto = _mapper.Map<AccountDto>(account);
            await _rabbitmqService.PublishMessageAsync(accountDto, RabbitMQQueues.ADD_ACCOUNT_QUEUE);

            return (accessToken, account.RefreshToken, message);
        }

        public async Task<(string hashPassword, string accessToken, string refreshToken)> LoginAsync(string email)
        {
            var account = await _accountRepository.GetByEmail(email);

            //create tokens(access and refresh token)
            var claims = GetClaimsForAccount(account);
            var accessToken = _jwtTokenService.GenerateAccessToken(claims);
            account.RefreshToken = _jwtTokenService.GenerateRefreshToken();
            account.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(_jwtOptions.RefreshTokenExpirationDays);
            var expiration = DateTime.UtcNow.AddMinutes(_jwtOptions.AccessTokenExpirationMinutes);

            await _accountRepository.UpdateAsync(account);

            return (account.Password, accessToken, account.RefreshToken);
        }

        public async Task<(string accessToken, string refreshToken)> RefreshTokenAsync(string accessToken, string refreshToken)
        {
            //get accountId from access token
            var accountId = _jwtTokenService.GetAccountIdFromAccessToken(accessToken);

            //get account
            var account = await _accountRepository.GetByIdAsync(accountId) ?? throw new Exception();

            //validation refresh token
            if ((account.RefreshTokenExpiryTime <= DateTime.UtcNow) || 
                (!account.RefreshToken.Equals(refreshToken))) { throw new Exception(); }

            //create new access and refresh token
            var claims = GetClaimsForAccount(account);
            var newAccessToken = _jwtTokenService.GenerateAccessToken(claims);
            account.RefreshToken = _jwtTokenService.GenerateRefreshToken();

            await _accountRepository.UpdateAsync(account);

            return(newAccessToken, account.RefreshToken);
        }

        public async Task<bool> ConfirmEmailAsync(Guid accountId, string token)
        {
            //get account
            var account = await _accountRepository.GetByIdAsync(accountId);

            //confirm email
            var result = _emailVerificationService.ConfirmEmail(token);
            if(string.IsNullOrEmpty(result))
            {
                return false;
            }

            account.IsEmailVerified = true;
            await _accountRepository.UpdateAsync(account);
            return true;
        }

        public async Task<bool> EmailExistsAsync(string email)
        {
            return await _accountRepository.EmailExistsAsync(email);
        }

        private List<Claim> GetClaimsForAccount(AccountModel account)
        {
            return new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, account.Id.ToString()),
                new Claim(ClaimTypes.Name, account.Email),
            };
        }

        public async Task<IEnumerable<AccountModel>> GetAllAccountsAsync()
        {
            return await _accountRepository.GetAllAsync();
        }
    }
}
