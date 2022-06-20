namespace ShopOnline.Api.Authentication.Interfaces
{
    public interface ICustomPasswordHasher
    {
        public (bool Verified, bool NeedsUpgrade) Check(string hash, string password);

        public string Hash(string password);
    }
}
