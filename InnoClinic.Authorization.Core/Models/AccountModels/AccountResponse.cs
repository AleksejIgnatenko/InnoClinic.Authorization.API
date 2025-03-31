namespace InnoClinic.Authorization.Core.Models.AccountModels
{
    public record AccountResponse(
        Guid Id,
        string Email,
        string Password,
        string PhoneNumber,
        string Role,
        bool IsEmailVerified,
        string PhotoId,
        string CreateBy,
        DateTime CreateAt,
        string UpdateBy,
        DateTime UpdateAt
    );
}
