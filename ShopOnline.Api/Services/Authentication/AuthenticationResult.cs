using ShopOnline.Api.Entities;

namespace ShopOnline.Api.Services.Authentication
{
    public record AuthenticationResult
    (
        User User,
        string Token
    );
}
