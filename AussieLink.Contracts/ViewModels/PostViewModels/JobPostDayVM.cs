using AussieLink.Contracts.CustomValidations.JobPostCustomValidations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AussieLink.Contracts.ViewModels.PostViewModels
{
    public class JobPostDayVM
    {
        [Display(Name = "Minimum Day")]
        [MinDayValidation]
        public byte? MinDay { get; set; }

        [Display(Name = "Maximum Day")]
        public byte? MaxDay { get; set; }
    }
}
