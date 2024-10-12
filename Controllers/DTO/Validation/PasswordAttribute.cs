
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace SysGaming_WalletAPI.Controllers.DTO.Validation
{

    public class PasswordAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var password = value as string;

            if (string.IsNullOrEmpty(password))
            {
                return new ValidationResult("Password Field is Required");
            }

            if (password.Length < 8)
            {
                return new ValidationResult("Password must contain at least 8 characters");
            }

            if (!Regex.IsMatch(password, @"[A-Z]"))
            {
                return new ValidationResult("The password should contain at least 1 uppercase character.");
            }

            if (!Regex.IsMatch(password, @"[0-9]"))
            {
                return new ValidationResult("The password must contain at least one number.");
            }

            if (!Regex.IsMatch(password, @"[\W_]"))
            {
                return new ValidationResult("The password must contain at least one special character.");
            }

            return ValidationResult.Success;
        }
    }
}