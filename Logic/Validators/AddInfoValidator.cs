using FluentValidation;
using Logic.Models.DTO.ProductInfoDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Validators
{
    public class AddInfoValidator : AbstractValidator<AddProductInfoDTO>
    {
        public AddInfoValidator()
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
