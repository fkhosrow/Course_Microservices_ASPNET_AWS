using System.ComponentModel.DataAnnotations;

namespace App.Models.Accounts
{
    public class ConfirmModel
    {
        [Required()]
        [Display(Name = "Email")]
        [EmailAddress]
        public string Email { get; set; }

        [Required()]
        [Display(Name = "Code")]
        public string Code { get; set; }
    }
}
