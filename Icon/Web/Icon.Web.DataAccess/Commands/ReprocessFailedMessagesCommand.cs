using System.Collections.Generic;

namespace Icon.Web.DataAccess.Commands
{
    public class ReprocessFailedMessagesCommand
    {
        public List<int> MessageHistoriesById { get; set; }
    }
}
