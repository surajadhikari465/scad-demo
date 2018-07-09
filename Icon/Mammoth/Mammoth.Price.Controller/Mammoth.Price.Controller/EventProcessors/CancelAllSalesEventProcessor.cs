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
    public class CancelAllSalesEventProcessor : IEventProcessor<CancelAllSalesEventModel>
    {
        private PriceControllerApplicationSettings settings;
        private IQueryHandler<GetCancelAllSalesDataParameters, List<CancelAllSalesEventModel>> getCancelAllSalesDataQuery;
        private IService<CancelAllSalesEventModel> service;
        private ICommandHandler<ArchiveEventsCommand> archiveEventsCommandHandler;
        private ICommandHandler<ReprocessFailedCancelAllSalesEventsCommand> reprocessFailedCancelAllSalesEventsCommandHandler;
        private IErrorAlerter errorAlerter;
        private ILogger logger;

        public CancelAllSalesEventProcessor(
            PriceControllerApplicationSettings settings,
            IQueryHandler<GetCancelAllSalesDataParameters, List<CancelAllSalesEventModel>> getCancelAllSalesDataQuery,
            IService<CancelAllSalesEventModel> service,
            ICommandHandler<ArchiveEventsCommand> archiveEventsCommandHandler,
            ICommandHandler<ReprocessFailedCancelAllSalesEventsCommand> reprocessFailedCancelAllSalesEventsCommandHandler,
            IErrorAlerter errorAlerter,
            ILogger logger)
        {
            this.settings = settings;
            this.getCancelAllSalesDataQuery = getCancelAllSalesDataQuery;
            this.service = service;
            this.archiveEventsCommandHandler = archiveEventsCommandHandler;
            this.reprocessFailedCancelAllSalesEventsCommandHandler = reprocessFailedCancelAllSalesEventsCommandHandler;
            this.errorAlerter = errorAlerter;
            this.logger = logger;
        }

        public void ProcessEvents(List<EventQueueModel> events)
        {
            var cancelAllSalesEvents = events
                .Where(m => m.EventTypeId == Constants.EventTypes.CancelAllSales)
                .ToList();

            if (cancelAllSalesEvents.Count > 0)
            {
                try
                {
                    List<CancelAllSalesEventModel> cancelAllSales = getCancelAllSalesDataQuery.Search(new GetCancelAllSalesDataParameters { Instance = settings.Instance });
                    service.Process(cancelAllSales);
                    events.RemoveAll(m => cancelAllSalesEvents.Contains(m));
                    ArchiveEvents(cancelAllSales, cancelAllSalesEvents);
                    if (cancelAllSales.Any(p => !string.IsNullOrWhiteSpace(p.ErrorMessage)))
                    {
                        var errorCancelAllSales = cancelAllSales.Where(p => !string.IsNullOrWhiteSpace(p.ErrorMessage)).ToList();
                        errorAlerter.AlertErrors(errorCancelAllSales);
                        reprocessFailedCancelAllSalesEventsCommandHandler.Execute(
                            new ReprocessFailedCancelAllSalesEventsCommand
                            {
                                Region = settings.CurrentRegion,
                                CancelAllSales = errorCancelAllSales,
                                Events = cancelAllSalesEvents
                            });
                    }
                }
                catch (Exception ex)
                {
                    logger.Error(new { Region = settings.CurrentRegion, Message = "Error occurred while processing Cancel All Sales Events." }.ToJson(), ex);
                }
            }
        }

        private void ArchiveEvents(List<CancelAllSalesEventModel> cancelAllSalesEventModels, List<EventQueueModel> eventQueueModels)
        {
            var processedEvents = eventQueueModels
                .Join(
                    cancelAllSalesEventModels,
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
                .Where(e => !cancelAllSalesEventModels.Any(qe => qe.QueueId == e.QueueId))
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