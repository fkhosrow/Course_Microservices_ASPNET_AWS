using System;
using System.ComponentModel.DataAnnotations;

namespace UdemyCourse.Models.Accounts
{
    public class ForgotPasswordModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
