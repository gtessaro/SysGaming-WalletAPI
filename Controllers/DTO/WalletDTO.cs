using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SysGaming_WalletAPI.Controllers.DTO
{
    public class WalletDTO
    {
        public decimal Balance { get; set; }
        public string Currency { get; set; } = "BRL";
    }
}