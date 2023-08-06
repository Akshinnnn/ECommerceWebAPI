using AutoMapper;
using Data.Entities;
using Logic.JWTService;
using Logic.Models.DTO.UserDTO;
using Logic.Models.JWTContentModel;
using Logic.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
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
        private readonly IGenericRepository<User> _userRepo;
        private readonly ITokenService _tokenService;

        public UserService(UserManager<User> userManager, SignInManager<User> signInManager,
            IMapper mapper, IConfiguration config,
            IGenericRepository<User> userRepo,
            ITokenService tokenService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _mapper = mapper;
            _config = config;
            _userRepo = userRepo;
            _tokenService = tokenService;
        }

        public async Task<TokenContent> Login(LoginUserDTO userDTO)
        {
            var user = await _userManager.FindByEmailAsync(userDTO.Email);

            if (user is not null)
            {
                SignInResult result = await _signInManager.PasswordSignInAsync(user, userDTO.Password, true, false);

                if (result.Succeeded)
                {
                    var tokenContent = new JWTHelper().GenerateToken(_config, user);

                    user.RefreshToken = tokenContent.RefreshToken;
                    _userRepo.Update(user);
                    await _userRepo.Commit();

                    await _tokenService.AddToken(user.Id, tokenContent.AccessToken);

                    return tokenContent;
                }

                return null;
            }

            return null;
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
                    return true;
                }

                return false;
            }

            return false;
        }
    }
}
