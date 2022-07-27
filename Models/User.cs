using CryptoWallet.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CryptoWallet.Models
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserID { get; set; }
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
        public RoleTypes role = 0;
        public RoleTypes Role { get => role; }
    }
}
