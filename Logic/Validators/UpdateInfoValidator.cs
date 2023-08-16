using FluentValidation;
using Logic.Models.DTO.ProductInfoDTO;

namespace Logic.Validators
{
    public class UpdateInfoValidator : AbstractValidator<UpdateProductInfoDTO>
    {
        public UpdateInfoValidator()
        {
            RuleFor(x => x.Header)
                .NotEmpty()
                .MinimumLength(5)
                .MaximumLength(50);

            RuleFor(x => x.Description)
                .NotEmpty()
                .MinimumLength(5)
                .MaximumLength(100);
        }
    }
}
