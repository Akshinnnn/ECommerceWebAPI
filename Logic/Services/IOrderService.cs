using Logic.Models.GenericResponseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Services
{
    public interface IOrderService
    {
        Task<GenericResponse<bool>> AddOrder(string userId);
    }
}
