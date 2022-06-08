using ShopOnline.Api.Entities;

namespace ShopOnline.Api.Repositories.Interfaces
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetItems();

        Task<Product> GetItem(int id);

        Task<Product> CreateItem(Product product);

        Task<IEnumerable<Product>> GetItemsByCategory(int id);

        Task<IEnumerable<Product>> GetItemsByKeywords(string keywords);
    }
}
