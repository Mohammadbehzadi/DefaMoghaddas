using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace Operations.Immplements
{
    public class EntityRepository<T> : IEntityRepository<T> where T : class
    {
        public readonly AppIdentityDbContext Context;
        private readonly DbSet<T> _dbSet;

        public EntityRepository(AppIdentityDbContext context)
        {
            Context = context;
            context.Configuration.AutoDetectChangesEnabled = true;
            _dbSet = context.Set<T>();
        }

        public IQueryable<T> Get()
        {
            return _dbSet;
        }

        public IQueryable<T> Get(Expression<Func<T, bool>> predicate)
        {
            return _dbSet.Where(predicate);
        }

        public T Add(T entity)
        {
            var _entity = _dbSet.Add(entity);
            Context.SaveChanges();
            return _entity;
        }

        public T Update(T entity)
        {
            Context.Entry(entity).State = EntityState.Modified;
            Context.SaveChanges();
            return entity;
        }

        public void Delete(T entity)
        {
            Context.Entry(entity).State = EntityState.Deleted;
            Context.SaveChanges();
        }
    }
}