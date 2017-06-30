using FastMember;
using GlobalEventController.Common;
using GlobalEventController.DataAccess.Infrastructure;
using Icon.DbContextFactory;
using Icon.Framework;
using System.Data.SqlClient;

namespace GlobalEventController.DataAccess.Commands
{
    public class ArchiveEventsCommandHandler : ICommandHandler<ArchiveEventsCommand>
    {
        private IDbContextFactory<IconContext> contextFactory;

        public ArchiveEventsCommandHandler(IDbContextFactory<IconContext> contextFactory)
        {
            this.contextFactory = contextFactory;
        }

        public void Handle(ArchiveEventsCommand command)
        {
            using (var context = contextFactory.CreateContext())
            {
                using (var sqlBulkCopy = new SqlBulkCopy(context.Database.Connection.ConnectionString))
                {
                    sqlBulkCopy.DestinationTableName = "app.EventQueueArchive";
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
                        sqlBulkCopy.WriteToServer(reader);
                    }
                }
            }
        }
    }
}
