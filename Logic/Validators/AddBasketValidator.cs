using FluentValidation;
using Logic.Models.DTO.BasketDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Validators
{
    public class AddBasketValidator : AbstractValidator<AddBasketDTO>
    {
        public AddBasketValidator()
        {
            RuleFor(x => x.ProductQuantity).NotEmpty();

            RuleFor(x => x.ProductId).NotEmpty();
        }
    }
}
