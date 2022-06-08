using Microsoft.EntityFrameworkCore;
using ShopOnline.Api.Data;
using ShopOnline.Api.Entities;
using ShopOnline.Api.Repositories.Interfaces;

namespace ShopOnline.Api.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ShopOnlineDbContext shopOnlineDbContext;

        public ProductRepository(ShopOnlineDbContext shopOnlineDbContext)
        {
            this.shopOnlineDbContext = shopOnlineDbContext;
        }

        public async Task<Product> CreateItem(Product product)
        {
            
            if (product != null)
            {

                var result = await this.shopOnlineDbContext.Products.AddAsync(product);
                await this.shopOnlineDbContext.SaveChangesAsync();
                return await GetItem(result.Entity.Id);
            }
            
            return null;
        }

        public async Task<Product> GetItem(int id)
        {
            var product = await shopOnlineDbContext.Products
                                .Include( p => p.ProductCategory)
                                .SingleOrDefaultAsync(p => p.Id == id);
            return product;
        }

        public async Task<IEnumerable<Product>> GetItems()
        {
            var products = await this.shopOnlineDbContext.Products
                                    .Include(p => p.ProductCategory)
                                    .ToArrayAsync();
            return products;
        }

        public async Task<IEnumerable<Product>> GetItemsByCategory(int id)
        {
            var products = await this.shopOnlineDbContext.Products
                                        .Include(p => p.ProductCategory)
                                        .Where(e => e.CategoryId == id)
                                        .ToListAsync();
            return products;
        }

        public async Task<IEnumerable<Product>> GetItemsByKeywords(string keywords)
        {
            var keywordsArray = keywords.Split();
            var allProducts = await this.shopOnlineDbContext.Products
                                            .Include(p => p.ProductCategory)
                                            .ToListAsync();
            var products = new List<Product>();

            foreach(var product in allProducts)
            {
                foreach(var k in keywordsArray)
                {
                    if(product.Name.Contains(k) || product.Description.Contains(k))
                    {
                        products.Add(product);
                        break;
                    }
                }
            }
            
            
            return products;
        }
    }
}
