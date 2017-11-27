using AussieLink.Contracts.ViewModels.PostViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AussieLink.Contracts.CustomValidations.JobPostCustomValidations
{
    public class MinSalaryValidation : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var model = (JobPostSalaryVM)validationContext.ObjectInstance;
            
            if (model.MinSalary < 0)
                return new ValidationResult("Salary should not be negative");

            if (model.MinSalary == null && model.MaxSalary != null)
                return new ValidationResult("Please enter minimum salary either or only");

            if (model.MinSalary >= model.MaxSalary)
                return new ValidationResult("Minium salary should be less than maium");

            return ValidationResult.Success;
        }
    }
}
