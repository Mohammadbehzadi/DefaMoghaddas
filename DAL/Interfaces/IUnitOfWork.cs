using System;

namespace DAL.Interfaces
{
    interface IUnitOfWork : IDisposable
    {
        void Commit();
        void Rollback();
        void SaveChanges();
    }
}
