using FluentValidation;
using Logic.Models.DTO.ProductDTO;

namespace Logic.Validators
{
    public class UpdateProductValidator : AbstractValidator<UpdateProductDTO>
    {
        public UpdateProductValidator()
        {
            RuleFor(x => x.ProductName)
                .NotNull()
                .MaximumLength(100)
                .MinimumLength(5);

            RuleFor(x => x.Price)
                .NotNull();

            RuleFor(x => x.Quantity)
                .NotNull();

            RuleFor(x => x.SubCategoryId)
                .NotNull();

            RuleFor(x => x.ProductionCompanyId)
                .NotNull();
        }
    }
}
