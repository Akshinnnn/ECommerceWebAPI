using Data.Entities;
using Logic.JWTService;
using Logic.Models.JWTContentModel;
using Logic.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Services.Implementations
{
    public class TokenService : ITokenService
    {
        private readonly IGenericRepository<User> _userRepo;
        private readonly IGenericRepository<Token> _tokenRepo;
        private readonly IConfiguration _config;

        public TokenService(IGenericRepository<User> userRepo, IConfiguration config, IGenericRepository<Token> tokenRepo)
        {
            _userRepo = userRepo;
            _config = config;
            _tokenRepo = tokenRepo;
        }

        public async Task AddToken(string id, string token)
        {
            var tokenEntity = new Token() 
            { 
                UserId = id,
                JWTToken = token,
                CreatedDate = DateTime.Now,
            };
            await _tokenRepo.Add(tokenEntity);
            await _tokenRepo.Commit();
        }

        public async Task<TokenContent> RefreshToken(string refreshToken)
        {
            var user = await _userRepo.GetByExpression(u => u.RefreshToken == refreshToken).FirstOrDefaultAsync();

            if (user is not null)
            {
                var tokenContent = new JWTHelper().GenerateToken(_config, user);

                user.RefreshToken = tokenContent.RefreshToken;
                _userRepo.Update(user);
                await _userRepo.Commit();

                await AddToken(user.Id, tokenContent.AccessToken);

                return tokenContent;
            }

            return default(TokenContent);
        }
    }
}
