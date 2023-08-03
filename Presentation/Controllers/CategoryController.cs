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
            var isAdded = await _categoryService.AddCategory(categoryDTO);

            if (!isAdded)
            {
                return BadRequest("Failed attempt to add an entity!");
            }

            return Ok(categoryDTO.CategoryName);
        }

        [HttpGet("GetCategories")]
        public async Task<IActionResult> Get()
        {
            if (await _categoryService.GetCategories() is not null)
            {
                return Ok(await _categoryService.GetCategories());
            }

            return NotFound("Empty!");
        }

        [HttpGet("GetCategoriesById")]
        public async Task<IActionResult> Get([FromQuery] int id)
        {
            if (await _categoryService.GetCategoryById(id) is not null)
            {
                return Ok(await _categoryService.GetCategoryById(id));
            }

            return NotFound("Not found!");
        }

        [HttpPut("UpdateCategory")]
        public async Task<IActionResult> Update([FromBody] UpdateCategoryDTO categoryDTO)
        {
            var isUpdated = await _categoryService.UpdateCategory(categoryDTO);

            if (isUpdated)
            {
                return Ok(categoryDTO.CategoryName);
            }

            return NotFound("Failed attempt to update an entity!");
        }

        [HttpPatch("SoftDeleteCategory")]
        public async Task<IActionResult> Delete([FromBody] int id)
        {
            var isDeleted = await _categoryService.SoftDeleteCategory(id);

            if (isDeleted)
            {
                return Ok(id);
            }

            return NotFound("Failed attempt to delete an entity!");
        }
    }
}
