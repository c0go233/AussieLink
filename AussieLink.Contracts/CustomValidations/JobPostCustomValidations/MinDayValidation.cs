using AussieLink.Contracts.ViewModels.PostViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AussieLink.Contracts.CustomValidations.JobPostCustomValidations
{
    public class MinDayValidation : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var model = (JobPostDayVM)validationContext.ObjectInstance;

            if (model.MinDay != null && model.MaxDay != null && model.MinDay >= model.MaxDay)
                return new ValidationResult("Minimum day should less than Maximum day");

            if (model.MinDay == null && model.MaxDay != null)
                return new ValidationResult("Please select minimum day either or select min day only");

            return ValidationResult.Success;
        }
    }
}
