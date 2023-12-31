﻿using Logic.Models.DTO.OrderDTO;
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
        Task<GenericResponse<IEnumerable<GetOrderDTO>>> GetOrders(string userId);
        Task<GenericResponse<bool>> RefundProduct(RefundProductDTO orderDTO, string userId);
        Task<GenericResponse<IEnumerable<GetOrderProductDTO>>> GetRefunds(string userId);
    }
}
