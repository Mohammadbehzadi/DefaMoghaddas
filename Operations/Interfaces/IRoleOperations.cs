using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Operations.Interfaces
{
    public interface IRoleOperations<T> : IBaseRepository<T> where T : class
    {
        IdentityRole Update(IdentityRole entity);
    }
}
