namespace InnoClinic.Authorization.Core.Models.AccountModels
{
    public record SignInModelRequest(
        string Email,
        string Password);
}
