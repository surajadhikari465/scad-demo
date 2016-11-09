using Icon.Dashboard.CommonDatabaseAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icon.Dashboard.IconDatabaseAccess
{

    public partial class AppLog : IAppLog
    {
        public string AppName
        {
            get
            {
                return this.App?.AppName;
            }
        }

        public DateTime LoggingTimestamp
        {
            get
            {
                return this.LogDate;
            }
        }
    }
}
