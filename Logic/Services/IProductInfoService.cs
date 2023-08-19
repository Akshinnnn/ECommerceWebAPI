using Logic.Models.DTO.ProductInfoDTO;
using Logic.Models.GenericResponseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Services
{
    public interface IProductInfoService
    {
        Task<GenericResponse<bool>> Add(AddProductInfoDTO productInfoDTO);
        Task<GenericResponse<IEnumerable<GetProductInfoDTO>>> Get();
        Task<GenericResponse<bool>> Update(UpdateProductInfoDTO productInfoDTO);
        Task<GenericResponse<bool>> SoftDelete(int id);
        Task<GenericResponse<GetProductInfoDTO>> GetById(int id);
    }
}
