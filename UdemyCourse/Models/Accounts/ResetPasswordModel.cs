using System.ComponentModel.DataAnnotations;

namespace App.Models.Accounts
{
    public class ResetPasswordModel  : ConfirmModel
    {
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Password and its confirmations do not match")]
        public string ConfirmPassword { get; set; }
    }
}
