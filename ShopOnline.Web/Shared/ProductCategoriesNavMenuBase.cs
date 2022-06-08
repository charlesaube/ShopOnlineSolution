using Microsoft.AspNetCore.Components;
using ShopOnline.Models.Dtos;
using ShopOnline.Web.Services.Interfaces;

namespace ShopOnline.Web.Shared
{
    public class ProductCategoriesNavMenuBase:ComponentBase
    {
        [Inject]
        public IProductService ProductService { get; set; }

        public IEnumerable<ProductCategoryDto> ProductCategoryDtos { get; set; }
        [Parameter]
        public EventCallback OnCategorySelected { get; set; }
        public string ErrorMessage { get; set; }

        public bool expandSubMenu { get; set; }


        protected override async Task OnInitializedAsync()
        {
            try
            {
                ProductCategoryDtos = await ProductService.GetProductCategories();
            }
            catch (Exception ex)
            {

                ErrorMessage = ex.Message;
            }
        }

        public void ToggleSubMenu()
        {
            expandSubMenu = !expandSubMenu;
        }

        public async Task CategorySelected()
        {
            await OnCategorySelected.InvokeAsync();
        }
    }
}
