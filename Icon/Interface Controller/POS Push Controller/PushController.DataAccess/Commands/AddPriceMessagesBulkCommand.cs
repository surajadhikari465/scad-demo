using Icon.Framework;
using System.Collections.Generic;

namespace PushController.DataAccess.Commands
{
    public class AddPriceMessagesBulkCommand
    {
        public List<MessageQueuePrice> Messages { get; set; }
    }
}
