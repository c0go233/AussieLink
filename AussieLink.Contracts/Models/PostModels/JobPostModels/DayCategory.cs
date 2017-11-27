using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AussieLink.Contracts.Models.PostModels.JobPostModels
{
    public class DayCategory
    {
        public byte DayCategoryId { get; set; }
        public string Name { get; set; }
        public byte Amount { get; set; }
    }
}
