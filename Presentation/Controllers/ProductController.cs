using Logic.Models.DTO.ProductDTO;
using Logic.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
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
            var isAdded = await _productService.Add(productDTO);

            if (isAdded)
            {
                return Ok(productDTO.ProductName);
            }
            return BadRequest("Failed attempt to add an entity!");    
        }

        [HttpGet("GetProducts")]
        public async Task<IActionResult> Get()
        {
            if (await _productService.GetProducts() is not null)
            {
                return Ok(await _productService.GetProducts());
            }

            return NotFound("Empty!");
        }

        [HttpGet("GetProductById")]
        public async Task<IActionResult> GetById([FromQuery] int id)
        {
            if (await _productService.GetProductById(id) is not null)
            {
                return Ok(await _productService.GetProductById(id));
            }

            return NotFound("Empty!");
        }

        [HttpPut("UpdateProduct")]
        public async Task<IActionResult> Update([FromBody] UpdateProductDTO productDTO)
        {
            var isUpdated = await _productService.Update(productDTO);

            if (isUpdated)
            {
                return Ok(productDTO.ProductName);
            }

            return NotFound("Failed attempt to update an entity!");
        }

        [HttpPatch("SoftDeleteProduct")]
        public async Task<IActionResult> SoftDelete(int id)
        {
            var isDeleted = await _productService.SoftDelete(id);   
            if (isDeleted)
            {
                return Ok(id);
            }

            return NotFound("Failed attempt to delete an entity!");
        }
    }
}
