using FluentValidation;
using Logic.Models.DTO.SubCategoryDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Validators
{
    public class AddSubCategoryValidator : AbstractValidator<AddSubCategoryDTO>
    {
        public AddSubCategoryValidator()
        {
            RuleFor(c => c.SubCategoryName)
                 .NotEmpty()
                 .MaximumLength(100);
        }
    }
}
