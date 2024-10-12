using System.ComponentModel.DataAnnotations;
<<<<<<< HEAD
using SysGaming_WalletAPI.Controllers.DTO.Validation;
=======
>>>>>>> 7925f320fca30d4a9e711ea192cffcfe073406a5

namespace SysGaming_WalletAPI.Controllers.DTO
{
    public class PlayerDTO()
    {
        [Required(ErrorMessage = "Name Field is Required.")]
        public string? Name { get; set; }
<<<<<<< HEAD
        [Required(ErrorMessage = "Email Field is Required")]
        [EmailAddress(ErrorMessage = "Invalid Email")]
=======
        [Required(ErrorMessage = "O campo Email é obrigatório.")]
        [EmailAddress(ErrorMessage = "O Email fornecido não é válido.")]
>>>>>>> 7925f320fca30d4a9e711ea192cffcfe073406a5
        public string? Email { get; set; }
        // [Required(ErrorMessage = "Password Field is Required")]
        // [StringLength(100, MinimumLength = 8, ErrorMessage = "Password must contain between 8 and 100 characters")]
        [Password]
        public string? Password { get; set; }
        [Required(ErrorMessage = "Wallet is Required")]
        public required WalletDTO WalletDTO { get; set; }
    }
}