using Mammoth.Common.ControllerApplication;
using Mammoth.Common.ControllerApplication.Models;
using Mammoth.Common.DataAccess.CommandQuery;
using Mammoth.Common.DataAccess.Models;
using Mammoth.Logging;
using Mammoth.Price.Controller.DataAccess.Commands;
using Mammoth.Price.Controller.DataAccess.Models;
using Mammoth.Price.Controller.DataAccess.Queries;
using MoreLinq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Mammoth.Price.Controller.ApplicationModules
{
    public class PriceQueueManager: IQueueManager<PriceEventModel>
    {
        private PriceControllerApplicationSettings settings;
        private IQueryHandler<UpdateAndGetEventQueueInProcessParameters, List<EventQueueModel>> updateAndGetEventQueueInProcessQueryHandler;
        private IQueryHandler<GetPriceDataParameters, List<PriceEventModel>> getPricesQueryHandler;
        private IQueryHandler<GetExistingPriceDataParameters, List<PriceEventModel>> getExistingPricesQueryHandler;
        private ICommandHandler<DeleteEventQueueCommand> deleteEventQueueCommandHandler;
        private ICommandHandler<ArchiveEventsCommand> archiveEventsCommandHandler;
        private ILogger logger;

        public PriceQueueManager(PriceControllerApplicationSettings settings,
            IQueryHandler<UpdateAndGetEventQueueInProcessParameters, List<EventQueueModel>> updateAndGetEventQueueInProcessQueryHandler,
            IQueryHandler<GetPriceDataParameters, List<PriceEventModel>> getPricesQueryHandler,
            IQueryHandler<GetExistingPriceDataParameters, List<PriceEventModel>> getExistingPricesQueryHandler,
            ICommandHandler<DeleteEventQueueCommand> deleteEventQueueCommandHandler,
            ICommandHandler<ArchiveEventsCommand> archiveEventsCommandHandler,
            ILogger logger)
        {
            this.settings = settings;
            this.updateAndGetEventQueueInProcessQueryHandler = updateAndGetEventQueueInProcessQueryHandler;
            this.getPricesQueryHandler = getPricesQueryHandler;
            this.getExistingPricesQueryHandler = getExistingPricesQueryHandler;
            this.deleteEventQueueCommandHandler = deleteEventQueueCommandHandler;
            this.archiveEventsCommandHandler = archiveEventsCommandHandler;
            this.logger = logger;
        }

        public ChangeQueueEvents<PriceEventModel> Get()
        {
            var updateQueueParameters = new UpdateAndGetEventQueueInProcessParameters
            {
                Instance = this.settings.Instance,
                MaxNumberOfRowsToMark = this.settings.MaxNumberOfRowsToMark
            };
            List<EventQueueModel> queuedEvents = this.updateAndGetEventQueueInProcessQueryHandler.Search(updateQueueParameters);

            ChangeQueueEvents<PriceEventModel> response = new ChangeQueueEvents<PriceEventModel>();

            if (queuedEvents.Any())
            {
                GetPriceDataParameters getPriceParameters = new GetPriceDataParameters();
                getPriceParameters.Instance = this.settings.Instance;
                getPriceParameters.Region = this.settings.CurrentRegion;

                GetExistingPriceDataParameters getExistingPriceParameters = new GetExistingPriceDataParameters();
                getExistingPriceParameters.Instance = this.settings.Instance;
                getExistingPriceParameters.Region = this.settings.CurrentRegion;

                List<PriceEventModel> priceData = getPricesQueryHandler.Search(getPriceParameters);
                List<PriceEventModel> existingPriceData = getExistingPricesQueryHandler.Search(getExistingPriceParameters);

                var prices = priceData.Concat(existingPriceData).ToList();

                response.QueuedEvents = queuedEvents;
                response.EventModels = prices;

                logger.Info(String.Format("Retrieved {0} Price rows for {1} Region for {2} distinct QueueIDs.",
                    prices.Count.ToString(),
                    this.settings.CurrentRegion,
                    prices.DistinctBy(il => il.QueueId).Count().ToString()));

            }

            return response;
        }

        public void Finalize(ChangeQueueEvents<PriceEventModel> queueRecords)
        {
            if (queueRecords.QueuedEvents.Any())
            {
                DeleteEvents(queueRecords);
                ArchiveEvents(queueRecords);
            }
        }

        private void DeleteEvents(ChangeQueueEvents<PriceEventModel> queueRecords)
        {
            deleteEventQueueCommandHandler.Execute(new DeleteEventQueueCommand
            {
                QueueIds = queueRecords.QueuedEvents.Select(q => q.QueueId)
            });
            logger.Info(String.Format("Deleted {0} Mammoth Price Events for {1} Region.", queueRecords.QueuedEvents.Count(), settings.CurrentRegion));
        }

        private void ArchiveEvents(ChangeQueueEvents<PriceEventModel> changeQueueEvents)
        {
            var processedEvents = changeQueueEvents.EventModels
                .Join(
                    changeQueueEvents.QueuedEvents,
                    priceEventModel => priceEventModel.QueueId,
                    queuedEventModel => queuedEventModel.QueueId,
                    (priceEventModel, queuedEventModel) => new ChangeQueueHistoryModel
                    {
                        EventTypeId = priceEventModel.EventTypeId,
                        Identifier = queuedEventModel.Identifier,
                        Item_Key = queuedEventModel.ItemKey,
                        Store_No = queuedEventModel.StoreNo,
                        QueueID = queuedEventModel.QueueId,
                        EventReferenceId = queuedEventModel.EventReferenceId,
                        QueueInsertDate = queuedEventModel.InsertDate,
                        InsertDate = DateTime.Now,
                        MachineName = Environment.MachineName,
                        Context = JsonConvert.SerializeObject(new { Region = settings.CurrentRegion, QueueModel = queuedEventModel, PriceModel = priceEventModel }),
                        ErrorCode = priceEventModel.ErrorMessage,
                        ErrorDetails = priceEventModel.ErrorMessage
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
                }).ToList();

            List<ChangeQueueHistoryModel> changeQueueHistoryModels = processedEvents.Concat(unprocessableQueueEvents).ToList();

            archiveEventsCommandHandler.Execute(new ArchiveEventsCommand
            {
                Events = changeQueueHistoryModels
            });
        }
    }
}
