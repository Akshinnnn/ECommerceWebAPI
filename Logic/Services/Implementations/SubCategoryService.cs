using AutoMapper;
using Data.Entities;
using FluentValidation;
using Logic.Models.DTO.CategoryDTO;
using Logic.Models.DTO.SubCategoryDTO;
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
    public class SubCategoryService : ISubCategoryService
    {
        private readonly IGenericRepository<SubCategory> _genericRepo;
        private readonly IMapper _mapper;
        private readonly IValidator<AddSubCategoryDTO> _validatorAddSubCategory;
        private readonly IValidator<UpdateSubCategoryDTO> _validatorUpdateSubCategory;

        public SubCategoryService(IGenericRepository<SubCategory> genericRepo, IMapper mapper,
            IValidator<AddSubCategoryDTO> validatorAddSubCategory,
            IValidator<UpdateSubCategoryDTO> validatorUpdateSubCategory)
        {
            _genericRepo = genericRepo;
            _mapper = mapper;
            _validatorAddSubCategory = validatorAddSubCategory;
            _validatorUpdateSubCategory = validatorUpdateSubCategory;
        }

        public async Task<GenericResponse<bool>> AddSubCategory(AddSubCategoryDTO subCategoryDTO)
        {
            GenericResponse<bool> res = new GenericResponse<bool>();
            var validator = await _validatorAddSubCategory.ValidateAsync(subCategoryDTO);

            if (validator.IsValid)
            {
                var entity = _mapper.Map<SubCategory>(subCategoryDTO);
                await _genericRepo.Add(entity);
                await _genericRepo.Commit();

                res.Success(true);
                return res;
            }
            res.Error(400, "Failed to add an entity!");
            return res;

        }

        public async Task<GenericResponse<IEnumerable<GetSubCategoryDTO>>> GetSubCategories()
        {
            GenericResponse<IEnumerable<GetSubCategoryDTO>> res = new GenericResponse<IEnumerable<GetSubCategoryDTO>>();


            var entities = await _genericRepo.GetAll().Include(c => c.Products).ToListAsync();

            if (entities is not null)
            {
                var categories = _mapper.Map<IEnumerable<GetSubCategoryDTO>>(entities);

                res.Success(categories);
                return res;
            }
            res.Error(400, "SubCategories do not exist!");
            return res;

        }

        public async Task<GenericResponse<GetSubCategoryDTO>> GetSubCategoryById(int id)
        {
            GenericResponse<GetSubCategoryDTO> res = new GenericResponse<GetSubCategoryDTO>();
            var entity = await _genericRepo.GetByExpression(c => c.Id == id).Include(c => c.Products).FirstOrDefaultAsync();

            if (entity is not null)
            {
                var category = _mapper.Map<GetSubCategoryDTO>(entity);

                res.Success(category);
                return res;
            }
            res.Error(400, "SubCategory does not exist!");
            return res;

        }

        public async Task<GenericResponse<bool>> SoftDeleteSubCategory(int id)
        {
            GenericResponse<bool> res = new GenericResponse<bool>();

            if (await _genericRepo.GetById(id) is not null)
            {
                var entity = await _genericRepo.GetById(id);
                entity.IsDeleted = true;

                _genericRepo.Update(entity);
                await _genericRepo.Commit();

                res.Success(true);
                return res;
            }
            res.Error(400, "SubCategory does not exist!");
            return res;

        }

        public async Task<GenericResponse<bool>> UpdateSubCategory(UpdateSubCategoryDTO subCategoryDTO)
        {
            GenericResponse<bool> res = new GenericResponse<bool>();
            var validator = await _validatorUpdateSubCategory.ValidateAsync(subCategoryDTO);

            if (validator.IsValid)
            {
                if (await _genericRepo.GetById(subCategoryDTO.Id) is not null)
                {
                    var entity = await _genericRepo.GetById(subCategoryDTO.Id);
                    var category = _mapper.Map(subCategoryDTO, entity);
                    category.UpdatedDate = DateTime.Now;

                    _genericRepo.Update(category);
                    await _genericRepo.Commit();

                    res.Success(true);
                    return res;
                }
                res.Error(400, "SubCategory does not exist!");
                return res;
            }
            res.Error(400, "Invalid property!");
            return res;

        }
    }
}
