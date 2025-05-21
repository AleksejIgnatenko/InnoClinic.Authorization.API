using Microsoft.AspNetCore.Mvc;

namespace InnoClinic.Authorization.Core.Abstractions;

/// <summary>
/// Defines the operations for sending emails.
/// </summary>
public interface IEmailService
{
    /// <summary>
    /// Asynchronously sends a verification email to the specified account.
    /// </summary>
    /// <param name="accountId">The unique identifier of the account.</param>
    /// <param name="urlHelper">The URL helper to generate confirmation links.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task SendVerificationEmailAsync(Guid accountId, string email, IUrlHelper urlHelper);

    /// <summary>
    /// Sends a notification about an appointment.
    /// </summary>
    /// <param name="accountId">The unique identifier of the account.</param>
    /// <param name="patientFullName">The full name of the patient.</param>
    /// <param name="date">The date of the appointment.</param>
    /// <param name="time">The time of the appointment.</param>
    /// <param name="medicalServiceName">The name of the medical service.</param>
    /// <param name="doctorFullName">The full name of the doctor.</param>
    string ConfirmEmail(string token);
}