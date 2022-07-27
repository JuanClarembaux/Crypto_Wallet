using CryptoWallet.Repositories.Repos;
using CryptoWallet.UOWork;
using CryptoWallet.Models;
using Microsoft.AspNetCore.Mvc;
using CryptoWallet.Enums;

namespace CryptoWallet.Services
{
    
    public class CryptoWalletService
    {
        private readonly IUnitOfWork _uow;
       
        public CryptoWalletService(IUnitOfWork uow)
        {
            _uow = uow;           
        }

        
        // Pocas veces codie algo tan bestia como esto. Hay que refinar agregando un metodo en la misma 
        // base de datos y despues llamar a ese metodo y punto. 
        public IEnumerable<Models.CryptoWallet> GetCryptoWalletsFromWallet(Wallet walletOwner)
        {
            List<Models.CryptoWallet> listCW = (List<Models.CryptoWallet>)_uow.CryptoWalletRepository.GetAll();
            foreach(Models.CryptoWallet cryptoWallet in listCW.ToList())
            {
                if(cryptoWallet.WalletID != walletOwner.WalletID)
                {
                    listCW.Remove(cryptoWallet);
                }
            }
            return listCW;
        }

        public int ModifyFundsCryptoWallet(Models.CryptoWallet cryptoWallet, int amount)
        {
            int newQuantity = cryptoWallet.quantity + amount;
            return newQuantity;
        }

        public float MaxFundsAmountCryptoWallet(Models.CryptoWallet cryptoWallet)
        {
            return cryptoWallet.quantity;
        }

        // ando confundido, no estoy seguro de que sean asi las cuentas.
        public int EquivalentDollarValue(int amount, int cryptoValue)
        {
           
            int equivalentInDollars = amount * cryptoValue;
            return equivalentInDollars;
        }

        public int EquivalentCryptoValue(int dollars,  int cryptoValue)
        {
            int amount =  dollars / cryptoValue ;
            return amount;
        }
    }
}
