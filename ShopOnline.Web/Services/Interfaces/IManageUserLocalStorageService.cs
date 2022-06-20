using ShopOnline.Models.Dtos.User;

namespace ShopOnline.Web.Services.Interfaces
{
    public interface IManageUserLocalStorageService
    {
        Task<UserResponseDto> GetCollection();
        Task<string> GetToken();
        Task<int> GetId();
        Task<int> GetCartId();
        Task SaveCollection(UserResponseDto userDto);
        Task RemoveCollection();
    }
}
