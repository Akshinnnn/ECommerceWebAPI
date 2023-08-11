﻿using Data.Entities;
using Logic.Models.DTO.UserDTO;
using Logic.Models.JWTContentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Services
{
    public interface IUserService
    {
        Task<bool> Register(RegisterUserDTO userDTO);
        Task<TokenContent> Login(LoginUserDTO userDTO);
        Task<bool> SoftDelete(string id);
        Task<bool> ConfirmEmail(string email, string token);
        Task<IEnumerable<GetUserDTO>> GetUsers();
    }
}
