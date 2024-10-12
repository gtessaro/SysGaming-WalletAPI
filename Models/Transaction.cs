
namespace SysGaming_WalletAPI.Models
{
    public class Transaction
    {
        public int Id { get; set; }
        public int PlayerId { get; set; }
        public DateTime DateTime { get; set; }
        public string? Type { get; set; } // Ex.: "DEPOSITE", "BET", "PRIZE", "CANCEL","WITHDRAW"
        public decimal Valor { get; set; }
        public Player? Player { get; set; }
    }
}
