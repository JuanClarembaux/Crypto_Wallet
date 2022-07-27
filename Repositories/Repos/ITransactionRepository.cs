using CryptoWallet.Models;
using Microsoft.EntityFrameworkCore;
using CryptoWallet.Migraciones;

namespace CryptoWallet.Repositories.Repos
{
    public class ITransactionRepository : GenericRepository<Transaction>
    {


        public ITransactionRepository(DatabaseContext context) : base(context)
        {

        }
    }
}