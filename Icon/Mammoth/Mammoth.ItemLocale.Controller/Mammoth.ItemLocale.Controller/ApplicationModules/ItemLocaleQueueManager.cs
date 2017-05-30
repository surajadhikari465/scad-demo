using Mammoth.Common;
using Mammoth.Common.ControllerApplication;
using Mammoth.Common.ControllerApplication.Models;
using Mammoth.Common.DataAccess.CommandQuery;
using Mammoth.Common.DataAccess.Models;
using Mammoth.Common.Email;
using Mammoth.ItemLocale.Controller.DataAccess.Commands;
using Mammoth.ItemLocale.Controller.DataAccess.Models;
using Mammoth.ItemLocale.Controller.DataAccess.Queries;
using Mammoth.Logging;
using MoreLinq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Mammoth.ItemLocale.Controller.ApplicationModules
{
    public class ItemLocaleQueueManager : IQueueManager<ItemLocaleEventModel>
    {
        private ItemLocaleControllerApplicationSettings settings;
        private IQueryHandler<UpdateAndGetEventQueueInProcessQuery, List<EventQueueModel>> updateAndGetEventQueueInProcessQueryHandler;
        private IQueryHandler<GetItemLocaleDataParameters, List<ItemLocaleEventModel>> getItemLocaleEventsQueryHandler;
        private ICommandHandler<DeleteEventQueueCommand> deleteEventQueueCommandHandler;
        private ICommandHandler<ArchiveEventsCommand> archiveEventsCommandHandler;
        private IEmailClient emailClient;
        private IEmailBuilder emailBuilder;
        private ILogger logger;

        public ItemLocaleQueueManager(ItemLocaleControllerApplicationSettings settings,
            IQueryHandler<UpdateAndGetEventQueueInProcessQuery, List<EventQueueModel>> updateAndGetEventQueueInProcessQueryHandler,
            IQueryHandler<GetItemLocaleDataParameters, List<ItemLocaleEventModel>> getItemLocaleEventsQueryHandler,
            ICommandHandler<DeleteEventQueueCommand> deleteEventQueueCommandHandler,
            ICommandHandler<ArchiveEventsCommand> archiveEventsCommandHandler,
            IEmailClient emailClient,
            IEmailBuilder emailBuilder,
            ILogger logger)
        {
            this.settings = settings;
            this.updateAndGetEventQueueInProcessQueryHandler = updateAndGetEventQueueInProcessQueryHandler;
            this.getItemLocaleEventsQueryHandler = getItemLocaleEventsQueryHandler;
            this.deleteEventQueueCommandHandler = deleteEventQueueCommandHandler;
            this.archiveEventsCommandHandler = archiveEventsCommandHandler;
            this.emailClient = emailClient;
            this.emailBuilder = emailBuilder;
            this.logger = logger;
        }

        public ChangeQueueEvents<ItemLocaleEventModel> Get()
        {
            var updateQueueParameters = new UpdateAndGetEventQueueInProcessQuery
            {
                Instance = this.settings.Instance,
                MaxNumberOfRowsToMark = this.settings.MaxNumberOfRowsToMark
            };
            List<EventQueueModel> queuedEvents = this.updateAndGetEventQueueInProcessQueryHandler.Search(updateQueueParameters);

            ChangeQueueEvents<ItemLocaleEventModel> response = new ChangeQueueEvents<ItemLocaleEventModel>();

            if (queuedEvents.Any())
            {
                response.QueuedEvents = queuedEvents;

                // Get itemLocale data for ItemLocale Add Or Update and ItemDelete/Deauthorization events
                List<ItemLocaleEventModel> itemLocales = getItemLocaleEventsQueryHandler.Search(new GetItemLocaleDataParameters
                {
                    Instance = this.settings.Instance,
                    Region = this.settings.CurrentRegion
                });

                if(itemLocales.Any())
                {
                    response.EventModels = itemLocales;
                }

                logger.Info(String.Format("Retrieved {0} ItemLocale rows for {1} Region for {2} distinct QueueIDs.",
                    itemLocales.Count.ToString(),
                    this.settings.CurrentRegion,
                    itemLocales.DistinctBy(il => il.QueueId).Count().ToString()));
            }

            return response;
        }

        public void Finalize(ChangeQueueEvents<ItemLocaleEventModel> queueRecords)
        {
            if (queueRecords.QueuedEvents.Any())
            {
                DeleteEvents(queueRecords);
                ArchiveEvents(queueRecords);
                SendAlert(queueRecords);
            }
        }

        private void DeleteEvents(ChangeQueueEvents<ItemLocaleEventModel> queueRecords)
        {
            deleteEventQueueCommandHandler.Execute(new DeleteEventQueueCommand
            {
                QueueIds = queueRecords.QueuedEvents.Select(q => q.QueueId)
            });
            logger.Info(String.Format("Deleted {0} Mammoth ItemLocale Events for {1} Region.", queueRecords.QueuedEvents.Count(), settings.CurrentRegion));
        }

        private void ArchiveEvents(ChangeQueueEvents<ItemLocaleEventModel> changeQueueEvents)
        {
            var processedEvents = changeQueueEvents.EventModels
                .Join(
                    changeQueueEvents.QueuedEvents,
                    itemLocaleEventModel => itemLocaleEventModel.QueueId,
                    queuedEventModel => queuedEventModel.QueueId,
                    (itemLocaleEventModel, queuedEventModel) => new ChangeQueueHistoryModel
                    {
                        EventTypeId = itemLocaleEventModel.EventTypeId,
                        Identifier = queuedEventModel.Identifier,
                        Item_Key = queuedEventModel.ItemKey,
                        Store_No = itemLocaleEventModel.StoreNo,
                        QueueID = queuedEventModel.QueueId,
                        EventReferenceId = queuedEventModel.EventReferenceId,
                        QueueInsertDate = queuedEventModel.InsertDate,
                        InsertDate = DateTime.Now,
                        MachineName = Environment.MachineName,
                        Context = JsonConvert.SerializeObject(new { Region = settings.CurrentRegion, QueueModel = queuedEventModel, ItemLocaleModel = itemLocaleEventModel }),
                        ErrorCode = itemLocaleEventModel.ErrorMessage,
                        ErrorDetails = ExceptionManager.GetErrorDescription(itemLocaleEventModel.ErrorMessage)
                    });


            //Queued events can be excluded from the result set of the database query because of inner joins. In this case 
            //we still want to archive what events were read by the controller so that we can have a history of what couldn't be sent.
            var unprocessableQueueEvents = changeQueueEvents.QueuedEvents
                .Where(e => !processedEvents.Any(qe => qe.QueueID == e.QueueId))
                .Select(e => new ChangeQueueHistoryModel
                {
                    EventTypeId = e.EventTypeId,
                    Identifier = e.Identifier,
                    Item_Key = e.ItemKey,
                    Store_No = e.StoreNo,
                    QueueID = e.QueueId,
                    EventReferenceId = e.EventReferenceId,
                    QueueInsertDate = e.InsertDate,
                    InsertDate = DateTime.Now,
                    MachineName = Environment.MachineName,
                    Context = JsonConvert.SerializeObject(new { Region = settings.CurrentRegion, QueueModel = e }),
                    ErrorCode = ApplicationErrors.UnprocessableEventCode,
                    ErrorDetails = ApplicationErrors.UnprocessableEventDescription
                });

            List<ChangeQueueHistoryModel> changeQueueHistoryModels = processedEvents.Concat(unprocessableQueueEvents).ToList();

            archiveEventsCommandHandler.Execute(new ArchiveEventsCommand
            {
                Events = changeQueueHistoryModels
            });
        }

        private void SendAlert(ChangeQueueEvents<ItemLocaleEventModel> queueRecords)
        {
            var failedEvents = queueRecords.EventModels
                .Where(q => !String.IsNullOrEmpty(q.ErrorMessage) 
                    && !settings.NonAlertErrors.Contains(q.ErrorMessage))
                .Select(em => new
                {
                    Resolution = "Perform Mammoth ItemLocale Refresh",
                    Region = em.Region,
                    ScanCode = em.ScanCode,
                    BusinessUnitId = em.BusinessUnitId,
                    QueueId = em.QueueId,
                    EventType = em.EventTypeId == Constants.EventTypes.ItemLocaleAddOrUpdate ? "ItemLocale AddOrUpdate" : "Item Delete",
                    ErrorMessage = em.ErrorMessage,
                    ExceptionSource = em.ErrorSource,
                    ExceptionType = em.ErrorDetails.IsJsonString() ? (string)JObject.Parse(em.ErrorDetails)[Constants.ExceptionProperties.ExceptionType] : em.ErrorDetails,
                    ExceptionMessage = em.ErrorDetails.IsJsonString() ? (string)JObject.Parse(em.ErrorDetails)[Constants.ExceptionProperties.ExceptionMessage] : em.ErrorDetails
                });


            if (failedEvents.Any())
            {
                emailClient.Send(emailBuilder
                    .BuildEmail(failedEvents.ToList(), "The following ItemLocale changes had errors when being inserted/updated in Mammoth. " +
                        "The resolution details are provided below. " + Environment.NewLine +
                        "The regional IRMA table 'mammoth.ChangeQueueHistory' will provide more details if necessary. " +
                        "Additionally the logs in the Mammoth database are available."),
                    "Mammoth ItemLocale Error - ACTION REQUIRED");
            }
        }
    }
}
