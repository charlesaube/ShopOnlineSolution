using ShopOnline.Api.Entities;
using ShopOnline.Models.Dtos;

namespace ShopOnline.Api.Repositories.Interfaces
{
    public interface ICartItemRepository
    {
        Task<CartItem> AddItem(CartItemToAddDto cartItemToAddDto );
        Task<CartItem> UpdateQty(int id, CartItemQtyUpdateDto cartItemQtyUpdateDto );
        Task<CartItem> DeleteItem( int id );
        Task<CartItem> GetItem( int id );
        Task<IEnumerable<CartItem>> GetItems(int userId);

        Task<int> GetCartIdOfUser(int userId);
    }
}
