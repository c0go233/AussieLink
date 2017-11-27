using AussieLink.Contracts.Models.AdModels.JobAdModels;
using AussieLink.Contracts.Models.PostModels;
using AussieLink.Contracts.Models.PostModels.JobPostModels;
using AussieLink.Contracts.Repositories;
using AussieLink.DAL.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AussieLink.DAL.Repositories
{
    public class ContractTypeRepository : BaseRepository<ContractType>, IContractTypeRepository
    {
        public ContractTypeRepository(DataContext context) : base(context) { }
    }

    public class JobTypeRepository : BaseRepository<JobType>, IJobTypeRepository
    {
        public JobTypeRepository(DataContext context) : base(context) { }
    }

    public class SalaryTypeRepository : BaseRepository<SalaryType>, ISalaryTypeRepository
    {
        public SalaryTypeRepository(DataContext context) : base(context) { }
    }

    public class DayCategoryRepository : BaseRepository<DayCategory>, IDayCategoryRepository
    {
        public DayCategoryRepository(DataContext context) : base(context) { }
    }

    public class WeeklySalaryCategoryRepository : BaseRepository<WeeklySalaryCategory>, IWeeklySalaryCategoryRepository
    {
        public WeeklySalaryCategoryRepository(DataContext context) : base(context) { }
    }

    public class HourlySalaryCategoryRepository : BaseRepository<HourlySalaryCategory>, IHourlySalaryCategoryRepository
    {
        public HourlySalaryCategoryRepository(DataContext context) : base(context) { }
    }
}
