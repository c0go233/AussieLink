using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AussieLink.Contracts.ViewModels.AdViewModels
{
    public class BaseAdFilterVM
    {
        public string Place { get; set; }
        public string Keyword { get; set; }

        public BaseAdFilterVM() { }

        public BaseAdFilterVM(string place, string keyword)
        {
            this.Place = place;
            this.Keyword = keyword;
        }
    }
}
