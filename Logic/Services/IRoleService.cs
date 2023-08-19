using Logic.Models.DTO.RoleDTO;
using Logic.Models.GenericResponseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Services
{
    public interface IRoleService
    {
        Task<GenericResponse<bool>> AddRole(AddRoleDTO roleDTO);
        Task<GenericResponse<bool>> UpdateRole(UpdateRoleDTO roleDTO);
        Task<GenericResponse<bool>> DeleteRole(string name);
        Task<GenericResponse<IEnumerable<GetRolesDTO>>> GetRoles();
    }
}
