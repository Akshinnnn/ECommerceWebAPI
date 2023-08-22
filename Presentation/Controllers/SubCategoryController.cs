using Azure;
using Logic.Models.DTO.CategoryDTO;
using Logic.Models.DTO.SubCategoryDTO;
using Logic.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
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
            var response = await _subCategoryService.AddSubCategory(subCategoryDTO);
            return StatusCode(response.StatusCode, response);
        }

        [HttpGet("GetSubCategories")]
        [AllowAnonymous]
        public async Task<IActionResult> Get()
        {
            var response = await _subCategoryService.GetSubCategories();
            return StatusCode(response.StatusCode, response);
        }

        [HttpGet("GetSubCategoriesById")]
        public async Task<IActionResult> Get([FromQuery] int id)
        {
            var response = await _subCategoryService.GetSubCategoryById(id);
            return StatusCode(response.StatusCode, response);
        }

        [HttpPut("UpdateSubCategory")]
        public async Task<IActionResult> Update([FromBody] UpdateSubCategoryDTO subCategoryDTO)
        {
            var response = await _subCategoryService.UpdateSubCategory(subCategoryDTO);
            return StatusCode(response.StatusCode, response);
        }

        [HttpPatch("SoftDeleteSubCategory")]
        public async Task<IActionResult> Delete([FromBody] int id)
        {
            var response = await _subCategoryService.SoftDeleteSubCategory(id);
            return StatusCode(response.StatusCode, response);
        }
    }
}
