using AussieLink.Contracts.Models.PostModels;
using AussieLink.Contracts.Models.PostModels.JobPostModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AussieLink.Contracts.ViewModels.PostViewModels
{
    public class JobCategoriesVM
    {
        public IEnumerable<Place> Places { get; set; }
        public IEnumerable<ContractType> ContractTypes { get; set; }
        public IEnumerable<SalaryType> SalaryTypes { get; set; }
        public IEnumerable<DayCategory> DayCategories { get; set; }
        public IEnumerable<JobType> JobTypes { get; set; }
    }
}
