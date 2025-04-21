using FluentValidation;
using FluentValidation.Results;
using InnoClinic.Authorization.Application.Validators;
using InnoClinic.Authorization.Core.Models.AccountModels;

namespace InnoClinic.Authorization.Application.Services
{
    /// <summary>
    /// Provides validation services for account entities.
    /// </summary>
    public class ValidationService : IValidationService
    {
        /// <summary>
        /// Validates the specified account уtity  and returns a dictionary of validation errors.
        /// </summary>
        /// <param name="entity">The account уntity  to validate.</param>
        /// <returns>A dictionary containing property names as keys and error messages as values.</returns>
        public List<ValidationFailure> Validation(AccountEntity entity)
        {
            var validator = new AccountValidator();
            return Validate(entity, validator);
        }

        private List<ValidationFailure> Validate<T>(T model, IValidator<T> validator)
        {
            var validationFailures = new List<ValidationFailure>();
            ValidationResult validationResult = validator.Validate(model);
            if (!validationResult.IsValid)
            {
                foreach (var failure in validationResult.Errors)
                {
                    validationFailures.Add(new ValidationFailure(failure.PropertyName, failure.ErrorMessage));
                }
            }

            return validationFailures;
        }
    }
}
