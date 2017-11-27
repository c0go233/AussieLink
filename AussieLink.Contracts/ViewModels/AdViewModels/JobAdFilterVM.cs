using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AussieLink.Contracts.ViewModels.AdViewModels
{
    public class JobAdFilterVM : BaseAdFilterVM
    {
        public JobAdCategoriesVM JobAdCategories { get; set; }
        public string ContractType { get; set; }
        public decimal? Salary { get; set; }
        public string SalaryType { get; set; }
        public byte? Day { get; set; }
        public string JobType { get; set; }

        //ajax call mapping uses default con 
        public JobAdFilterVM() { }


        //this con only used for sending vm to view.
        public JobAdFilterVM(string place, string contractType,
            string salaryType, decimal? salary,
            byte? day, string jobType, string keyword) : base(place, keyword)
        {
            this.JobAdCategories = new JobAdCategoriesVM();
            this.ContractType = contractType;
            this.Day = day;
            this.JobType = jobType;
            this.SalaryType = salaryType;
            setSalary(salary, salaryType);
        }

        private void setSalary(decimal? salary, string salaryType)
        {
            if (salary == null || !isValidSalaryType(salaryType))
                this.Salary = null;
            else
            {
                int salaryInInt = (int)salary.Value;
                if (string.Equals(salaryType, "a hour", StringComparison.OrdinalIgnoreCase) && salaryInInt > 20)
                    this.Salary = null;
                else if (string.Equals(salaryType, "a week", StringComparison.OrdinalIgnoreCase) && salaryInInt < 300)
                    this.Salary = null;
                else
                {
                    string salaryInString = salaryInInt + ".00";
                    decimal salaryInDecimal;
                    this.Salary = decimal.TryParse(salaryInString, out salaryInDecimal) ? salaryInDecimal : (decimal?)null;
                }
            }
        }

        private bool isValidSalaryType(string salaryType)
        {
            if (!string.Equals(salaryType, "a hour", StringComparison.OrdinalIgnoreCase)
                 && !string.Equals(salaryType, "a week", StringComparison.OrdinalIgnoreCase))
                return false;
            return true;
        }



    }
}
