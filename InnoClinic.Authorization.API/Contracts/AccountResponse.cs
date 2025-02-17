using InnoClinic.Authorization.Core.Enums;

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
        DateTime CreateBy,
        DateTime CreateAt,
        DateTime UpdateBy,
        DateTime UpdateAt
    );
}
