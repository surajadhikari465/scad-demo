using FastMember;
using GlobalEventController.Common;
using GlobalEventController.DataAccess.Infrastructure;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalEventController.DataAccess.Commands
{
    public class ArchiveFailedEventsCommandHandler : ICommandHandler<ArchiveFailedEventsCommand>
    {
        private IContextManager contextManager;

        public ArchiveFailedEventsCommandHandler(IContextManager contextManager)
        {
            this.contextManager = contextManager;
        }

        public void Handle(ArchiveFailedEventsCommand command)
        {
            using (var bulkInsert = new SqlBulkCopy(contextManager.IconContext.Database.Connection.ConnectionString))
            {
                bulkInsert.DestinationTableName = "app.EventQueueArchive";
                using (var reader = ObjectReader.Create(
                    ConvertToArchiveModel(command.FailedEvents),
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

        private List<EventQueueArchive> ConvertToArchiveModel(List<ArchiveEventModelWrapper<FailedEvent>> failedEvents)
        {
            return failedEvents.Select(e => new EventQueueArchive
            {
                EventId = e.EventModel.Event.EventId,
                Context = JsonConvert.SerializeObject(e.EventModel),
                EventMessage = e.EventModel.Event.EventMessage,
                EventQueueInsertDate = e.EventModel.Event.InsertDate,
                EventReferenceId = e.EventModel.Event.EventReferenceId,
                QueueId = e.EventModel.Event.QueueId,
                RegionCode = e.EventModel.Event.RegionCode,
                ErrorCode = e.ErrorCode,
                ErrorDetails = e.ErrorDetails
            }).ToList();
        }
    }
}
