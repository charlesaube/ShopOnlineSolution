using Microsoft.AspNetCore.Components;
using ShopOnline.Models.Dtos;
using ShopOnline.Web.Services.Interfaces;

namespace ShopOnline.Web.Pages.Product
{
    public class ProductsBySearchBase:ComponentBase
    {

        [Inject]
        public IProductService ProductService { get; set; }

        [Parameter]
        public string SearchQuery { get; set; }

        public IEnumerable<ProductDto> Products { get; set; }

        public string ErrorMessage { get; set; }


        protected override async Task OnParametersSetAsync()
        {
            try
            {
                SearchQuery = SearchQuery ?? " ";
                Products = await ProductService.GetItemsByKeywords(SearchQuery);
            }
            catch (Exception ex)
            {

                ErrorMessage = ex.Message;
            }
        }

    }
}
