using System.Collections.Generic;

namespace Icon.Web.DataAccess.Commands
{
    public class ReprocessFailedIconPosPushPublishCommand
    {
        public List<int> IconPosPushPublishIds { get; set; }
        public string RegionalConnectionStringName { get; set; }
    }
}
