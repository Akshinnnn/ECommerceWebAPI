using FluentValidation;
using Logic.Models.DTO.UserDTO;

namespace Logic.Validators
{
    public class UpdateUserValidator : AbstractValidator<UpdateUserDTO>
    {
        public UpdateUserValidator()
        {
            RuleFor(x => x.UserName)
                .MaximumLength(100)
                .MinimumLength(5);

            RuleFor(x => x.Email).EmailAddress();

            RuleFor(x => x.Password)
                .MaximumLength(30)
                .MinimumLength(8);
        }
    }
}
