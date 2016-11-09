using System;
using System.Collections.Generic;

namespace Icon.ApiController.DataAccess.Commands
{
    public class UpdateMessageQueueStatusCommand<T>
    {
        public List<T> QueuedMessages { get; set; }
        public int MessageStatusId { get; set; }
        public bool ResetInProcessBy { get; set; }
    }
}
