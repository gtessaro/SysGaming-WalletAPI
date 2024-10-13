using System;

namespace SysGaming_WalletAPI.Exceptions
{
    public class DuplicateEmailException(string message) : Exception(message) 
    {
    }
}