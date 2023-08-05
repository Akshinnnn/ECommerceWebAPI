using Data.Entities;
using Logic.Models.JWTContentModel;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.JWTService
{
    public interface IJWTHelper
    {
        public TokenContent GenerateToken(IConfiguration configuration, User user);
        public string GenerateRefreshToken();
    }
}
