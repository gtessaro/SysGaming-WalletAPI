
using SysGaming_WalletAPI.Models;

namespace SysGaming_WalletAPI.Controllers.DTO
{
    public class BetDTO
    {
        public int PlayerId { get; set; }
        public decimal Value { get; set; }
        public BetStatus? Status { get; set; }
        public decimal? Prize { get; set; }
        public DateTime DateTime { get; set; }
    }
}