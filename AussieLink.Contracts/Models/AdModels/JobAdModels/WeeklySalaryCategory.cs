using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AussieLink.Contracts.Models.AdModels.JobAdModels
{
    public class WeeklySalaryCategory
    {
        public byte WeeklySalaryCategoryId { get; set; }
        public string Name { get; set; }
        public decimal Amount { get; set; }
    }
}
