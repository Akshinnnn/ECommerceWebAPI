using Data.Entities;
using Logic.Models.DTO.CategoryDTO;
using Logic.Models.GenericResponseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Services
{
    public interface ICategoryService
    {
        Task<GenericResponse<bool>> AddCategory(AddCategoryDTO categoryDTO);
        Task<GenericResponse<IEnumerable<GetCategoryDTO>>> GetCategories();
        Task<GenericResponse<bool>> UpdateCategory(UpdateCategoryDTO categoryDTO);
        Task<GenericResponse<bool>> SoftDeleteCategory(int id);
        Task<GenericResponse<GetCategoryDTO>> GetCategoryById(int id);
    }
}
