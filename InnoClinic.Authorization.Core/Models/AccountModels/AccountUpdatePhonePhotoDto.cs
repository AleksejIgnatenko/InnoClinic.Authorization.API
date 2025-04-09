namespace InnoClinic.Authorization.Core.Models.AccountModels
{
    /// <summary>
    /// Represents a data transfer object for updating an account's phone number and profile photo.
    /// </summary>
    public record AccountUpdatePhonePhotoDto(
        /// <summary>
        /// The unique identifier of the account.
        /// </summary>
        Guid Id,

        /// <summary>
        /// The phone number associated with the account. This property is optional.
        /// </summary>
        string? PhoneNumber,

        /// <summary>
        /// The identifier of the new profile photo. This property is optional.
        /// </summary>
        string? PhotoId
    );
}