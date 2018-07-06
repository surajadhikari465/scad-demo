using Mammoth.Common;
using Mammoth.Common.ControllerApplication;
using Mammoth.Common.ControllerApplication.Models;
using Mammoth.Common.ControllerApplication.Services;
using Mammoth.Common.DataAccess.CommandQuery;
using Mammoth.Common.DataAccess.Models;
using Mammoth.Logging;
using Mammoth.Price.Controller.ApplicationModules;
using Mammoth.Price.Controller.Common;
using Mammoth.Price.Controller.DataAccess.Commands;
using Mammoth.Price.Controller.DataAccess.Models;
using Mammoth.Price.Controller.DataAccess.Queries;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Mammoth.Price.Controller.EventProcessors
{
    public class PriceEventProcessor : IEventProcessor<PriceEventModel>
    {
        private PriceControllerApplicationSettings settings;
        private IQueryHandler<GetPriceDataParameters, List<PriceEventModel>> getPricesQueryHandler;
        private IQueryHandler<GetExistingPriceDataParameters, List<PriceEventModel>> getExistingPricesQueryHandler;
        private IService<PriceEventModel> service;
        private ICommandHandler<ArchiveEventsCommand> archiveEventsCommandHandler;
        private IErrorAlerter errorAlerter;
        private ILogger logger;

        public PriceEventProcessor(
            PriceControllerApplicationSettings settings,
            IQueryHandler<GetPriceDataParameters, List<PriceEventModel>> getPricesQueryHandler,
            IQueryHandler<GetExistingPriceDataParameters, List<PriceEventModel>> getExistingPricesQueryHandler,
            IService<PriceEventModel> service,
            ICommandHandler<ArchiveEventsCommand> archiveEventsCommandHandler,
            IErrorAlerter errorAlerter,
            ILogger logger)
        {
            this.settings = settings;
            this.getPricesQueryHandler = getPricesQueryHandler;
            this.getExistingPricesQueryHandler = getExistingPricesQueryHandler;
            this.service = service;
            this.archiveEventsCommandHandler = archiveEventsCommandHandler;
            this.errorAlerter = errorAlerter;
            this.logger = logger;
        }

        public void ProcessEvents(List<EventQueueModel> events)
        {
            var priceEvents = events
                .Where(m => m.EventTypeId == Constants.EventTypes.Price || m.EventTypeId == Constants.EventTypes.PriceRollback)
                .ToList();

            if (priceEvents.Count > 0)
            {
                try
                {
                    List<PriceEventModel> prices = GetPrices();
                    service.Process(prices);
                    events.RemoveAll(m => priceEvents.Contains(m));
                    ArchiveEvents(prices, priceEvents);
                    if (prices.Any(p => !string.IsNullOrWhiteSpace(p.ErrorMessage)))
                    {
                        errorAlerter.AlertErrors(prices.Where(p => !string.IsNullOrWhiteSpace(p.ErrorMessage)).ToList());
                    }
                }
                catch(Exception ex)
                {
                    logger.Error(new { Region = settings.CurrentRegion, Message = "Error occurred while processing Price Events." }.ToJson(), ex);
                }
            }
        }

        private List<PriceEventModel> GetPrices()
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
            return prices;
        }

        private void ArchiveEvents(List<PriceEventModel> priceEventModels, List<EventQueueModel> eventQueueModels)
        {
            var processedEvents = eventQueueModels
                .Join(
                    priceEventModels,
                    queuedEventModel => queuedEventModel.QueueId,
                    priceEventModel => priceEventModel.QueueId,
                    (queuedEventModel, priceEventModel) => new ChangeQueueHistoryModel
                    {
                        EventTypeId = queuedEventModel.EventTypeId,
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
                        ErrorDetails = priceEventModel.ErrorDetails
                    });

            //Queued events can be excluded from the result set of the database query because of inner joins. In this case 
            //we still want to archive what events were read by the controller so that we can have a history of what couldn't be sent.
            var unprocessableQueueEvents = eventQueueModels
                .Where(e => !priceEventModels.Any(qe => qe.QueueId == e.QueueId))
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
