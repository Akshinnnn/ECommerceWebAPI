using Logic.Models.DTO.BasketDTO;
using Logic.Models.GenericResponseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Services
{
    public interface IBasketService
    {
        Task<GenericResponse<bool>> AddProductToBasket(AddBasketDTO basketDTO, string userId);
        Task<GenericResponse<IEnumerable<GetBasketDTO>>> GetBasket(string userId);
        Task<GenericResponse<bool>> DeleteProductFromBasket(int id);
    }
}
