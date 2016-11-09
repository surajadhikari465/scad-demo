using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using Icon.Logging;
using GlobalEventController.Common;

namespace GlobalEventController.Common
{
    public static class EventRegistrationService
    {
        private static NLogLoggerInstance logger = new NLogLoggerInstance(typeof(EventRegistrationService), StartupOptions.Instance.ToString());

        public static List<string> RegisteredEvents { get; set; }

        public static void RegisterEvents()
        {
            RegisteredEvents = new List<string>();

            string[] configuredEvents = ConfigurationManager.AppSettings["GlobalEvents"].Split(',');

            foreach (string configuredEvent in configuredEvents)
            {
                RegisteredEvents.Add(configuredEvent);
            }

            logger.Info("Registered the following events: " + String.Join(",", RegisteredEvents));
        }
    }
}
