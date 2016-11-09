using Icon.Common.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icon.Monitoring.DataAccess.Queries
{
    public class GetIrmaPushIdParameters : IQuery<int>
    {
        public bool EmptyParameter { get; set; }
    }
}
