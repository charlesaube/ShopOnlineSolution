using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShopOnline.Api.Extensions;
using ShopOnline.Api.Repositories.Interfaces;
using ShopOnline.Api.Services.Interfaces;
using ShopOnline.Models.Dtos;

namespace ShopOnline.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class ProductController : ControllerBase
    {
        private readonly IProductService productService;

        public ProductController(IProductService productService)
        {
            this.productService = productService;
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<ProductDto>> GetItem(int id)
        {
            try
            {
                var product = await this.productService.GetItem(id);             

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

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetItems()
        {
            try
            {
                var products = await this.productService.GetItems();

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

        [HttpGet("Categories")]
        public async Task<ActionResult<IEnumerable<ProductCategoryDto>>> GetProductCategories()
        {
            try
            {
                var productCategories = await productService.GetCategories();
                var productCategoriesDto = productCategories.ConvertToDto();
                return Ok(productCategoriesDto);
            }
            catch (Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error retrieving data from the database");
            }
        }

        [HttpGet]
        [Route("ByCategory/{categoryId}")]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetItemsByCategory(int categoryId)
        {
            try
            {
                var products = await productService.GetItemsByCategory(categoryId);
                var productsDto = products.ConvertToDto();
                return Ok(productsDto);
            }
            catch (Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error retrieving data from the database");
            }
        }

        [HttpGet]
        [Route("ByKeyword/{keywords}")]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetItemsByKeyword(string keywords)
        {
            try
            {
                var products = await productService.GetItemsByKeywords(keywords);

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

        [HttpPost]
        [Route("Create")]
        public async Task<ActionResult<ProductDto>> CreateItem([FromBody]ProductDto productDto)
        {
            try
            {
                if (productDto != null)
                {
                    var product = productDto.ConvertFromDto();
                
                    var createdProduct = await productService.CreateItem(product);
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
