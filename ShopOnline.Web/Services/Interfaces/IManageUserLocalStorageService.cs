using ShopOnline.Models.Dtos.User;

namespace ShopOnline.Web.Services.Interfaces
{
    public interface IManageUserLocalStorageService
    {
        Task<UserDto> GetCollection();
        Task<string> GetToken();
        Task<int> GetId();
        Task<int> GetCartId();
        Task SaveCollection(UserDto userDto);
        Task RemoveCollection();
    }
}
