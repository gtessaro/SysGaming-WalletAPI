using System;

namespace SysGaming_WalletAPI.Exceptions
{
    public class InsuficientBalaceException : Exception 
    {
        public InsuficientBalaceException(string message) : base(message) { }
    }
}