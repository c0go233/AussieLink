using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AussieLink.Contracts.ViewModels.AdViewModels
{
    public class BaseAdPostVM
    {
        public string PostId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Place { get; set; }
        public string UerName { get; set; }
        public string DateCreated { get; set; }
        public bool Selected { get; set; }
        public bool Complete { get; set; }
    }
}
