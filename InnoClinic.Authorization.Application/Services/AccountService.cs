using System.Security.Claims;
using InnoClinic.Authorization.Core.Exceptions;
using InnoClinic.Authorization.Core.Models;
using InnoClinic.Authorization.DataAccess.Repositories;
using InnoClinic.Authorization.Infrastructure.Jwt;
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

        public AccountService(IAccountRepository accountRepository, IJwtTokenService jwtTokenService, IOptions<JwtOptions> jwtOptions, IValidationService validationService, IEmailVerificationService emailVerificationService)
        {
            _accountRepository = accountRepository;
            _jwtTokenService = jwtTokenService;
            _jwtOptions = jwtOptions.Value;
            _validationService = validationService;
            _emailVerificationService = emailVerificationService;
        }

        public async Task<(string accessToken, string refreshToken, string message)> CreateAccountAsync(string email, string password, string phonNumber, IUrlHelper urlHelper)
        {
            var account = new AccountModel { Email = email, Password = password, PhoneNumber = phonNumber, CreateAt = DateTime.UtcNow };

            //validation account
            var validationErrors = _validationService.AccountValidation(account);
            if(validationErrors.Count != 0)
            {
                throw new ValidationException(validationErrors);
            }

            //creating and sending emails for email verification
            string message = $"Для подтверждения почты проверьте электронную почту и перейдите по ссылке, указанной в письме. ";
            var claims = GetClaimsForAccount(account);
            await _emailVerificationService.SendVerificationEmailAsync(account, urlHelper);

            //create tokens(access and refresh token)
            var accessToken = _jwtTokenService.GenerateAccessToken(claims);
            account.RefreshToken = _jwtTokenService.GenerateRefreshToken();
            account.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(_jwtOptions.RefreshTokenExpirationDays);
            var expiration = DateTime.UtcNow.AddMinutes(_jwtOptions.AccessTokenExpirationMinutes);

            //create account
            await _accountRepository.CreateAsync(account);

            return (accessToken, account.RefreshToken, message);
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

        public async Task<bool> CheckEmailAvailabilityAsync(string email)
        {
            return await _accountRepository.CheckEmailAvailabilityAsync(email);
        }

        private List<Claim> GetClaimsForAccount(AccountModel account)
        {
            return new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, account.Id.ToString()),
                new Claim(ClaimTypes.Name, account.Email),
            };
        }
    }
}
