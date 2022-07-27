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
namespace CryptoWallet.Controlers
{
    
    public class WalletsController : Controller
    {
        private readonly IUnitOfWork _context;
        private readonly WalletService _service;
        public WalletsController(IUnitOfWork context, WalletService service)
        {
            _context = context;
            _service = service; 
        }

        
        public ActionResult<Wallet> WalletByUserId(int id)
        {
            if (_service.GetWalletByUserID(id) == null)
            {
                return NotFound();
            }
            Wallet wallet = new Wallet();

            wallet = _service.GetWalletByUserID(id);

            if (wallet == null)
            {
                return NotFound();
            }

            return Ok(wallet);
        }

        // GET: Wallets/Create  Create a wallet.
        /*
        [Route("wallet/create")]
        [HttpPost]
        public ActionResult<Wallet> Create(WalletRequest wallet)
        {
            Wallet newWallet = new Wallet();
            newWallet = _context.WalletRepository.findId(wallet);
            newWallet = _service.LoadWalletWithCryptoWallets(newWallet);
            string StatusMessage;
            try
            {
                if (_context != null)
                {


                    if (newWallet.cryptoWallets != null)
                    {
                        if (_context.WalletRepository.findId(newWallet.WalletID) == null)
                        {
                            StatusMessage = "Wallet already created";
                            return View(wallet);
                        }
                        else
                        {
                            _context.WalletRepository.Add(newWallet);
                            _context.Save();
                            StatusMessage = "Success";
                        }
                    }
                    else
                    {
                        StatusMessage = "invalid wallet data";
                        // This return should send us to login since it's basically a crash.
                        return Ok();
                    }
                }
                else
                {
                    return BadRequest("No me preguntes como, no hay context");
                }
            } catch(Exception ex)
            {
                StatusMessage = ex.Message;
                return BadRequest();
            }

            StatusMessage = "Teoricamente no es posible llegar aca";
            return Ok(newWallet);
        }*/        
    }
}
