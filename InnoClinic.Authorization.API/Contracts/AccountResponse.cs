namespace InnoClinic.Authorization.API.Contracts
{
    public record AccountResponse(
        Guid Id,
        string Email,
        string Password,
        string PhoneNumber,
        string Role,
        bool IsEmailVerified,
        Guid PhotoId,
        string CreateBy,
        DateTime CreateAt,
        string UpdateBy,
        DateTime UpdateAt
    );
}
