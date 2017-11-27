using AussieLink.Contracts.CustomValidations.JobPostCustomValidations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AussieLink.Contracts.ViewModels.PostViewModels
{
    public class JobPostSalaryVM
    {
        [Display(Name = "Salary")]
        [SalaryTypeValidation]
        public int? SalaryTypeId { get; set; }

        [Display(Name = "Minimum Salary")]
        [MinSalaryValidation]
        public decimal? MinSalary { get; set; }

        [Display(Name = "Maximum Salary")]
        [MaxSalaryValidation]
        public decimal? MaxSalary { get; set; }
    }
}
