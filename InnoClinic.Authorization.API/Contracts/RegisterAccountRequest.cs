namespace InnoClinic.Authorization.API.Contracts
{
    public record RegisterAccountRequest(
        string Email,
        string Password
        );
}
