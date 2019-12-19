using System.Collections.Generic;

namespace Icon.Web.DataAccess.Commands
{
    public class ReprocessIconItemChangesCommand
    {
        public List<int> IconItemChangeIds { get; set; }
        public string RegionalConnectionStringName { get; set; }
    }
}
