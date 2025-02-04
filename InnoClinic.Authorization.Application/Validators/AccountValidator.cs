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

            //RuleFor(x => x.PhoneNumber)
            //    .NotEmpty().WithMessage("Пожалуйста, введите номер телефона")
            //    .Matches(@"^\+375\(\d{2}\)\d{3}-\d{2}-\d{2}$").WithMessage("Номер телефона должен соответствовать формату +375(XX)XXX-XX-XX");
        }
    }
}
