using Logic.Models.DTO.SubCategoryDTO;
using Logic.Models.GenericResponseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Services
{
    public interface ISubCategoryService
    {
        Task<GenericResponse<bool>> AddSubCategory(AddSubCategoryDTO subCategoryDTO);
        Task<GenericResponse<IEnumerable<GetSubCategoryDTO>>> GetSubCategories();
        Task<GenericResponse<bool>> UpdateSubCategory(UpdateSubCategoryDTO subCategoryDTO);
        Task<GenericResponse<bool>> SoftDeleteSubCategory(int id);
        Task<GenericResponse<GetSubCategoryDTO>> GetSubCategoryById(int id);
    }
}
