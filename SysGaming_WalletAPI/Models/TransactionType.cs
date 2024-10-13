using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SysGaming_WalletAPI.Models
{
    public enum TransactionType
    {
        DEPOSIT = 1,     
        BET = 2,         
        PRIZE = 3,       
        CANCEL = 4,
        WITHDRAW = 5
    }
}