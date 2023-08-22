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
            var res = await _orderService.AddOrder(User.FindFirstValue("UserId")!);
            return StatusCode(res.StatusCode, res);
        }
    }
}
