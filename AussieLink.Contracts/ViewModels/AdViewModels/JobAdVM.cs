using AussieLink.Contracts.ViewModels.PostViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AussieLink.Contracts.ViewModels.AdViewModels
{
    public class JobAdVM : BaseAdVM
    {
        public JobAdFilterVM JobAdFilterVM { get; set; }

        public JobAdVM() { }

        public JobAdVM(string place, string contractType, string salaryType, decimal? salary,
            byte? day, string jobType, string keyword, int pageIndex, string currentPostId) 
            : base(pageIndex, currentPostId)
        {
            this.JobAdFilterVM = new JobAdFilterVM(place, contractType, salaryType, salary, day, jobType, keyword);
        }
    }
}
