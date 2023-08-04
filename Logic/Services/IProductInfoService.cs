using Logic.Models.DTO.ProductInfoDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Services
{
    public interface IProductInfoService
    {
        Task<bool> Add(AddProductInfoDTO productInfoDTO);
        Task<IEnumerable<GetProductInfoDTO>> Get();
        Task<bool> Update(UpdateProductInfoDTO productInfoDTO);
        Task<bool> SoftDelete(int id);
        Task<GetProductInfoDTO> GetById(int id);
    }
}
