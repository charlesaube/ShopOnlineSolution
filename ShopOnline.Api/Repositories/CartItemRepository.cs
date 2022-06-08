using Microsoft.EntityFrameworkCore;
using ShopOnline.Api.Data;
using ShopOnline.Api.Entities;
using ShopOnline.Api.Repositories.Interfaces;
using ShopOnline.Models.Dtos;

namespace ShopOnline.Api.Repositories
{
    public class CartItemRepository : ICartItemRepository
    {
        private readonly ShopOnlineDbContext shopOnlineDbContext;

        public CartItemRepository(ShopOnlineDbContext shopOnlineDbContext)
        {
            this.shopOnlineDbContext = shopOnlineDbContext;
        }

        private async Task<bool> CartItemExists(int cartId,int productId)
        {
            return await shopOnlineDbContext.CartItems.AnyAsync(x => x.CartId == cartId && x.ProductId == productId);
        }
       
        public async Task<CartItem> AddItem(CartItemToAddDto cartItemToAddDto)
        {

            if (await CartItemExists(cartItemToAddDto.CartId, cartItemToAddDto.ProductId) == false)
            {   
                var item = await (from product in shopOnlineDbContext.Products
                        where product.Id == cartItemToAddDto.ProductId
                        select new CartItem
                        {
                            CartId = cartItemToAddDto.CartId,
                            ProductId = product.Id,
                            Qty = cartItemToAddDto.Qty

                        }).SingleOrDefaultAsync();
                if (item != null)
                {
         
                    var result = await this.shopOnlineDbContext.CartItems.AddAsync(item);
                    await this.shopOnlineDbContext.SaveChangesAsync();
                    return result.Entity;
                }
            }
            return null;
        }

        public async Task<CartItem> DeleteItem(int id)
        {
            var cartItem = await shopOnlineDbContext.CartItems.FirstOrDefaultAsync(c => c.Id == id);
            if(cartItem != null)
            {
                shopOnlineDbContext.CartItems.Remove(cartItem);
                await shopOnlineDbContext.SaveChangesAsync();
            }
            return cartItem;
        }

        public async Task<CartItem> GetItem(int id)
        {
            return await (from cart in shopOnlineDbContext.Carts
                          join cartItem in shopOnlineDbContext.CartItems
                          on cart.Id equals cartItem.CartId
                          where cartItem.Id == id
                          select new CartItem
                          {
                              Id = cartItem.Id,
                              ProductId = cartItem.ProductId,
                              Qty = cartItem.Qty,
                              CartId = cartItem.CartId
                          }).SingleOrDefaultAsync();
        }

        public async Task<IEnumerable<CartItem>> GetItems(int userId)
        {
            return await(from cart in shopOnlineDbContext.Carts
                         join cartItem in this.shopOnlineDbContext.CartItems
                         on cart.Id equals cartItem.CartId
                         where cart.UserId == userId
                         select new CartItem
                         {
                             Id = cartItem.Id,
                             ProductId= cartItem.ProductId,
                             Qty = cartItem.Qty,
                             CartId = cartItem.CartId
                         }).ToListAsync();
        }


        public async Task<CartItem> UpdateQty(int id, CartItemQtyUpdateDto cartItemQtyUpdateDto)
        {
            var cartItem = await shopOnlineDbContext.CartItems.FindAsync(id);
            if (cartItem != null)
            {
                cartItem.Qty = cartItemQtyUpdateDto.Qty;
                await shopOnlineDbContext.SaveChangesAsync();
                return cartItem;
            }

            return null;
        }

        public async Task<int> GetCartIdOfUser(int userId)
        {
            var cart = await shopOnlineDbContext.Carts.FirstOrDefaultAsync(x => x.UserId == userId);
 
            return cart.Id;
        }
    }
}
