using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AussieLink.Contracts.ViewModels
{
    public class ResetPasswordVM
    {
        [Required]
        [DataType(DataType.Password)]
        [StringLength(50, ErrorMessage = "{0} should be between {2} and {1} characters", MinimumLength = 6)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "The password and confirmation passowrd does not match")]
        [Display(Name = "Confirm Password")]
        public string ConfirmPassword { get; set; }

        public string Email { get; set; }
    }
}
