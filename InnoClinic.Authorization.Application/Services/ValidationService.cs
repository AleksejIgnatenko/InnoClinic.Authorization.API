using FluentValidation.Results;
using InnoClinic.Authorization.Application.Validators;
using InnoClinic.Authorization.Core.Models;

namespace InnoClinic.Authorization.Application.Services
{
    /// <summary>
    /// Provides validation services for account models.
    /// </summary>
    public class ValidationService : IValidationService
    {
        /// <summary>
        /// Validates the specified account model and returns a dictionary of validation errors.
        /// </summary>
        /// <param name="accountModel">The account model to validate.</param>
        /// <returns>A dictionary containing property names as keys and error messages as values.</returns>
        public Dictionary<string, string> Validation(AccountModel accountModel)
        {
            Dictionary<string, string> errors = new Dictionary<string, string>();

            AccountValidator validations = new AccountValidator();
            ValidationResult validationResult = validations.Validate(accountModel);
            if (!validationResult.IsValid)
            {
                foreach (var failure in validationResult.Errors)
                {
                    errors[failure.PropertyName] = failure.ErrorMessage;
                }
            }

            return errors;
        }
    }
}
