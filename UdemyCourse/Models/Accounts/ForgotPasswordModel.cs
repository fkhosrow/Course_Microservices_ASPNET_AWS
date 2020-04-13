using System.ComponentModel.DataAnnotations;

namespace App.Models.Accounts
{
    public class ForgotPasswordModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
