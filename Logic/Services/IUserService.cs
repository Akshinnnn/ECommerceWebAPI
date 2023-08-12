using Data.Entities;
using Logic.Models.DTO.UserDTO;
using Logic.Models.GenericResponseModel;
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
        Task<GenericResponse<bool>> Register(RegisterUserDTO userDTO);
        Task<GenericResponse<TokenContent>> Login(LoginUserDTO userDTO);
        Task<GenericResponse<bool>> SoftDelete(string id);
        Task<GenericResponse<bool>> ConfirmEmail(string email, string token);
        Task<GenericResponse<IEnumerable<GetUserDTO>>> GetUsers();
    }
}
