using AutoMapper;
using Data.Entities;
using Logic.Models.DTO.ProductDTO;
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

        public ProductService(IGenericRepository<Product> genericRepo, IMapper mapper)
        {
            _genericRepo = genericRepo;
            _mapper = mapper;
        }

        public async Task<bool> Add(AddProductDTO productDTO)
        {
            if (productDTO is not null)
            {
                var entity = _mapper.Map<Product>(productDTO);

                await _genericRepo.Add(entity);
                await _genericRepo.Commit();
                return true;
            }

            return false;
        }

        public async Task<GetProductDTO> GetProductById(int id)
        {
            var entity = await _genericRepo.GetByExpression(p => p.Id == id)
                .Include(p => p.ProductInformations)
                .Include(p => p.Images).FirstOrDefaultAsync();
            var product = _mapper.Map<GetProductDTO>(entity);

            return product;
        }

        public async Task<IEnumerable<GetProductDTO>> GetProducts()
        {
            var entities = await _genericRepo.GetAll().Include(p => p.ProductInformations)
                .Include(p => p.Images).ToListAsync();
            var products = _mapper.Map<IEnumerable<GetProductDTO>>(entities);

            return products;
        }

        public async Task<bool> SoftDelete(int id)
        {
            if (await _genericRepo.GetById(id) is not null)
            {
                var entity = await _genericRepo.GetById(id);
                entity.IsDeleted = true;

                _genericRepo.Update(entity);
                await _genericRepo.Commit();
                return true;
            }

            return false;
        }

        public async Task<bool> Update(UpdateProductDTO productDTO)
        {
            if (await _genericRepo.GetById(productDTO.Id) is not null)
            {
                var entity = await _genericRepo.GetById(productDTO.Id);
                var product = _mapper.Map(productDTO, entity);
                product.UpdatedDate = DateTime.Now;

                _genericRepo.Update(product);
                await _genericRepo.Commit();
                return true;
            }

            return false;
        }
    }
}
