namespace InnoClinic.Authorization.Core.Models.AccountModels
{
    public record RegisterAccountRequest(
        string Email,
        string Password
        );
}
