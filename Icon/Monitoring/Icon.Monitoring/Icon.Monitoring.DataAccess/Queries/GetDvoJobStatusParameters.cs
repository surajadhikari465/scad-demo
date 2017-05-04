using Icon.Common.DataAccess;
using Icon.Monitoring.Common.Dvo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icon.Monitoring.DataAccess.Queries
{
    public class GetDvoJobStatusParameters : IQuery<List<DvoRegionalJobStatus>>
    {
        public List<string> Regions { get; set; }
    }
}
