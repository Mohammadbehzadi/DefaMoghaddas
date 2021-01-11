using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Operations.Interfaces
{
    public interface IEntityRepository<T>

    {
        IQueryable<T> Get();
        IQueryable<T> Get(Expression<Func<T, bool>> predicate);
        T Add(T entity);
        T Update(T entity);
        void Delete(T entity);
    }
}
