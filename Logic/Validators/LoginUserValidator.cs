using FluentValidation;
using Logic.Models.DTO.UserDTO;

namespace Logic.Validators
{
    public class LoginUserValidator : AbstractValidator<LoginUserDTO>
    {
        public LoginUserValidator()
        {
            RuleFor(x => x.Email).EmailAddress()
                .NotEmpty();

            RuleFor(x => x.Password)
                .NotEmpty()
                .MaximumLength(30)
                .MinimumLength(8);
        }
    }
}
