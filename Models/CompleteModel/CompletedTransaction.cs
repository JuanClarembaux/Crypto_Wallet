using CryptoWallet.Models;
using CryptoWallet.Enums;
namespace CryptoWallet.Models.CompleteModel
{
    public class CompletedTransaction
    {   
        public Transaction emissorUser;
        public Transaction receptorUser;
        public int transactionNumber;
        public TransactionTypes transactionTypes;
        public CryptoTypes emittedCrypto;
        public CryptoTypes recivedCrypto;
        public CryptoWallet emissorCryptoWallet;
        public CryptoWallet receptorCryptoWallet;
        public DateTime dateTime;

        public CompletedTransaction(Transaction emitterTransaction, Transaction receptorTransaction, IUnitOfWork context)
        { 
            this.emissorUser = emitterTransaction;
            this.receptorUser = receptorTransaction;
            this.transactionNumber = emitterTransaction.transactionNumber;
            this.transactionTypes = emitterTransaction.transactionTypes;
            this.emittedCrypto = emitterTransaction.cryptoType;
            this.recivedCrypto = receptorTransaction.cryptoType;
            this.emissorCryptoWallet = context.CryptoWalletRepository.findId(emitterTransaction.CryptoWalletID);
            this.receptorCryptoWallet = context.CryptoWalletRepository.findId(receptorTransaction.CryptoWalletID);
            this.dateTime = emitterTransaction.Date;
         }
    }
}
