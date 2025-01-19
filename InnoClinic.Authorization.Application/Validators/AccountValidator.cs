using FluentValidation;
using InnoClinic.Authorization.Core.Models;

namespace InnoClinic.Authorization.Application.Validators
{
    internal class AccountValidator : AbstractValidator<AccountModel>
    {
        public AccountValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Пожалуйста, введите email")
                .EmailAddress().WithMessage("Вы ввели неверный email");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Пожалуйста, введите пароль");
        }
    }
}
