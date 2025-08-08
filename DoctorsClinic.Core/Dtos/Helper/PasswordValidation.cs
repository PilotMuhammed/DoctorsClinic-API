using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DoctorsClinic.Core.Dtos.Helper
{
    public class PasswordValidation : ValidationAttribute
    {
        protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
        {
            var password = value as string;

            if (string.IsNullOrWhiteSpace(password))
                return new ValidationResult("Password is required.");

            if (password.Length < 8)
                return new ValidationResult("Password must be at least 8 characters long.");

            if (!Regex.IsMatch(password, @"[A-Z]"))
                return new ValidationResult("Password must contain at least one uppercase letter.");

            if (!Regex.IsMatch(password, @"[a-z]"))
                return new ValidationResult("Password must contain at least one lowercase letter.");

            if (!Regex.IsMatch(password, @"\d"))
                return new ValidationResult("Password must contain at least one digit (0-9).");

            if (!Regex.IsMatch(password, @"[^a-zA-Z\d]"))
                return new ValidationResult("Password must contain at least one special character (e.g., !@#$%^&*).");

            return ValidationResult.Success!;
        }
    }
}
