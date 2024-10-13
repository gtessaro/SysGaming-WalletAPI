using System;

namespace SysGaming_WalletAPI.Exceptions
{
    public class NotFoundException : Exception 
    {
        public NotFoundException(string message) : base(message) { }
    }
}