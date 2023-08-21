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
        private readonly IGenericRepository<Basket> _basketGenericRepo;
        private readonly IBasketRepository _basketRepository;
        private readonly IGenericRepository<Product> _productRepo;
        private readonly IValidator<AddBasketDTO> _basketValidator;
        private readonly IMapper _mapper;

        public BasketService(IGenericRepository<Basket> basketRepo, IValidator<AddBasketDTO> basketValidator,
            IMapper mapper, IGenericRepository<Product> productRepo, IBasketRepository basketRepository)
        {
            _basketGenericRepo = basketRepo;
            _basketValidator = basketValidator;
            _mapper = mapper;
            _productRepo = productRepo;
            _basketRepository = basketRepository;
        }

        public async Task<GenericResponse<bool>> AddProductToBasket(AddBasketDTO basketDTO, string userId)
        {
            GenericResponse<bool> res = new GenericResponse<bool>();
            var validator = await _basketValidator.ValidateAsync(basketDTO);

            if (validator.IsValid)
            {
                var basketWithSameProduct = await _basketGenericRepo
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

                            await _basketGenericRepo.Add(basket);
                            await _basketGenericRepo.Commit();

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

                _basketGenericRepo.Update(basketWithSameProduct);
                await _basketGenericRepo.Commit();

                res.Success(true);
                return res;

            }
            res.Error(400, "Invalid property!");
            return res;
        }

        public async Task<GenericResponse<bool>> ClearBasket(string userId)
        {
            GenericResponse<bool> res = new GenericResponse<bool>();

            var baskets = await _basketGenericRepo.GetByExpression(b => b.UserId == userId).ToListAsync();
            if (baskets is not null)
            {
                _basketRepository.ClearBasket(b => b.UserId == userId);
                await _basketGenericRepo.Commit();

                res.Success(true);
                return res;
            }
            res.Error(400, "Your basket is empty!");
            return res;
        }

        public async Task<GenericResponse<bool>> DeleteProductFromBasket(int id)
        {
            GenericResponse<bool> res = new GenericResponse<bool>();

            if (await _basketGenericRepo.GetById(id) is not null)
            {
                _basketGenericRepo.Delete(id);
                await _basketGenericRepo.Commit();

                res.Success(true);
                return res;
            }

            res.Error(400, "Basket does not exist!");
            return res;
        }

        public async Task<GenericResponse<IEnumerable<GetBasketDTO>>> GetBasket(string userId)
        {
            GenericResponse<IEnumerable<GetBasketDTO>> res = new GenericResponse<IEnumerable<GetBasketDTO>>();

            var baskets = await _basketGenericRepo.GetByExpression(b => b.UserId == userId).ToListAsync();

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
