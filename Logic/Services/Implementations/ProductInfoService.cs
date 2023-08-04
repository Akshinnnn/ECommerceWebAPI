using AutoMapper;
using Data.Entities;
using Logic.Models.DTO.ProductInfoDTO;
using Logic.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Services.Implementations
{
    public class ProductInfoService : IProductInfoService
    {
        private readonly IGenericRepository<ProductInformation> _genericRepo;
        private readonly IMapper _mapper;

        public ProductInfoService(IGenericRepository<ProductInformation> genericRepo, IMapper mapper)
        {
            _genericRepo = genericRepo;
            _mapper = mapper;
        }

        public async Task<bool> Add(AddProductInfoDTO productInfoDTO)
        {
            if (productInfoDTO is not null)
            {
                var entity = _mapper.Map<ProductInformation>(productInfoDTO);

                await _genericRepo.Add(entity);
                await _genericRepo.Commit();

                return true;
            }

            return false;
        }

        public async Task<IEnumerable<GetProductInfoDTO>> Get()
        {
            var entities = await _genericRepo.GetAll().ToListAsync();
            var infos = _mapper.Map<IEnumerable<GetProductInfoDTO>>(entities);

            return infos;
        }

        public async Task<GetProductInfoDTO> GetById(int id)
        {
            var entity = await _genericRepo.GetById(id);
            var info = _mapper.Map<GetProductInfoDTO>(entity);

            return info;
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

        public async Task<bool> Update(UpdateProductInfoDTO productInfoDTO)
        {
            if (await _genericRepo.GetById(productInfoDTO.Id) is not null)
            {
                var entity = await _genericRepo.GetById(productInfoDTO.Id);
                var info = _mapper.Map(productInfoDTO, entity);
                info.UpdatedDate = DateTime.Now;

                _genericRepo.Update(info);
                await _genericRepo.Commit();

                return true;
            }

            return false;
        }
    }
}
