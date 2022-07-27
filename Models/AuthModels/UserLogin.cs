using System.ComponentModel.DataAnnotations;

namespace CryptoWallet.Models.AuthModels
{
    public class UserLogin
    {
        [Required]
        [EmailAddress]
        public string Mail { get; set; } = string.Empty;
        [Required]
        [MinLength(6), MaxLength(16)]
        public string Password { get; set; } = string.Empty;
    }
}
