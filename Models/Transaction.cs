
namespace SysGaming_WalletAPI.Models
{
    public class Transaction
    {
        public int Id { get; set; }
        public int PlayerId { get; set; }
        public DateTime DateTime { get; set; }
        public string? Type { get; set; } // Ex.: "Depósito", "Aposta", "Prêmio", "Cancelamento"
        public decimal Valor { get; set; }
        public Player? Player { get; set; }
    }
}
