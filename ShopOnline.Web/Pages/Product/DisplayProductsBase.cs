using Microsoft.AspNetCore.Components;
using ShopOnline.Models.Dtos;

namespace ShopOnline.Web.Pages
{
    public class DisplayProductsBase:ComponentBase
    {
        [Parameter]
        public IEnumerable<ProductDto> Products { get; set; }
        public Random random { get; set; }


        protected async override Task OnInitializedAsync()
        {
           random = new Random();
        }
    }
}
