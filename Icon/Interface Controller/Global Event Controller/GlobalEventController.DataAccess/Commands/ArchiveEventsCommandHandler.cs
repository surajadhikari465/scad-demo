using FastMember;
using GlobalEventController.Common;
using GlobalEventController.DataAccess.Infrastructure;
using Icon.DbContextFactory;
using Icon.Framework;
using Icon.Logging;
using System;
using System.Data.SqlClient;
using System.Linq;

namespace GlobalEventController.DataAccess.Commands
{
    public class ArchiveEventsCommandHandler : ICommandHandler<ArchiveEventsCommand>
    {
        private IDbContextFactory<IconContext> contextFactory;
        private ILogger<ArchiveEventsCommandHandler> logger;
        public ArchiveEventsCommandHandler(IDbContextFactory<IconContext> contextFactory, ILogger<ArchiveEventsCommandHandler> logger)
        {
            this.contextFactory = contextFactory;
            this.logger = logger;
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
                foreach (var eventQueue in command.Events)
                {
                    if (!string.IsNullOrEmpty(eventQueue.ErrorDetails) && !string.IsNullOrEmpty(eventQueue.EventMessage))
                    {
                        if (eventQueue.ErrorDetails.Contains("ScanCodeExistsConstraint"))
                        {
                            IRMAItemSubscription irmaItemSubscription = context.IRMAItemSubscription
                                .Where(a => a.identifier.Equals(eventQueue.EventMessage)
                                                     && a.regioncode == eventQueue.RegionCode
                                                     && a.deleteDate == null)
                                .FirstOrDefault();
                                
                            if (irmaItemSubscription != null)
                            {
                                irmaItemSubscription.deleteDate = DateTime.Now;
                                logger.Error(String.Format(
                                    "Error was caught and the subscription was marked as deleted for Errorcode:ScanCodeExistsConstraint.Identifier {0}, RegionCode {1}",
                                    irmaItemSubscription.identifier, irmaItemSubscription.regioncode));
                            }
                        }
                    }
                }
                context.SaveChanges();

            }
        }
    }
}