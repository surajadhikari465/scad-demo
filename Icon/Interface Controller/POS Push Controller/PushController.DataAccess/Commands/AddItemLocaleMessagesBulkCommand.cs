using Icon.Framework;
using System.Collections.Generic;

namespace PushController.DataAccess.Commands
{
    public class AddItemLocaleMessagesBulkCommand
    {
        public List<MessageQueueItemLocale> Messages { get; set; }
    }
}
