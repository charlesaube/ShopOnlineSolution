using Blazored.LocalStorage;
using ShopOnline.Models.Dtos;
using ShopOnline.Web.Services.Interfaces;

namespace ShopOnline.Web.Services
{
    public class ManageCartItemsLocalStorageService : IManageCartItemsLocalStorageService
    {
        private readonly ILocalStorageService localStorageService;
        private readonly IShoppingCartService shoppingCartService;
        private readonly IManageUserLocalStorageService userLocalStorage;
        private const string key = "CartItemCollection";

        public ManageCartItemsLocalStorageService(ILocalStorageService localStorageService, IShoppingCartService shoppingCartService,IManageUserLocalStorageService userLocalStorage)
        {
            this.localStorageService = localStorageService;
            this.shoppingCartService = shoppingCartService;
            this.userLocalStorage = userLocalStorage;
        }
        public async Task<List<CartItemDto>> GetCollection()
        {
            return await this.localStorageService.GetItemAsync<List<CartItemDto>>(key)
                ?? await AddCollection();
        }

        public async Task RemoveCollection()
        {
            await this.localStorageService.RemoveItemAsync(key);
            shoppingCartService.RaiseEventOnShoppingCartChanged(0);
        }

        public async Task SaveCollection(List<CartItemDto> cartItemDtos)
        {
            await this.localStorageService.SetItemAsync(key, cartItemDtos);
        }

        private async Task<List<CartItemDto>> AddCollection()
        {
            var currentUser = await userLocalStorage.GetCollection();
            if(currentUser != null)
            {
                var cartItemsCollection = await this.shoppingCartService.GetItems(currentUser.Id);
                if (cartItemsCollection != null)
                {
                    await this.localStorageService.SetItemAsync(key, cartItemsCollection);
                }
                return cartItemsCollection;
            }
            return new List<CartItemDto>();
           

        }
    }
}
