using ShopOnline.Api.Entities;
using ShopOnline.Api.Services.Authentication;
using ShopOnline.Models.Dtos;
using ShopOnline.Models.Dtos.User;

namespace ShopOnline.Api.Extensions
{
    public static class DtoConversions
    {
        public static IEnumerable<ProductCategoryDto> ConvertToDto(this IEnumerable<ProductCategory> productCategories)
        {
            return (from productCategory in productCategories
                    select new ProductCategoryDto
                    {
                        Id = productCategory.Id,
                        Name = productCategory.Name,
                        IconCSS = productCategory.IconCSS
                    }).ToList();
        }

        public static IEnumerable<ProductDto> ConvertToDto(this IEnumerable<Product> products)
        {
            return (from product in products                 
                    select new ProductDto
                    {
                        Id = product.Id,
                        Name = product.Name,
                        Description = product.Description,
                        ImageURL = product.ImageURL,
                        Price = product.Price,
                        Qty = product.Qty,
                        CategoryId = product.ProductCategory.Id,
                        CategoryName = product.ProductCategory.Name
                    }).ToList();
        }

        public static ProductDto ConvertToDto(this Product product)
        {
            return new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                ImageURL = product.ImageURL,
                Price = product.Price,
                Qty = product.Qty,
                CategoryId = product.ProductCategory.Id,
                CategoryName = product.ProductCategory.Name
            };
        }

        public static Product ConvertFromDto(this ProductDto productDto)
        {
            return new Product
            {
                Id = 0,
                Name = productDto.Name,
                Description = productDto.Description,
                ImageURL = productDto.ImageURL,
                Price = productDto.Price,
                Qty = productDto.Qty,
                CategoryId = productDto.CategoryId,
            };
        }


        public static IEnumerable<CartItemDto> ConvertToDto(this IEnumerable<CartItem> cartItems, IEnumerable<Product> products)
        {
            return (from cartItem in cartItems
                    join product in products
                    on cartItem.ProductId equals product.Id
                    select new CartItemDto
                    {
                        Id = cartItem.Id,
                        CartId = cartItem.CartId,
                        ProductId = cartItem.ProductId,
                        ProductName = product.Name,
                        ProductDescription = product.Description,
                        ProductImageURL = product.ImageURL,
                        Price= product.Price,
                        TotalPrice = product.Price * cartItem.Qty,
                        Qty = cartItem.Qty,
                    }).ToList();
        }

        public static CartItemDto ConvertToDto(this CartItem cartItem,Product product)
        {
            return new CartItemDto
            {
                Id = cartItem.Id,
                CartId = cartItem.CartId,
                ProductId = cartItem.ProductId,
                ProductName = product.Name,
                ProductDescription = product.Description,
                ProductImageURL = product.ImageURL,
                Price = product.Price,
                TotalPrice = product.Price * cartItem.Qty,
                Qty = cartItem.Qty
            };
        }

        public static UserResponseDto ConvertToDto(this AuthenticationResult result,int cartId)
        {
            return new UserResponseDto
            (
               result.User.Id,
               result.User.UserName,
               result.User.FullName,
               result.User.Email,
               cartId,
               result.Token
            );
        }
    }
}
