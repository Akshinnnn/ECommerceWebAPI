using Logic.Models.DTO.RoleDTO;
using Logic.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly IRoleService _roleService;

        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        [HttpPost("AddRole")]
        public async Task<IActionResult> Add([FromBody] AddRoleDTO roleDTO)
        {
            var isAdded = await _roleService.AddRole(roleDTO);

            if (isAdded)
            {
                return Ok(roleDTO.Name);
            }

            return BadRequest("Failed attempt to add an entity!");
        }
    }
}
