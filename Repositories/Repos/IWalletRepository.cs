using CryptoWallet.Models;
using Microsoft.EntityFrameworkCore;
using CryptoWallet.Migraciones;

namespace CryptoWallet.Repositories.Repos
{
    public class IWalletRepository : GenericRepository<Wallet>
    {


        public IWalletRepository(DatabaseContext context) : base(context)
        {

        }
    }
}