using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icon.RenewableContext
{
    public interface IRenewableContext
    {
        void Refresh();
        void Refresh(object parameters);
    }

    public interface IRenewableContext<T> : IRenewableContext where T : DbContext, new()
    {
        T Context { get; set; }
    }
}
