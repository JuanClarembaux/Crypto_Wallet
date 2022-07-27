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
using CryptoWallet.Models.AuthModels;
using Microsoft.AspNetCore.Authorization;

namespace CryptoWallet.Controllers
{
    [Route("users/[controller]")]
    [ApiController]
    public class UsersController : Controller
    {
        private readonly IUnitOfWork _context;

        public UsersController(IUnitOfWork context)
        {
            _context = context;
        }

        // GET: Users
        [HttpGet("listuser")]
        [Authorize]
        public async Task<IActionResult> Index()
        {
            var users = _context.UserRepository.GetAll();

            return Ok(users);
        }

        // GET: Users/Details
        [HttpGet("detailsuser")]
        [Authorize]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.UserRepository.findId == null) return NotFound("Usuario no encontrado");


            var user = _context.UserRepository.findId(id);

            if (user == null) return NotFound("Usuario no encontrado");


            return Ok(user);
        }

        // POST: Users/Create
        /*
        [HttpPost("create_user")]

        public async Task<IActionResult> Create(UserRequest user)
        {
            User newUser = new User();
            if (user.userName != null)
            {
                _context.UserRepository.Add(newUser);
                _context.Save();

            }
            else
            {
                return BadRequest("Modelo inválido");
            }

            return View(newUser);
        }*/

        // POST: Users/Edit/5

        [HttpPut("edit")]
        [Authorize]
        public async Task<IActionResult> Edit(int id, UserRegister userr)
        {

            if (id == null || _context.UserRepository.findId == null) return NotFound("Usuario no encontrado");


            var user = _context.UserRepository.findId(id);

            if (user == null) return NotFound("Usuario no encontrado");

            if (!ModelState.IsValid) return BadRequest("Modelo inválido");

            user.userName = userr.userName;
            user.fullName = userr.fullName;
            user.mail = userr.mail;
            user.password = userr.password;

            _context.UserRepository.Update(user);

            _context.Save();

            return Ok(user);
        }

        // GET: Users/Delete/5

        [HttpDelete("deleteuser")]
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.UserRepository == null) return NotFound("Usuario no encontrado");

            var user = _context.UserRepository.findId(id);

            if (user == null) return NotFound("Usuario no encontrado");

            _context.UserRepository.Delete(user);
            _context.Save();
            return Ok(user);
        }

        // POST: Users/Delete/5

        /*[HttpDelete, ActionName("deleteuserconfirmed")]
        [Authorize]

        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.UserRepository == null) return NotFound("Usuario no encontrado");

            var user = _context.UserRepository.findId(id);

            if (user != null) _context.UserRepository.Delete(user);

            _context.Save();

            return RedirectToAction(nameof(Index));
        }*/

    }
}
