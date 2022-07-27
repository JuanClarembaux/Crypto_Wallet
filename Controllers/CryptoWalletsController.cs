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
using CryptoWallet.Models.Request;
using CryptoWallet.Services;
using CryptoWallet.Enums;
using CryptoWallet.Services.UserService;
using CryptoWallet.Models.CreditCardModel;
using Microsoft.AspNetCore.Authorization;

namespace CryptoWallet.Controlers
{
    [Route("cryptowallets/[controller]")]
    [ApiController]
    public class CryptoWalletsController : Controller
    {
        private readonly IUserService _userService;
        private readonly IUnitOfWork _context;
        private readonly CryptoWalletService _service;
        private readonly WalletService _serviceWallet;
        private readonly TransactionService transactionService;
        public CryptoWalletsController(CryptoWalletService service, IUnitOfWork context, WalletService serviceWallet, TransactionService transactionController, IUserService userService)
        {
            _userService = userService;
            _service = service;
            _context = context;
            _serviceWallet = serviceWallet;
            this.transactionService = transactionController;
        }

        // GET: List of CryptoWallets of specific Wallet.
        
        [HttpPost("getcryptoswallet")]
        [Authorize]
        public ActionResult<IEnumerable<Models.CryptoWallet>> GetAllCryptoWalletsByWallet(WalletRequest walletOwner)
        {
            Wallet wallet = new Wallet();
            wallet.UserID = walletOwner.userID;
            wallet = _serviceWallet.GetWalletByUserID(walletOwner.userID);
            wallet = _serviceWallet.LoadWalletWithCryptoWallets(wallet);
            List<Models.CryptoWallet> cryptoWalletList = new List<Models.CryptoWallet>(); 
            cryptoWalletList = (List<Models.CryptoWallet>)_service.GetCryptoWalletsFromWallet(wallet);

            return Ok(cryptoWalletList);
        }

        [HttpGet("cryptowalletbyid")]
        [Authorize]
        public ActionResult<Models.CryptoWallet> GetCryptoWalletByID(int id)
        {
            if (_context.CryptoWalletRepository.findId(id) == null)
            {
                return NotFound();
            }

            var cryptoWallet = _context.CryptoWalletRepository.findId(id);

            if (cryptoWallet == null)
            {
                return NotFound();
            }

            return Ok(cryptoWallet);
        }

        // GET: Wallets/Create  Create a CryptoWallet.
        [HttpPost("createcryptowallet")]
        [Authorize]
        public async Task<ActionResult<CryptoWalletRequest>> CreateCryptoWallet(CryptoWalletRequest cryptoWalletRequested)
        {
            Models.CryptoWallet newCryptoWallet = new Models.CryptoWallet();
            newCryptoWallet.cryptoType = cryptoWalletRequested.cryptoType;

            newCryptoWallet.WalletID = _userService.GetUserId();

            _context.CryptoWalletRepository.Add(newCryptoWallet);

            _context.Save();
            return Ok(newCryptoWallet);
        }

        [HttpPut("addfiatfunds")]
        [Authorize]
        public ActionResult<Wallet> AddFiatFundsCryptoWallet(int cryptoWalletID, int amount)
        {
            Models.CryptoWallet cryptoWallet = new Models.CryptoWallet();

            cryptoWallet = _context.CryptoWalletRepository.findId(cryptoWalletID);
            int check = cryptoWallet.quantity;

            if (cryptoWallet.cryptoType != CryptoTypes.dollar) return BadRequest("Mistake wallet type");

            if (check + amount < check) return BadRequest("Wrong operation");

            cryptoWallet.quantity = _service.ModifyFundsCryptoWallet(cryptoWallet, amount);
            _context.CryptoWalletRepository.Update(cryptoWallet);
            _context.Save();
            
            return Ok(cryptoWallet);
        }

        
        [HttpGet("extractfiatfunds")]
        [Authorize]
        public ActionResult<Wallet> ExtractFiatFundsCryptoWallet(int cryptoWalletID, int amount)
        {
            Models.CryptoWallet cryptoWallet = new Models.CryptoWallet();
            cryptoWallet = _context.CryptoWalletRepository.findId(cryptoWalletID);
            int check = cryptoWallet.quantity;

            if (cryptoWallet.cryptoType != CryptoTypes.dollar) return BadRequest("Mistake wallet type");

            if (check - amount < 0) return BadRequest("Insuficient funds");

            amount = amount * -1;
            cryptoWallet.quantity = _service.ModifyFundsCryptoWallet(cryptoWallet, amount);            
            _context.CryptoWalletRepository.Update(cryptoWallet);            
            _context.Save();

            return Ok(cryptoWallet);
        }
        
        [HttpPut("addcryptofunds")]
        [Authorize]
        public ActionResult<Wallet> AddCryptoFundsCryptoWallet(int cryptoWalletID, int amount)
        {
            Models.CryptoWallet cryptoWallet = new Models.CryptoWallet();

            cryptoWallet = _context.CryptoWalletRepository.findId(cryptoWalletID);
            int check = cryptoWallet.quantity;

            if (cryptoWallet.cryptoType == CryptoTypes.dollar) return BadRequest("Mistake wallet type");

            if (check + amount < check) return BadRequest("Wrong operation");

            cryptoWallet.quantity = _service.ModifyFundsCryptoWallet(cryptoWallet, amount);
            _context.CryptoWalletRepository.Update(cryptoWallet);
            _context.Save();
            
            return Ok(cryptoWallet);
        }

        
        [HttpGet("extractcryptofunds")]
        [Authorize]
        public ActionResult<Wallet> ExtractCryptoFundsCryptoWallet(int cryptoWalletID, int amount)
        {
            Models.CryptoWallet cryptoWallet = new Models.CryptoWallet();
            cryptoWallet = _context.CryptoWalletRepository.findId(cryptoWalletID);

            int check = cryptoWallet.quantity;

            if (cryptoWallet.cryptoType == CryptoTypes.dollar) return BadRequest("Mistake wallet type");

            if (check - amount < 0) return BadRequest("Insuficient funds");

            amount = amount * -1;
            cryptoWallet.quantity = _service.ModifyFundsCryptoWallet(cryptoWallet, amount);            
            _context.CryptoWalletRepository.Update(cryptoWallet);            
            _context.Save();

            return Ok(cryptoWallet);
        }

        // Add two values, for the values of both cryptos.
        /*
        [HttpGet("exchangecrypto")]
        public ActionResult<Wallet> ExchangeCryptoCurrency(int cryptoWalletEmitterID, int amount, int valueCryptoEmitter, CryptoTypes cryptoTypeReceptor, int valueCryptoReceptor)
        {
            CryptoWallet cryptoWalletEmitter = new CryptoWallet();
            cryptoWalletEmitter = _context.CryptoWalletRepository.findId(cryptoWalletEmitterID);

            int check = cryptoWalletEmitter.quantity;
            if(cryptoWalletEmitter == null)
            {
                return BadRequest("Crypto wallet not found");
            }
            else
            {

            
                if (check - amount < 0f)
                {
                    return BadRequest("Insufficient funds");
                }
                else
                {
                    int dollarEquivalent = _service.EquivalentDollarValue(amount, valueCryptoEmitter);
                    cryptoWalletEmitter.quantity = cryptoWalletEmitter.quantity - amount;
                    CryptoWallet cryptoWalletReceptor = new CryptoWallet();
                    Wallet wallet = new Wallet();
                    wallet = _context.WalletRepository.findId(cryptoWalletEmitter.WalletID);
                    wallet = _serviceWallet.LoadWalletWithCryptoWallets(wallet);
                    if(_serviceWallet.FindIfCryptoWalletType(wallet, cryptoTypeReceptor))
                    {
                       foreach(CryptoWallet cwallet in wallet.cryptoWallets)
                        {
                            if(cwallet.cryptoType == cryptoTypeReceptor)
                            {
                                cryptoWalletReceptor = cwallet;
                            }
                        }
                    } else
                    {
                            cryptoWalletReceptor.WalletID = wallet.WalletID;
                            cryptoWalletReceptor.quantity = 0;
                            cryptoWalletReceptor.cryptoType = cryptoTypeReceptor;
                        _context.CryptoWalletRepository.Add(cryptoWalletReceptor);
                    }
                    cryptoWalletReceptor.quantity = cryptoWalletReceptor.quantity + _service.EquivalentCryptoValue(dollarEquivalent, valueCryptoReceptor);
                    _context.CryptoWalletRepository.Update(cryptoWalletEmitter);
                    _context.CryptoWalletRepository.Update(cryptoWalletReceptor);
                    
                    transactionService.RegisterCryptoTransaction(wallet.UserID, TransactionTypes.convertEmissor, amount, cryptoWalletEmitter.cryptoType, wallet.UserID, _service.EquivalentCryptoValue(dollarEquivalent, valueCryptoEmitter), cryptoWalletReceptor.cryptoType);
                    

                    _context.Save();

                }
            }
            return Ok(cryptoWalletEmitter);
        }

        
        [HttpGet("sendcrypto")]
        public ActionResult<Wallet> SendCryptoCurrency(int cryptoWalletEmitterID, int amount, int userIDReceptor)
        {
            CryptoWallet cryptoWalletEmitter = new CryptoWallet();
            cryptoWalletEmitter = _context.CryptoWalletRepository.findId(cryptoWalletEmitterID);

            int check = cryptoWalletEmitter.quantity;
            if (cryptoWalletEmitter == null)
            {
                return BadRequest("Crypto wallet not found");
            }
            else
            {


                if (check - amount < 0f)
                {
                    return BadRequest("Insufficient funds");
                }
                else
                {
                    cryptoWalletEmitter.quantity = cryptoWalletEmitter.quantity - amount;

                    Wallet tempWalletEmitter = new Wallet();
                    tempWalletEmitter = _context.WalletRepository.findId(cryptoWalletEmitter.WalletID);
                    tempWalletEmitter = _serviceWallet.LoadWalletWithCryptoWallets(tempWalletEmitter);
                    User tempUserReceptor = new User();
                    tempUserReceptor = _context.UserRepository.findId(userIDReceptor);
                    Wallet tempWalletReceptor = new Wallet();
                    tempWalletReceptor = _serviceWallet.GetWalletByUserID(userIDReceptor);
                    tempWalletReceptor = _serviceWallet.LoadWalletWithCryptoWallets(tempWalletReceptor);
                    List<CryptoWallet> allCryptoWallets = new List<CryptoWallet>();
                    allCryptoWallets = (List<CryptoWallet>)_service.GetCryptoWalletsFromWallet(tempWalletReceptor);
                    CryptoWallet cryptoWalletReceptor = new CryptoWallet();

                    foreach(CryptoWallet cryptoWallet in allCryptoWallets)
                    {
                        if(cryptoWallet.cryptoType == cryptoWalletEmitter.cryptoType)
                        {
                            cryptoWalletReceptor = cryptoWallet;
                        }
                    }

                    if (cryptoWalletReceptor == null)
                    {
                        cryptoWalletReceptor.WalletID = tempWalletReceptor.WalletID;
                        cryptoWalletReceptor.quantity = 0;
                        cryptoWalletReceptor.cryptoType = cryptoWalletEmitter.cryptoType;
                        _context.CryptoWalletRepository.Add(cryptoWalletReceptor);
                    }
                    cryptoWalletReceptor.quantity = cryptoWalletReceptor.quantity + amount;
                    _context.CryptoWalletRepository.Update(cryptoWalletEmitter);
                    _context.CryptoWalletRepository.Update(cryptoWalletReceptor);
                    
                    transactionService.RegisterCryptoTransaction(tempWalletEmitter.UserID, TransactionTypes.convertEmissor, amount, cryptoWalletEmitter.cryptoType, tempWalletReceptor.UserID, amount, cryptoWalletEmitter.cryptoType);

                    _context.Save();

                }
            }
            return Ok(cryptoWalletEmitter);
        }

        [HttpGet("maxfunds")]
        public float MaxFundsAmountCryptoWallet(int cryptoWalletID)
        {
            CryptoWallet cryptoWallet = new CryptoWallet();
            cryptoWallet = _context.CryptoWalletRepository.findId(cryptoWalletID);
            
            return _service.MaxFundsAmountCryptoWallet(cryptoWallet);
        }
        */
        /*[HttpGet("equivalentdollarvalue")]
        public float EquivalentDollarValue(int cryptoWalletID)
        {
            CryptoWallet cryptoWallet = new CryptoWallet();
            cryptoWallet = _context.CryptoWalletRepository.findId(cryptoWalletID);

            return _service.EquivalentDollarValue(cryptoWallet.quantity, cryptoWallet.cryptoType);
        }

        [HttpGet("equivalentcryptovalue")]
        public float EquivalentCryptoValue(float dollars, CryptoTypes cryptoType)
        {
            return _service.EquivalentCryptoValue(dollars, cryptoType);
        }*/
    }
}
