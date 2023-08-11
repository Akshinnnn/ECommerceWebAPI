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
            if (registered)
            {
                return Ok($"User registered and email confirmation link sent to {userDTO.Email}");               
            }
            
            return BadRequest("Failed to register!");    
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginUserDTO userDTO)
        {
            var tokenContent = await _userService.Login(userDTO);   
            
            if (tokenContent is not null)
            {
                return Ok(tokenContent);
            }
            
            return BadRequest("Email or password is not correct!");
        }

        [HttpGet("GetUsers")]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _userService.GetUsers();
            if (users is not null)
            {
                return Ok(users);
            }

            return NoContent();
        }

        [HttpPatch]
        public async Task<IActionResult> SoftDelete()
        {
            var isDeleted = await _userService.SoftDelete(User.FindFirstValue("UserId"));

            if (isDeleted)
            {
                return Ok();
            }

            return BadRequest();
        }

        [HttpPost("ConfirmEmail")]
        public async Task<IActionResult> ConfirmEmail(string email, string token)
        {
            var isConfirmed = await _userService.ConfirmEmail(email, token);
            if (isConfirmed)
            {
                return Ok("Email is successfully confirmed!");
            }
            return BadRequest("Failed to confirm an email!");
        }
    }
}
