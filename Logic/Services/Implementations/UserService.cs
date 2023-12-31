﻿using AutoMapper;
using Data.Entities;
using FluentValidation;
using Logic.JWTService;
using Logic.Models.DTO.UserDTO;
using Logic.Models.EmailConfigurationModel;
using Logic.Models.GenericResponseModel;
using Logic.Models.JWTContentModel;
using Logic.Models.ResetPasswordModel;
using Logic.Repository;
using Logic.Validators;
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
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Web;

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
        private readonly IValidator<RegisterUserDTO> _registerValidator;
        private readonly IValidator<LoginUserDTO> _loginValidator;
        private readonly IValidator<UpdateUserDTO> _updateValidator;

        public UserService(UserManager<User> userManager, SignInManager<User> signInManager,
            IMapper mapper, IConfiguration config,
            ITokenService tokenService,
            IEmailService emailService,
            IMessageService messageService, IGenericRepository<User> userRepo,
            IValidator<LoginUserDTO> loginValidator,
            IValidator<RegisterUserDTO> registerValidator,
            IValidator<UpdateUserDTO> updateValidator)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _mapper = mapper;
            _config = config;
            _tokenService = tokenService;
            _emailService = emailService;
            _messageService = messageService;
            _userRepo = userRepo;
            _loginValidator = loginValidator;
            _registerValidator = registerValidator;
            _updateValidator = updateValidator;
        }

        public async Task<GenericResponse<bool>> ConfirmEmail(string email, string token)
        {
            GenericResponse<bool> response = new GenericResponse<bool>();
            var user = await _userManager.FindByEmailAsync(email);

            if (user is not null && user.IsDeleted == false)
            {
                var result = await _userManager.ConfirmEmailAsync(user, token);
                if (result.Succeeded)
                {
                    response.Success(true);
                    return response;
                }
                response.Error(400, "Failed to confirm an email!");
                return response;
            }
            response.Error(400, "Failed to confirm an email!");
            return response;

        }

        public async Task<GenericResponse<bool>> ForgotPassword(string email)
        {
            GenericResponse<bool> response = new GenericResponse<bool>();
            var user = await _userManager.FindByEmailAsync(email);

                if (user is not null && user.IsDeleted == false)
                {
                    var token = HttpUtility.UrlEncode(await _userManager.GeneratePasswordResetTokenAsync(user));
                    var forgotPasswordLink = $"https://localhost:44381/api/User/GetResetPassword?email={user.Email}&token={token}";
                    var message = await _messageService.GenerateMessage(user, forgotPasswordLink);
                    await _emailService.SendEmail(message);

                    response.Success(true);
                    return response;
                }
                response.Error(400, "User does not exist!");
                return response;

        }

        public GenericResponse<ResetPasswordDTO> GetResetPassword(string email, string token)
        {
            GenericResponse<ResetPasswordDTO> response = new GenericResponse<ResetPasswordDTO>();
            var resetPasswordModel = new ResetPasswordDTO()
            {
                Token = token,
                Email = email,
            };
            response.Success(resetPasswordModel);
            return response;
        }

        public async Task<GenericResponse<IEnumerable<GetUserDTO>>> GetUsers()
        {
            GenericResponse<IEnumerable<GetUserDTO>> res = new GenericResponse<IEnumerable<GetUserDTO>>();

            var users = await _userRepo.GetAll()
            .Where(u => u.IsDeleted == false)
            .Include(u => u.Orders)
            .Include(u => u.Baskets)
            .ToListAsync();

            if (users is not null)
            {
                var userDTO = _mapper.Map<IEnumerable<GetUserDTO>>(users);

                res.Success(userDTO);
                return res;
            }
            res.Error(400, "Users do not exist!");
            return res;

        }

        public async Task<GenericResponse<TokenContent>> Login(LoginUserDTO userDTO)
        {
            GenericResponse<TokenContent> res = new GenericResponse<TokenContent>();
            var validator = await _loginValidator.ValidateAsync(userDTO);

            if (validator.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(userDTO.Email);

                if (user is not null && user.IsDeleted == false)
                {
                    Microsoft.AspNetCore.Identity.SignInResult result = await _signInManager.PasswordSignInAsync(user, userDTO.Password, true, false);

                    if (result.Succeeded)
                    {
                        var tokenContent = new JWTHelper().GenerateToken(_config, user);

                        user.RefreshToken = tokenContent.RefreshToken;

                        await _userManager.UpdateAsync(user);

                        await _tokenService.AddToken(user.Id, tokenContent.AccessToken!);

                        res.Success(tokenContent);
                        return res;
                    }

                    res.Error(400, "Email or password is not correct!");
                    return res;
                }

                res.Error(400, "Email or password is not correct!");
                return res;
            }
            res.Error(400, "Invalid property!");
            return res;

        }

        public async Task<GenericResponse<bool>> Logout()
        {
            GenericResponse<bool> res = new GenericResponse<bool>();

            await _signInManager.SignOutAsync();
            res.Success(true);
            return res;
        }

        public async Task<GenericResponse<TokenContent>> RefreshTheToken(string resfreshToken)
        {
            var res = new GenericResponse<TokenContent>();

            var tokenContent = await _tokenService.RefreshToken(resfreshToken);
            if (tokenContent is not null)
            {
                res.Success(tokenContent);
                return res;
            }
            res.Error(400, "Failed to generate token!");
            return res;

        }

        public async Task<GenericResponse<bool>> Register(RegisterUserDTO userDTO)
        {
            GenericResponse<bool> res = new GenericResponse<bool>();
            var validator = await _registerValidator.ValidateAsync(userDTO);

            if (validator.IsValid)
            {
                if (await _userManager.FindByNameAsync(userDTO.UserName) is null)
                {
                    if (userDTO.Password.Equals(userDTO.PasswordConfirmation))
                    {
                        var user = _mapper.Map<User>(userDTO);
                        var result = await _userManager.CreateAsync(user, userDTO.Password);

                        if (result.Succeeded)
                        {
                            await _userManager.AddToRoleAsync(user, "User");

                            var token = HttpUtility.UrlEncode(await _userManager.GenerateEmailConfirmationTokenAsync(user));
                            var confirmEmailLink = $"https://localhost:44381/api/User/ConfirmEmail?email={user.Email}&token={token}";
                            var message = await _messageService.GenerateMessage(user, confirmEmailLink);
                            await _emailService.SendEmail(message);

                            res.Success(true);
                            return res;
                        }

                        res.Error(400, "Failed to register");
                        return res;
                    }

                    res.Error(400, "Passwords do not match!");
                    return res;
                }
                res.Error(400, $"Username {userDTO.UserName} is taken!");
                return res;
            }
            res.Error(400, $"Invalid property!");
            return res;

        }

        public async Task<GenericResponse<bool>> ResetPassword(ResetPasswordDTO resetPasswordDTO)
        {
            GenericResponse<bool> res = new GenericResponse<bool>();

            var user = await _userManager.FindByEmailAsync(resetPasswordDTO.Email);

            if (user is not null)
            {
                if (resetPasswordDTO.NewPassword == resetPasswordDTO.ConfirmPassword)
                {
                    var resetPasswordResult = await _userManager
                        .ResetPasswordAsync(user, resetPasswordDTO.Token, resetPasswordDTO.NewPassword);

                    res.Success(true);
                    return res;
                }
                res.Error(400, "Passwords do not match!");
                return res;
            }
            res.Error(400, "User does not exist!");
            return res;

        }

        public async Task<GenericResponse<bool>> SoftDelete(string id)
        {
            var res = new GenericResponse<bool>();

            if (await _userManager.FindByIdAsync(id) is not null)
            {
                var user = await _userManager.FindByIdAsync(id);
                user.IsDeleted = true;

                await _userManager.UpdateAsync(user);

                res.Success(true);
                return res;
            }
            res.Error(400, "User does not exist!");
            return res;

        }

        public async Task<GenericResponse<bool>> Update(UpdateUserDTO userDTO)
        {
            var res = new GenericResponse<bool>();
            var validator = await _updateValidator.ValidateAsync(userDTO);

            if (validator.IsValid)
            {
                var entity = await _userManager.FindByIdAsync(userDTO.Id);
                var user = _mapper.Map(userDTO, entity);

                await _userManager.UpdateAsync(user);

                res.Success(true);
                return res;
            }
            res.Error(400, "Invalid property!");
            return res;

        }
    }
}
