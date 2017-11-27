using AussieLink.Contracts.ExtensionMethods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AussieLink.Contracts.ViewModels.AdViewModels
{
    public class JobAdDetailVM : BaseAdDetailVM
    {
        public JobAdPostVM JobAdPostVM { get; set; }

        public JobAdDetailVM(string returnUrl, bool needBackToList) 
            : base(returnUrl, needBackToList, Enums.PostType.JOB.ToDescription())
        {
            this.JobAdPostVM = new JobAdPostVM();
        }
    }
}
