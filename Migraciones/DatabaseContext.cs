using CryptoWallet.Models;
using Microsoft.EntityFrameworkCore;

namespace CryptoWallet.Migraciones
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {

        }

        public DbSet<CreditCard> creditCards { get; set; }
        public DbSet<Models.CryptoWallet> cryptoWallets { get; set; }
        public DbSet<Transaction> transactions { get; set; }
        public DbSet<Wallet> wallets { get; set; }
        public DbSet<User> users { get; set; }
    }
}
