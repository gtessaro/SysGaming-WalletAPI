using System;

namespace SysGaming_WalletAPI.Exceptions
{
    public class InsuficientBalanceException : Exception 
    {
        public InsuficientBalanceException(string message) : base(message) { }
    }
}