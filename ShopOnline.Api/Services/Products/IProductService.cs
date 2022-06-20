using ShopOnline.Api.Entities;

namespace ShopOnline.Api.Services.Products
{
    public interface IProductService
    {
        Task<IEnumerable<Product>> GetItems();

        Task<IEnumerable<ProductCategory>> GetCategories();

        Task<Product> GetItem(int id);
        Task<ProductCategory> GetCategory(int id);

        Task<Product> CreateItem(Product product);

        Task<IEnumerable<Product>> GetItemsByCategory(int id);

        Task<IEnumerable<Product>> GetItemsByKeywords(string keywords);
    }
}
