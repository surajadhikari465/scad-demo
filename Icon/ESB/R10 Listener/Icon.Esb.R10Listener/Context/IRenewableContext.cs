using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icon.Esb.R10Listener.Context
{
    public interface IRenewableContext<T> where T : DbContext
    {
        T Context { get; }
        void Refresh();
        void SaveChanges();
    }
}
