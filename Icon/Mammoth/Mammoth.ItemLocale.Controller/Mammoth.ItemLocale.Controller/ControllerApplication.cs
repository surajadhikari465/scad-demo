using Mammoth.Common.ControllerApplication;
using Mammoth.Common.ControllerApplication.Services;
using Mammoth.ItemLocale.Controller.DataAccess.Models;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Timers;
using Mammoth.Common;
using Mammoth.Common.Email;
using Mammoth.Logging;

namespace Mammoth.ItemLocale.Controller
{
    public class ControllerApplication : IControllerApplication
    {
        private ItemLocaleControllerApplicationSettings settings;
        private IQueueManager<ItemLocaleEventModel> itemLocelQueueManager;
        private IService<ItemLocaleEventModel> itemLocelService;
        private IEmailClient emailClient;
        private ILogger logger;

        public ControllerApplication(
            ItemLocaleControllerApplicationSettings settings,
            IQueueManager<ItemLocaleEventModel> itemLocelQueueManager,
            IService<ItemLocaleEventModel> itemLocelService,
            IEmailClient emailClient, ILogger logger)
        {
            this.settings = settings;
            this.itemLocelQueueManager = itemLocelQueueManager;
            this.itemLocelService = itemLocelService;
            this.emailClient = emailClient;
            this.logger = logger;
        }

        public void Run()
        {
            foreach (var region in settings.Regions)
            {
                settings.CurrentRegion = region;

                var events = itemLocelQueueManager.Get();

                while (events.QueuedEvents.Any())
                {
                    try
                    {
                        this.itemLocelService.Process(events.EventModels);
                    }
                    catch(Exception ex)
                    {
                        logger.Error(string.Format("Unexpected exception occured when processing events in {0} controller.", settings.ControllerName), ex);

                        var errorCode = ExceptionManager.GetErrorCode(ex);
                        foreach (var item in events.EventModels)
                        {
                            item.ErrorMessage = errorCode;
                            item.ErrorDetails = errorCode;
                            item.ErrorSource = Constants.SourceSystem.MammothItemLocaleController;
                        }
                    }
                    finally
                    {
                        this.itemLocelQueueManager.Finalize(events);
                    }

                    events.ClearEvents();
                    events = this.itemLocelQueueManager.Get();
                }
            }
        }
    }
}
