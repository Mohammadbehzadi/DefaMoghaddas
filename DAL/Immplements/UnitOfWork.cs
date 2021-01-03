using System;
using System.Data.Entity;
using DAL.Interfaces;

namespace DAL.Immplements
{
    class UnitOfWork<TContext> : IUnitOfWork, IDisposable where TContext : DbContext
    {
        private readonly DbContextTransaction _transaction;
        private readonly TContext _context;
        private bool _disposed;

        public UnitOfWork(TContext context)
        {
            _context = context;
            _transaction = _context.Database.BeginTransaction();
        }
        public TContext Context
        {
            get { return _context; }
        }
        public void Commit()
        {
            _context.SaveChanges();
            _transaction.Commit();
        }
        public void Rollback()
        {
            _transaction.Rollback();
        }
        public void SaveChanges()
        {
            _context.SaveChanges();
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _transaction.Dispose();
                }
            }
            _disposed = true;
        }
    }
}
