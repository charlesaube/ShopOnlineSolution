using ShopOnline.Models.Dtos.User;

namespace ShopOnline.Api.Services.Authentication
{
    public interface IAuthenticationService
    {

        Task<AuthenticationResult> Register(UserRegistrationDto registrationRequest);

        Task<AuthenticationResult> Authenticate(UserLoginDto userLoginDto);

    }
}
