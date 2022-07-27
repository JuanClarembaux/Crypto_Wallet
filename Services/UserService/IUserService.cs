namespace CryptoWallet.Services.UserService
{
    public interface IUserService
    {
        string GetMyName();
        void ExpiresToken();
        int GetUserId();
    }
}
