using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShopOnline.Api.Extensions;
using ShopOnline.Api.Services.Authentication;
using ShopOnline.Api.Services.ShoppingCart;
using ShopOnline.Models.Dtos.User;


namespace ShopOnline.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly IShoppingCartService _shoppingCartService;
        private readonly IConfiguration config;

        public AuthController(IAuthenticationService authenticationService,IShoppingCartService shoppingCartService,IConfiguration config)
        {
            this._authenticationService = authenticationService;
            this._shoppingCartService = shoppingCartService;
            this.config = config;
        }
        
        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<ActionResult<string>> Register([FromBody]UserRegistrationDto registrationRequest)
        {
            try
            {
                if(registrationRequest != null)
                {
                    var newUser = await _authenticationService.Register(registrationRequest);
                    if(newUser != null)
                    {
                        return CreatedAtAction(nameof(Register), new { id = newUser.User.Id }, "Account successfully created.");
                    }             
                } 
                return BadRequest("Email/Username unavailable.");
            }
            catch (Exception ex)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult<UserResponseDto>> Login([FromBody] UserLoginDto userLoginDto)
        {
            try
            {

                var authResult = await _authenticationService.Authenticate(userLoginDto);
                if(authResult.User != null)
                {
                    var cartId = await _shoppingCartService.GetCartId(authResult.User.Id);
                    var userLoginResponse = authResult.ConvertToDto(cartId);
                    return Ok(userLoginResponse);
                }

                return BadRequest("Incorrect login.");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }

}
