using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CryptoWallet.Migraciones;
using CryptoWallet.Models;
using CryptoWallet.UOWork;
using CryptoWallet.Services;
using CryptoWallet.Enums;
using CryptoWallet.Models.CompleteModel;
using CryptoWallet.Services.UserService;

namespace CryptoWallet.Controlers
{/*
    [Route("transactions/[controller]")]
    [ApiController]
    public class TransactionsController : Controller
    {
        private readonly IUserService _userService;
        private readonly IUnitOfWork _context;
        private readonly TransactionService _service;
        private readonly CryptoWalletService _serviceCryptoWallet;
        private readonly WalletService _serviceWallet;
        public TransactionsController(TransactionService service, CryptoWalletService serviceCryptoWallet, IUnitOfWork context, WalletService serviceWallet, IUserService userService)
        {
            _service = service;
            _serviceCryptoWallet = serviceCryptoWallet;
            _serviceWallet = serviceWallet;
            _context = context;
            _userService = userService;
        }

        // GET: Transactions
        // GET: List of Transactions of specific Wallet. The list is not of model 
        // But instead of CompletedModel (containing both emisor and receptor).
        // Cambiar a UserRequest
        [Route("transaction/getfromcryptowallet")]
        [HttpGet]
        public ActionResult<IEnumerable<CompletedTransaction>> GetAllTransactionsByCryptoWallet(int cryptoWalletID)
        {
            List<CompletedTransaction> transactionList = new List<CompletedTransaction>();
            CryptoWallet cryptoWallet = new CryptoWallet();
            cryptoWallet = _context.CryptoWalletRepository.findId(cryptoWalletID);
            if (cryptoWallet != null && _context.TransactionRepository != null)
            {
                if(cryptoWallet.cryptoType != CryptoTypes.dollar)
                {
                    Wallet wallet = new Wallet();
                    wallet = _context.WalletRepository.findId(cryptoWallet.WalletID);
                    User tempUser = new User();
                    tempUser = _context.UserRepository.findId(_userService.GetUserId());
                    transactionList = _service.GetTransactionsOfCryptoWallet(tempUser, cryptoWallet.cryptoType);
                } else
                {
                    return BadRequest();
                }
            }
            else
            {
                return BadRequest();
            }

           

            return Ok(transactionList);
        }

        // GET: Transactions
        // GET: List of Transactions of fiat Wallet. 
        // Cambiar a user request.
        [Route("transaction/getfromfiatwallet")]
        [HttpGet]
        public ActionResult<IEnumerable<Transaction>> GetAllFiatTransactionsOfWallet(int walletID)
        {
            List<Transaction> fiatTransactionList = new List<Transaction>();
            Wallet wallet = _context.WalletRepository.findId(walletID);
            wallet = _serviceWallet.LoadWalletWithCryptoWallets(wallet);
            CryptoWallet cryptoWallet = new CryptoWallet(); 
            foreach (CryptoWallet cryptoW in wallet.cryptoWallets)
            {
                if( cryptoW.cryptoType == CryptoTypes.dollar)
                {
                    cryptoWallet = cryptoW;
                }
            }
            if ( cryptoWallet != null && _context.TransactionRepository != null)
            {
                fiatTransactionList = _service.GetAllFiatTransactions(_context.UserRepository.findId(wallet.UserID));
            }
            else
            {
                return BadRequest();
            }


            return Ok(fiatTransactionList);
        }



    }*/
}
