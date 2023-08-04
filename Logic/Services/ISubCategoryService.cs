using Logic.Models.DTO.SubCategoryDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Services
{
    public interface ISubCategoryService
    {
        Task<bool> AddSubCategory(AddSubCategoryDTO subCategoryDTO);
        Task<IEnumerable<GetSubCategoryDTO>> GetSubCategories();
        Task<bool> UpdateSubCategory(UpdateSubCategoryDTO subCategoryDTO);
        Task<bool> SoftDeleteSubCategory(int id);
        Task<GetSubCategoryDTO> GetSubCategoryById(int id);
    }
}
