using Logic.Models.DTO.ProductionCompanyDTO;
using Logic.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class ProductionCompanyController : ControllerBase
    {
        private readonly IProductionCompanyService _productionCompanyService;

        public ProductionCompanyController(IProductionCompanyService productionCompanyService)
        {
            _productionCompanyService = productionCompanyService;
        }

        [HttpPost("AddCompany")]
        public async Task<IActionResult> Add([FromBody] AddCompanyDTO companyDTO)
        {
            var res = await _productionCompanyService.Add(companyDTO);
            return StatusCode(res.StatusCode, res);
        }

        [HttpGet("GetCompanies")]
        [AllowAnonymous]
        public async Task<IActionResult> Get()
        {
            var res = await _productionCompanyService.GetAll();
            return StatusCode(res.StatusCode, res);
        }

        [HttpGet("GetCompanyById")]
        public async Task<IActionResult> Get([FromQuery] int id)
        {
            var res = await _productionCompanyService.GetById(id);
            return StatusCode(res.StatusCode, res);
        }

        [HttpPut("UpdateCompany")]
        public async Task<IActionResult> Update([FromBody] UpdateCompanyDTO companyDTO)
        {
            var res = await _productionCompanyService.Update(companyDTO);
            return StatusCode(res.StatusCode, res);
        }

        [HttpPatch("SoftDeleteCompany")]
        public async Task<IActionResult> Delete([FromBody] int id)
        {
            var res = await _productionCompanyService.SoftDelete(id);
            return StatusCode(res.StatusCode, res);
        }
    }
}
