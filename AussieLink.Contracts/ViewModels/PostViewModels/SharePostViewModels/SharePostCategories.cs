using AussieLink.Contracts.Models.PostModels;
using AussieLink.Contracts.Models.PostModels.SharePostModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AussieLink.Contracts.ViewModels.PostViewModels.SharePostViewModels
{
    public class SharePostCategories
    {
        public IEnumerable<Place> Places { get; set; }
        public IEnumerable<SuburbVM> SydneySuburbs { get; set; }
        public IEnumerable<SuburbVM> MelbourneSuburbs { get; set; }
        public IEnumerable<SuburbVM> BrisbaneSuburbs { get; set; }
        public IEnumerable<SuburbVM> AdelaideSuburbs { get; set; }
        public IEnumerable<SuburbVM> PerthSuburbs { get; set; }
        public IEnumerable<Gender> Genders { get; set; }
        public IEnumerable<ShareType> ShareTypes { get; set; }
    }
}
