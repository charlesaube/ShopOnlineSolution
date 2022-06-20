using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShopOnline.Api.Extensions;
using ShopOnline.Api.Services.Products;
using ShopOnline.Models.Dtos;

namespace ShopOnline.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            this._productService = productService;
        }

        [HttpGet("{id:int}"),AllowAnonymous]
        public async Task<ActionResult<ProductDto>> GetItem(int id)
        {
            try
            {
                var product = await this._productService.GetItem(id);             

                if (product == null )
                {
                    return NotFound();
                }
                else
                {
                    var productDto = product.ConvertToDto();
                    return Ok(productDto);
                }
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error retrieving data from the database");
            }
        }

        [HttpGet, AllowAnonymous]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetItems()
        {
            try
            {
                var products = await this._productService.GetItems();

                if (products == null)
                {
                    return NotFound();
                }
                else
                {
                    var productDto = products.ConvertToDto();
                    return Ok(productDto);
                }
            }
            catch (Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error retrieving data from the database");
            }
        }

        [HttpGet("Categories"), AllowAnonymous]
        public async Task<ActionResult<IEnumerable<ProductCategoryDto>>> GetProductCategories()
        {
            try
            {
                var productCategories = await _productService.GetCategories();
                var productCategoriesDto = productCategories.ConvertToDto();
                return Ok(productCategoriesDto);
            }
            catch (Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error retrieving data from the database");
            }
        }

        [HttpGet, AllowAnonymous]
        [Route("ByCategory/{categoryId}")]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetItemsByCategory(int categoryId)
        {
            try
            {
                var products = await _productService.GetItemsByCategory(categoryId);
                var productsDto = products.ConvertToDto();
                return Ok(productsDto);
            }
            catch (Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error retrieving data from the database");
            }
        }

        [HttpGet, AllowAnonymous]
        [Route("ByKeyword/{keywords}")]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetItemsByKeyword(string keywords)
        {
            try
            {
                var products = await _productService.GetItemsByKeywords(keywords);

                if (!products.Any())
                {
                    return NotFound("No product found.");
                }
                else
                {
                    var productsDto = products.ConvertToDto();
                    return Ok(productsDto);
                }
            }
            catch (Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error retrieving data from the database");
            }
        }

        [Authorize(Roles = "Admin,Seller")]
        [HttpPost]
        [Route("Create")]
        public async Task<ActionResult<ProductDto>> CreateItem([FromBody]ProductDto productDto)
        {
            try
            {
                if (productDto != null)
                {
                    var product = productDto.ConvertFromDto();
                
                    var createdProduct = await _productService.CreateItem(product);
                    var newProductDto = createdProduct.ConvertToDto();
                    return CreatedAtAction(nameof(CreateItem), new { id = createdProduct.Id }, newProductDto);
                }
                return BadRequest("Invalid information provided.");
            }
            catch (Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error creating data in the database");
            }
        }
    }
}
