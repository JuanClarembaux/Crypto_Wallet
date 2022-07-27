using CryptoWallet.Migraciones;
using CryptoWallet.Repositories.Repos;
namespace CryptoWallet.UOWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DatabaseContext context;
        public ICreditCardRepository CreditCardRepository { get; }
        public ICryptoWalletRepository CryptoWalletRepository { get; }
        public ITransactionRepository TransactionRepository { get; }
        public IUserRepository UserRepository { get; }
        public IWalletRepository WalletRepository { get; }

        public UnitOfWork(DatabaseContext context)
        {
            this.context = context;
            CreditCardRepository = new ICreditCardRepository(context);
            CryptoWalletRepository = new ICryptoWalletRepository(context);
            TransactionRepository = new ITransactionRepository(context);
            UserRepository = new IUserRepository(context);
            WalletRepository = new IWalletRepository(context);
        }

        public void Dispose()
        {
            context.Dispose();
        }

        public void Save()
        {
            context.SaveChanges();
        }
    }
}
