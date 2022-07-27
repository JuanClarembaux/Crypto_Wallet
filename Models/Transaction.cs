using CryptoWallet.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CryptoWallet.Models
{
    public class Transaction
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TransactionID { get; set; }

        public int UserID { get; set; }
        
        [Required]
        public int transactionNumber { get; set; }
        [Required]
        public TransactionTypes transactionTypes { get; set; }
        [Required]
        public CryptoTypes cryptoType { get; set; }
        public int CryptoWalletID { get; set; }
       
        [Required]
        public float monto { get; set; }
        public DateTime date = DateTime.Now;
        public DateTime Date { get => date; }
    }
}
