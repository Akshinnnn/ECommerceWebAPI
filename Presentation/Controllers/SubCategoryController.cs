using Logic.Models.DTO.CategoryDTO;
using Logic.Models.DTO.SubCategoryDTO;
using Logic.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubCategoryController : ControllerBase
    {
        private readonly ISubCategoryService _subCategoryService;

        public SubCategoryController(ISubCategoryService subCategoryService)
        {
            _subCategoryService = subCategoryService;
        }

        [HttpPost("AddSubCategory")]
        public async Task<IActionResult> Add([FromBody] AddSubCategoryDTO subCategoryDTO)
        {
            var isAdded = await _subCategoryService.AddSubCategory(subCategoryDTO);

            if (!isAdded)
            {
                return BadRequest("Failed attempt to add an entity!");
            }

            return Ok(subCategoryDTO.SubCategoryName);
        }

        [HttpGet("GetSubCategories")]
        public async Task<IActionResult> Get()
        {
            if (await _subCategoryService.GetSubCategories() is not null)
            {
                return Ok(await _subCategoryService.GetSubCategories());
            }

            return NotFound("Empty!");
        }

        [HttpGet("GetSubCategoriesById")]
        public async Task<IActionResult> Get([FromQuery] int id)
        {
            if (await _subCategoryService.GetSubCategoryById(id) is not null)
            {
                return Ok(await _subCategoryService.GetSubCategoryById(id));
            }

            return NotFound("Not found!");
        }

        [HttpPut("UpdateSubCategory")]
        public async Task<IActionResult> Update([FromBody] UpdateSubCategoryDTO subCategoryDTO)
        {
            var isUpdated = await _subCategoryService.UpdateSubCategory(subCategoryDTO);

            if (isUpdated)
            {
                return Ok(subCategoryDTO.SubCategoryName);
            }

            return NotFound("Failed attempt to update an entity!");
        }

        [HttpPatch("SoftDeleteSubCategory")]
        public async Task<IActionResult> Delete([FromBody] int id)
        {
            var isDeleted = await _subCategoryService.SoftDeleteSubCategory(id);

            if (isDeleted)
            {
                return Ok(id);
            }

            return NotFound("Failed attempt to delete an entity!");
        }
    }
}
