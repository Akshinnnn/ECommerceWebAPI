using Logic.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private readonly ITokenService _tokenService;

        public TokenController(ITokenService tokenService)
        {
            _tokenService = tokenService;
        }

        [HttpPost("RefreshTheToken")]
        public async Task<IActionResult> RefreshToken([FromBody] string refreshToken)
        {
            var tokenContent = await _tokenService.RefreshToken(refreshToken);

            if (tokenContent is not null)
            {
                return Ok(tokenContent);
            }

            return BadRequest("Validation has failed!");
        }
    }
}
