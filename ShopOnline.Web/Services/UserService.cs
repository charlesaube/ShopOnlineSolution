using ShopOnline.Models.Dtos.User;
using ShopOnline.Web.Services.Interfaces;
using System.Net.Http.Json;

namespace ShopOnline.Web.Services
{
    public class UserService : IUserService
    {
        private readonly HttpClient httpClient;

        public UserService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }
        public async Task<UserResponseDto> Login(UserLoginDto userLoginDto)
        {
            try
            {
                var response = await httpClient.PostAsJsonAsync<UserLoginDto>($"api/auth/login", userLoginDto);

                if (response.IsSuccessStatusCode)
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                    {
                        return default(UserResponseDto);
                    }

                    return await response.Content.ReadFromJsonAsync<UserResponseDto>();
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

        public async Task<bool> Register(UserRegistrationDto userRegistrationDto)
        {
            try
            {
                var response = await httpClient.PostAsJsonAsync<UserRegistrationDto>($"api/auth/login", userRegistrationDto);

                if (response.IsSuccessStatusCode)
                {

                    return true;
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
