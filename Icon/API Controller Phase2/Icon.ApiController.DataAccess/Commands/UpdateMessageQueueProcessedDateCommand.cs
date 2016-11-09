using System;
using System.Collections.Generic;

namespace Icon.ApiController.DataAccess.Commands
{
    public class UpdateMessageQueueProcessedDateCommand<T>
    {
        public List<T> MessagesToUpdate { get; set; }
        public DateTime ProcessedDate { get; set; }
    }
}
