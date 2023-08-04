using AutoMapper;
using Data.Entities;
using Logic.Models.DTO.CategoryDTO;
using Logic.Models.DTO.SubCategoryDTO;
using Logic.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Services.Implementations
{
    public class SubCategoryService : ISubCategoryService
    {
        private readonly IGenericRepository<SubCategory> _genericRepo;
        private readonly IMapper _mapper;

        public SubCategoryService(IGenericRepository<SubCategory> genericRepo, IMapper mapper)
        {
            _genericRepo  = genericRepo;
            _mapper = mapper;
        }

        public async Task<bool> AddSubCategory(AddSubCategoryDTO subCategoryDTO)
        {
            if (subCategoryDTO.SubCategoryName is not null)
            {
                var entity = _mapper.Map<SubCategory>(subCategoryDTO);
                await _genericRepo.Add(entity);
                await _genericRepo.Commit();

                return true;
            }

            return false;
        }

        public async Task<IEnumerable<GetSubCategoryDTO>> GetSubCategories()
        {
            var entities = await _genericRepo.GetAll().Include(c => c.Products).ToListAsync();
            var categories = _mapper.Map<IEnumerable<GetSubCategoryDTO>>(entities);

            return categories;
        }

        public async Task<GetSubCategoryDTO> GetSubCategoryById(int id)
        {
            var entity = await _genericRepo.GetByExpression(c => c.Id == id).Include(c => c.Products).FirstOrDefaultAsync();
            var category = _mapper.Map<GetSubCategoryDTO>(entity);

            return category;
        }

        public async Task<bool> SoftDeleteSubCategory(int id)
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

        public async Task<bool> UpdateSubCategory(UpdateSubCategoryDTO subCategoryDTO)
        {
            if (await _genericRepo.GetById(subCategoryDTO.Id) is not null)
            {
                var entity = await _genericRepo.GetById(subCategoryDTO.Id);
                var category = _mapper.Map(subCategoryDTO, entity);
                category.UpdatedDate = DateTime.Now;

                _genericRepo.Update(category);
                await _genericRepo.Commit();

                return true;
            }

            return false;
        }
    }
}
