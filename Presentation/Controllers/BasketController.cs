using Data.Entities;
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
    [Authorize]
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
            var res = await _basketService.AddProductToBasket(basketDTO, User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            return StatusCode(res.StatusCode, res);
        }

        [HttpGet("GetBasket")]
        public async Task<IActionResult> Get()
        {
            var res = await _basketService.GetBasket(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            return StatusCode(res.StatusCode, res);
        }

        [HttpDelete("DeleteBasket")]
        public async Task<IActionResult> Delete([FromBody]int id)
        {
            var res = await _basketService.DeleteProductFromBasket(id);
            return StatusCode(res.StatusCode, res);
        }

        [HttpDelete("ClearBasket")]
        public async Task<IActionResult> ClearBasket()
        {
            var res = await _basketService.ClearBasket(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            return StatusCode(res.StatusCode, res);
        }
    }
}
