using Microsoft.AspNetCore.Components;
using ShopOnline.Models.Dtos;
using ShopOnline.Web.Services.Interfaces;

namespace ShopOnline.Web.Pages
{
    public class ProductDetailsBase:ComponentBase
    {
        [Parameter]
        public int Id { get; set; }

        [Inject]
        public IProductService ProductService { get; set; }
        [Inject]
        public IShoppingCartService ShoppingCartService { get; set; }
        [Inject]
        public IManageProductsLocalStorageService ManageProductsLocalStorageService { get; set; }
        [Inject]
        public IManageCartItemsLocalStorageService ManageCartItemsLocalStorageService { get; set; }
        [Inject]
        public IManageUserLocalStorageService ManageUserLocalStorageService { get; set; }
        [Inject]
        public NavigationManager NavigationManager { get; set; }

        public ProductDto Product { get; set; }

        public string ErrorMessage { get; set; }

        private List<CartItemDto> ShoppingCartItems { get; set; }

        protected override async Task OnInitializedAsync()
        {
            try
            {             
                ShoppingCartItems = await ManageCartItemsLocalStorageService.GetCollection();
                Product = await GetProductById(Id);
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }
        }

        protected async Task AddToCart_Click()
        {
            try
            {
                var cartItemToAddDto = new CartItemToAddDto
                {
                    CartId = await ManageUserLocalStorageService.GetCartId(),
                    ProductId = Product.Id,
                    Qty = 1
                };
                var newCartItemDto = await ShoppingCartService.AddItem(cartItemToAddDto);

                if(newCartItemDto != null)
                {
                    ShoppingCartItems.Add(newCartItemDto);
                    await ManageCartItemsLocalStorageService.SaveCollection(ShoppingCartItems);
                }
                NavigationManager.NavigateTo("/ShoppingCart");
            }
            catch (Exception ex)
            {

                ErrorMessage = ex.Message;
            }
        }

        private async Task<ProductDto> GetProductById(int id)
        {
            var productDtos = await ManageProductsLocalStorageService.GetCollection();

            if(productDtos != null)
            {
                return productDtos.SingleOrDefault(e => e.Id == id);
            }
            return null;
        }
    }
}
