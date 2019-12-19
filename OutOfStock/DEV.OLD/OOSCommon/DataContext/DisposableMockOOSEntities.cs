using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OOSCommon.DataContext
{
    public class DisposableMockOOSEntities : OOSEntitiesMock, IDisposableOOSEntities
    {
        public void Dispose()
        {}

        public void SaveChanges()
        {}
    }
}
