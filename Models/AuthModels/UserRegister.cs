using CryptoWallet.Enums;
using System.ComponentModel.DataAnnotations;

namespace CryptoWallet.Models.AuthModels
{
    public class UserRegister
    {        
        [Required]
        public string userName { get; set; }
        [Required]
        public string fullName { get; set; }
        [Required]
        [EmailAddress]
        public string mail { get; set; }
        [Required]
        [MinLength(6), MaxLength(16)]
        public string password { get; set; }
    }
}
