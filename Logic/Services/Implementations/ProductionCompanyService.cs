using AutoMapper;
using Data.Entities;
using FluentValidation;
using Logic.Models.DTO.ProductionCompanyDTO;
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
    public class ProductionCompanyService : IProductionCompanyService
    {
        private readonly IGenericRepository<ProductionCompany> _genericRepo;
        private readonly IMapper _mapper;
        private readonly IValidator<AddCompanyDTO> _addCompanyValidator;
        private readonly IValidator<UpdateCompanyDTO> _updateCompanyValidator;

        public ProductionCompanyService(IGenericRepository<ProductionCompany> genericRepo, IMapper mapper)
        {
            _genericRepo = genericRepo;
            _mapper = mapper;
        }

        public async Task<GenericResponse<bool>> Add(AddCompanyDTO companyDTO)
        {
            var res = new GenericResponse<bool>();
            var validator = await _addCompanyValidator.ValidateAsync(companyDTO);   
            try
            {
                if (validator.IsValid)
                {
                    var company = _mapper.Map<ProductionCompany>(companyDTO);

                    await _genericRepo.Add(company);
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

        public async Task<GenericResponse<IEnumerable<GetCompanyDTO>>> GetAll()
        {
            var res = new GenericResponse<IEnumerable<GetCompanyDTO>>();

            try
            {
                var entities = await _genericRepo.GetAll().ToListAsync();

                if (entities is not null)
                {
                    var companies = _mapper.Map<IEnumerable<GetCompanyDTO>>(entities);

                    res.Success(companies);
                    return res;
                }
                res.Error(400, "Companies do not exist!");
                return res;
            }
            catch (Exception ex)
            {
                res.InternalError();
            }
            return res;

        }

        public async Task<GenericResponse<GetCompanyDTO>> GetById(int id)
        {
            var res = new GenericResponse<GetCompanyDTO>();

            try
            {
                var entity = await _genericRepo.GetById(id);

                if (entity is not null)
                {
                    var company = _mapper.Map<GetCompanyDTO>(entity);

                    res.Success(company);
                    return res;
                }
                res.Error(400, "Company does not exist!");
                return res;
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
                res.Error(400, "Company does not exist!");
                return res;
            }
            catch (Exception ex)
            {
                res.InternalError();
            }
            return res;
            
        }

        public async Task<GenericResponse<bool>> Update(UpdateCompanyDTO companyDTO)
        {
            var res = new GenericResponse<bool>();
            var validator = await _updateCompanyValidator.ValidateAsync(companyDTO);
            try
            {
                if (validator.IsValid)
                {
                    if (await _genericRepo.GetById(companyDTO.Id) is not null)
                    {
                        var entity = await _genericRepo.GetById(companyDTO.Id);
                        var company = _mapper.Map(companyDTO, entity);
                        entity.UpdatedDate = DateTime.Now;

                        _genericRepo.Update(entity);
                        await _genericRepo.Commit();

                        res.Success(true);
                        return res;
                    }
                    res.Error(400, "Company does not exist!");
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
    }
}
