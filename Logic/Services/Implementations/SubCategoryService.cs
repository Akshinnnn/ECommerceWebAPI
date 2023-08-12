using AutoMapper;
using Data.Entities;
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

        public SubCategoryService(IGenericRepository<SubCategory> genericRepo, IMapper mapper)
        {
            _genericRepo  = genericRepo;
            _mapper = mapper;
        }

        public async Task<GenericResponse<bool>> AddSubCategory(AddSubCategoryDTO subCategoryDTO)
        {
            GenericResponse<bool> res = new GenericResponse<bool>();

            try
            {
                if (subCategoryDTO.SubCategoryName is not null)
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
            catch (Exception ex)
            {
                res.InternalError();
            }
            return res;
        }

        public async Task<GenericResponse<IEnumerable<GetSubCategoryDTO>>> GetSubCategories()
        {
            GenericResponse<IEnumerable<GetSubCategoryDTO>> res = new GenericResponse<IEnumerable<GetSubCategoryDTO>>();

            try
            {
                var entities = await _genericRepo.GetAll().Include(c => c.Products).ToListAsync();
                var categories = _mapper.Map<IEnumerable<GetSubCategoryDTO>>(entities);

                res.Success(categories);
                return res;
            }
            catch (Exception ex)
            {
                res.InternalError();
            }
            return res;
        }

        public async Task<GenericResponse<GetSubCategoryDTO>> GetSubCategoryById(int id)
        {
            GenericResponse<GetSubCategoryDTO> res = new GenericResponse<GetSubCategoryDTO>();

            try
            {
                var entity = await _genericRepo.GetByExpression(c => c.Id == id).Include(c => c.Products).FirstOrDefaultAsync();
                var category = _mapper.Map<GetSubCategoryDTO>(entity);

                res.Success(category);
                return res;
            }
            catch (Exception ex)
            {
                res.InternalError();
            }
            return res;
        }

        public async Task<GenericResponse<bool>> SoftDeleteSubCategory(int id)
        {
            GenericResponse<bool> res = new GenericResponse<bool>();

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
                res.Error(400, "User does not exist!");
                return res;
            }catch(Exception ex)
            {
                res.InternalError();
            }
            return res;
        }

        public async Task<GenericResponse<bool>> UpdateSubCategory(UpdateSubCategoryDTO subCategoryDTO)
        {
            GenericResponse<bool> res = new GenericResponse<bool>();

            try
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
                res.Error(400, "User does not exist!");
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
