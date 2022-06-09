using Microsoft.IdentityModel.Tokens;
using ShopOnline.Api.Entities;
using ShopOnline.Api.Extensions;
using ShopOnline.Api.Repositories.Interfaces;
using ShopOnline.Api.Services.Interfaces;
using ShopOnline.Models.Dtos.User;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ShopOnline.Api.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository userRepository;
        private readonly ICustomPasswordHasher passwordHasher;
        private readonly IConfiguration config;

        public UserService(IUserRepository userRepository,ICustomPasswordHasher passwordHasher,IConfiguration config)
        {
            this.userRepository = userRepository;
            this.passwordHasher = passwordHasher;
            this.config = config;
        }

        public async Task<User> AddUser(User newUser)
        {
            return await userRepository.AddUser(newUser);
        }

        public async Task<User> Authenticate(UserLoginDto userLoginDto)
        {
            var currentUser = await GetUserByLoginIdentifier(userLoginDto.LoginIdentifier);

            if (currentUser != null)
            {
                var (verified, needUpgrade) = passwordHasher.Check(currentUser.PasswordHash, userLoginDto.Password);
                if (verified)
                {
                    return currentUser;
                }
            }
            return null;
        }

        public async Task<User> GetUserById(int userId)
        {
            return await userRepository.GetUserById(userId);
        }

        public async Task<User> GetUserByLoginIdentifier(string loginIdentifier)
        {
            return await userRepository.GetUserByLoginIdentifier(loginIdentifier);
        }

        public async Task<User> GetUserByUserName(string UserName)
        {
            return await userRepository.GetUserByUserName(UserName);
        }

        public async Task<User> Register(UserRegistrationDto registrationRequest)
        {
            var hash = passwordHasher.Hash(registrationRequest.Password);
            var newUser = new User
            {
                UserName = registrationRequest.UserName,
                FullName = registrationRequest.FullName,
                Email = registrationRequest.Email,
                PasswordHash = hash,
            };
            newUser = await AddUser(newUser);
            return newUser;
        }

        public string GenerateToken(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim("UserId", user.Id.ToString()),
                new Claim(ClaimTypes.Role, user.Role),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.FullName),
            };

            var token = new JwtSecurityToken(config["Jwt:Issuer"],
                config["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddMinutes(120),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);

        }

    }
}
