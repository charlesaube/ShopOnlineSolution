using ShopOnline.Models.Dtos;

namespace ShopOnline.Web.Services.Interfaces
{
    public interface IManageProductsLocalStorageService
    {
        Task<IEnumerable<ProductDto>> GetCollection();
        Task RemoveCollection();
    }
}
