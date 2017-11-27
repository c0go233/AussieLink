using AussieLink.Contracts.Enums;
using AussieLink.Contracts.ExtensionMethods;
using AussieLink.Contracts.Models.PostModels.JobPostModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AussieLink.Services.HelperClasses
{
    public class JobRenderingTextConverter
    {
        public static string GetSalaryRenderingText(IEnumerable<Salary> salaries)
        {
            var count = salaries.Count();

            if (count == 0)
                return null;

            string salaryText = "";
            var minSalary = salaries.Single(m => m.Size == Size.MINIMUM.ToDescription());
            salaryText += "$" + minSalary.Amount.ToString();

            if (count == 2)
            {
                var maxSalary = salaries.Single(m => m.Size == Size.MAXIMUM.ToDescription());
                salaryText += " ~ " + "$" + maxSalary.Amount.ToString();
            }

            salaryText += " " + minSalary.SalaryType.Name;
            return salaryText;
        }

        public static string GetJobDayRenderingText(IEnumerable<JobDay> jobDays)
        {
            var count = jobDays.Count();

            if (count == 0)
                return null;

            string dayText = "";
            var minDay = jobDays.Single(m => m.Size == Size.MINIMUM.ToDescription());
            dayText += minDay.Amount.ToString();

            if (count == 1 && minDay.Amount == 1)
                return dayText += " day";
            else if (count == 2)
            {
                var maxDay = jobDays.Single(m => m.Size == Size.MAXIMUM.ToDescription());
                dayText += " ~ " + maxDay.Amount.ToString();
            }

            dayText += " days";
            return dayText;

        }
        
    }
}
