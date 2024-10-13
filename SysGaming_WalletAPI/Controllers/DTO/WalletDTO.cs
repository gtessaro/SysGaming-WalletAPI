using System.ComponentModel.DataAnnotations;

namespace SysGaming_WalletAPI.Controllers.DTO
{
    public class WalletDTO
    {
        public int PlayerId {get;set;}
        
        [Required(ErrorMessage = "Balance Field is Required")]
        public decimal Balance { get; set; }
        [Required(ErrorMessage = "Currency Field is Required")]
        public string Currency { get; set; } = "BRL";
    }
}