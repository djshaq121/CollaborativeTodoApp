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
using Todo.Service.Entities;

namespace Todo.Service.Services
{
    public class TokenService : ITokenService
    {
        private readonly SymmetricSecurityKey key;

        public const string pool = "abcdefghijklmnopqrstuvwyxzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        public TokenService(IConfiguration configuration)
        {
            key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["TokenKey"]));
        }

        public string CreateToken(AppUser user)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.NameId, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName)
            };

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(3),
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }

        public string CreateShareableToken()
        {
            //var key = new byte[32];
            //using (var generator = RandomNumberGenerator.Create())
            //    generator.GetBytes(key);

            Random rand = new Random();
            var token = new char[32];
            for (int i = 0; i < 32; i++)
            {
                token[i] = pool[rand.Next(pool.Length)];
            }

            return new string(token);
        }
    }
}
