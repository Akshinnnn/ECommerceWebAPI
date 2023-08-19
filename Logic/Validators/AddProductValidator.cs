using FluentValidation;
using Logic.Models.DTO.ProductDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Validators
{
    public class AddProductValidator : AbstractValidator<AddProductDTO>
    {
        public AddProductValidator()
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
