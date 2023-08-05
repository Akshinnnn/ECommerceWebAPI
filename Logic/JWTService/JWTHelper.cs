using Data.Entities;
using Logic.Models.JWTContentModel;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Logic.JWTService
{
    public class JWTHelper : IJWTHelper
    {
        public TokenContent GenerateToken(IConfiguration configuration, User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Key"]));

            var issuer = configuration["JWT:Issuer"];
            var audience = configuration["JWT:Audience"];
            SigningCredentials credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

            List<Claim> claims = new List<Claim>() 
            {
                new Claim(ClaimTypes.Email, user.Email),
                new Claim("UserId", user.Id)
            };

            var token = new JwtSecurityToken(
                issuer,
                audience,
                claims,
                notBefore:null,
                DateTime.Now.AddHours(1),
                credentials);

            var tokenContent = new TokenContent()
            {
                AccessToken = new JwtSecurityTokenHandler().WriteToken(token),
                RefreshToken = GenerateRefreshToken()
            };

            return tokenContent;
        }

        public string GenerateRefreshToken()
        {
            byte[] byteArr = new byte[32];
            using (var nums = RandomNumberGenerator.Create())
            {
                nums.GetBytes(byteArr);
                return Convert.ToBase64String(byteArr).ToString();
            }
        }
    }
}
