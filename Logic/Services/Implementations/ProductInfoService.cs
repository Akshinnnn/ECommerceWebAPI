﻿using AutoMapper;
using Data.Entities;
using Logic.Models.DTO.ProductInfoDTO;
using Logic.Models.GenericResponseModel;
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

        public async Task<GenericResponse<bool>> Add(AddProductInfoDTO productInfoDTO)
        {
            var res = new GenericResponse<bool>();

            try
            {
                if (productInfoDTO is not null)
                {
                    var entity = _mapper.Map<ProductInformation>(productInfoDTO);

                    await _genericRepo.Add(entity);
                    await _genericRepo.Commit();

                    res.Success(true);
                    return res;
                }
                res.Error(400, "Invalid properties!");
                return res;
            }
            catch (Exception ex)
            {
                res.InternalError();
            }
            return res;
        }

        public async Task<GenericResponse<IEnumerable<GetProductInfoDTO>>> Get()
        {
            var res = new GenericResponse<IEnumerable<GetProductInfoDTO>>();

            try
            {
                var entities = await _genericRepo.GetAll().ToListAsync();

                if (entities is not null)
                {
                    var infos = _mapper.Map<IEnumerable<GetProductInfoDTO>>(entities);

                    res.Success(infos);
                    return res;
                }
                res.Error(400, "Informations do not exist!");
            }
            catch (Exception ex)
            {
                res.InternalError();
            }
            return res;
            
        }

        public async Task<GenericResponse<GetProductInfoDTO>> GetById(int id)
        {
            var res = new GenericResponse<GetProductInfoDTO>();

            try
            {
                var entity = await _genericRepo.GetById(id);

                if (entity is not null)
                {
                    var info = _mapper.Map<GetProductInfoDTO>(entity);

                    res.Success(info);
                    return res;
                }
                res.Error(400, "Information does not exist!");
            }
            catch (Exception ex)
            {
                res.InternalError();
            }
            return res;

        }

        public async Task<GenericResponse<bool>> SoftDelete(int id)
        {
            var res = new GenericResponse<bool>();

            try
            {
                if (await _genericRepo.GetById(id) is not null)
                {
                    var entity = await _genericRepo.GetById(id);
                    entity.IsDeleted = true;

                    _genericRepo.Update(entity);
                    await _genericRepo.Commit();

                    res.Success(true);
                    return res;
                }
                res.Error(400, "Information does not exist!");
                return res;
            }
            catch (Exception ex)
            {
                res.InternalError();
            }
            return res;
            
        }

        public async Task<GenericResponse<bool>> Update(UpdateProductInfoDTO productInfoDTO)
        {
            var res = new GenericResponse<bool>();

            try
            {
                if (await _genericRepo.GetById(productInfoDTO.Id) is not null)
                {
                    var entity = await _genericRepo.GetById(productInfoDTO.Id);
                    var info = _mapper.Map(productInfoDTO, entity);
                    info.UpdatedDate = DateTime.Now;

                    _genericRepo.Update(info);
                    await _genericRepo.Commit();

                    res.Success(true);
                    return res;
                }
                res.Error(400, "Information does not exist!");
                return res;
            }
            catch (Exception ex)
            {
                res.InternalError();
            }
            return res;
            
        }
    }
}
