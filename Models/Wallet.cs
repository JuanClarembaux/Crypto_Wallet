using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CryptoWallet.Models
{
    public class Wallet
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int WalletID { get; set; }
        public int UserID { get; set; }
        
        public virtual ICollection<CryptoWallet>? cryptoWallets { get; set; }
    }
}