using AutoMapper;
using Data.Entities;
using Logic.Models.DTO.ProductionCompanyDTO;
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

        public ProductionCompanyService(IGenericRepository<ProductionCompany> genericRepo, IMapper mapper)
        {
            _genericRepo = genericRepo;
            _mapper = mapper;
        }

        public async Task<bool> Add(AddCompanyDTO companyDTO)
        {
            if (companyDTO is not null)
            {
                var company = _mapper.Map<ProductionCompany>(companyDTO);

                await _genericRepo.Add(company);
                await _genericRepo.Commit();

                return true;
            }

            return false;
        }

        public async Task<IEnumerable<GetCompanyDTO>> GetAll()
        {
            var entities = await _genericRepo.GetAll().ToListAsync();
            var companies = _mapper.Map<IEnumerable<GetCompanyDTO>>(entities);

            return companies;   
        }

        public async Task<GetCompanyDTO> GetById(int id)
        {
            var entity = await _genericRepo.GetById(id);
            var company = _mapper.Map<GetCompanyDTO>(entity);

            return company;
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

        public async Task<bool> Update(UpdateCompanyDTO companyDTO)
        {
            if (await _genericRepo.GetById(companyDTO.Id) is not null)
            {
                var entity = await _genericRepo.GetById(companyDTO.Id);
                var company = _mapper.Map(companyDTO, entity);
                entity.UpdatedDate = DateTime.Now;

                _genericRepo.Update(entity);
                await _genericRepo.Commit();
                return true;
            }

            return false;
        }
    }
}
