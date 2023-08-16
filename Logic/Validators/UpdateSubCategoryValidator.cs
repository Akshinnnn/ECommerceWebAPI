using FluentValidation;
using Logic.Models.DTO.SubCategoryDTO;

namespace Logic.Validators
{
    public class UpdateSubCategoryValidator : AbstractValidator<UpdateSubCategoryDTO>
    {
        public UpdateSubCategoryValidator()
        {
            RuleFor(c => c.SubCategoryName)
                 .NotEmpty()
                 .MaximumLength(100);
        }
    }
}
