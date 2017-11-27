using AussieLink.Contracts.CustomValidations;
using AussieLink.Contracts.CustomValidations.JobPostCustomValidations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AussieLink.Contracts.Enums;
using AussieLink.Contracts.ExtensionMethods;
using System.Web;

namespace AussieLink.Contracts.ViewModels.PostViewModels
{
    public class JobPostVM : BasePostVM
    {

        [Display(Name = "Job Type")]
        [Required]
        public int JobTypeId { get; set; }

        [Display(Name = "Contract Type")]
        [Required]
        public int ContractTypeId { get; set; }

        public JobPostDayVM JobPostDayVM { get; set; }
        public JobPostSalaryVM JobPostSalaryVM { get; set; }
        public JobCategoriesVM JobCategories { get; set; }

        public JobPostVM() : base(Enums.PostType.JOB.ToDescription()) { Setup(); }

        private void Setup()
        {
            JobPostDayVM = new JobPostDayVM();
            JobPostSalaryVM = new JobPostSalaryVM();
            JobCategories = new JobCategoriesVM();
        }



    }
}
