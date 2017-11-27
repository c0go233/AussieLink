using AussieLink.Contracts.Models.AdModels.JobAdModels;
using AussieLink.Contracts.ViewModels.PostViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AussieLink.Contracts.ViewModels.AdViewModels
{
    public class JobAdCategoriesVM : JobCategoriesVM
    {
        public IEnumerable<WeeklySalaryCategory> WeeklySalaryCategories { get; set; }
        public IEnumerable<HourlySalaryCategory> HourlySalaryCategories { get; set; }
    }
}
