using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SysGaming_WalletAPI.Controllers.DTO
{
    public class LoginDTO
    {
        public required string Email { get; set; }
        public required string Password { get; set; }
    }
}