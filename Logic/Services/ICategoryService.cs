using Data.Entities;
using Logic.Models.DTO.CategoryDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Services
{
    public interface ICategoryService
    {
        Task<bool> AddCategory(AddCategoryDTO categoryDTO);
        Task<IEnumerable<GetCategoryDTO>> GetCategories();
        Task<bool> UpdateCategory(UpdateCategoryDTO categoryDTO);
        Task<bool> SoftDeleteCategory(int id);
        Task<GetCategoryDTO> GetCategoryById(int id);
    }
}
