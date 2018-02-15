using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icon.Monitoring.Common
{
    public interface IMonitorCache
    {
        object Get(string key);
        void Set(string key, object item, DateTimeOffset offset);
        bool Contains(string key);
    }
}
