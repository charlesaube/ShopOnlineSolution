using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShopOnline.Api.Extensions;
using ShopOnline.Api.Services.Interfaces;
using ShopOnline.Models.Dtos.User;


namespace ShopOnline.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserService userService;
        private readonly IShoppingCartService shoppingCartService;
        private readonly ICustomPasswordHasher passwordHasher;
        private readonly IConfiguration config;

        public AuthController(IUserService userService,IShoppingCartService shoppingCartService,ICustomPasswordHasher passwordHasher,IConfiguration config)
        {
            this.userService = userService;
            this.shoppingCartService = shoppingCartService;
            this.passwordHasher = passwordHasher;
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
                    var newUser = await userService.Register(registrationRequest);
                    if(newUser != null)
                    {
                        return CreatedAtAction(nameof(Register), new { id = newUser.Id }, "Account successfully created.");
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
        public async Task<ActionResult<UserDto>> Login([FromBody] UserLoginDto userLoginDto)
        {
            try
            {

                var currentUser = await userService.Authenticate(userLoginDto);
                if(currentUser != null)
                {
                    var cartId = await shoppingCartService.GetCartId(currentUser.Id);
                    var token = userService.GenerateToken(currentUser);
                    var userLoginResponse = currentUser.ConvertToDto(cartId, token);
                    return Ok(userLoginResponse);
                }

                return BadRequest("Incorrect login.");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }


        /*
        [AllowAnonymous]
        [HttpPost("authenticate")]
        public IActionResult Authenticate(UserLoginDto userLoginDto)
        {
            var response = _userService.Authenticate(model, ipAddress());
            setTokenCookie(response.RefreshToken);
            return Ok(response);
        }

        [AllowAnonymous]
        [HttpPost("refresh-token")]
        public IActionResult RefreshToken()
        {
            var refreshToken = Request.Cookies["refreshToken"];
            var response = _userService.RefreshToken(refreshToken, ipAddress());
            setTokenCookie(response.RefreshToken);
            return Ok(response);
        }

        [HttpPost("revoke-token")]
        public IActionResult RevokeToken(RevokeTokenRequest model)
        {
            // accept refresh token in request body or cookie
            var token = model.Token ?? Request.Cookies["refreshToken"];

            if (string.IsNullOrEmpty(token))
                return BadRequest(new { message = "Token is required" });

            _userService.RevokeToken(token, ipAddress());
            return Ok(new { message = "Token revoked" });
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var users = _userService.GetAll();
            return Ok(users);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var user = _userService.GetById(id);
            return Ok(user);
        }

        [HttpGet("{id}/refresh-tokens")]
        public IActionResult GetRefreshTokens(int id)
        {
            var user = _userService.GetById(id);
            return Ok(user.RefreshTokens);
        }

        // helper methods

        private void setTokenCookie(string token)
        {
            // append cookie with refresh token to the http response
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTime.UtcNow.AddDays(7)
            };
            Response.Cookies.Append("refreshToken", token, cookieOptions);
        }

        private string ipAddress()
        {
            // get source ip address for the current request
            if (Request.Headers.ContainsKey("X-Forwarded-For"))
                return Request.Headers["X-Forwarded-For"];
            else
                return HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
        }*/
    }

}
