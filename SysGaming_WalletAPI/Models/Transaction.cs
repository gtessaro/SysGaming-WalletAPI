

namespace SysGaming_WalletAPI.Models
{
    public class Transaction
    {
        public int Id { get; set; }
        public int PlayerId { get; set; }
        public DateTime DateTime { get; set; }
        public TransactionType? Type { get; set; } // Ex.: "DEPOSIT", "BET", "PRIZE", "CANCEL","WITHDRAW"
        public decimal Value { get; set; }
        public Player? Player { get; set; }
    }
}
