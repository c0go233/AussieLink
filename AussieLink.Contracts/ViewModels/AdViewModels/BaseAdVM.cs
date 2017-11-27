using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AussieLink.Contracts.ViewModels.AdViewModels
{
    public class BaseAdVM
    {
        public int PageIndex { get; set; }
        public string CurrentPostId { get; set; }

        public BaseAdVM() { }

        public BaseAdVM(int pageIndex, string currentPostId)
        {
            this.PageIndex = pageIndex;
            this.CurrentPostId = currentPostId;
        }
    }
}
