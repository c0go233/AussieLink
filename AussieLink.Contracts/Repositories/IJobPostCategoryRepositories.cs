using AussieLink.Contracts.Models.AdModels.JobAdModels;
using AussieLink.Contracts.Models.PostModels;
using AussieLink.Contracts.Models.PostModels.JobPostModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AussieLink.Contracts.Repositories
{
    public interface IContractTypeRepository : IBaseRepository<ContractType> { }
    public interface IJobTypeRepository : IBaseRepository<JobType> { }
    public interface ISalaryTypeRepository : IBaseRepository<SalaryType> { }
    public interface IDayCategoryRepository : IBaseRepository<DayCategory> { }
    public interface IWeeklySalaryCategoryRepository : IBaseRepository<WeeklySalaryCategory> { }
    public interface IHourlySalaryCategoryRepository : IBaseRepository<HourlySalaryCategory> { }
}
