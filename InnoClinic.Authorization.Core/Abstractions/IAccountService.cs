using InnoClinic.Authorization.Core.Models;

namespace InnoClinic.Authorization.Application.Services
{
    public interface IAccountService
    {
        Task<(string accessToken, string refreshToken)> CreateAccountAsync(AccountModel account);
        Task<(string accessToken, string refreshToken)> RefreshTokenAsync(string accessToken, string refreshToken);
    }
}