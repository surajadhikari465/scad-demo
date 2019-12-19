using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OOSCommon.DataContext;

namespace OOS.Model
{
    public interface IOOSEntitiesFactory
    {
        IDisposableOOSEntities New();
    }
}
