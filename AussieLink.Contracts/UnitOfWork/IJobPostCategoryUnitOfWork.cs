using AussieLink.Contracts.Repositories;
using System;
using System.Linq.Expressions;

namespace AussieLink.Contracts.UnitOfWork
{
    public interface IJobPostCategoryUnitOfWork : IDisposable
    {
        IPlaceRepository Places { get; }
        IContractTypeRepository ContractTypes { get; }
        IDayCategoryRepository DayCategories { get; }
        IJobTypeRepository JobTypes { get; }
        ISalaryTypeRepository SalaryTypes { get; }
        IWeeklySalaryCategoryRepository WeeklySalaryCategories { get; }
        IHourlySalaryCategoryRepository HourlySalaryCategories { get; }

        int? GetPrimaryKeyValueByName<TEntity>(IBaseRepository<TEntity> repository, Expression<Func<TEntity, bool>> predicate, string name) where TEntity : class;
        int Complete();
    }
}