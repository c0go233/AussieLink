using AussieLink.Contracts.Models.CommentModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AussieLink.Contracts.Models.PostModels.JobPostModels
{
    public class JobPost : BasePost
    {
        public JobType JobType { get; set; }
        public int JobTypeId { get; set; }

        public ContractType ContractType { get; set; }
        public int ContractTypeId { get; set; }

        public ICollection<Salary> Salaries{ get; set; }
        public ICollection<JobDay> JobDays { get; set; }

    }
}
