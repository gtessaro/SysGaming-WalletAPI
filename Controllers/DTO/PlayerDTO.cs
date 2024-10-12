using System.ComponentModel.DataAnnotations;

namespace SysGaming_WalletAPI.Controllers.DTO
{
    public class PlayerDTO()
    {
        public string? Name { get; set; }
        [Required(ErrorMessage = "O campo Email é obrigatório.")]
        [EmailAddress(ErrorMessage = "O Email fornecido não é válido.")]
        public string? Email { get; set; }
        public string? Password { get; set; }
        public required WalletDTO WalletDTO { get; set; }
    }
}