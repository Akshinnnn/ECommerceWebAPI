using Logic.Models.DTO.RoleDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Services
{
    public interface IRoleService
    {
        Task<bool> AddRole(AddRoleDTO roleDTO);
    }
}
