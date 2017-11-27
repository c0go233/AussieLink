using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AussieLink.Contracts.ViewModels.AdViewModels
{
    public class GetJobAdVM
    {
        public IEnumerable<JobAdPostVM> PagedPosts { get; set; }
        public Pager Pager { get; set; }

        public GetJobAdVM() {}
    }
}
