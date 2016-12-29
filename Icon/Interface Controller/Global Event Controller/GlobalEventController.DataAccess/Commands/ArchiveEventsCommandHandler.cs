using FastMember;
using GlobalEventController.Common;
using GlobalEventController.DataAccess.Infrastructure;
using Icon.Framework;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace GlobalEventController.DataAccess.Commands
{
    public class ArchiveEventsCommandHandler : ICommandHandler<ArchiveEventsCommand>
    {
        private IContextManager contextManager;

        public ArchiveEventsCommandHandler(IContextManager contextManager)
        {
            this.contextManager = contextManager;
        }

        public void Handle(ArchiveEventsCommand command)
        {
            using (var bulkInsert = new SqlBulkCopy(contextManager.IconContext.Database.Connection.ConnectionString))
            {
                bulkInsert.DestinationTableName = "app.EventQueueArchive";
                using (var reader = ObjectReader.Create(
                    command.Events,
                    nameof(EventQueueArchive.EventQueueArchiveId),
                    nameof(EventQueueArchive.QueueId),
                    nameof(EventQueueArchive.EventId),
                    nameof(EventQueueArchive.EventMessage),
                    nameof(EventQueueArchive.EventReferenceId),
                    nameof(EventQueueArchive.RegionCode),
                    nameof(EventQueueArchive.EventQueueInsertDate),
                    nameof(EventQueueArchive.Context),
                    nameof(EventQueueArchive.ErrorCode),
                    nameof(EventQueueArchive.ErrorDetails)))
                {
                    bulkInsert.WriteToServer(reader);
                }
            }
        }
    }
}
