using Icon.Common;
using Icon.Logging;
using System;
using System.Configuration;
using System.Threading;
using System.Timers;

namespace KitBuilder.DataPurge.Service.Services
{
	public class PurgeApplicationService : IPurgeApplicationService
	{
		private static ILogger<PurgeApplicationService> logger = new NLogLogger<PurgeApplicationService>();
		private System.Timers.Timer timer = null;
		private TimeSpan startTime;
		private TimeSpan endTime;
		private IDataPurgeService service;
		private int maxEntries;

		public PurgeApplicationService(IDataPurgeService service)
		{
			try
			{
				this.service = service;
				maxEntries = AppSettingsAccessor.GetIntSetting("MaxEntries", 50000);

				DateTime timeStamp;
				if (!DateTime.TryParse(ConfigurationManager.AppSettings["PurgeStartTime"], out timeStamp))
					throw new ArgumentException("Invalid or missing PurgeStartTime configuration setting");

				startTime = new TimeSpan(timeStamp.Hour, timeStamp.Minute, 0);

				if (!DateTime.TryParse(ConfigurationManager.AppSettings["PurgeEndTime"], out timeStamp))
					throw new ArgumentException("Invalid or missing PurgeEndTime configuration setting");

				endTime = new TimeSpan(timeStamp.Hour, timeStamp.Minute, 0);

				//Initilize timer if all settings have been validated
				int runInterval;
				int.TryParse(ConfigurationManager.AppSettings["RunInterval"], out runInterval);
				this.timer = new System.Timers.Timer(runInterval > 0 ? runInterval : 1200000); //Use default interval == 1200000 in case if config setting is missing or invalid
			}
			catch (Exception ex) { logger.Error(ex.Message); }
		}

		public void Start()
		{
			if (this.timer == null) return;
			this.timer.Elapsed += RunService;
			this.timer.Start();
		}

		private void RunService(object sender, ElapsedEventArgs e)
		{
			this.timer.Stop();
			var now = DateTime.Now;

			if (now.TimeOfDay <= startTime || now.TimeOfDay >= endTime)
			{
				this.timer.Start();
				return;
			}

			logger = new NLogLogger<PurgeApplicationService>();

			try
			{
				logger.Info("Kit Builder Data Purge service <PurgeApplicationService> starts to purge data.");
				service.PurgeData(maxEntries);

				Thread.Sleep(60000);
			}
			catch (Exception ex)
			{
				logger.Error("Kit Builder Data Purge service <PurgeApplicationService> failed with exception: " + ex.Message);
			}
			finally
			{
				this.timer.Start();
			}
		}

		public void Stop()
		{
			if (timer == null) return;

			timer.Stop();
			timer.Elapsed -= RunService;
		}
	}
}