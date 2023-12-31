﻿using AutoMapper;
using Data.Entities;
using Logic.JWTService;
using Logic.Models.DTO.UserDTO;
using Logic.Models.EmailConfigurationModel;
using Logic.Models.ResetPasswordModel;
using Logic.Services;
using Microsoft.AspNetCore.Authorization;
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

        [HttpGet("Logout")]
        public async Task<IActionResult> Logout()
        {
            var res = await _userService.Logout();
            return StatusCode(res.StatusCode, res);
        }

        [HttpGet("GetUsers")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetUsers()
        {
            var res = await _userService.GetUsers();
            return StatusCode(res.StatusCode, res);
        }

        [HttpPatch("UpdateAccount")]
        public async Task<IActionResult> UpdateAccount([FromBody] UpdateUserDTO userDTO)
        {
            var res = await _userService.Update(userDTO);
            return StatusCode(res.StatusCode, res);
        }

        [HttpPatch("DeleteAccount")]
        public async Task<IActionResult> SoftDelete()
        {
            var res = await _userService.SoftDelete(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            return StatusCode(res.StatusCode, res);
        }

        [HttpPost("ForgotPassword")]
        public async Task<IActionResult> ForgotPassword(string email)
        {
            var res = await _userService.ForgotPassword(email);
            return StatusCode(res.StatusCode, res);
        }

        [HttpPost("ResetPassword")]
        public async Task<IActionResult> ResetPassword([FromBody]ResetPasswordDTO resetPasswordDTO)
        {
            var res = await _userService.ResetPassword(resetPasswordDTO);
            return StatusCode(res.StatusCode, res);
        }

        [HttpGet("GetResetPasswordModel")]
        public IActionResult GetResetPasswordModel(string email, string token)
        {
            var res = _userService.GetResetPassword(email, token);
            return StatusCode(res.StatusCode, res);
        }

        [HttpPost("ConfirmEmail")]
        public async Task<IActionResult> ConfirmEmail(string email, string token)
        {
            var res = await _userService.ConfirmEmail(email, token);
            return StatusCode(res.StatusCode, res);
        }

        [HttpPost("ResfreshTheToken")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> RefreshToken(string refreshToken)
        {
            var res = await _userService.RefreshTheToken(refreshToken);
            return StatusCode(res.StatusCode, res);
        }
    }
}
