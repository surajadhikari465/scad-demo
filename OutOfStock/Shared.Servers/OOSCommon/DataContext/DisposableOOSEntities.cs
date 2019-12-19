using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OOSCommon.DataContext
{
    public class DisposableOOSEntities : OOSEntities, IDisposableOOSEntities
    {
        public DisposableOOSEntities(string oosRepositoryConnectionString) : base(oosRepositoryConnectionString)
        {}

        public new void SaveChanges()
        {
            base.SaveChanges();
        }
    }
}
