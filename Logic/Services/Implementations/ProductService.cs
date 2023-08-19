using AutoMapper;
using Data.Entities;
using FluentValidation;
using Logic.Models.DTO.ProductDTO;
using Logic.Models.GenericResponseModel;
using Logic.Repository;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Services.Implementations
{
    public class ProductService : IProductService
    {
        private readonly IGenericRepository<Product> _genericRepo;
        private readonly IMapper _mapper;
        private readonly IValidator<AddProductDTO> _addProductValidator;
        private readonly IValidator<UpdateProductDTO> _updateProductValidator;

        public ProductService(IGenericRepository<Product> genericRepo, IMapper mapper,
            IValidator<UpdateProductDTO> updateProductValidator,
            IValidator<AddProductDTO> addProductValidator)
        {
            _genericRepo = genericRepo;
            _mapper = mapper;
            _updateProductValidator = updateProductValidator;
            _addProductValidator = addProductValidator;
        }

        public async Task<GenericResponse<bool>> Add(AddProductDTO productDTO)
        {
            var response = new GenericResponse<bool>();
            var validator = await _addProductValidator.ValidateAsync(productDTO);

            if (validator.IsValid)
            {
                var entity = _mapper.Map<Product>(productDTO);

                await _genericRepo.Add(entity);
                await _genericRepo.Commit();

                response.Success(true);
                return response;
            }

            response.Error(400, "Invalid information added!");
            return response;

        }

        public async Task<GenericResponse<GetProductDTO>> GetProductById(int id)
        {
            var response = new GenericResponse<GetProductDTO>();

            var entity = await _genericRepo.GetByExpression(p => p.Id == id)
            .Include(p => p.ProductInformations)
            .Include(p => p.Images).FirstOrDefaultAsync();

            if (entity is not null)
            {
                var product = _mapper.Map<GetProductDTO>(entity);
                response.Success(product);
                return response;
            }
            response.Error(400, "Product does not exist!");
            return response;

        }

        public async Task<GenericResponse<IEnumerable<GetProductDTO>>> GetProducts()
        {
            var response = new GenericResponse<IEnumerable<GetProductDTO>>();

            var entities = await _genericRepo.GetAll().Include(p => p.ProductInformations)
            .Include(p => p.Images).ToListAsync();

            if (entities is not null)
            {
                var products = _mapper.Map<IEnumerable<GetProductDTO>>(entities);

                response.Success(products);
                return response;
            }
            response.Error(400, "Products do not exist!");
            return response;

        }

        public async Task<GenericResponse<bool>> SoftDelete(int id)
        {
            var response = new GenericResponse<bool>();

            if (await _genericRepo.GetById(id) is not null)
            {
                var entity = await _genericRepo.GetById(id);
                entity.IsDeleted = true;

                _genericRepo.Update(entity);
                await _genericRepo.Commit();

                response.Success(true);
                return response;
            }
            response.Error(400, "Product does not exist!");
            return response;

        }

        public async Task<GenericResponse<bool>> Update(UpdateProductDTO productDTO)
        {
            var response = new GenericResponse<bool>();
            var validator = await _updateProductValidator.ValidateAsync(productDTO);

            if (validator.IsValid)
            {
                if (await _genericRepo.GetById(productDTO.Id) is not null)
                {
                    var entity = await _genericRepo.GetById(productDTO.Id);
                    var product = _mapper.Map(productDTO, entity);
                    product.UpdatedDate = DateTime.Now;

                    _genericRepo.Update(product);
                    await _genericRepo.Commit();

                    response.Success(true);
                    return response;
                }

                response.Error(400, "Product does not exist!");
                return response;
            }
            response.Error(400, "Invalid information!");
            return response;

        }
    }
}
