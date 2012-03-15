using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Linq.Expressions;

namespace efsession
{
    public class Session : ISession
    {
        private readonly DbContext _dbContext;
        public Session(DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public TEntity Single<TEntity>(Expression<Func<TEntity, bool>> expression) where TEntity : class
        {
            return _dbContext.Set<TEntity>().SingleOrDefault(expression);
        }

        public IQueryable<TEntity> Query<TEntity>() where TEntity : class
        {
            return _dbContext.Set<TEntity>().AsQueryable();
        }

        public void Commit()
        {
            try { _dbContext.SaveChanges(); }
            catch (DbEntityValidationException ex)
            {
                var m = ex.ToFriendlyMessage();
                throw new DbEntityValidationException(m);
            }
        }

        public void Dispose()
        {
            _dbContext.Dispose();
        }

        public void Add<TEntity>(IEnumerable<TEntity> items) where TEntity : class
        {
            items.ToList().ForEach(Add);
        }

        public void Add<TEntity>(TEntity item) where TEntity : class
        {
            _dbContext.Set<TEntity>().Add(item);
        }

        public void Remove<TEntity>(TEntity item) where TEntity : class
        {
            _dbContext.Set<TEntity>().Remove(item);
        }

        public void Remove<TEntity>(Expression<Func<TEntity, bool>> expression) where TEntity : class
        {
            var items = Query<TEntity>().Where(expression);
            Remove<TEntity>(items);
        }

        public void Remove<TEntity>(IEnumerable<TEntity> items) where TEntity : class
        {
            items.ToList().ForEach(Remove);
        }
    }
}