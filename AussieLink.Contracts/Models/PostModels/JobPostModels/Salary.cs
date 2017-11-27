using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AussieLink.Contracts.Models.PostModels.JobPostModels
{
    public class Salary
    {
        public int SalaryId { get; set; }
        public decimal Amount { get; set; }
        public string Size { get; set; }
        public int PostId { get; set; }

        public SalaryType SalaryType { get; set; }
        public int SalaryTypeId { get; set; }
    }
}
