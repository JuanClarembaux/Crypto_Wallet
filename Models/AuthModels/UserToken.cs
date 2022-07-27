namespace CryptoWallet.Models.AuthModels
{
    public class UserToken
    {
        public string Username { get; set; } = string.Empty;
        public DateTime TokenCreated { get; set; }
        public DateTime TokenExpires { get; set; }
    }
}
