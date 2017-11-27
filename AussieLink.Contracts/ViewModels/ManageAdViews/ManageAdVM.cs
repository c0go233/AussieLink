using AussieLink.Contracts.Models.PostModels;
using AussieLink.Contracts.ViewModels.AdViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AussieLink.Contracts.ViewModels.ManageAdViews
{
    public class ManageAdVM
    {
        public IEnumerable<ManageAdPostTypeCategory> PostTypes { get; set; }
        public IEnumerable<ManageAdPost> Posts { get; set; }
        public string CurrentMyAccountMenu { get; set; }
        public string PostType { get; set; }
        public int TotalPostCount { get; set; }
        public string PagerUrl { get; set; }
        public Pager Pager { get; set; }
    }
}
