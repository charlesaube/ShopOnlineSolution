using ShopOnline.Api.Authentication.Interfaces;
using ShopOnline.Api.Entities;
using ShopOnline.Api.Repositories.Interfaces;
using ShopOnline.Models.Dtos.User;

namespace ShopOnline.Api.Services.Authentication
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IUserRepository _userRepository;
        private readonly ICustomPasswordHasher _passwordHasher;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;

        public AuthenticationService(IUserRepository userRepository,ICustomPasswordHasher passwordHasher,IJwtTokenGenerator jwtTokenGenerator)
        {
            this._userRepository = userRepository;
            this._passwordHasher = passwordHasher;
            this._jwtTokenGenerator = jwtTokenGenerator;
        }

        public async Task<AuthenticationResult> Authenticate(UserLoginDto userLoginDto)
        {
            //Check if user exist
            var currentUser = await _userRepository.GetUserByLoginIdentifier(userLoginDto.LoginIdentifier);

            if (currentUser == null)
            {
                throw new Exception("Error while attempting to authenticate - Invalid credentials");
            }

            //Check if password matches
            var (verified, needUpgrade) = _passwordHasher.Check(currentUser.PasswordHash, userLoginDto.Password);
            if (!verified)
            {
                throw new Exception("Error while attempting to authenticate - Invalid credentials");
            }

            //Generate token
            var token = _jwtTokenGenerator.GenerateToken(currentUser);

            return new AuthenticationResult(currentUser, token);



        }

        public async Task<AuthenticationResult> Register(UserRegistrationDto registrationRequest)
        {
            //Check if user already exists
            if(await _userRepository.GetUserByLoginIdentifier(registrationRequest.Email) is not User user)
            {
                throw new Exception("User already exists");
            }
            //Hash password
            var hash = _passwordHasher.Hash(registrationRequest.Password);

            //Create and persist new user
            var newUser = new User
            {
                UserName = registrationRequest.UserName,
                FullName = registrationRequest.FullName,
                Email = registrationRequest.Email,
                PasswordHash = hash,
            };
            newUser = await _userRepository.AddUser(newUser);

            //Create and return token
            var token= _jwtTokenGenerator.GenerateToken(newUser);
            return new AuthenticationResult(newUser, token);
        }

    }
}
