using CryptoWallet.Models;
using Microsoft.EntityFrameworkCore;
using CryptoWallet.Migraciones;

namespace CryptoWallet.Repositories.Repos
{
    public class ICryptoWalletRepository : GenericRepository<Models.CryptoWallet>
    {


        public ICryptoWalletRepository(DatabaseContext context) : base(context)
        { 
        
        }
      
    }
    
}