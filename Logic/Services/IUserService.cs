using Data.Entities;
using Logic.Models.DTO.UserDTO;
using Logic.Models.GenericResponseModel;
using Logic.Models.JWTContentModel;
using Logic.Models.ResetPasswordModel;
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
        Task<GenericResponse<TokenContent>> RefreshTheToken(string resfreshToken);
        Task<GenericResponse<bool>> ForgotPassword(string email);
        GenericResponse<ResetPasswordDTO> GetResetPassword(string email, string token);
        Task<GenericResponse<bool>> ResetPassword(ResetPasswordDTO resetPasswordDTO);
        Task<GenericResponse<bool>> Update(UpdateUserDTO userDTO);
        Task<GenericResponse<bool>> Logout();
    }
}
