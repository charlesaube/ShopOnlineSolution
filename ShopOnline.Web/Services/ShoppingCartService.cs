using Newtonsoft.Json;
using ShopOnline.Models.Dtos;
using ShopOnline.Web.Services.Interfaces;
using System.Net.Http.Json;
using System.Text;

namespace ShopOnline.Web.Services
{
    public class ShoppingCartService : IShoppingCartService
    {
        private readonly HttpClient httpClient;
        private readonly IManageUserLocalStorageService userLocalStorage;

        public event Action<int> OnShoppingCartChanged;
        public ShoppingCartService(HttpClient httpClient,IManageUserLocalStorageService userLocalStorage)
        {
            this.httpClient = httpClient;
            this.userLocalStorage = userLocalStorage;
        }

        public async Task<CartItemDto> AddItem(CartItemToAddDto cartItemToAddDto)
        {
            try
            {
                var userId = await userLocalStorage.GetId();
                var response = await httpClient.PostAsJsonAsync<CartItemToAddDto>($"api/ShoppingCart/{userId}/Item", cartItemToAddDto);

                if (response.IsSuccessStatusCode)
                {
                    if(response.StatusCode == System.Net.HttpStatusCode.NoContent)
                    {
                        return default(CartItemDto);
                    }

                    return await response.Content.ReadFromJsonAsync<CartItemDto>();
                }
                else
                {
                    var message = await response.Content.ReadAsStringAsync();
                    throw new Exception($"Http status:{response.StatusCode} Message -{message}");
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<CartItemDto> DeleteItem(int id)
        {
            try
            {
                var userId = await userLocalStorage.GetId();
                var response = await httpClient.DeleteAsync($"api/ShoppingCart/{userId}/Item/{id}");
                if (response.IsSuccessStatusCode)
                {              
                    return await response.Content.ReadFromJsonAsync<CartItemDto>();
                }
                return default(CartItemDto);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<List<CartItemDto>> GetItems(int userId)
        {
            try
            {
                var response = await httpClient.GetAsync($"api/ShoppingCart/{userId}/Items");
                if (response.IsSuccessStatusCode)
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                    {
                        return Enumerable.Empty<CartItemDto>().ToList();
                    }
                    return await response.Content.ReadFromJsonAsync<List<CartItemDto>>();
                }
                else
                {
                    var message = await response.Content.ReadAsStringAsync();
                    throw new Exception($"Http status:{response.StatusCode} Message -{message}");
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void RaiseEventOnShoppingCartChanged(int totalQty)
        {
            if(OnShoppingCartChanged != null)
            {
                OnShoppingCartChanged.Invoke(totalQty);
            }
        }

        public async Task<CartItemDto> UpdateCartItemQty(CartItemQtyUpdateDto cartItemQtyUpdateDto)
        {
            try
            {
                var userId = await userLocalStorage.GetId();
                var jsonRequest = JsonConvert.SerializeObject(cartItemQtyUpdateDto);
                var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json-patch+json");
                    
                var response = await httpClient.PatchAsync($"api/ShoppingCart/{userId}/Item/{cartItemQtyUpdateDto.CartItemId}",
                                                                                                content);
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<CartItemDto>();
                }
                else
                {
                    var message = await response.Content.ReadAsStringAsync();
                    throw new Exception($"Http status:{response.StatusCode} Message -{message}");
                }
            }

            catch (Exception)
            {

                throw;
            }
        }
    }
}
