using CryptoWallet.Models;
using Microsoft.EntityFrameworkCore;
using CryptoWallet.Migraciones;

namespace CryptoWallet.Repositories.Repos
{
    public class ICreditCardRepository : GenericRepository<CreditCard>
    {

       
        public ICreditCardRepository(DatabaseContext context) : base(context)
        {
           
        }
    }
}
