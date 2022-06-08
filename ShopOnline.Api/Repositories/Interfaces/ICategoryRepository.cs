using ShopOnline.Api.Entities;

namespace ShopOnline.Api.Repositories.Interfaces
{
    public interface ICategoryRepository
    {
        Task<IEnumerable<ProductCategory>> GetCategories();
        Task<ProductCategory> GetCategory(int id);
    }
}
