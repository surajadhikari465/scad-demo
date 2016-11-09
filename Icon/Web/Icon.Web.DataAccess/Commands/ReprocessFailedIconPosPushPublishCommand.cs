using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icon.Web.DataAccess.Commands
{
    public class ReprocessFailedIconPosPushPublishCommand
    {
        public List<int> IconPosPushPublishIds { get; set; }
        public string RegionalConnectionStringName { get; set; }
    }
}
