using AutoMapper;
using Data.Entities;
using Logic.Models.DTO.OrderDTO;
using Logic.Models.GenericResponseModel;
using Logic.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Diagnostics.Activity;

namespace Logic.Services.Implementations
{
    public class OrderService : IOrderService
    {
        private readonly IGenericRepository<Basket> _basketGenericRepo;
        private readonly IBasketRepository _basketRepo;
        private readonly IGenericRepository<Order> _orderGenericRepo;
        private readonly IGenericRepository<Product> _productGenericRepo;
        private readonly IOrderRepository _orderProductRepo;
        private readonly IProductRepository _productRepo;
        private readonly IMapper _mapper;

        public OrderService(IGenericRepository<Basket> basketGenericRepo,
            IGenericRepository<Order> orderGenericRepo,
            IOrderRepository orderProductRepo,
            IProductRepository productRepo,
            IGenericRepository<Product> productGenericRepo,
            IBasketRepository basketRepo,
            IMapper mapper)
        {
            _basketGenericRepo = basketGenericRepo;
            _orderGenericRepo = orderGenericRepo;
            _orderProductRepo = orderProductRepo;
            _productRepo = productRepo;
            _productGenericRepo = productGenericRepo;
            _basketRepo = basketRepo;
            _mapper = mapper;
        }

        public async Task<GenericResponse<bool>> AddOrder(string userId)
        {
            GenericResponse<bool> res = new GenericResponse<bool>();
            List<OrderProduct> orderProductEntities = new List<OrderProduct>();
            List<Product> productEntities = new List<Product>();

            if (await _basketGenericRepo.GetByExpression(b => b.UserId == userId).ToListAsync() is not null)
            {
                var orderNumber = Guid.NewGuid();
                var orderReceipt = new Order()
                {
                    UserId = userId,
                    OrderNumber = orderNumber
                };

                var userBasket = await _basketGenericRepo.GetByExpression(b => b.UserId == userId)
                    .Include(b => b.Product)
                    .ToListAsync();
                foreach (var product in userBasket)
                {
                    orderReceipt.ProductQuantity += product.ProductQuantity;
                }

                await _orderGenericRepo.Add(orderReceipt);
                await _orderGenericRepo.Commit();

                var userOrder = await _orderGenericRepo.GetByExpression(o => o.OrderNumber == orderNumber)
                    .FirstOrDefaultAsync();

                foreach (var product in userBasket)
                {
                    orderProductEntities.Add(new OrderProduct()
                    {
                        ProductId = product.ProductId,
                        OrderId = userOrder.Id,
                        ProductPrice = product.Product.Price * product.ProductQuantity
                    });

                    var productEntity = await _productGenericRepo.GetById(product.ProductId);
                    productEntity.Quantity -= product.ProductQuantity;
                    productEntities.Add(productEntity);
                }

                await _orderProductRepo.AddRange(orderProductEntities.ToArray());
                await _productRepo.UpdateRange(productEntities.ToArray());

                await _basketRepo.ClearBasket(b => b.UserId == userId);

                res.Success(true);
                return res;
            }
            res.Error(400, "Basket is empty!");
            return res;
        }

        public async Task<GenericResponse<IEnumerable<GetOrderDTO>>> GetOrders(string userId)
        {
            GenericResponse<IEnumerable<GetOrderDTO>> res = new GenericResponse<IEnumerable<GetOrderDTO>>();
            var orders = await _orderGenericRepo.GetByExpression(o => o.UserId == userId).Include(o => o.OrderProducts).ToListAsync();
            if (orders is not null)
            {
                var orderEntity = _mapper.Map<IEnumerable<GetOrderDTO>>(orders);

                res.Success(orderEntity);
                return res;
            }
            res.Error(400, "You do not have orders!");
            return res;
        }
    }
}
