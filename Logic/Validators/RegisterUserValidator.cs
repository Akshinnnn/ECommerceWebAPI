using FluentValidation;
using Logic.Models.DTO.UserDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Validators
{
    public class RegisterUserValidator : AbstractValidator<RegisterUserDTO>
    {
        public RegisterUserValidator()
        {
            RuleFor(x => x.UserName).NotEmpty()
                .MaximumLength(100)
                .MinimumLength(5);

            RuleFor(x => x.Email).EmailAddress()
                .NotEmpty();

            RuleFor(x => x.Password)
                .NotEmpty()
                .MaximumLength(30)
                .MinimumLength(8);

            RuleFor(x => x.PasswordConfirmation)
                .NotEmpty()
                .MaximumLength(30)
                .MinimumLength(8);

            RuleFor(x => x.PhoneNumber)
                .NotEmpty();
        }
    }
}
