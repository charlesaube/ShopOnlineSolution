using ShopOnline.Api.Entities;

namespace ShopOnline.Api.Authentication.Interfaces
{
    public interface IJwtTokenGenerator
    {
        string GenerateToken(User user);
    }
}
