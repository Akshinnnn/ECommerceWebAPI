using Logic.Models.DTO.ProductDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Services
{
    public interface IProductService
    {
        Task<bool> Add(AddProductDTO productDTO);
        Task<bool> Update(UpdateProductDTO productDTO);
        Task<bool> SoftDelete(int id);
        Task<IEnumerable<GetProductDTO>> GetProducts();
        Task<GetProductDTO> GetProductById(int id);
    }
}
