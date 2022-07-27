using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CryptoWallet.Models
{
    public class CreditCard
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int? id { get; set; }

        public int idUser { get; set; }
        [ForeignKey("idUser")]
        public User? user { get; set; }

        [Required]
        [RegularExpression(@"[0-9]{8,16}")]
        public long? cardNumber { get; set; }

        [Required]
        public string? cardHolder { get; set; }

        [Required]
        [RegularExpression(@"[0-9]{3,3}")]
        public int? securityCode { get; set; }

      
    }
}
