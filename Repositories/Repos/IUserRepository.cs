using CryptoWallet.Models;
using Microsoft.EntityFrameworkCore;
using CryptoWallet.Migraciones;

namespace CryptoWallet.Repositories.Repos
{
    public class IUserRepository : GenericRepository<User>
    {


        public IUserRepository(DatabaseContext context) : base(context)
        {

        }
        public User FindByEmail(string mail)
        {
            var comprobacion = dbSet.SingleOrDefault<User>(x => x.mail == mail);
            if (comprobacion == null) return null;
            return comprobacion;
        }
        public User GetByEmail(string mail)
        {
            return dbSet.FirstOrDefault<User>(x => x.mail == mail);
        }
    }
}