using Logic.Models.DTO.OrderDTO;
using Logic.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpPost("MakeOrder")]
        public async Task<IActionResult> AddOrder()
        {
            var res = await _orderService.AddOrder(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            return StatusCode(res.StatusCode, res);
        }

        [HttpGet("GetOrders")]
        public async Task<IActionResult> Get()
        {
            var res = await _orderService.GetOrders(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            return StatusCode(res.StatusCode, res);
        }

        [HttpPost("Refund")]
        public async Task<IActionResult> Refund(RefundProductDTO orderDTO)
        {
            var res = await _orderService.RefundProduct(orderDTO,User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            return StatusCode(res.StatusCode, res);
        }

        [HttpGet("GetRefunds")]
        public async Task<IActionResult> GetRefunds()
        {
            var res = await _orderService.GetRefunds(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            return StatusCode(res.StatusCode, res);
        }
    }
}
