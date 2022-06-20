using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using ShopOnline.Models.Dtos.User;
using ShopOnline.Web.Services.Interfaces;

namespace ShopOnline.Web.Pages
{
    public class LoginBase:ComponentBase
    {
        [Inject]
        public IUserService userService { get; set; }
        [Inject]
        public IManageUserLocalStorageService userLocalStorageService { get; set; }
        [Inject]
        public AuthenticationStateProvider AuthStateProvider { get; set; }
        [Inject]
        public NavigationManager NavigationManager { get; set; }

        public string loginIdentifier { get; set; }
        public string loginPassword { get; set; }

        public string ErrorMessage { get; set; }

        public async Task OnLogin()
        {
            try
            {
                var userLogin = new UserLoginDto(loginIdentifier, loginPassword);
                var user = await userService.Login(userLogin);
                if (user != null)
                {
                    await userLocalStorageService.SaveCollection(user);
                }
                await AuthStateProvider.GetAuthenticationStateAsync();

                NavigationManager.NavigateTo("");

            }
            catch (Exception ex)
            {
    
                ErrorMessage = "Incorrect login";
            }
        }
    }
}
