using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GlobalEventController.Common;
using Icon.Logging;
using GlobalEventController.DataAccess.Infrastructure;
using GlobalEventController.DataAccess.Commands;
using Icon.Framework;

namespace GlobalEventController.Controller.EventOperations
{
    public class EventArchiver : IEventArchiver
    {
        private ICommandHandler<ArchiveEventsCommand> archiveEventsCommandHandler;
        private ILogger<EventArchiver> logger;

        public List<EventQueueArchive> Events { get; set; }

        public EventArchiver(
            ICommandHandler<ArchiveEventsCommand> archiveEventsCommandHandler,
            ILogger<EventArchiver> logger)
        {
            this.archiveEventsCommandHandler = archiveEventsCommandHandler;
            this.logger = logger;
            this.Events = new List<EventQueueArchive>();
        }

        public void ArchiveEvents()
        {
            if (Events.Any())
            {
                archiveEventsCommandHandler.Handle(new ArchiveEventsCommand { Events = Events });
            }
        }

        public void ClearArchiveEvents()
        {
            Events.Clear();
        }
    }
}
