namespace InnoClinic.Authorization.API.Contracts
{
    public record AccountRequest(
        string Email,
        string Password,
        string PhoneNumber
        );
}
