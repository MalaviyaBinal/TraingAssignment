
using HalloDocWebEntity.Data;
using HalloDocWebEntity.ViewModel;
using HalloDocWebRepo.Interface;
using HalloDocWebServices.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Collections;
using System.IdentityModel.Tokens.Jwt;
using System.IO.Compression;
using System.Net;
using System.Net.Mail;
using System.Security.Claims;
using System.Text;

namespace HalloDocWebServices.Implementation
{
    public class JWT_Service : IJWT_Service
    {
        private readonly IConfiguration _config;

        public JWT_Service(IConfiguration config)
        {
            _config = config;
        }

        public string GenerateToken(Aspnetuser user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, user.Email),
                new Claim("passwordClaim",user.Passwordhash),
                new Claim("userEmail",user.Email),
                new Claim("userName",user.Usarname),
                new Claim(ClaimTypes.Role,"patient"),
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expire = DateTime.UtcNow.AddDays(1);

            var token = new JwtSecurityToken(
                 _config["Jwt:Issuer"],
                 _config["Jwt:Audience"],
                 claims,
                 expires : expire,
                 signingCredentials : creds
            );
            Console.WriteLine("hello");
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public bool ValidateToken(string token, out JwtSecurityToken jwttoken)
        {
            jwttoken = null;
            if(token == null)
            {
                return false;
            }
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_config["Jwt:Key"]);
            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                jwttoken = (JwtSecurityToken)validatedToken;
                if (jwttoken != null)
                    return true;
                return false;

            }
            catch
            {
                return false;
            }
        }
    }
}
