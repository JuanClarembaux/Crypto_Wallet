using CryptoWallet.Models;
using CryptoWallet.Models.AuthModels;
using CryptoWallet.Services.UserService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace CryptoWallet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorizationController : ControllerBase
    {
        private readonly IUnitOfWork _context;
        private readonly IConfiguration _configuration;
        private readonly IUserService _userService;
        public AuthorizationController(IConfiguration configuration, IUserService userService, IUnitOfWork context)
        {
            _context = context;
            _configuration = configuration;
            _userService = userService;
        }
        /*
        public ActionResult<string> GetMe()
        {
            var userName = _userService.GetMyName();
            return Ok(userName);
        }*/

        [HttpPost("register")]
        public async Task<ActionResult<UserRegister>> Register(UserRegister request)
        {
            var userValidation = _context.UserRepository.FindByEmail(request.mail);            

            if (userValidation != null) return BadRequest("El usuario ya existe");

            User user = new User();
            user.fullName = request.fullName;
            user.userName = request.userName;
            user.mail = request.mail;
            user.password = request.password;
            _context.UserRepository.Add(user);

            _context.Save();

            Wallet wallet = new Wallet();
            wallet.UserID = user.UserID;
            _context.WalletRepository.Add(wallet);

            _context.Save();

            Models.CryptoWallet cryptoWallet = new Models.CryptoWallet();
            cryptoWallet.WalletID = wallet.WalletID;
            cryptoWallet.cryptoType = Enums.CryptoTypes.dollar;
            cryptoWallet.quantity = 0;
            _context.CryptoWalletRepository.Add(cryptoWallet);

            _context.WalletRepository.Update(wallet);
            _context.Save();

            return Ok(user);
        }

        [HttpPost("login")]
        public async Task<ActionResult<string>> Login(UserLogin request)
        {
            var user = _context.UserRepository.FindByEmail(request.Mail);
            if (user == null) return BadRequest("User not found.");

            string token = CreateToken(user);

            return Ok(token);
        }
        
        private string CreateToken(User user)
        {
            List<Claim> claims = new List<Claim>
            {                
                new Claim(ClaimTypes.NameIdentifier, user.userName),
                new Claim(ClaimTypes.Email, user.mail.ToString()),
                new Claim(ClaimTypes.Name, user.fullName),
                new Claim(ClaimTypes.Role, user.Role.ToString()),
                new Claim(ClaimTypes.Sid, user.UserID.ToString())
            };

            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(
                _configuration.GetSection("AppSettings:Token").Value));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddMinutes(23),
                signingCredentials: creds) ;

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }
    }
}
