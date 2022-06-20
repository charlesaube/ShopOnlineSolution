using Blazored.LocalStorage;
using ShopOnline.Models.Dtos.User;
using ShopOnline.Web.Services.Interfaces;

namespace ShopOnline.Web.Services
{
    public class ManageUserLocalStorageService : IManageUserLocalStorageService
    {

        private readonly ILocalStorageService localStorageService;

        private const string key = "UserCollection";

        public ManageUserLocalStorageService(ILocalStorageService localStorageService)
        {
            this.localStorageService = localStorageService;

        }
        public async Task<UserResponseDto> GetCollection()
        {
            return await this.localStorageService.GetItemAsync<UserResponseDto>(key);          
        }

        public async Task RemoveCollection()
        {
            await this.localStorageService.RemoveItemAsync(key);
        }

        public async Task SaveCollection(UserResponseDto userDto)
        {
            await this.localStorageService.SetItemAsync(key, userDto);
        }

        public async Task<string> GetToken()
        {
           var user = await this.localStorageService.GetItemAsync<UserResponseDto>(key); 
           if(user != null)
            {
                return user.JwtToken;
            }
           return null;
        }

        public async Task<int> GetId()
        {
            var user = await this.localStorageService.GetItemAsync<UserResponseDto>(key);
            if (user != null)
            {
                return user.Id;
            }
            return 1;
        }

        public async Task<int> GetCartId()
        {
            var user = await this.localStorageService.GetItemAsync<UserResponseDto>(key);
            if (user != null)
            {
                return user.CartId;
            }
            return 1;
        }
    }
}
