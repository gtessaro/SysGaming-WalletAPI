using System.ComponentModel.DataAnnotations;
using SysGaming_WalletAPI.Models;

namespace SysGaming_WalletAPI.Controllers.DTO
{
    public class TransactionDTO
    {
        public int PlayerId { get; set; }
        public DateTime DateTime { get; set; }
        public TransactionType? Type { get; set; } // Ex.: "DEPOSIT", "BET", "PRIZE", "CANCEL","WITHDRAW"
        public decimal Value { get; set; }
    }
}