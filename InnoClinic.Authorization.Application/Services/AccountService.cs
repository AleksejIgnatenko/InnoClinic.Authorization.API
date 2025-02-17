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
using Newtonsoft.Json.Linq;

namespace InnoClinic.Authorization.Application.Services
{
    /// <summary>
    /// Provides account management services, including account creation, login, and email verification.
    /// </summary>
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IJwtTokenService _jwtTokenService;
        private readonly JwtOptions _jwtOptions;
        private readonly IValidationService _validationService;
        private readonly IEmailService _emailVerificationService;
        private readonly IMapper _mapper;
        private readonly IRabbitMQService _rabbitmqService;

        /// <summary>
        /// Initializes a new instance of the <see cref="AccountService"/> class.
        /// </summary>
        /// <param name="accountRepository">The repository containing account data operations.</param>
        /// <param name="jwtTokenService">The service for generating JWT tokens.</param>
        /// <param name="jwtOptions">The JWT options configuration.</param>
        /// <param name="validationService">The service for validating account models.</param>
        /// <param name="emailVerificationService">The service for sending email verification.</param>
        /// <param name="mapper">The mapper for object mapping.</param>
        /// <param name="rabbitmqService">The service for RabbitMQ messaging.</param>
        public AccountService(
            IAccountRepository accountRepository,
            IJwtTokenService jwtTokenService,
            IOptions<JwtOptions> jwtOptions,
            IValidationService validationService,
            IEmailService emailVerificationService,
            IMapper mapper,
            IRabbitMQService rabbitmqService)
        {
            _accountRepository = accountRepository;
            _jwtTokenService = jwtTokenService;
            _jwtOptions = jwtOptions.Value;
            _validationService = validationService;
            _emailVerificationService = emailVerificationService;
            _mapper = mapper;
            _rabbitmqService = rabbitmqService;
        }

        /// <summary>
        /// Asynchronously creates a new account and sends a verification email.
        /// </summary>
        /// <param name="email">The email address of the new account.</param>
        /// <param name="password">The password for the new account.</param>
        /// <param name="urlHelper">The URL helper for generating confirmation links.</param>
        /// <returns>A tuple containing the access token, refresh token, and a message.</returns>
        public async Task<(string accessToken, string refreshToken, string message)> CreateAccountAsync(string email, string password, IUrlHelper urlHelper)
        {
            var account = new AccountModel
            {
                Id = Guid.NewGuid(),
                Email = email,
                Password = password,
                Role = Core.Enums.RoleEnum.Patient,
                CreateBy = DateTime.UtcNow,
                CreateAt = DateTime.UtcNow,
            };

            // Validate account
            var validationErrors = _validationService.Validation(account);
            if (validationErrors.Count != 0)
            {
                throw new ValidationException(validationErrors);
            }

            // Creating and sending email for email verification
            string message = "Для подтверждения почты проверьте электронную почту и перейдите по ссылке, указанной в письме.";
            await _emailVerificationService.SendVerificationEmailAsync(account, urlHelper);

            // Create tokens (access and refresh token)
            var claims = GetClaimsForAccount(account);
            var accessToken = _jwtTokenService.GenerateAccessToken(claims);
            account.RefreshToken = _jwtTokenService.GenerateRefreshToken();
            account.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(_jwtOptions.RefreshTokenExpirationDays);

            // Create account
            await _accountRepository.CreateAsync(account);

            // Send to RabbitMQ
            var accountDto = _mapper.Map<AccountDto>(account);
            await _rabbitmqService.PublishMessageAsync(accountDto, RabbitMQQueues.ADD_ACCOUNT_QUEUE);

            return (accessToken, account.RefreshToken, message);
        }

        /// <summary>
        /// Asynchronously logs in a user and generates access and refresh tokens.
        /// </summary>
        /// <param name="email">The user's email address.</param>
        /// <returns>A tuple containing the hashed password, access token, and refresh token.</returns>
        public async Task<(string hashPassword, string accessToken, string refreshToken)> LoginAsync(string email)
        {
            var account = await _accountRepository.GetByEmail(email);

            // Create tokens (access and refresh token)
            var claims = GetClaimsForAccount(account);
            var accessToken = _jwtTokenService.GenerateAccessToken(claims);
            account.RefreshToken = _jwtTokenService.GenerateRefreshToken();
            account.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(_jwtOptions.RefreshTokenExpirationDays);

            await _accountRepository.UpdateAsync(account);

            return (account.Password, accessToken, account.RefreshToken);
        }

        /// <summary>
        /// Asynchronously refreshes the access token using the provided refresh token.
        /// </summary>
        /// <param name="refreshToken">The refresh token to validate and use for generating a new access token.</param>
        /// <returns>A tuple containing the new access token and refresh token.</returns>
        public async Task<(string accessToken, string refreshToken)> RefreshTokenAsync(string refreshToken)
        {
            // Get account
            var account = await _accountRepository.GetByRefreshTokenAsync(refreshToken);

            // Validate refresh token
            if ((account.RefreshTokenExpiryTime <= DateTime.UtcNow) ||
                (!account.RefreshToken.Equals(refreshToken)))
            {
                throw new Exception("Invalid refresh token.");
            }

            // Create new access and refresh token
            var claims = GetClaimsForAccount(account);
            var newAccessToken = _jwtTokenService.GenerateAccessToken(claims);
            account.RefreshToken = _jwtTokenService.GenerateRefreshToken();

            await _accountRepository.UpdateAsync(account);

            return (newAccessToken, account.RefreshToken);
        }

        /// <summary>
        /// Asynchronously confirms the user's email address using the provided token.
        /// </summary>
        /// <param name="accountId">The ID of the account to confirm.</param>
        /// <param name="token">The token used to confirm the email.</param>
        /// <returns>A boolean indicating whether the email was successfully confirmed.</returns>
        public async Task<bool> ConfirmEmailAsync(Guid accountId, string token)
        {
            // Get account
            var account = await _accountRepository.GetByIdAsync(accountId);

            // Confirm email
            var result = _emailVerificationService.ConfirmEmail(token);
            if (string.IsNullOrEmpty(result))
            {
                return false;
            }

            account.IsEmailVerified = true;
            await _accountRepository.UpdateAsync(account);
            return true;
        }

        /// <summary>
        /// Asynchronously checks if the specified email already exists.
        /// </summary>
        /// <param name="email">The email address to check.</param>
        /// <returns>A boolean indicating whether the email exists.</returns>
        public async Task<bool> EmailExistsAsync(string email)
        {
            return await _accountRepository.EmailExistsAsync(email);
        }

        /// <summary>
        /// Retrieves the claims for the specified account.
        /// </summary>
        /// <param name="account">The account model.</param>
        /// <returns>A list of claims associated with the account.</returns>
        private List<Claim> GetClaimsForAccount(AccountModel account)
        {
            return new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, account.Id.ToString()),
                new Claim(ClaimTypes.Role, account.Role.ToString()),
            };
        }

        /// <summary>
        /// Asynchronously retrieves all accounts.
        /// </summary>
        /// <returns>A collection of account models.</returns>
        public async Task<IEnumerable<AccountModel>> GetAllAccountsAsync()
        {
            return await _accountRepository.GetAllAsync();
        }

        /// <summary>
        /// Gets an account by its ID asynchronously.
        /// </summary>
        /// <param name="token">The access token containing the account ID.</param>
        /// <returns>The account model corresponding to the provided ID.</returns>
        public async Task<AccountModel> GetAccountByIdAsync(string token)
        {
            var accountId = _jwtTokenService.GetAccountIdFromAccessToken(token);
            return await _accountRepository.GetByIdAsync(accountId);
        }

        /// <summary>
        /// Retrieves a list of accounts by their unique identifiers asynchronously.
        /// </summary>
        /// <param name="accountIds">A list of unique identifiers for the accounts.</param>
        /// <returns>An <see cref="IEnumerable{AccountModel}"/> containing the accounts associated with the specified identifiers.</returns>
        public async Task<IEnumerable<AccountModel>> GetAccountsByIdsAsync(List<Guid> accountIds)
        {
            return await _accountRepository.GetByIdAsync(accountIds);
        }
    }
}
