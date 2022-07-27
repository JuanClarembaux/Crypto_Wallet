using CryptoWallet.Enums;
using System.ComponentModel.DataAnnotations;

namespace CryptoWallet.Models.CreditCardModel

{
    public class CreditCardCreate
    {
        [Required]
        [RegularExpression(@"[0-9]{8,16}")]
        public long cardNumber { get; set; }

        [Required]        
        public string? cardHolder { get; set; }

        [Required]
        [RegularExpression(@"[0-9]{3,3}")]
        public string? securityCode { get; set; }
    }
}
