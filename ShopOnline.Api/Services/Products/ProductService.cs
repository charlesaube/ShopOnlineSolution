using ShopOnline.Api.Entities;
using ShopOnline.Api.Repositories.Interfaces;

namespace ShopOnline.Api.Services.Products
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository productRepository;
        private readonly ICategoryRepository categoryRepository;

        public ProductService(IProductRepository productRepository, ICategoryRepository categoryRepository)
        {
            this.productRepository = productRepository;
            this.categoryRepository = categoryRepository;
        }

        public async Task<Product> CreateItem(Product product)
        {
            return await productRepository.CreateItem(product);
        }

        public async Task<IEnumerable<ProductCategory>> GetCategories()
        {
            return await categoryRepository.GetCategories();
        }

        public async Task<ProductCategory> GetCategory(int id)
        {
            return await categoryRepository.GetCategory(id);
        }

        public async Task<Product> GetItem(int id)
        {
            return await productRepository.GetItem(id);
        }

        public async Task<IEnumerable<Product>> GetItems()
        {
           return await productRepository.GetItems();
        }

        public async Task<IEnumerable<Product>> GetItemsByCategory(int id)
        {
            return await productRepository.GetItemsByCategory(id);
        }

        public async Task<IEnumerable<Product>> GetItemsByKeywords(string keywords)
        {
            return await productRepository.GetItemsByKeywords(keywords);
        }
    }
}
