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
        private readonly IGenericRepository<OrderProduct> _orderProductGenericRepo;
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
            IMapper mapper,
            IGenericRepository<OrderProduct> orderProductGenericRepo)
        {
            _basketGenericRepo = basketGenericRepo;
            _orderGenericRepo = orderGenericRepo;
            _orderProductRepo = orderProductRepo;
            _productRepo = productRepo;
            _productGenericRepo = productGenericRepo;
            _basketRepo = basketRepo;
            _mapper = mapper;
            _orderProductGenericRepo = orderProductGenericRepo;
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

        public async Task<GenericResponse<IEnumerable<GetOrderProductDTO>>> GetRefunds(string userId)
        {
            GenericResponse<IEnumerable<GetOrderProductDTO>> res = new GenericResponse<IEnumerable<GetOrderProductDTO>>();
            List<OrderProduct> refunds = new List<OrderProduct>();

            var orders = await _orderGenericRepo.GetByExpression(o => o.UserId == userId).Include(o => o.OrderProducts).ToListAsync();
            if (orders is not null)
            {
                foreach (var order in orders)
                {
                    foreach (var product in order.OrderProducts)
                    {
                        if (product.RefundDate is not null)
                        {
                            refunds.Add(product);
                        }
                    }
                }
                var refundDTO = _mapper.Map<IEnumerable<GetOrderProductDTO>>(refunds);

                res.Success(refundDTO);
                return res;
            }
            res.Error(400,"Order does not exist!");
            return res;
        }

        public async Task<GenericResponse<bool>> RefundProduct(RefundProductDTO orderDTO, string userId)
        {
            GenericResponse<bool> res = new GenericResponse<bool>();

            var order = await _orderGenericRepo.GetByExpression(o => o.UserId == userId && o.Id == orderDTO.OrderId)
                .FirstOrDefaultAsync();

            if (order is not null)
            {
                var product = await _orderProductGenericRepo.GetByExpression(op => op.OrderId == order.Id && op.ProductId == orderDTO.ProductId)
                    .FirstOrDefaultAsync();
                if (product is not null)
                {
                    if (DateTime.Now.Subtract(order.CreatedDate).TotalDays <= 14)
                    {
                        product.RefundDate = DateTime.Now;

                        _orderProductGenericRepo.Update(product);
                        await _orderProductGenericRepo.Commit();

                        res.Success(true);
                        return res;
                    }
                    res.Error(400, "Refund is acceptable within 14 days after the order date!");
                    return res;
                }
                res.Error(400, "Product does not exist in a current order!");
                return res;
            }
            res.Error(400, "Order does not exist!");
            return res;
        }
    }
}
