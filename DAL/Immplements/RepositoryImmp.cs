using System;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using DAL.Interfaces;
using Domain.ShareModels;

namespace DAL.Immplements
{
    public class RepositoryImmp<TContext, TEntity> : RepositoryContext<TContext, TEntity>
        where TContext : DbContext, new()
        where TEntity : class
    {
        public RepositoryImmp(TContext context) : base(context) { }
        public override async Task<OperationResult> AddAsync(TEntity item)
        {
            Context.Entry(item).State = EntityState.Added;
            var task = Task.Factory.StartNew(() => SaveChanges());
            task.Wait();
            return await task;
        }
        public override OperationResult Add(TEntity item)
        {
            Context.Entry(item).State = EntityState.Added;
            return SaveChanges();
        }
      

        
        public override async Task<OperationResult> AddAsync(TEntity[] items)
        {
            foreach (var item in items)
                Context.Entry(item).State = EntityState.Added;

            var task = Task.Factory.StartNew(() => SaveChanges());
            task.Wait();
            return await task;
        }
        public override OperationResult Add(TEntity[] items)
        {
            foreach (var item in items)
                Context.Entry(item).State = EntityState.Added;

            return SaveChanges();
        }
        public override async Task<OperationResult> RemoveAsync(TEntity item)
        {
            Context.Entry(item).State = EntityState.Deleted;
            var task = Task.Factory.StartNew(() => SaveChanges());
            task.Wait();
            return await task;
        }
        public override OperationResult Remove(TEntity item)
        {
            Context.Entry(item).State = EntityState.Deleted;
            return SaveChanges();
        }
        public override async Task<OperationResult> RemoveAsync(TEntity[] items)
        {
            foreach (var item in items)
                Context.Entry(item).State = EntityState.Deleted;

            var task = Task.Factory.StartNew(() => SaveChanges());
            task.Wait();
            return await task;
        }
        public override OperationResult Remove(TEntity[] items)
        {
            foreach (var item in items)
                Context.Entry(item).State = EntityState.Deleted;

            return SaveChanges();
        }
        public override async Task<OperationResult> ModifyAsync(TEntity item)
        {
            Context.Entry(item).State = EntityState.Modified;

            var task = Task.Factory.StartNew(() => SaveChanges());
            task.Wait();
            return await task;
        }
        public override OperationResult Modify(TEntity item)
        {
            Context.Entry(item).State = EntityState.Modified;
            return SaveChanges();
        }
        public override async Task<OperationResult> ModifyAsync(TEntity[] items)
        {
            foreach (var item in items)
                Context.Entry(item).State = EntityState.Modified;

            var task = Task.Factory.StartNew(() => SaveChanges());
            task.Wait();
            return await task;
        }
        public override OperationResult Modify(TEntity[] items)
        {
            foreach (var item in items)
                Context.Entry(item).State = EntityState.Modified;

            return SaveChanges();
        }
        public override async Task<TEntity> FindItemAsync(Expression<Func<TEntity, bool>> predicate)
        {
            try
            {
                var task = DbSet.SingleAsync(predicate);
                task.Wait();
                return await task;
            }
            catch (Exception)
            {
                return null;
            }
        }
        public override TEntity FindItem(Expression<Func<TEntity, bool>> predicate)
        {
            try
            {
                return DbSet.Single(predicate);
            }
            catch (Exception)
            {
                return null;
            }
        }
        public override async Task<IQueryable<TEntity>> FindItemsAsync(Expression<Func<TEntity, bool>> predicate)
        {
            try
            {
                var task = Task.Factory.StartNew(() => DbSet.Where(predicate));
                task.Wait();
                return await task;
            }
            catch (Exception)
            {
                return null;
            }
        }
        public override IQueryable<TEntity> FindItems(Expression<Func<TEntity, bool>> predicate)
        {
            try
            {
                var findItems = DbSet.Where(predicate);

                return findItems;
            }
            catch (Exception)
            {
                return null;
            }
        }
        public override IQueryable<TEntity> GetAll()
        {
            try
            {
                return DbSet;
            }
            catch (Exception)
            {
                return null;
            }
        }
        private new OperationResult SaveChanges()
        {
            using (IUnitOfWork unitOfWork = new UnitOfWork<TContext>(Context))
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
