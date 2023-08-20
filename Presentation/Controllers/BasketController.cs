using Logic.Models.DTO.BasketDTO;
using Logic.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BasketController : ControllerBase
    {
        private readonly IBasketService _basketService;

        public BasketController(IBasketService basketService)
        {
            _basketService = basketService;
        }

        [HttpPost("AddBasket")]
        public async Task<IActionResult> Add([FromBody] AddBasketDTO basketDTO)
        {
            var res = await _basketService.AddProductToBasket(basketDTO, User.FindFirstValue(ClaimTypes.NameIdentifier));
            return StatusCode(res.StatusCode, res);
        }
    }
}
