using AutoMapper;
using Data.Entities;
using FluentValidation;
using Logic.Models.DTO.BasketDTO;
using Logic.Models.GenericResponseModel;
using Logic.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Services.Implementations
{
    public class BasketService : IBasketService
    {
        private readonly IGenericRepository<Basket> _basketRepo;
        private readonly IGenericRepository<Product> _productRepo;
        private readonly IValidator<AddBasketDTO> _basketValidator;
        private readonly IMapper _mapper;

        public BasketService(IGenericRepository<Basket> basketRepo, IValidator<AddBasketDTO> basketValidator,
            IMapper mapper, IGenericRepository<Product> productRepo)
        {
            _basketRepo= basketRepo;
            _basketValidator= basketValidator;
            _mapper= mapper;
            _productRepo = productRepo;
        }

        public async Task<GenericResponse<bool>> AddProductToBasket(AddBasketDTO basketDTO, string userId)
        {
            GenericResponse<bool> res = new GenericResponse<bool>();
            var validator = await _basketValidator.ValidateAsync(basketDTO);

            if (validator.IsValid)
            {
                var basketWithSameProduct = await _basketRepo
                    .GetByExpression(b => b.UserId == userId && b.ProductId == basketDTO.ProductId)
                    .FirstOrDefaultAsync();

                if (basketWithSameProduct is null)
                {
                    if (await _productRepo.GetById(basketDTO.ProductId) is not null)
                    {
                        var product = await _productRepo.GetById(basketDTO.ProductId);
                        if (product.Quantity >= basketDTO.ProductQuantity)
                        {
                            var basket = _mapper.Map<Basket>(basketDTO);
                            basket.UserId = userId;

                            await _basketRepo.Add(basket);
                            await _basketRepo.Commit();

                            res.Success(true);
                            return res;
                        }
                        res.Error(400, "There are not enough products in the stock!");
                        return res;
                    }
                    res.Error(400, "Product does not exist!");
                    return res;
                }

                basketWithSameProduct.ProductQuantity += basketDTO.ProductQuantity;

                _basketRepo.Update(basketWithSameProduct);
                await _basketRepo.Commit();

                res.Success(true);
                return res;

            }
            res.Error(400, "Invalid property!");
            return res;
        }

        public async Task<GenericResponse<bool>> DeleteProductFromBasket(int id)
        {
            GenericResponse<bool> res = new GenericResponse<bool>();

            if (await _basketRepo.GetById(id) is not null)
            {
                _basketRepo.Delete(id);
                await _basketRepo.Commit();

                res.Success(true);
                return res;
            }

            res.Error(400, "Basket does not exist!");
            return res;
        }

        public async Task<GenericResponse<IEnumerable<GetBasketDTO>>> GetBasket(string userId)
        {
            GenericResponse<IEnumerable<GetBasketDTO>> res = new GenericResponse<IEnumerable<GetBasketDTO>>();

            var baskets = await _basketRepo.GetByExpression(b => b.UserId == userId).ToListAsync();

            if (baskets is not null)
            {
                var basketDTO = _mapper.Map<IEnumerable<GetBasketDTO>>(baskets);

                res.Success(basketDTO);
                return res;
            }
            res.Error(400, "Basket is empty!");
            return res;
        }
    }
}
