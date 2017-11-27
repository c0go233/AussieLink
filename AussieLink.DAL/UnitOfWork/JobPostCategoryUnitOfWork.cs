using AussieLink.Contracts.Repositories;
using AussieLink.Contracts.UnitOfWork;
using AussieLink.DAL.Data;
using AussieLink.DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AussieLink.DAL.UnitOfWork
{
    public class JobPostCategoryUnitOfWork : IJobPostCategoryUnitOfWork
    {
        private readonly DataContext Context;
        public IPlaceRepository Places { get; }
        public IJobTypeRepository JobTypes { get; }
        public ISalaryTypeRepository SalaryTypes { get; }
        public IContractTypeRepository ContractTypes { get; }
        public IDayCategoryRepository DayCategories { get; }
        public IWeeklySalaryCategoryRepository WeeklySalaryCategories { get; set; }
        public IHourlySalaryCategoryRepository HourlySalaryCategories { get; set; }

        public JobPostCategoryUnitOfWork(DataContext context)
        {
            this.Context = context;
            Places = new PlaceRepository(context);
            JobTypes = new JobTypeRepository(context);
            SalaryTypes = new SalaryTypeRepository(context);
            ContractTypes = new ContractTypeRepository(context);
            DayCategories = new DayCategoryRepository(context);
            WeeklySalaryCategories = new WeeklySalaryCategoryRepository(context);
            HourlySalaryCategories = new HourlySalaryCategoryRepository(context);
        }

        public int? GetPrimaryKeyValueByName<TEntity>(IBaseRepository<TEntity> repository, Expression<Func<TEntity, bool>> predicate, string name) where TEntity : class
        {
            if (name == null)
                return null;

            var entity = repository.SingleOrDefault(predicate);
            if (entity == null)
                return null;

            ObjectContext objectContext = ((IObjectContextAdapter)Context).ObjectContext;
            ObjectSet<TEntity> set = objectContext.CreateObjectSet<TEntity>();
            IEnumerable<string> keyNames = set.EntitySet.ElementType.KeyMembers.Select(k => k.Name);

            var primarykey = entity.GetType().GetProperties().Single(p => p.Name == keyNames.First()).GetValue(entity, null);

            try { return Convert.ToInt32(primarykey); }
            catch (Exception) { return null; }
        }

        public int Complete()
        {
            return Context.SaveChanges();
        }

        public void Dispose()
        {
            Context.Dispose();
        }
    }
}
