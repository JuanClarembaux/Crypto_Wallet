using CryptoWallet.Models;
using CryptoWallet.Enums;
using CryptoWallet.UOWork;
using CryptoWallet.Models.CompleteModel;
using CryptoWallet.Models.Request;
using CryptoWallet.Services.UserService;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using System.Linq;

namespace CryptoWallet.Services
{
    public class WalletService
    {
        private readonly IUnitOfWork _uow;
        
        public WalletService(IUnitOfWork uow)
        {
            _uow = uow;            
        }

        public Wallet GetWalletByUserID(int id)
        {
            List<Wallet> allWallets = new List<Wallet>();
             allWallets = (List<Wallet>)_uow.WalletRepository.GetAll();
            Wallet walletFinded = new Wallet();
            foreach(Wallet wallet in allWallets)
            {
                if(wallet.UserID == id)
                {
                    walletFinded = wallet;
                }
            }            
            if(walletFinded.UserID == null)
            {
                return null;
            }

            return walletFinded;            
        }

        public bool FindIfCryptoWalletType(Wallet wallet, CryptoTypes cryptoRequest)
        {
            bool isTypeAlready = false;
            foreach(Models.CryptoWallet cryptowallet in wallet.cryptoWallets)
            {
                if(cryptowallet.cryptoType == cryptoRequest)
                {
                    isTypeAlready = true;
                }
            }

            return isTypeAlready;
        }

        public Wallet LoadWalletWithCryptoWallets(Wallet wallet)
        {

            List<Models.CryptoWallet> allCryptoWallets = (List<Models.CryptoWallet>)_uow.CryptoWalletRepository.GetAll();
            foreach (Models.CryptoWallet cryptowallet in allCryptoWallets)
            {
                if (cryptowallet.WalletID == wallet.WalletID)
                {
                    wallet.cryptoWallets.Add(cryptowallet);
                }
            }

            return wallet;
        }
    }
}
