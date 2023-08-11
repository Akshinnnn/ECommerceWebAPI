using AutoMapper;
using Data.Entities;
using Logic.JWTService;
using Logic.Models.DTO.UserDTO;
using Logic.Models.EmailConfigurationModel;
using Logic.Models.JWTContentModel;
using Logic.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IMapper _mapper;
        private readonly IConfiguration _config;
        private readonly ITokenService _tokenService;
        private readonly IEmailService _emailService;
        private readonly IMessageService _messageService;
        private readonly IGenericRepository<User> _userRepo;

        public UserService(UserManager<User> userManager, SignInManager<User> signInManager,
            IMapper mapper, IConfiguration config,
            ITokenService tokenService,
            IEmailService emailService,
            IMessageService messageService, IGenericRepository<User> userRepo)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _mapper = mapper;
            _config = config;
            _tokenService = tokenService;
            _emailService = emailService;
            _messageService = messageService;
            _userRepo = userRepo;
        }

        public async Task<bool> ConfirmEmail(string email, string token)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user is not null)
            {
                var result = await _userManager.ConfirmEmailAsync(user, token);
                if (result.Succeeded)
                {
                    return true;
                }
                return false;
            }
            return false;
        }

        public async Task<IEnumerable<GetUserDTO>> GetUsers()
        {
            var users = await _userRepo.GetAll()
                .Where(u => u.IsDeleted == false)
                .Include(u => u.Orders)
                .Include(u => u.Baskets)
                .ToListAsync();
            var userDTO = _mapper.Map<IEnumerable<GetUserDTO>>(users);

            return userDTO;
        }

        public async Task<TokenContent> Login(LoginUserDTO userDTO)
        {
            var user = await _userManager.FindByEmailAsync(userDTO.Email);

            if (user is not null)
            {
                Microsoft.AspNetCore.Identity.SignInResult result = await _signInManager.PasswordSignInAsync(user, userDTO.Password, true, false);

                if (result.Succeeded)
                {
                    var tokenContent = new JWTHelper().GenerateToken(_config, user);

                    user.RefreshToken = tokenContent.RefreshToken;

                    await _userManager.UpdateAsync(user);

                    await _tokenService.AddToken(user.Id, tokenContent.AccessToken!);

                    return tokenContent;
                }

                return default(TokenContent);
            }

            return default(TokenContent);
        }

        public async Task<bool> Register(RegisterUserDTO userDTO)
        {
            if (userDTO.Password.Equals(userDTO.PasswordConfirmation))
            {
                var user = _mapper.Map<User>(userDTO);
                var result = await _userManager.CreateAsync(user, userDTO.Password);

                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, "User");

                    var message = await _messageService.GenerateMessage(user);
                    await _emailService.SendEmail(message);

                    return true;
                }

                return false;
            }

            return false;
        }

        public async Task<bool> SoftDelete(string id)
        {
            if (await _userManager.FindByIdAsync(id) is not null)
            {
                var user = await _userManager.FindByIdAsync(id);
                user.IsDeleted = true;

                await _userManager.UpdateAsync(user);
                return true;
            }

            return false;
        }
    }
}
