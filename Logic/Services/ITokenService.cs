using Logic.Models.JWTContentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Services
{
    public interface ITokenService
    {
        public Task<TokenContent> RefreshToken(string refreshToken);
        public Task AddToken(string id, string token);
    }
}
