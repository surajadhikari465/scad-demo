using Mammoth.Common.ControllerApplication;
using Mammoth.Common.ControllerApplication.Services;
using Mammoth.ItemLocale.Controller.DataAccess.Models;
using System.Linq;
using System;
using System.Data.SqlClient;
using Mammoth.Common;
using Mammoth.Common.Email;
using Mammoth.Logging;

namespace Mammoth.ItemLocale.Controller
{
	public class ControllerApplication : IControllerApplication
	{
		private ItemLocaleControllerApplicationSettings settings;
		private IQueueManager<ItemLocaleEventModel> itemLocaleQueueManager;
		private IService<ItemLocaleEventModel> itemLocaleService;
		private IEmailClient emailClient;
		private ILogger logger;
		const int SQL_TIMEOUT = -2; //System.Data.SqlClient.TdsEnums.TIMEOUT_EXPIRED == -2

		public ControllerApplication(
				ItemLocaleControllerApplicationSettings settings,
				IQueueManager<ItemLocaleEventModel> itemLocelQueueManager,
				IService<ItemLocaleEventModel> itemLocelService,
				IEmailClient emailClient, ILogger logger)
		{
			this.settings = settings;
			this.itemLocaleQueueManager = itemLocelQueueManager;
			this.itemLocaleService = itemLocelService;
			this.emailClient = emailClient;
			this.logger = logger;
		}

		Mammoth.Common.ControllerApplication.ChangeQueueEvents<ItemLocaleEventModel> GetEvents()
		{
			try { return itemLocaleQueueManager.Get(); }
			catch(SqlException sqlEx )
			{
				switch(sqlEx.Number)
				{
					case SQL_TIMEOUT:
						logger.Warn($"ControllerApplication.Get(): Region {settings.CurrentRegion}: Failed to extract queue events in {settings.ControllerName} controller. Execution Timeout Expired.");
						break;
					default:
						logger.Error($"ControllerApplication.Get(): Region {settings.CurrentRegion}: Failed to extract queue events in {settings.ControllerName} controller. SQLException.Number = {sqlEx.Number.ToString()}.", sqlEx);
						break;
				}
			}
			catch(Exception ex)
			{
					logger.Error($"ControllerApplication.Get(): Region {settings.CurrentRegion}: Failed to extract queue events in {settings.ControllerName} controller. Possible network issue.", ex);
			}

			return null;
		}

		public void Run()
		{
			foreach(var region in settings.Regions)
			{
				settings.CurrentRegion = region;
				var events = GetEvents();

				while(events != null && events.QueuedEvents.Any())
				{
					try
					{
						this.itemLocaleService.Process(events.EventModels);
					}
					catch(Exception ex)
					{
						logger.Error($"ControllerApplication.Process(events.EventModels): Exception occured when processing events in {this.settings.ControllerName} controller. Region {this.settings.CurrentRegion}", ex);

						var errorCode = ExceptionManager.GetErrorCode(ex);
						foreach(var item in events.EventModels)
						{
							item.ErrorMessage = errorCode;
							item.ErrorDetails = errorCode;
							item.ErrorSource = Constants.SourceSystem.MammothItemLocaleController;
						}
					}
					finally
					{
						try
						{
							this.itemLocaleQueueManager.Finalize(events);
						}
						catch(Exception ex)
						{
							logger.Error($"ControllerApplication.Process(events.EventModels): Exception occured executing itemLocaleQueueManager.Finalize(events) in {this.settings.ControllerName} controller. Region {this.settings.CurrentRegion}", ex);
						}
					}

					events.ClearEvents();
					events = GetEvents();
				}
			}
		}
	}
}