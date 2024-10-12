using System.ComponentModel.DataAnnotations;
using SysGaming_WalletAPI.Controllers.DTO.Validation;

namespace SysGaming_WalletAPI.Controllers.DTO
{
    public class PlayerDTO()
    {
        [Required(ErrorMessage = "Name Field is Required.")]
        public string? Name { get; set; }
        [Required(ErrorMessage = "Email Field is Required")]
        [EmailAddress(ErrorMessage = "Invalid Email")]
        public string? Email { get; set; }
        // [Required(ErrorMessage = "Password Field is Required")]
        // [StringLength(100, MinimumLength = 8, ErrorMessage = "Password must contain between 8 and 100 characters")]
        [Password]
        public string? Password { get; set; }
        [Required(ErrorMessage = "Wallet is Required")]
        public required WalletDTO WalletDTO { get; set; }
    }
}