using Logic.JWTService;
using Logic.Models.DTO.UserDTO;
using Logic.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
                return Ok(userDTO.UserName);
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
    }
}
