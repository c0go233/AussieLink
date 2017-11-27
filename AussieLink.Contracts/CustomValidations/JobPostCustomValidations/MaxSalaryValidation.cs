using AussieLink.Contracts.ViewModels.PostViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AussieLink.Contracts.CustomValidations.JobPostCustomValidations
{
    public class MaxSalaryValidation : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var model = (JobPostSalaryVM)validationContext.ObjectInstance;

            if (model.MaxSalary < 0)
                return new ValidationResult("Salary should not be negative");

            return ValidationResult.Success;
        }
    }
}
