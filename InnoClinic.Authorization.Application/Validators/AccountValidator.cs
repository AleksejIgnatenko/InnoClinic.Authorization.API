using FluentValidation;
using InnoClinic.Authorization.Core.Models.AccountModels;

namespace InnoClinic.Authorization.Application.Validators
{
    /// <summary>
    /// Validates account details for user registration and management.
    /// </summary>
    internal class AccountValidator : AbstractValidator<AccountEntity>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AccountValidator"/> class.
        /// </summary>
        public AccountValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Please enter an email")
                .EmailAddress().WithMessage("You have entered an invalid email");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Please enter a password");
        }
    }
}