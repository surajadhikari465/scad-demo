using Icon.Common.DataAccess;
using Icon.Monitoring.DataAccess.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icon.Monitoring.DataAccess.Queries
{
    public class GetAppLogByAppAndMessageParameters : IQuery<AppLog>
    {
        public string Message { get; set; }
        public string AppConfigAppName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
