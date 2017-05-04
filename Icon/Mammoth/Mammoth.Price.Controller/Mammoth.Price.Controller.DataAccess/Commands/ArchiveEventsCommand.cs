using Mammoth.Common.DataAccess.Models;
using System.Collections.Generic;

namespace Mammoth.Price.Controller.DataAccess.Commands
{
    public class ArchiveEventsCommand
    {
        public IEnumerable<ChangeQueueHistoryModel> Events { get; set; }
    }
}
