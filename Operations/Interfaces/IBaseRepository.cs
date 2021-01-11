using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Operations.Interfaces
{
    public interface IBaseRepository<T> where T : class
    {
        IQueryable<T> Get();
        T Add(T entity);
        bool Delete(string id);
    }
}
