using System;
using System.Linq;
using System.Linq.Expressions;

namespace efsession
{
    public interface IQueryStore
    {
        TEntity Single<TEntity>(Expression<Func<TEntity, bool>> expression) where TEntity : class;
        IQueryable<TEntity> Query<TEntity>() where TEntity : class;
    }
}