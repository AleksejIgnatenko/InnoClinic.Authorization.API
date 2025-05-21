using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using InnoClinic.Authorization.Core.Models.NotificationModels;
using System.Text;
using System.Text.Json;
using InnoClinic.Authorization.Core.Abstractions;

namespace InnoClinic.Authorization.Application.Services;

/// <summary>
/// Provides email services, including sending verification emails and confirming email addresses.
/// </summary>
public class EmailService : IEmailService
{
    private readonly IDataProtector _dataProtector;
    private readonly HttpClient _httpClient;

    /// <summary>
    /// Initializes a new instance of the <see cref="EmailService"/> class.
    /// </summary>
    /// <param name="dataProtectionProvider">The data protection provider for token generation.</param>
    /// <param name="httpContextAccessor">The HTTP context accessor for generating URLs.</param>
    public EmailService(IDataProtectionProvider dataProtectionProvider)
    {
        _dataProtector = dataProtectionProvider.CreateProtector("EmailConfirmation");
        _httpClient = new HttpClient();
    }

    /// <summary>
    /// Asynchronously sends a verification email to the specified account.
    /// </summary>
    /// <param name="account">The account model containing user information.</param>
    /// <param name="urlHelper">The URL helper to generate confirmation links.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async Task SendVerificationEmailAsync(Guid accountId, string email, IUrlHelper urlHelper)
    {
        var token = GenerateEmailConfirmationToken(email);

        var callbackUrl = $"http://localhost:5001/api/Account/confirm-email?accountId={accountId}&token={token}";
        
        var sendVerificationEmailRequest = new SendVerificationEmailRequest(accountId, email, callbackUrl);

        var json = JsonSerializer.Serialize(sendVerificationEmailRequest);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await _httpClient.PostAsync("http://innoclinic_notification_api:8080/api/Notification/send-verification-email", content);
    }

    /// <summary>
    /// Confirms the email using the provided token.
    /// </summary>
    /// <param name="token">The token used to confirm the email.</param>
    /// <returns>A string indicating the result of the email confirmation.</returns>
    public string ConfirmEmail(string token)
    {
        return _dataProtector.Unprotect(token);
    }

    /// <summary>
    /// Generates an email confirmation token for the specified account.
    /// </summary>
    /// <param name="account">The account model for which to generate the token.</param>
    /// <returns>A protected token as a string.</returns>
    private string GenerateEmailConfirmationToken(string email)
    {
        var protectedToken = _dataProtector.Protect(email);
        return protectedToken;
    }
}
