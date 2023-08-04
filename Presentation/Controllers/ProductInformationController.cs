using Logic.Models.DTO.ProductInfoDTO;
using Logic.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
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
            var isAdded = await _productInfoService.Add(productInfoDTO);

            if (isAdded)
            {
                return Ok(productInfoDTO.Header);
            }

            return BadRequest("Failed attempt to add an entity!");
        }

        [HttpGet("GetProductInfo")]
        public async Task<IActionResult> Get()
        {
            if (await _productInfoService.Get() is not null)
            {
                return Ok(await _productInfoService.Get());
            }

            return NotFound();
        }

        [HttpGet("GetProductInfoById")]
        public async Task<IActionResult> GetById([FromQuery] int id)
        {
            if (await _productInfoService.GetById(id) is not null)
            {
                return Ok(await _productInfoService.GetById(id));
            }

            return NotFound();
        }

        [HttpPut("UpdateProductInfo")]
        public async Task<IActionResult> Update([FromBody] UpdateProductInfoDTO productInfoDTO)
        {
            var isUpdated = await _productInfoService.Update(productInfoDTO);

            if (isUpdated)
                return Ok(productInfoDTO.Header);

            return NotFound();
        }

        [HttpPut("SoftDeleteProductInfo")]
        public async Task<IActionResult> Delete([FromBody] int id)
        {
            var isDeleted = await _productInfoService.SoftDelete(id);

            if (isDeleted)
                return Ok(id);

            return NotFound();
        }
    }
}
