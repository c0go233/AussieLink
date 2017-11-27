using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AussieLink.Contracts.ViewModels
{
    public class ForgotPasswordVM
    {
        [Required]
        [EmailAddress]
        [StringLength(100, ErrorMessage = "{0} should be less than {1} characters")]
        public string Email { get; set; }
    }
}
