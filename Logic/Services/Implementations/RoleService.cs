using AutoMapper;
using Logic.Models.DTO.RoleDTO;
using Microsoft.AspNetCore.Identity;
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

        public async Task<bool> AddRole(AddRoleDTO roleDTO)
        {
            var role = _mapper.Map<IdentityRole>(roleDTO);
            await _roleManager.CreateAsync(role);
            return true;
        }
    }
}
