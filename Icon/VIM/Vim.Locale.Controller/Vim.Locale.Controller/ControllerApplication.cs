using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vim.Common.ControllerApplication;
using Vim.Common.ControllerApplication.Services;
using Vim.Common.DataAccess;
using Vim.Locale.Controller.DataAccess.Models;

namespace Vim.Locale.Controller
{
    public class ControllerApplication : IControllerApplication
    {
        private IQueueManager<LocaleEventModel> localeQueueManager;
        private IService<LocaleEventModel> localeService;

        public ControllerApplication(IQueueManager<LocaleEventModel> hierarchyQueueManager,
            IService<LocaleEventModel> localeventManager)
        {
            this.localeQueueManager = hierarchyQueueManager;
            this.localeService = localeventManager;
        }

        public void Run()
        {
            List<int> eventTypeIds = new List<int> { EventTypes.LocaleAdd, EventTypes.LocaleUpdate };

            List<LocaleEventModel> storeEvents = this.localeQueueManager.Get(eventTypeIds);

            while (storeEvents.Any())
            {
                this.localeService.Process(storeEvents);
                this.localeQueueManager.Finalize(storeEvents);

                storeEvents = this.localeQueueManager.Get(eventTypeIds);
            }
        }
    }
}

