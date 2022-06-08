namespace ShopOnline.Api.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
      
        public string Role { get; set; }   //User role: Customer / Seller / Admin

        // public List<RefreshToken> RefreshTokens { get; set; }

    }
}
