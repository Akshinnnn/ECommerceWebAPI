using FluentValidation;
using Logic.Models.DTO.ProductionCompanyDTO;

namespace Logic.Validators
{
    public class UpdateCompanyValidator : AbstractValidator<UpdateCompanyDTO>
    {
        public UpdateCompanyValidator()
        {
            RuleFor(c => c.CompanyName)
                 .NotEmpty().WithMessage("Category name can not be empty!")
                 .MaximumLength(100).WithMessage("Length of category name can not exceed 100 characters!");
        }
    }
}
