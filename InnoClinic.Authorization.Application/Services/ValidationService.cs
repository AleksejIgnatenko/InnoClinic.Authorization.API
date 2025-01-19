using FluentValidation.Results;
using InnoClinic.Authorization.Application.Validators;
using InnoClinic.Authorization.Core.Models;

namespace InnoClinic.Authorization.Application.Services
{
    public class ValidationService : IValidationService
    {
        public Dictionary<string, string> AccountValidation(AccountModel accountModel)
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
