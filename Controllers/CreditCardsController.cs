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
using CryptoWallet.Models.CreditCardModel;
using Microsoft.AspNetCore.Authorization;
using CryptoWallet.Services.UserService;
using CryptoWallet.Models.Request;

namespace CryptoWallet.Controllers
{
    
    [Route("creditcards/[controller]")]
    [ApiController]
    public class CreditCardsController : Controller
    {
        private readonly IUnitOfWork _context;
        private readonly IUserService _userService;

        public CreditCardsController(IUnitOfWork context, IUserService userService)
        {
            _context = context;
            _userService = userService;
        }

        // GET: CreditCards
        [HttpGet("listcards")]
        [Authorize]
        public async Task<IActionResult> Index()
        {
            var creditCards = _context.CreditCardRepository.GetAll();
            return Ok(creditCards);
        }

        // GET CreditCards/Details

        [HttpGet("detailscards")]
        [Authorize]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.CreditCardRepository.findId == null) return NotFound("Tarjeta no encontrada");

            var creditCard = _context.CreditCardRepository.findId(id);

            if (creditCard == null) return NotFound("Tarjeta no encontrada");

            return Ok(creditCard);
        }

        // POST: CreditCards/Create

        [HttpPost("createcreditcard")]
        [Authorize]
        public async Task<ActionResult<CreditCardCreate>> Create(CreditCardCreate creditCard)
        {

            CreditCard newCard = new CreditCard();

            //TODO: Problema int CreditCard
            newCard.cardNumber = creditCard.cardNumber;
            newCard.cardHolder = creditCard.cardHolder;
            newCard.securityCode = int.Parse(creditCard.securityCode);

            newCard.idUser = _userService.GetUserId();

            _context.CreditCardRepository.Add(newCard);

            _context.Save();
            return Ok(newCard);
        }

        // POST: CreditCards/Edit/5

        [HttpPut("editcard")]
        [Authorize]

        public async Task<IActionResult> Edit(int id, CreditCardCreate creditCards)
        {           

            if (id == null || _context.CreditCardRepository.findId == null) return NotFound("Tarjeta no encontrada");

            var creditCard = _context.CreditCardRepository.findId(id);

            if (creditCard == null) return NotFound("Tarjeta no encontrada");
            
            if (!ModelState.IsValid) return BadRequest("Modelo inválido");

            creditCard.cardNumber = (creditCards.cardNumber);
            creditCard.cardHolder = creditCards.cardHolder;
            creditCard.securityCode = int.Parse(creditCards.securityCode);


            _context.CreditCardRepository.Update(creditCard);
            _context.Save();

            return Ok(creditCard);
        }

        // GET: CreditCards/Delete/5

        [HttpDelete("deletecard")]
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.CreditCardRepository == null) return NotFound("Tarjeta no encontrada");

            var creditCard = _context.CreditCardRepository.findId(id);

            if (creditCard == null) return NotFound("Tarjeta no encontrada");

            _context.CreditCardRepository.Delete(creditCard);
            _context.Save();
            return Ok(creditCard);
        }

        // POST: CreditCards/Delete/5
        /*[HttpDelete, ActionName("deletecardconfirmed")]
        [Authorize]

        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.CreditCardRepository == null) return NotFound("Tarjeta no encontrada");

            var creditCard = _context.CreditCardRepository.findId(id);

            if (creditCard != null) _context.CreditCardRepository.Delete(creditCard);

            return RedirectToAction(nameof(Index));
        }*/

    }
}
