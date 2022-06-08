using ShopOnline.Api.Entities;
using ShopOnline.Api.Extensions;
using ShopOnline.Api.Repositories.Interfaces;
using ShopOnline.Api.Services.Interfaces;
using ShopOnline.Models.Dtos.User;

namespace ShopOnline.Api.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository userRepository;
        private readonly ICustomPasswordHasher passwordHasher;

        public UserService(IUserRepository userRepository,ICustomPasswordHasher passwordHasher)
        {
            this.userRepository = userRepository;
            this.passwordHasher = passwordHasher;
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

    }
}
