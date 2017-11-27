using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AussieLink.Contracts.ViewModels.ManageAdViews
{
    public class ManageProfileVM
    {
        public string CurrentMyAccountMenu { get; set; }
        public string Email { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "Name should be less then {1} characters")]
        public string Name { get; set; }

        public ManageProfileVM() { }

        public ManageProfileVM(string currentMyAccountMenu, string name, string email)
        {
            this.CurrentMyAccountMenu = currentMyAccountMenu;
            this.Name = name;
            this.Email = email;
        }
    }
}
