using CryptoWallet.Models;
using CryptoWallet.Enums;
using CryptoWallet.UOWork;
using CryptoWallet.Models.CompleteModel;

namespace CryptoWallet.Services
{
    public class TransactionService
    {
        private readonly IUnitOfWork _uow;
        private readonly CryptoWalletService _cryptoWalletService;
        private readonly WalletService _walletService;

        public TransactionService(IUnitOfWork uow,CryptoWalletService cryptoWalletService, WalletService walletService)
        {
            _uow = uow;
            _cryptoWalletService = cryptoWalletService;
            _walletService = walletService;
        }

        // Kinda pointless, but it's the default i guess.
        public List<Transaction> GetAllTransactionsOfUser(User userOwner)
        {
            List<Transaction> transactionList = (List<Transaction>)_uow.TransactionRepository.GetAll();
            List<Transaction> userTransactions = new List<Transaction>();
            
            foreach (var transaction in transactionList)
            {
                if(transaction.UserID == userOwner.UserID)
                {
                    userTransactions.Add(transaction);
                }
            }

            userTransactions = GetAllTransactionsByTransactionNumber(userTransactions, transactionList);

            return userTransactions;
        }

        // Returns all transactions related to Fiat. Since this transactions lacks a second wallet, the logic is simpler.
        public List<Transaction> GetAllFiatTransactions(User userOwner)
        {
            List<Transaction> transactionList = (List<Transaction>)_uow.TransactionRepository.GetAll();
            List<Transaction> userTransactions = new List<Transaction>();
            foreach (var transaction in transactionList)
            {
                if (transaction.UserID == userOwner.UserID && (transaction.transactionTypes == TransactionTypes.deposit || transaction.transactionTypes == TransactionTypes.extract) )
                {
                    userTransactions.Add(transaction);
                }
            }

           
            return userTransactions;
        }
    
        // The mother of all omelettes. Even if it says it gets the transactions of any CryptoWallets, will crash if used with Fiat wallets.
        // The logic is correct but extremly inneficient. In a potential refactorization, most logic should be replaced
        // by Stored methods of the database. The amount of foreachs is painful.

        // After giving it a user and a type of crypto, returns a list of completedTransactions
        // meaning, it already fused the emissor and receptor in one receipt.

        public List<CompletedTransaction> GetTransactionsOfCryptoWallet(User userOwner, CryptoTypes crypto)
        {
            List<Transaction> transactionList = (List<Transaction>)_uow.TransactionRepository.GetAll();
            List<Transaction> userTransactions = new List<Transaction>();
            List<CompletedTransaction> completedTransactions = new List<CompletedTransaction>();
            foreach (var transaction in transactionList)
            {
                if (transaction.UserID == userOwner.UserID && transaction.cryptoType == crypto)
                {
                    userTransactions.Add(transaction);
                }
            }
            userTransactions = GetAllTransactionsByTransactionNumber(userTransactions, transactionList);

            BuildAllCompletedTransactions(userTransactions);

            return completedTransactions;
        }

        // We need to fetch more transactions than just the ones of the owner, since there are operations
        // related to wallets of other users.
        private List<Transaction> GetAllTransactionsByTransactionNumber(List<Transaction> userTransactions, List<Transaction> allTransactions)
        {
           List<Transaction> filteredTransactions = userTransactions;
            foreach (Transaction transaction in allTransactions)
            {
                foreach(Transaction userTransaction in userTransactions)
                {
                    if (transaction.transactionNumber == userTransaction.transactionNumber &! filteredTransactions.Contains(transaction))
                        {
                            filteredTransactions.Add(transaction);
                        }
                }
            }

            return filteredTransactions;
        }

        // After we get all the transactions related to a wallet, to feed the View with the data ordered
        // we send a list of CompletedTransaction, containing the emmisor and receptor of each transaction.
        public List<CompletedTransaction> BuildAllCompletedTransactions(List<Transaction> userTransactions)
        {
            List<CompletedTransaction> listCompletedTransactions = new List<CompletedTransaction>();
            foreach(Transaction transactionEmmi in userTransactions) if(transactionEmmi.transactionTypes == TransactionTypes.sendEmissor || transactionEmmi.transactionTypes == TransactionTypes.convertEmissor)
            {
                foreach(Transaction transactionRecep in userTransactions) if (transactionRecep.transactionTypes == TransactionTypes.sendReceptor || transactionRecep.transactionTypes == TransactionTypes.convertReceptor)
                        {
                    if(transactionRecep.transactionNumber == transactionEmmi.transactionNumber && transactionRecep.TransactionID != transactionRecep.TransactionID)
                    {
                        if(transactionEmmi.transactionNumber == transactionRecep.transactionNumber)
                                {
                                    CompletedTransaction tempTransaction = BuildCompleteTransaction(transactionEmmi, transactionRecep);
                                    if(!listCompletedTransactions.Contains(tempTransaction) && tempTransaction != null)
                                    {
                                        listCompletedTransactions.Add(tempTransaction);
                                    }
                                }
                    }
                }
            }
            
            
            return listCompletedTransactions;
        }

        // Custom constructor fetching the emitter and the receptor.
        public CompletedTransaction BuildCompleteTransaction(Transaction emitterTransaction, Transaction receptorTransaction)
        {
            if (emitterTransaction.transactionNumber == receptorTransaction.transactionNumber)
            {
                CompletedTransaction completedTransaction = new CompletedTransaction(emitterTransaction, receptorTransaction, _uow);
                return completedTransaction;
            }
            else
                return null;
        }

        public int GenerateCompleteTransactionNumber(int emitterTransaction)
        {
            string emitterTransactionString = emitterTransaction.ToString() + "0";

            return Int16.Parse(emitterTransactionString);
        }


        public void RegisterFiatTransaction(int idUser, TransactionTypes transactionType, float monto)
        {

          
                Wallet tempWallet = _uow.WalletRepository.findId(idUser);
                tempWallet = _walletService.LoadWalletWithCryptoWallets(tempWallet);
            List<Models.CryptoWallet> tempListWallets = (List<Models.CryptoWallet>)_cryptoWalletService.GetCryptoWalletsFromWallet(tempWallet);
            Models.CryptoWallet fiatWallet = tempListWallets[tempListWallets.FindIndex(obj => obj.cryptoType == CryptoTypes.dollar)];
                Transaction newTransaction = new Transaction();
                newTransaction.UserID = idUser;
                newTransaction.cryptoType = CryptoTypes.dollar;
                newTransaction.transactionTypes = transactionType;
                newTransaction.monto = monto;
                
                newTransaction.CryptoWalletID = fiatWallet.CryptoWalletID;
                newTransaction.date = DateTime.Now;

                int transactionNumberGenerated = GenerateCompleteTransactionNumber(newTransaction.TransactionID);

                newTransaction.transactionNumber = transactionNumberGenerated;

                _uow.TransactionRepository.Add(newTransaction);
                _uow.Save();

               
            
        }

        
        public void RegisterCryptoTransaction(int idUserEmitter, TransactionTypes transactionType, float montoEmitted, CryptoTypes montoType, int idUserReceptor, float montoRecieved, CryptoTypes montoRecievedType)
        {
                Wallet tempWallet = _uow.WalletRepository.findId(idUserEmitter);
                tempWallet = _walletService.LoadWalletWithCryptoWallets(tempWallet);
            List<Models.CryptoWallet> tempListWallets = (List<Models.CryptoWallet>)_cryptoWalletService.GetCryptoWalletsFromWallet(tempWallet);
            Models.CryptoWallet emitterWallet = tempListWallets[tempListWallets.FindIndex(obj => obj.cryptoType == montoType)];


                Transaction emitterTransaction = new Transaction();
                emitterTransaction.UserID = idUserEmitter;
                emitterTransaction.cryptoType = emitterWallet.cryptoType;
                // should check this part. I must divide the transaction types.
                emitterTransaction.transactionTypes = transactionType;
                emitterTransaction.monto = montoEmitted;
                // Ah si, transacciones, viniendo a morderme el culo.
               emitterTransaction.CryptoWalletID = emitterWallet.CryptoWalletID;
                emitterTransaction.date = DateTime.Now;
                int transactionNumberGenerated = GenerateCompleteTransactionNumber(emitterTransaction.TransactionID);
                emitterTransaction.transactionNumber = transactionNumberGenerated;

                _uow.TransactionRepository.Add(emitterTransaction);

                // Define the wallet that recieved the transaction.
                Wallet tempWalletReceptor = _uow.WalletRepository.findId(idUserReceptor);
                tempWalletReceptor = _walletService.LoadWalletWithCryptoWallets(tempWalletReceptor);
            List<Models.CryptoWallet> tempListWalletsReceptor = (List<Models.CryptoWallet>)_cryptoWalletService.GetCryptoWalletsFromWallet(tempWalletReceptor);
            Models.CryptoWallet receptorWallet = tempListWallets[tempListWallets.FindIndex(obj => obj.cryptoType == montoRecievedType)];


                Transaction receptorTransaction = new Transaction();
                receptorTransaction.UserID = idUserReceptor;
                receptorTransaction.cryptoType = receptorWallet.cryptoType;
                // Same as the other transaction in constructor.
                receptorTransaction.transactionTypes = transactionType;
                receptorTransaction.monto = montoRecieved;
              
                receptorTransaction.CryptoWalletID = receptorWallet.CryptoWalletID;
                receptorTransaction.date = DateTime.Now;
                receptorTransaction.transactionNumber = transactionNumberGenerated;

                _uow.TransactionRepository.Add(receptorTransaction);


                _uow.Save();

            
        }

    }
}
