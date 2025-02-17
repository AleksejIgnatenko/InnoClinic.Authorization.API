using InnoClinic.Authorization.Core.Models;

namespace InnoClinic.Authorization.Application.Services
{
    /// <summary>
    /// Provides validation services for account models.
    /// </summary>
    public interface IValidationService
    {
        /// <summary>
        /// Validates the specified account model and returns a dictionary of validation errors.
        /// </summary>
        /// <param name="accountModel">The account model to validate.</param>
        /// <returns>A dictionary containing validation error messages, where the key is the field name.</returns>
        Dictionary<string, string> Validation(AccountModel accountModel);
    }
}