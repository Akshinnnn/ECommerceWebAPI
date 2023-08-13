using Logic.Models.DTO.CategoryDTO;
using Logic.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpPost("AddCategory")]
        public async Task<IActionResult> Add([FromBody] AddCategoryDTO categoryDTO)
        {
            var res = await _categoryService.AddCategory(categoryDTO);
            return StatusCode(res.StatusCode, res);
        }

        [HttpGet("GetCategories")]
        public async Task<IActionResult> Get()
        {
            var res = await _categoryService.GetCategories();
            return StatusCode(res.StatusCode, res);
        }

        [HttpGet("GetCategoriesById")]
        public async Task<IActionResult> Get([FromQuery] int id)
        {
            var res = await _categoryService.GetCategoryById(id);
            return StatusCode(res.StatusCode, res);
        }

        [HttpPut("UpdateCategory")]
        public async Task<IActionResult> Update([FromBody] UpdateCategoryDTO categoryDTO)
        {
            var res = await _categoryService.UpdateCategory(categoryDTO);
            return StatusCode(res.StatusCode, res);
        }

        [HttpPatch("SoftDeleteCategory")]
        public async Task<IActionResult> Delete([FromBody] int id)
        {
            var res = await _categoryService.SoftDeleteCategory(id);
            return StatusCode(res.StatusCode, res);
        }
    }
}
