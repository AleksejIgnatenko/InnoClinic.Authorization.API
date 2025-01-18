using FluentValidation;
using InnoClinic.Authorization.Core.Models;

namespace InnoClinic.Authorization.Application.Validators
{
    internal class AccountValidator : AbstractValidator<AccountModel>
    {
        public AccountValidator()
        {
            RuleFor(x => x.Email)
                .EmailAddress()
                .NotEmpty();

            RuleFor(x => x.Password)
                .NotEmpty();
        }
    }
}
