using FluentValidation;
using Logic.Models.DTO.ProductionCompanyDTO;

namespace Logic.Validators
{
    public class AddCompanyValidator : AbstractValidator<AddCompanyDTO>
    {
        public AddCompanyValidator()
        {
            RuleFor(c => c.CompanyName)
                 .NotEmpty().WithMessage("Category name can not be empty!")
                 .MaximumLength(100).WithMessage("Length of category name can not exceed 100 characters!");
        }
    }
}
