
namespace ShopOnline.Models.Dtos.User
{
    public record UserRegistrationDto
    (
        string UserName,
        string Email,
        string FullName,
        string Password 
    );
}
