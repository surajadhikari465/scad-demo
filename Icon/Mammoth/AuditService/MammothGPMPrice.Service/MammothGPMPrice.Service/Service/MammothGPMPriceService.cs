using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Mammoth.Logging;
using MammothGpmService.Controller;

namespace MammothGpmService.Service
{
	public class MammothGpmPriceService : IMammothGpmPriceService
	{
		private System.Timers.Timer timer = null;
		private DateTime scheduleTime;
		private readonly int dayOfTheWeek = 0;
		private ILogger logger;
		private PriceDataContoller controller;

		public MammothGpmPriceService(PriceDataContoller controller)
		{
			this.controller = controller;
			try
			{
				this.timer = new System.Timers.Timer();
				scheduleTime = DateTime.Today.AddDays(1).AddHours(int.Parse(ConfigurationManager.AppSettings["ScheduledTime"]));
				//int runInterval;
				//int.TryParse(ConfigurationManager.AppSettings["RunInterval"], out runInterval);
				//this.timer = new System.Timers.Timer(runInterval > 0 ? runInterval : 30); //for testing to control timer
			}
			catch (Exception ex)
			{
				logger.Error("Problem occured with setup the defaults to this service", ex); 
			}
		}
		public void Start()
		{
			if (this.timer == null) return;
			timer.Interval = scheduleTime.Subtract(DateTime.Now).TotalSeconds * 1000;//comment this when test timer controls are active
			this.timer.Elapsed += RunService;
			this.timer.Start();
		}
		public void Stop()
		{
			if (timer == null) return;
			timer.Stop();
			timer.Elapsed -= RunService;
		}
		private void RunService(object sender, ElapsedEventArgs e)
		{
			this.timer.Stop();
			try
			{
				controller.Execute();
			}
			catch (Exception ex)
			{
				logger.Error("Problem occured with running this service", ex);
			}
			finally
			{
				timer.Start();
			}
		}
	}
}
