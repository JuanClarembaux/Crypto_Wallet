using CryptoWallet.Repositories.Repos;

namespace CryptoWallet.UOWork
{
    public interface IUnitOfWork : IDisposable
    {
        public ICreditCardRepository CreditCardRepository { get; }
        public ICryptoWalletRepository CryptoWalletRepository { get; }
        public ITransactionRepository TransactionRepository { get; }
        public IUserRepository UserRepository { get; }
        public IWalletRepository WalletRepository { get; }

        void Save();        
    }
}
