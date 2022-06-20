using ShopOnline.Models.Dtos.User;

namespace ShopOnline.Web.Services.Interfaces
{
    public interface IUserService
    {
        Task<UserResponseDto> Login(UserLoginDto userLoginDto);
        Task<bool> Register(UserRegistrationDto userRegistrationDto);

    }
}
