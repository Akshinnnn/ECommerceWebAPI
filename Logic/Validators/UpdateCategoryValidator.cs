using FluentValidation;
using Logic.Models.DTO.CategoryDTO;

namespace Logic.Validators
{
    public class UpdateCategoryValidator : AbstractValidator<UpdateCategoryDTO>
    {
        public UpdateCategoryValidator()
        {
            RuleFor(c => c.CategoryName)
                .NotEmpty()
                .NotNull()
                .WithMessage("Category name can not be empty!");

            RuleFor(c => c.CategoryName)
                .MaximumLength(100)
                .WithMessage("Length of category name can not exceed 100 characters!");
        }
    }
    
}
