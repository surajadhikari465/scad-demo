using Icon.Dashboard.CommonDatabaseAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icon.Dashboard.IconDatabaseAccess
{
    public interface IIconDatabaseService : IDashboardAppLogger, IDashboardApiJobMonitor
    {
    }
}
