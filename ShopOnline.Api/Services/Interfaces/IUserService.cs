using ShopOnline.Api.Entities;
using ShopOnline.Models.Dtos.User;

namespace ShopOnline.Api.Services.Interfaces
{
    public interface IUserService
    {
        Task<User> AddUser(User newUser);
        Task<User> GetUserByUserName(string UserName);

        Task<User> GetUserByLoginIdentifier(string loginIdentifier);
        Task<User> GetUserById(int userId);

        Task<User> Register(UserRegistrationDto registrationRequest);

        Task<User> Authenticate(UserLoginDto userLoginDto);
    }
}
