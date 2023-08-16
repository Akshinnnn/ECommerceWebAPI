using Data.Entities;
using FluentValidation;
using Logic.Models.DTO.CategoryDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Validators
{
    public class AddCategoryValidator : AbstractValidator<AddCategoryDTO>
    {
        public AddCategoryValidator()
        {
            RuleFor(c => c.CategoryName)
                 .NotEmpty().WithMessage("Category name can not be empty!")
                 .MaximumLength(100).WithMessage("Length of category name can not exceed 100 characters!");               
        }
    }
}
