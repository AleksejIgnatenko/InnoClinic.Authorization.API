namespace InnoClinic.Authorization.Core.Models.NotificationModels
{
    public record SendVerificationEmailRequest(
        Guid AccountId,
        string Email,
        string CallbackUrl);
}
