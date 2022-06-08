using Microsoft.AspNetCore.Components;
using ShopOnline.Models.Dtos;
using ShopOnline.Web.Services.Interfaces;

namespace ShopOnline.Web.Pages
{
    public class ShoppingCartBase:ComponentBase
    {
        [Inject]
        public IShoppingCartService ShoppingCartService { get; set; }
        [Inject]
        public IManageCartItemsLocalStorageService ManageCartItemsLocalStorageService { get; set; }

        public List<CartItemDto> ShoppingCartItems  { get; set; }

        public string ErrorMessage  { get; set; }

        public string TotalPrice { get; set; }
        public int TotalQuantity { get; set; }

        public int ChangedQty { get; set; }

        protected override async Task OnInitializedAsync()
        {
            try
            {
                ShoppingCartItems = await ManageCartItemsLocalStorageService.GetCollection();
                CartChanged();
            }
            catch (Exception ex)
            {

                ErrorMessage = ex.Message;
            }
        }

        protected void OnQtyInputEvent(ChangeEventArgs changeEvent)
        {
            if(changeEvent.Value != null)
            {
                ChangedQty = int.Parse(changeEvent.Value.ToString());
            }       
        }

        protected async Task UpdateItemQty_OnChanged(CartItemDto cartItemDto,int itemQty)
        {
            try
            {
                if(ChangedQty != cartItemDto.Qty && ChangedQty != 0)
                {
                    var cartItemQtyUpdateDto = new CartItemQtyUpdateDto 
                    { 
                        CartItemId = cartItemDto.Id, 
                        Qty = itemQty 
                    };
                    var updatedcartItemDto = await ShoppingCartService.UpdateCartItemQty(cartItemQtyUpdateDto);
                    await UpdateCartItemTotalPrice(updatedcartItemDto);
                    CartChanged();
                    ChangedQty = 0;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void CalculateCartSummaryTotals()
        {
            SetTotalPrice();
            SetTotalQuantity();
        }

        private void SetTotalPrice()
        {
            TotalPrice = this.ShoppingCartItems.Sum(e => e.TotalPrice).ToString("C");
        }

        private void SetTotalQuantity()
        {
            TotalQuantity = this.ShoppingCartItems.Sum(e => e.Qty);
        }

        private async Task UpdateCartItemTotalPrice(CartItemDto updatedCartItemDto)
        {
            var cartItemDto = GetCartItem(updatedCartItemDto.Id);
            if(cartItemDto != null)
            {
                cartItemDto.Qty = updatedCartItemDto.Qty;
                cartItemDto.TotalPrice = updatedCartItemDto.TotalPrice;
            }
            await ManageCartItemsLocalStorageService.SaveCollection(ShoppingCartItems);
        }

        protected async Task DeleteItem_OnClick(int id)
        {
            try
            {
                var deletedCartItem = await ShoppingCartService.DeleteItem(id);
                RemoveCartItem(id);
                CartChanged();
            }
            catch (Exception)
            {

                throw;
            }
        }
        private CartItemDto GetCartItem(int id)
        {
            return ShoppingCartItems.FirstOrDefault(x => x.Id == id);
        }
        private async Task RemoveCartItem(int id)
        {
            var cartItemDto = GetCartItem(id);
            ShoppingCartItems.Remove(cartItemDto);

            await ManageCartItemsLocalStorageService.SaveCollection(ShoppingCartItems);
        }

        private async void CartChanged()
        {

            CalculateCartSummaryTotals();
            ShoppingCartService.RaiseEventOnShoppingCartChanged(TotalQuantity);
        }
    }
}
