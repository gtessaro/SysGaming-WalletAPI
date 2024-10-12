

namespace SysGaming_WalletAPI.Models
{
    public class Bet
    {
        public int Id { get; set; }
        public int PlayerId { get; set; }
        public decimal Value { get; set; }
        public string? Status { get; set; } // Ex.: "Win", "Lost", "Canceled"
        public decimal? Prize { get; set; }
        public DateTime DateTime { get; set; }
        public Player? Player { get; set; }
    }
}
