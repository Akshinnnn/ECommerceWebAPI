using AutoMapper;
using Data.Entities;
using Logic.Models.DTO.CategoryDTO;
using Logic.Repository;
using Logic.Repository.Implementations;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Services.Implementations
{
    public class CategoryService : ICategoryService
    {
        private readonly IGenericRepository<Category> _genericRepo;
        private readonly IMapper _mapper;

        public CategoryService(IGenericRepository<Category> genericRepo, IMapper mapper)
        {
            _genericRepo = genericRepo;
            _mapper = mapper;
        }

        public async Task<bool> AddCategory(AddCategoryDTO categoryDTO)
        {
            if (categoryDTO is not null)
            {
                var entity = _mapper.Map<Category>(categoryDTO);
                await _genericRepo.Add(entity);
                await _genericRepo.Commit();

                return true;
            }

            return false;
        }

        public async Task<IEnumerable<GetCategoryDTO>> GetCategories()
        {
            var entities = await _genericRepo.GetAll().Include(c => c.SubCategories).ToListAsync();
            var categories = _mapper.Map<IEnumerable<GetCategoryDTO>>(entities);

            return categories;
        }

        public async Task<GetCategoryDTO> GetCategoryById(int id)
        {
            var entity = await _genericRepo.GetByExpression(c => c.Id == id).Include(c => c.SubCategories).FirstOrDefaultAsync();
            var category = _mapper.Map<GetCategoryDTO>(entity);

            return category;
        }

        public async Task<bool> SoftDeleteCategory(int id)
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

        public async Task<bool> UpdateCategory(UpdateCategoryDTO categoryDTO)
        {
            if (await _genericRepo.GetById(categoryDTO.Id) is not null)
            {
                var entity = await _genericRepo.GetById(categoryDTO.Id);
                var category = _mapper.Map(categoryDTO, entity);
                category.UpdatedDate = DateTime.Now;

                _genericRepo.Update(category);
                await _genericRepo.Commit();

                return true;
            }

            return false;
        }
    }
}
