using AussieLink.Contracts.ExtensionMethods;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace AussieLink.Contracts.ViewModels.PostViewModels.SharePostViewModels
{
    public class SharePostVM : BasePostVM
    {
        public SharePostCategories SharePostCategories { get; set; }

        [Required]
        public decimal? Price { get; set; }

        [Required]
        [Display(Name = "Available Date")]
        public string DateAvailableFrom { get; set; }

        [Required]
        [Display(Name = "Share Type")]
        public string ShareType { get; set; }

        [Required]
        public string Suburb { get; set; }

        [Required]
        public string Gender { get; set; }

        public IEnumerable<PictureVM> SavedPictures { get; set; }

        public SharePostVM() : base(Enums.PostType.SHARE.ToDescription())
        {
            SharePostCategories = new SharePostCategories();
        }
    }
}
