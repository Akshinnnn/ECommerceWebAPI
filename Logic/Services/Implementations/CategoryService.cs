using AutoMapper;
using Data.Entities;
using FluentValidation;
using Logic.Models.DTO.CategoryDTO;
using Logic.Models.GenericResponseModel;
using Logic.Repository;
using Logic.Repository.Implementations;
using Logic.Validators;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
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
        private IValidator<AddCategoryDTO> _validatorAddCategory;
        private IValidator<UpdateCategoryDTO> _validatorUpdateCategory;

        public CategoryService(IGenericRepository<Category> genericRepo, IMapper mapper,
            IValidator<AddCategoryDTO> validatorAddCategory,
            IValidator<UpdateCategoryDTO> validatorUpdateCategory)
        {
            _genericRepo = genericRepo;
            _mapper = mapper;
            _validatorAddCategory = validatorAddCategory;
            _validatorUpdateCategory = validatorUpdateCategory;
        }

        public async Task<GenericResponse<bool>> AddCategory(AddCategoryDTO categoryDTO)
        {
            var res = new GenericResponse<bool>();

            var validateResult = await _validatorAddCategory.ValidateAsync(categoryDTO);

            if (validateResult.IsValid)
            {
                var entity = _mapper.Map<Category>(categoryDTO);
                await _genericRepo.Add(entity);
                await _genericRepo.Commit();

                res.Success(true);
                return res;
            }

            res.Error(400, "Invalid property");
            return res;
        }

        public async Task<GenericResponse<IEnumerable<GetCategoryDTO>>> GetCategories()
        {
            var res = new GenericResponse<IEnumerable<GetCategoryDTO>>();

            var entities = await _genericRepo.GetAll().Include(c => c.SubCategories).ToListAsync();

            if (entities is not null)
            {
                var categories = _mapper.Map<IEnumerable<GetCategoryDTO>>(entities);

                res.Success(categories);
                return res;
            }
            res.Error(400, "Categories do not exist!");
            return res;
        }

        public async Task<GenericResponse<GetCategoryDTO>> GetCategoryById(int id)
        {
            var res = new GenericResponse<GetCategoryDTO>();

            var entity = await _genericRepo.GetByExpression(c => c.Id == id)
                .Include(c => c.SubCategories)
                .FirstOrDefaultAsync();

            if (entity is not null)
            {
                var category = _mapper.Map<GetCategoryDTO>(entity);

                res.Success(category);
                return res;
            }
            res.Error(400, "Category does not exist!");
            return res;
        }

        public async Task<GenericResponse<bool>> SoftDeleteCategory(int id)
        {
            var res = new GenericResponse<bool>();

            if (await _genericRepo.GetById(id) is not null)
            {
                var entity = await _genericRepo.GetById(id);
                entity.IsDeleted = true;

                _genericRepo.Update(entity);
                await _genericRepo.Commit();

                res.Success(true);
                return res;
            }

            res.Error(400, "Category does not exist!");
            return res;
        }

        public async Task<GenericResponse<bool>> UpdateCategory(UpdateCategoryDTO categoryDTO)
        {
            var res = new GenericResponse<bool>();
            var validationResult = await _validatorUpdateCategory.ValidateAsync(categoryDTO);

            if (validationResult.IsValid)
            {
                if (await _genericRepo.GetById(categoryDTO.Id) is not null)
                {
                    var entity = await _genericRepo.GetById(categoryDTO.Id);
                    var category = _mapper.Map(categoryDTO, entity);
                    category.UpdatedDate = DateTime.Now;

                    _genericRepo.Update(category);
                    await _genericRepo.Commit();

                    res.Success(true);
                    return res;
                }

                res.Error(400, "Category does not exist!");
                return res;
            }
            res.Error(400, "Invalid information!");
            return res;
        }
    }
}
