using AussieLink.Contracts.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AussieLink.Contracts.ViewModels.PostViewModels
{
    public class BasePostVM
    {
        public string PostId { get; set; }

        [Display(Name = "Location")]
        [Required]
        public int PlaceId { get; set; }

        [Required]
        [StringLength(200, ErrorMessage = "{0} Should be less than {1}")]
        public string Title { get; set; }

        [Required]
        [StringLength(4000, ErrorMessage = "{0} should be less than {1}")]
        public string Description { get; set; }

        public string PostType { get; set; }

        public BasePostVM(string postType)
        {
            this.PostType = postType;
        }
    }
}
