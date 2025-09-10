using DoctorsClinic.Core.Helper;
using System.ComponentModel.DataAnnotations;

namespace DoctorsClinic.Core.Dtos.Account
{
    public class ChangePassword
    {
        public required string OldPassword { get; set; }
        [PasswordValidation]
        public required string NewPassword { get; set; }
        [Compare("NewPassword", ErrorMessage = "Passwords do not match.")]
        public required string ConfirmPassword { get; set; }
    }
}