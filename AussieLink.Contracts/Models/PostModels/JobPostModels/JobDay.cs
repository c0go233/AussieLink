using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AussieLink.Contracts.Models.PostModels.JobPostModels
{
    public class JobDay
    {
        public int JobDayId { get; set; }
        public int PostId { get; set; }
        public byte Amount { get; set; }
        public string Size { get; set; }
    }
}
