using Mammoth.Common;
using Mammoth.Common.ControllerApplication;
using Mammoth.Common.ControllerApplication.Services;
using Mammoth.Common.DataAccess;
using Mammoth.Common.Email;
using Mammoth.Logging;
using Mammoth.Price.Controller.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;


namespace Mammoth.Price.Controller
{
    public class ControllerApplication : IControllerApplication
    {
        private PriceControllerApplicationSettings settings;
        private IQueueManager<PriceEventModel> priceQueueManager;
        private IService<PriceEventModel> priceService;
        private IEmailClient emailClient;
        private ILogger logger;

        public ControllerApplication(
            PriceControllerApplicationSettings settings,
            IQueueManager<PriceEventModel> priceQueueManager,
            IEmailClient emailClient, ILogger logger,
            IService<PriceEventModel> priceEventManager)
        {
            this.settings = settings;
            this.priceQueueManager = priceQueueManager;
            this.priceService = priceEventManager;
            this.emailClient = emailClient;
            this.logger = logger;
        }

        public void Run()
        {
            foreach (var region in settings.Regions)
            {
                settings.CurrentRegion = region;

                var events = priceQueueManager.Get();

                while (events.QueuedEvents.Any())
                {
                    try
                    {
                        this.priceService.Process(events.EventModels);
                    }
                    catch (Exception ex)
                    {
                        logger.Error(string.Format("Unexpected exception occured when processing events in {0} controller.", settings.ControllerName), ex);

                        var errorCode = ExceptionManager.GetErrorCode(ex);
                        foreach (var item in events.EventModels)
                        {
                            item.ErrorMessage = errorCode;
                        }
                    }
                    finally
                    {
                        this.priceQueueManager.Finalize(events);
                    }

                    events.ClearEvents();
                    events = this.priceQueueManager.Get();
                }
            }
        }
    }
}
