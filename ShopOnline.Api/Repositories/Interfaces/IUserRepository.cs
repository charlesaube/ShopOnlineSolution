using ShopOnline.Api.Entities;

namespace ShopOnline.Api.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<User> AddUser(User newUser);
        Task<User> GetUserByUserName(string UserName);

        Task<User> GetUserByLoginIdentifier(string loginIdentifier);
        Task<User> GetUserById(int userId);
    }
}
