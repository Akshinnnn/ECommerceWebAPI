using Logic.Models.DTO.ProductDTO;
using Logic.Models.GenericResponseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Services
{
    public interface IProductService
    {
        Task<GenericResponse<bool>> Add(AddProductDTO productDTO);
        Task<GenericResponse<bool>> Update(UpdateProductDTO productDTO);
        Task<GenericResponse<bool>> SoftDelete(int id);
        Task<GenericResponse<IEnumerable<GetProductDTO>>> GetProducts();
        Task<GenericResponse<GetProductDTO>> GetProductById(int id);
    }
}
