using System.Collections.Generic;

namespace Icon.Web.DataAccess.Commands
{
    public class ReprocessFailedLocaleMessageCommand
    {
        public List<int> MessageQueueIds { get; set; }
    }
}
