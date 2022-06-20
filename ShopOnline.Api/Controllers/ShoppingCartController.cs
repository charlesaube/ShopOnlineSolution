using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShopOnline.Api.Extensions;
using ShopOnline.Api.Services.Products;
using ShopOnline.Api.Services.ShoppingCart;
using ShopOnline.Models.Dtos;
using System.Security.Claims;

namespace ShopOnline.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ShoppingCartController : ControllerBase
    {
        private readonly IShoppingCartService _shoppingCartService;
        private readonly IProductService _productService;
    

        public ShoppingCartController(IShoppingCartService shoppingCartService,IProductService productService)
        {
            this._shoppingCartService = shoppingCartService;
            this._productService = productService;
        }

        [HttpGet]
        [Route("{userId}/Items")]
        public async Task<ActionResult<IEnumerable<CartItemDto>>> GetItems(int userId)
        {
            try
            {  
                if(CheckUserIdentity(HttpContext,userId))
                {   
                    var cartItems = await _shoppingCartService.GetItems(userId);
                    if(cartItems == null)
                    {
                        return NoContent();
                    }
                    var products = await _productService.GetItems();
                    if (products == null)
                    {
                        throw new Exception("No products exist in the system");
                    }
                    var cartItemsDto = cartItems.ConvertToDto(products);

                    return Ok(cartItemsDto);
                }
                return Unauthorized("User unauthorized to access the specified data");
            }
            catch (Exception ex)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        private bool CheckUserIdentity(HttpContext httpContext, int userId)
        {
            var identity = httpContext.User.Identity as ClaimsIdentity;
            if (identity != null)
            {
                var userIdClaim = identity.Claims.FirstOrDefault(e => e.Type == "UserId");
                if (userIdClaim.Value == userId.ToString())
                {
                    return true;
                }
            }
            return false;
        }

        [HttpGet("{userId}/Item/{id:int}")]
        public async Task<ActionResult<CartItemDto>> GetItem(int userId,int id)
        {
            try
            {
                if(!CheckUserIdentity(HttpContext, userId))
                {
                    return Unauthorized("User unauthorized to access the specified data");
                }
                var cartItem = await _shoppingCartService.GetItem(id);
                if (cartItem == null)
                {
                    return NotFound();
                }
                var product =await _productService.GetItem(cartItem.ProductId);
                if (product == null)
                {
                    return NotFound();
                }
                var cartItemDto = cartItem.ConvertToDto(product);
                return Ok(cartItemDto);
            }
            catch (Exception ex)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost("{userId}/Item")]
        public async Task<ActionResult<CartItemDto>> PostItem(int userId,[FromBody]CartItemToAddDto cartItemToAddDto)
        {
            try
            {
                if (!CheckUserIdentity(HttpContext, userId))
                {
                    return Unauthorized("User unauthorize");
                }
                var newCartItem = await _shoppingCartService.AddItem(cartItemToAddDto);

                if(newCartItem == null)
                {
                    return NoContent();
                }
                var product = await _productService.GetItem(newCartItem.ProductId);
                if (product == null)
                {
                    throw new Exception($"Something went wrong when retrieving product (productId:{cartItemToAddDto.ProductId})");
                }
                var newCartItemDto = newCartItem.ConvertToDto(product);
                return CreatedAtAction(nameof(GetItem), new {userId = userId,id=newCartItemDto.Id},newCartItemDto);
            }
            catch (Exception ex)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpDelete("{userId}/Item/{id:int}")]
        public async Task<ActionResult<CartItemDto>> DeleteItem(int userId,int id)
        {
            try
            {
                if (!CheckUserIdentity(HttpContext, userId))
                {
                    return Unauthorized("User unauthorize");
                }
                var deletedCartItem = await _shoppingCartService.DeleteItem(id);
                if (deletedCartItem == null)
                {
                    return NotFound();
                }
                var product = await _productService.GetItem(deletedCartItem.ProductId);
                if (product == null)
                {
                    return NotFound();
                }
                var deletedCartItemDto = deletedCartItem.ConvertToDto(product);
                return Ok(deletedCartItemDto);
            }
            catch (Exception ex)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPatch("{userId}/Item/{id:int}")]
        public async Task<ActionResult<CartItemDto>> UpdateCartItemQty(int userId,int id,[FromBody] CartItemQtyUpdateDto cartItemQtyUpdate)
        {
            try
            {
                if (!CheckUserIdentity(HttpContext, userId))
                {
                    return Unauthorized("User unauthorize");
                }
                var updatedCartItem = await _shoppingCartService.UpdateQty(id, cartItemQtyUpdate);
                if(updatedCartItem == null)
                {
                    return NotFound();
                }
                var product = await _productService.GetItem(updatedCartItem.ProductId);
                if(product == null)
                {
                    return NotFound();
                }
                var updatedCartItemDto = updatedCartItem.ConvertToDto(product);
                return Ok(updatedCartItemDto);

            }
            catch (Exception ex)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
