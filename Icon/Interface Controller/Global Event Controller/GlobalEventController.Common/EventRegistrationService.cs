using System;
using System.Collections.Generic;
using System.Linq;
using System.Configuration;
using Icon.Logging;

namespace GlobalEventController.Common
{
	public static class EventRegistrationService
	{
		private static NLogLoggerInstance logger = new NLogLoggerInstance(typeof(EventRegistrationService), StartupOptions.Instance.ToString());

		public static List<string> RegisteredEvents { get; set; }

		public static void RegisterEvents()
		{
			RegisteredEvents = new HashSet<string>(ConfigurationManager.AppSettings["GlobalEvents"].Split(',').Select(x => x.Trim()), StringComparer.InvariantCultureIgnoreCase).ToList();
			logger.Info($"Registered the following events: {String.Join(",", RegisteredEvents)}");
		}
	}
}
