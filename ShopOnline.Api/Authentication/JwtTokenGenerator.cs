using Microsoft.IdentityModel.Tokens;
using ShopOnline.Api.Authentication.Interfaces;
using ShopOnline.Api.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ShopOnline.Api.Authentication
{
    public class JwtTokenGenerator:IJwtTokenGenerator
    {
        //Jwt settings used to create tokens
        private readonly IConfiguration _config;

        public JwtTokenGenerator(IConfiguration config)
        {
            this._config = config;
        }
        public string GenerateToken(User user)
        {
            //Create token signin credentials
            var signingCredentials = new SigningCredentials(
                new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(_config["Jwt:Secret"])),
                SecurityAlgorithms.HmacSha256);

            //Create token claims
            var claims = new[]
            {
                new Claim("UserId", user.Id.ToString()),
                new Claim(ClaimTypes.Role, user.Role),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.FullName)
            };

            //Create token
            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
                _config["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddMinutes(int.Parse(_config["Jwt:ExpiryMinutes"])),
                signingCredentials: signingCredentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
