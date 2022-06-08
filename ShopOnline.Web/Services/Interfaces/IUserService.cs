using ShopOnline.Models.Dtos.User;

namespace ShopOnline.Web.Services.Interfaces
{
    public interface IUserService
    {
        Task<UserDto> Login(UserLoginDto userLoginDto);
        Task<bool> Register(UserRegistrationDto userRegistrationDto);

    }
}
