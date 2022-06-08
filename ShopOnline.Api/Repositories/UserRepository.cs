using Microsoft.EntityFrameworkCore;
using ShopOnline.Api.Data;
using ShopOnline.Api.Entities;
using ShopOnline.Api.Repositories.Interfaces;
using ShopOnline.Models.Dtos;

namespace ShopOnline.Api.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ShopOnlineDbContext shopOnlineDbContext;

        public UserRepository(ShopOnlineDbContext shopOnlineDbContext)
        {
            this.shopOnlineDbContext = shopOnlineDbContext;
        }
        public async Task<User> AddUser(User newUser)
        {
            if(newUser != null)
            {
                if(await UserExist(newUser) == false)
                {     
                    var user = await shopOnlineDbContext.AddAsync(newUser);
                    var cart = new Cart { UserId = user.Entity.Id };
                    await shopOnlineDbContext.AddAsync(cart);
                    await shopOnlineDbContext.SaveChangesAsync();
                    return user.Entity;
                }
            }
            return null;
        }
        private async Task<bool> UserExist(User user)
        {
            return await shopOnlineDbContext.Users.AnyAsync(e => e.UserName == user.UserName || e.Email == user.Email);
        }

        public async Task<User> GetUserByLoginIdentifier(string loginIdentifier)
        {
            return await shopOnlineDbContext.Users.FirstOrDefaultAsync(e => e.Email == loginIdentifier || e.UserName == loginIdentifier);  
        }

        public async Task<User> GetUserById(int userId)
        {
            return await shopOnlineDbContext.Users.FirstOrDefaultAsync(e => e.Id == userId);
        }

        public async Task<User> GetUserByUserName(string UserName)
        {
            return await shopOnlineDbContext.Users.FirstOrDefaultAsync(e => e.UserName == UserName);
        }
    }
}
