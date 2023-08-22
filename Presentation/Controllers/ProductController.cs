using Logic.Models.DTO.ProductDTO;
using Logic.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpPost("AddProduct")]
        public async Task<IActionResult> Add([FromBody] AddProductDTO productDTO)
        {
            var res = await _productService.Add(productDTO);
            return StatusCode(res.StatusCode, res);
        }

        [HttpGet("GetProducts")]
        [AllowAnonymous]
        public async Task<IActionResult> Get()
        {
            var res = await _productService.GetProducts();
            return StatusCode(res.StatusCode, res);
        }

        [HttpGet("GetProductById")]
        public async Task<IActionResult> GetById([FromQuery] int id)
        {
            var res = await _productService.GetProductById(id);
            return StatusCode(res.StatusCode, res);
        }

        [HttpPut("UpdateProduct")]
        public async Task<IActionResult> Update([FromBody] UpdateProductDTO productDTO)
        {
            var res = await _productService.Update(productDTO);
            return StatusCode(res.StatusCode, res);
        }

        [HttpPatch("SoftDeleteProduct")]
        public async Task<IActionResult> SoftDelete(int id)
        {
            var res = await _productService.SoftDelete(id);
            return StatusCode(res.StatusCode, res);
        }
    }
}
