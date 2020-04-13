using System.ComponentModel.DataAnnotations;

namespace App.Models.Accounts
{
    public class SignInModel
    {
        [Required]
        [EmailAddress]
        [Display(Name ="Email")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Remember Me")]
        public bool RememberMe { get; set; }

        public bool ResetPassword { get; set; }
    }
}
