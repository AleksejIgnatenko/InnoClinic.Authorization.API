using InnoClinic.Authorization.Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace InnoClinic.Authorization.Application.Services
{
    public interface IAccountService
    {
        Task<(string accessToken, string refreshToken, string message)> CreateAccountAsync(string email, string password, string phonNumber, IUrlHelper urlHelper);
        Task<(string accessToken, string refreshToken)> RefreshTokenAsync(string accessToken, string refreshToken);
        Task<bool> ConfirmEmailAsync(Guid accountId, string token);
    }
}