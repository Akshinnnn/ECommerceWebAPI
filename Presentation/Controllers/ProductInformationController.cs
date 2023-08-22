using Logic.Models.DTO.ProductInfoDTO;
using Logic.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class ProductInformationController : ControllerBase
    {
        private readonly IProductInfoService _productInfoService;

        public ProductInformationController(IProductInfoService productInfoService)
        {
            _productInfoService = productInfoService;
        }

        [HttpPost("AddProductInfo")]
        public async Task<IActionResult> Add([FromBody] AddProductInfoDTO productInfoDTO)
        {
            var res = await _productInfoService.Add(productInfoDTO);
            return StatusCode(res.StatusCode, res);
        }

        [HttpGet("GetProductInfo")]
        [AllowAnonymous]
        public async Task<IActionResult> Get()
        {
            var res = await _productInfoService.Get();
            return StatusCode(res.StatusCode, res);
        }

        [HttpGet("GetProductInfoById")]
        public async Task<IActionResult> GetById([FromQuery] int id)
        {
            var res = await _productInfoService.GetById(id);
            return StatusCode(res.StatusCode, res);
        }

        [HttpPut("UpdateProductInfo")]
        public async Task<IActionResult> Update([FromBody] UpdateProductInfoDTO productInfoDTO)
        {
            var res = await _productInfoService.Update(productInfoDTO);
            return StatusCode(res.StatusCode, res);
        }

        [HttpPut("SoftDeleteProductInfo")]
        public async Task<IActionResult> Delete([FromBody] int id)
        {
            var res = await _productInfoService.SoftDelete(id);
            return StatusCode(res.StatusCode, res);
        }
    }
}
