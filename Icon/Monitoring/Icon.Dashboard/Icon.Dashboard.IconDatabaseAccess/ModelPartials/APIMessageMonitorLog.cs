using Icon.Dashboard.CommonDatabaseAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icon.Dashboard.IconDatabaseAccess
{
    public partial class APIMessageMonitorLog : IAPIMessageMonitorLog
    {

        public string MessageTypeName
        {
            get
            {
                return this.MessageType?.MessageTypeName;
            }
        }
    }
}
