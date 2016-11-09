using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SupportMonitor.Core.Repositories
{
    public interface IRepository<T>
    {
        IEnumerable<T> All();
    }
}
