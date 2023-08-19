using AutoMapper;
using Logic.Models.DTO.RoleDTO;
using Logic.Models.GenericResponseModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Services.Implementations
{
    public class RoleService : IRoleService
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMapper _mapper;

        public RoleService(RoleManager<IdentityRole> roleManager,
            IMapper mapper)
        {
            _roleManager = roleManager;
            _mapper = mapper;
        }

        public async Task<GenericResponse<bool>> AddRole(AddRoleDTO roleDTO)
        {
            var res = new GenericResponse<bool>();
            var role = _mapper.Map<IdentityRole>(roleDTO);
            await _roleManager.CreateAsync(role);

            res.Success(true);
            return res;

        }

        public async Task<GenericResponse<bool>> DeleteRole(string name)
        {
            var res = new GenericResponse<bool>();
            var role = await _roleManager.FindByNameAsync(name);

            if (role is not null)
            {
                await _roleManager.DeleteAsync(role);

                res.Success(true);
                return res;
            }
            res.Error(400, "Role does not exist!");
            return res;

        }

        public async Task<GenericResponse<IEnumerable<GetRolesDTO>>> GetRoles()
        {
            var res = new GenericResponse<IEnumerable<GetRolesDTO>>();
            var roles = await _roleManager.Roles.ToListAsync();

            if (roles is not null)
            {
                var rolesDTO = _mapper.Map<IEnumerable<GetRolesDTO>>(roles);
                res.Success(rolesDTO);
                return res;
            }
            res.Error(400, "Roles do not exist!");
            return res;

        }

        public async Task<GenericResponse<bool>> UpdateRole(UpdateRoleDTO roleDTO)
        {
            var res = new GenericResponse<bool>();
            var role = await _roleManager.FindByIdAsync(roleDTO.Id);

            if (role is not null)
            {
                await _roleManager.UpdateAsync(role);

                res.Success(true);
                return res;
            }
            res.Error(400, "Role does not exist!");
            return res;
        }
    }
}
