using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SysGaming_WalletAPI.Controllers.DTO
{
    public class PlayerDTO()
    {
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public required WalletDTO WalletDTO { get; set; }
    }
}