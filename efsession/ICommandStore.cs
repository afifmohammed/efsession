using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace efsession
{
    public interface ICommandStore
    {
        void Add<TEntity>(TEntity item) where TEntity : class;
        void Add<TEntity>(IEnumerable<TEntity> items) where TEntity : class;
        void Remove<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : class;
    }
}