using CryptoWallet.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CryptoWallet.Models
{
    public class CryptoWallet
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CryptoWalletID { get; set; }
        
        public int WalletID { get; set; }
        
        [Required]
        public CryptoTypes cryptoType { get; set; }
        public int quantity { get; set; }
        
    }
}
