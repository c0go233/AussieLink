using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace AussieLink.Contracts.Repositories
{
    public interface IBaseRepository<TEntity> where TEntity : class
    {
        void Add(TEntity entity);
        void AddRange(IEnumerable<TEntity> entities);
        TEntity FindById(object id);
        IEnumerable<TEntity> GetAll();
        void Remove(TEntity entitiy);
        void RemoveRage(IEnumerable<TEntity> entities);
        TEntity SingleOrDefault(Expression<Func<TEntity, bool>> predicate);
        IEnumerable<TEntity> WhereBy(Expression<Func<TEntity, bool>> predicate);
    }
}