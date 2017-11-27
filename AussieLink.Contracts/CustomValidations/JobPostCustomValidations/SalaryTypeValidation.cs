using AussieLink.Contracts.ViewModels.PostViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AussieLink.Contracts.CustomValidations.JobPostCustomValidations
{
    public class SalaryTypeValidation : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var model = (JobPostSalaryVM)validationContext.ObjectInstance;

            if (model.MinSalary != null || model.MaxSalary != null)
            {
                if (model.SalaryTypeId == null)
                    return new ValidationResult("Please select salary type");
            }
            return ValidationResult.Success;
        }
    }
}
