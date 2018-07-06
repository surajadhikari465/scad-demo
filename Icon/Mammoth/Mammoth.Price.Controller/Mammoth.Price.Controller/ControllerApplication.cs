using Mammoth.Common.ControllerApplication;
using Mammoth.Common.ControllerApplication.Models;
using Mammoth.Common.DataAccess.CommandQuery;
using Mammoth.Common.DataAccess.Models;
using Mammoth.Logging;
using Mammoth.Price.Controller.ApplicationModules;
using Mammoth.Price.Controller.Common;
using Mammoth.Price.Controller.DataAccess.Commands;
using Mammoth.Price.Controller.DataAccess.Models;
using Mammoth.Price.Controller.EventProcessors;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Mammoth.Price.Controller
{
    public class ControllerApplication : IControllerApplication
    {
        private PriceControllerApplicationSettings settings;
        private IQueueManager queueManager;
        private IEventProcessor<PriceEventModel> priceEventProcessor;
        private IEventProcessor<CancelAllSalesEventModel> cancelAllSalesEventProcessor;
        private ICommandHandler<ArchiveEventsCommand> archiveEventsCommandHandler;
        private ILogger logger;

        public ControllerApplication(
            PriceControllerApplicationSettings settings,
            IQueueManager queueManager,
            IEventProcessor<PriceEventModel> priceEventProcessor,
            IEventProcessor<CancelAllSalesEventModel> cancelAllSalesEventProcessor,
            ICommandHandler<ArchiveEventsCommand> archiveEventsCommandHandler,
            ILogger logger)
        {
            this.settings = settings;
            this.queueManager = queueManager;
            this.priceEventProcessor = priceEventProcessor;
            this.cancelAllSalesEventProcessor = cancelAllSalesEventProcessor;
            this.archiveEventsCommandHandler = archiveEventsCommandHandler;
            this.logger = logger;
        }

        public void Run()
        {
            foreach (var region in settings.Regions)
            {
                settings.CurrentRegion = region;

                logger.Info(new { Region = region, Message = "Beginning event processing." }.ToJson());

                List<EventQueueModel> events = queueManager.GetEvents();
                while (events.Any())
                {
                    priceEventProcessor.ProcessEvents(events);
                    cancelAllSalesEventProcessor.ProcessEvents(events);
                    if (events.Any())
                        ArchiveUnprocessedEvents(events);
                    queueManager.DeleteInProcessEvents();
                    events = queueManager.GetEvents();
                }

                logger.Info(new { Region = region, Message = "Ending event processing." }.ToJson());
            }
        }

        private void ArchiveUnprocessedEvents(List<EventQueueModel> events)
        {
            logger.Info(new { Region = settings.CurrentRegion, Message = "Archiving unprocessable events." }.ToJson());

            try
            {
                archiveEventsCommandHandler.Execute(new ArchiveEventsCommand
                {
                    Events = events.Select(e => new ChangeQueueHistoryModel
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
                    }).ToList()
                });
            }
            catch(Exception ex)
            {
                logger.Error(new { Region = settings.CurrentRegion, Message = "Error occurred when archiving unprocessable events." }.ToJson(), ex);
            }
        }
    }
}