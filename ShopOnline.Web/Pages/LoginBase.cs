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

        public UserLoginDto userLogin { get; set; }

        public string ErrorMessage { get; set; }

        protected override async Task OnInitializedAsync()
        {
            userLogin = new UserLoginDto();
        }

        public async Task OnLogin()
        {
            try
            {
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
