using System.Security.Claims;
using AutoMapper;
using InnoClinic.Authorization.Core.Models;
using InnoClinic.Authorization.DataAccess.Repositories;
using InnoClinic.Authorization.Infrastructure.Jwt;
using Microsoft.Extensions.Options;

namespace InnoClinic.Authorization.Application.Services
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IJwtTokenService _jwtTokenService;
        private readonly JwtOptions _jwtOptions;

        public AccountService(IAccountRepository accountRepository, IJwtTokenService jwtTokenService, IOptions<JwtOptions> jwtOptions)
        {
            _accountRepository = accountRepository;
            _jwtTokenService = jwtTokenService;
            _jwtOptions = jwtOptions.Value;
        }

        public async Task<(string accessToken, string refreshToken)> CreateAccountAsync(AccountModel account)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, account.Id.ToString()),
                new Claim(ClaimTypes.Name, account.Email),
            };

            var accessToken = _jwtTokenService.GenerateAccessToken(claims);
            
            account.RefreshToken = _jwtTokenService.GenerateRefreshToken();
            account.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(_jwtOptions.RefreshTokenExpirationDays);

            var expiration = DateTime.UtcNow.AddMinutes(_jwtOptions.AccessTokenExpirationMinutes);

            await _accountRepository.CreateAsync(account);

            return (accessToken, account.RefreshToken);
        }

        public async Task<(string accessToken, string refreshToken)> RefreshTokenAsync(string accessToken, string refreshToken)
        {
            var accountId = _jwtTokenService.GetAccountIdFromAccessToken(accessToken);

            var account = await _accountRepository.GetByIdAsync(accountId) ?? throw new Exception();

            if((account.RefreshTokenExpiryTime <= DateTime.UtcNow) || 
                (!account.RefreshToken.Equals(refreshToken))) { throw new Exception(); }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, account.Id.ToString()),
                new Claim(ClaimTypes.Name, account.Email),
            };

            var newAccessToken = _jwtTokenService.GenerateAccessToken(claims);
            account.RefreshToken = _jwtTokenService.GenerateRefreshToken();

            await _accountRepository.UpdateAsync(account);

            return(newAccessToken, account.RefreshToken);
        }
    }
}
