using InnoClinic.Authorization.Core.Models.AccountModels;
using Microsoft.AspNetCore.Mvc;

namespace InnoClinic.Authorization.Application.Services
{
    /// <summary>
    /// Defines the operations for sending emails.
    /// </summary>
    public interface IEmailService
    {
        /// <summary>
        /// Asynchronously sends a verification email to the specified account.
        /// </summary>
        /// <param name="account">The account model containing user information.</param>
        /// <param name="urlHelper">The URL helper to generate confirmation links.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task SendVerificationEmailAsync(AccountEntity account, IUrlHelper urlHelper);

        /// <summary>
        /// Confirms the email using the provided token.
        /// </summary>
        /// <param name="token">The token used to confirm the email.</param>
        /// <returns>A string indicating the result of the email confirmation.</returns>
        string ConfirmEmail(string token);
    }
}