using Microsoft.AspNetCore.Components;
using ShopOnline.Models.Dtos;
using ShopOnline.Web.Services.Interfaces;

namespace ShopOnline.Web.Pages.Product
{
    public class ProductsByCategoryBase:ComponentBase
    {
        [Inject]
        public IProductService ProductService { get; set; }
        [Inject]
        public IManageProductsLocalStorageService ManageProductsLocalStorageService { get; set; }

        [Parameter]
        public int CategoryId { get; set; }

        public IEnumerable<ProductDto> Products { get; set; }
        public string CategoryName { get; set; }

        public string ErrorMessage { get; set; }


        protected override async Task OnParametersSetAsync()
        {
            try
            {
                Products = await GetProductsCollectionByCategory(CategoryId);
                if(Products != null)
                {
                    var product = Products.FirstOrDefault();
                    if (product != null)
                    {
                        CategoryName = product.CategoryName;
                    }
                }
            }
            catch (Exception ex)
            {

                ErrorMessage = ex.Message;
            }
        }

        private async Task<IEnumerable<ProductDto>> GetProductsCollectionByCategory(int categoryId)
        {
            var products = await ManageProductsLocalStorageService.GetCollection();
            if (products != null)
            {
                var productsByCategory = products.Where(x => x.CategoryId == categoryId);
                return productsByCategory;
            }
            else
            {
                return await ProductService.GetItemsByCategory(categoryId);
            }

        }
      

    }
}
