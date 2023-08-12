using AutoMapper;
using Data.Entities;
using Logic.JWTService;
using Logic.Models.DTO.UserDTO;
using Logic.Models.EmailConfigurationModel;
using Logic.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using System.Security.Claims;
using System.Web;

namespace Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("Registration")]
        public async Task<IActionResult> Register([FromBody] RegisterUserDTO userDTO)
        {
            var registered = await _userService.Register(userDTO);
            return StatusCode(registered.StatusCode, registered);  
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginUserDTO userDTO)
        {
            var res = await _userService.Login(userDTO);
            return StatusCode(res.StatusCode, res);
        }

        [HttpGet("GetUsers")]
        public async Task<IActionResult> GetUsers()
        {
            var res = await _userService.GetUsers();
            return StatusCode(res.StatusCode, res);
        }

        [HttpPatch("DeleteAccount")]
        public async Task<IActionResult> SoftDelete()
        {
            var res = await _userService.SoftDelete(User.FindFirstValue("UserId"));
            return StatusCode(res.StatusCode, res);
        }

        [HttpPost("ConfirmEmail")]
        public async Task<IActionResult> ConfirmEmail(string email, string token)
        {
            var isConfirmed = await _userService.ConfirmEmail(email, token);
            return StatusCode(isConfirmed.StatusCode, isConfirmed);
        }
    }
}
