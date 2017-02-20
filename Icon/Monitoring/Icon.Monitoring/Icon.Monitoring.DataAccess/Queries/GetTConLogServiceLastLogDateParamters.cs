using Icon.Common.DataAccess;
using Icon.Monitoring.Common.Dvo;
using System;

namespace Icon.Monitoring.DataAccess.Queries
{
    public class GetTConLogServiceLastLogDateParameters : IQuery<string>
    {
        public bool EmptyParameter { get; set; }
    }
}
