
namespace SysGaming_WalletAPI.Models
{
    public class Player
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public DateTime CreatedDate { get; set; }

        public Wallet? Wallet { get; set; }

        public ICollection<Bet>? bets { get; set; }

        public ICollection<Transaction>? Transactions { get; set; }
    }
}