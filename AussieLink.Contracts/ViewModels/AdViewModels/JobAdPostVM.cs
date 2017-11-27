using AussieLink.Contracts.ViewModels.PostViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AussieLink.Contracts.ViewModels.AdViewModels
{
    public class JobAdPostVM : BaseAdPostVM
    {
        public string ContractType { get; set; }
        public string JobType { get; set; }
        public string Salary { get; set; }
        public string JobDay { get; set; }

        public JobAdPostVM()
        {}
    }
}
