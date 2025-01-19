using InnoClinic.Authorization.Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace InnoClinic.Authorization.Application.Services
{
    public interface IEmailVerificationService
    {
        Task SendVerificationEmailAsync(AccountModel account, IUrlHelper urlHelper);
        string ConfirmEmail(string token);
    }
}