using Logic.Models.DTO.RoleDTO;
using Logic.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
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
            var res = await _roleService.AddRole(roleDTO);
            return StatusCode(res.StatusCode, res);
        }

        [HttpGet("GetRoles")]
        public async Task<IActionResult> Get()
        {
            var res = await _roleService.GetRoles();
            return StatusCode(res.StatusCode, res);
        }

        [HttpPut("UpdateRole")]
        public async Task<IActionResult> Update([FromBody] UpdateRoleDTO roleDTO)
        {
            var res = await _roleService.UpdateRole(roleDTO);
            return StatusCode(res.StatusCode, res);
        }

        [HttpDelete("DeleteRole")]
        public async Task<IActionResult> Delete([FromBody] string id)
        {
            var res = await _roleService.DeleteRole(id);
            return StatusCode(res.StatusCode, res);
        }
    }
}
