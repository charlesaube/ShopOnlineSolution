using ShopOnline.Api.Entities;
using ShopOnline.Api.Repositories.Interfaces;
using ShopOnline.Models.Dtos;

namespace ShopOnline.Api.Services.ShoppingCart
{
    public class ShoppingCartService : IShoppingCartService
    {
        private readonly ICartItemRepository cartItemRepository;

        public ShoppingCartService(ICartItemRepository cartItemRepository)
        {
            this.cartItemRepository = cartItemRepository;
        }
        public async Task<CartItem> AddItem(CartItemToAddDto cartItemToAddDto)
        {
            return await cartItemRepository.AddItem(cartItemToAddDto);
        }

        public async Task<CartItem> DeleteItem(int id)
        {
            return await cartItemRepository.DeleteItem(id);
        }

        public async Task<int> GetCartId(int userId)
        {
            return await cartItemRepository.GetCartIdOfUser(userId);
        }

        public async Task<CartItem> GetItem(int id)
        {
            return await cartItemRepository.GetItem(id);
        }

        public async Task<IEnumerable<CartItem>> GetItems(int userId)
        {
            return await cartItemRepository.GetItems(userId);
        }

        public async Task<CartItem> UpdateQty(int id, CartItemQtyUpdateDto cartItemQtyUpdateDto)
        {
            return await cartItemRepository.UpdateQty(id, cartItemQtyUpdateDto);
        }
    }
}
