using System.Collections.Generic;

namespace WebSupport.DataAccess.Commands
{
    public class MassInsertToEPlumQueueCommand
    {
        public string Region { get; set; }

        public IEnumerable<string> BusinessUnitIds { get; set; }
    }
}
