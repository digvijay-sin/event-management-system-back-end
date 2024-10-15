using EventManagement.Core.RequestDTO;
using EventManagement.Core.ResponseDTO;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace EventManagement.API.Service
{
    public class TokenService
    {
        private readonly IConfiguration _config;

        public TokenService(IConfiguration config)
        {
            _config = config;
        }

        private string GenerateToken(UserResponseDTO user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            var securitykey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));

            var credentials = new SigningCredentials(securitykey, SecurityAlgorithms.HmacSha256);
            var claims = new List<Claim>();
            if(user.Role != null)
            {

                claims = new List<Claim>()
                {
                    new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                    new Claim(ClaimTypes.Name, user.Username),
                    new Claim(ClaimTypes.Role, user.Role.Name)
                };
            }
            else
            {

                claims = new List<Claim>()
                {
                    new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                    new Claim(ClaimTypes.Name, user.Username),                    
                };
            }


            var tokenDescriptor = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                expires: DateTime.Now.AddMinutes(60),
                claims : claims,
                signingCredentials: credentials
            );
            return tokenHandler.WriteToken(tokenDescriptor);
        }

        public string GetToken(UserResponseDTO user) {
            return GenerateToken(user);
        }
    }
}
