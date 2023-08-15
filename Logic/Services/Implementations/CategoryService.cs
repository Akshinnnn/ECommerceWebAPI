﻿using AutoMapper;
using Data.Entities;
using FluentValidation;
using Logic.Models.DTO.CategoryDTO;
using Logic.Models.GenericResponseModel;
using Logic.Repository;
using Logic.Repository.Implementations;
using Logic.Validators;
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

        public async Task<GenericResponse<bool>> AddCategory(AddCategoryDTO categoryDTO)
        {
            var res = new GenericResponse<bool>();
            var validator = new AddCategoryValidator().Validate(categoryDTO);
            
            try
            {
                if (validator.IsValid)
                {
                    var entity = _mapper.Map<Category>(categoryDTO);
                    await _genericRepo.Add(entity);
                    await _genericRepo.Commit();

                    res.Success(true);
                    return res;
                }

                res.Error(400, validator.Errors.FirstOrDefault().ToString());
                return res;
            }
            catch(Exception ex)
            {
                res.InternalError();
            }

            return res;
        }

        public async Task<GenericResponse<IEnumerable<GetCategoryDTO>>> GetCategories()
        {
            var res = new GenericResponse<IEnumerable<GetCategoryDTO>>();

            try
            {
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
            catch (Exception ex)
            {
                res.InternalError();
            }
            return res;

        }

        public async Task<GenericResponse<GetCategoryDTO>> GetCategoryById(int id)
        {
            var res = new GenericResponse<GetCategoryDTO>();

            try
            {
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
            catch (Exception ex)
            {
                res.InternalError();
            }
            return res;
            
        }

        public async Task<GenericResponse<bool>> SoftDeleteCategory(int id)
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

                res.Error(400, "Category does not exist!");
                return res;
            }
            catch (Exception ex)
            {
                res.InternalError();
            }
            return res;
        }

        public async Task<GenericResponse<bool>> UpdateCategory(UpdateCategoryDTO categoryDTO)
        {
            var res = new GenericResponse<bool>();
            var validator = new UpdateCategoryValidator().Validate(categoryDTO);

            try
            {
                if (validator.IsValid)
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
            catch (Exception ex)
            {
                res.InternalError();
            }
            return res;

        }
    }
}
