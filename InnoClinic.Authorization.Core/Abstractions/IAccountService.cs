using InnoClinic.Authorization.Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace InnoClinic.Authorization.Application.Services
{
    public interface IAccountService
    {
        Task<(string accessToken, string refreshToken, string message)> CreateAccountAsync(string email, string password, IUrlHelper urlHelper);
        Task<(string hashPassword, string accessToken, string refreshToken)> LoginAsync(string email);
        Task<(string accessToken, string refreshToken)> RefreshTokenAsync(string accessToken, string refreshToken);
        Task<bool> ConfirmEmailAsync(Guid accountId, string token);
        Task<bool> EmailExistsAsync(string email);
        Task<IEnumerable<AccountModel>> GetAllAccountsAsync();
    }
}