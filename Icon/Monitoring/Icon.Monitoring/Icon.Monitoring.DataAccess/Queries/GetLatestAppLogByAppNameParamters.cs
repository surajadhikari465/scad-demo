using Icon.Common.DataAccess;
using Icon.Monitoring.Common.Dvo;
using Icon.Monitoring.DataAccess.Model;
using System;

namespace Icon.Monitoring.DataAccess.Queries
{
    public class GetLatestAppLogByAppNameParameters : IQuery<AppLog>
    {
        public GetLatestAppLogByAppNameParameters(string appName)
        {
            this.AppName = appName;
        }
        public string AppName { get; set; }
 
    }
}
