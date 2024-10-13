
namespace SysGaming_WalletAPI.Models
{
    public class Wallet
    {
        public int Id { get; set; }
        public int PlayerId { get; set; }
        public decimal Balance { get; set; }
        public string Currency { get; set; } = "BRL";
        public Player? Player { get; set; }
    }
}