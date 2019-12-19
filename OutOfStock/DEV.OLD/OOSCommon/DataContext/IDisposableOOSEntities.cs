using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OOSCommon.DataContext
{
    public interface IDisposableOOSEntities : IOOSEntities, IDisposable
    {
        void SaveChanges();
    }
}
