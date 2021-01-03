using System;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using DAL.Immplements;
using Domain.ShareModels;

namespace DAL.Interfaces
{
    public abstract class RepositoryContext<TContext, TEntity>
        where TContext : DbContext, new()
        where TEntity : class
    {
        public TContext Context;
        protected readonly DbSet<TEntity> DbSet;
        protected RepositoryContext()
            : this(new TContext())
        {
        }
        protected RepositoryContext(TContext context)
        {
            Context = context;
            DbSet = Context.Set<TEntity>();
            Context.Configuration.LazyLoadingEnabled = false;
        }
        public abstract Task<OperationResult> AddAsync(TEntity item);
        public abstract OperationResult Add(TEntity item);
        public abstract Task<OperationResult> AddAsync(TEntity[] items);
        public abstract OperationResult Add(TEntity[] items);
        public abstract Task<OperationResult> RemoveAsync(TEntity item);
        public abstract OperationResult Remove(TEntity item);
        public abstract Task<OperationResult> RemoveAsync(TEntity[] items);
        public abstract OperationResult Remove(TEntity[] items);
        public abstract Task<OperationResult> ModifyAsync(TEntity item);
        public abstract OperationResult Modify(TEntity item);
        public abstract Task<OperationResult> ModifyAsync(TEntity[] items);
        public abstract OperationResult Modify(TEntity[] items);
        public abstract Task<TEntity> FindItemAsync(Expression<Func<TEntity, bool>> predicate);
        public abstract TEntity FindItem(Expression<Func<TEntity, bool>> predicate);
        public abstract Task<IQueryable<TEntity>> FindItemsAsync(Expression<Func<TEntity, bool>> predicate);
        public abstract IQueryable<TEntity> FindItems(Expression<Func<TEntity, bool>> predicate);
        public abstract IQueryable<TEntity> GetAll();
        public OperationResult SaveChanges()
        {
            using (var unitOfWork = new UnitOfWork<TContext>(Context))
            {
                try
                {
                    unitOfWork.Commit();
                    return OperationResult.Success;
                }
                catch (DbEntityValidationException exception)
                {
                    unitOfWork.Rollback();
                    return OperationResult.Failed(exception.Message);
                }
                catch (Exception exception)
                {
                    unitOfWork.Rollback();
                    return OperationResult.Failed(exception.Message);
                }
            }
        }

      

       
    }
}
