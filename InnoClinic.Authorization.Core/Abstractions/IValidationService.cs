using FluentValidation.Results;
using InnoClinic.Authorization.Core.Models.AccountModels;

namespace InnoClinic.Authorization.Core.Abstractions;

/// <summary>
/// Provides validation services for account entities.
/// </summary>
public interface IValidationService
{
    /// <summary>
    /// Validates the specified account entity and returns a dictionary of validation errors.
    /// </summary>
    /// <param name="entity">The account entity to validate.</param>
    /// <returns>A dictionary containing validation error messages, where the key is the field name.</returns>
    List<ValidationFailure> Validation(AccountEntity accountModel);
}