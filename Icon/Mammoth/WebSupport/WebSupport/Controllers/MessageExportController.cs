using System;
using System.Data;
using System.Data.SqlClient;
using Icon.Logging;
using Newtonsoft.Json;
using System.Linq;
using System.Web.Mvc;
using WebSupport.ViewModels;
using WebSupport.DataAccess;
using Icon.Common.DataAccess;
using WebSupport.DataAccess.Queries;
using System.Collections.Generic;
using WebSupport.DataAccess.TransferObjects;
using WebSupport.Models;
using System.Configuration;
using WebSupport.Helpers;

namespace WebSupport.Controllers
{
	public class MessageExportController : Controller
	{
		private ILogger logger;
		private IQueryHandler<GetStoresForRegionParameters, IList<StoreTransferObject>> queryForStores;

		public MessageExportController(
			ILogger argLogger,
			IQueryHandler<GetStoresForRegionParameters, IList<StoreTransferObject>> queryForStores)
		{
			logger = argLogger;
			this.queryForStores = queryForStores;
		}

		[HttpGet]
		public ActionResult Index()
		{
			return View(new MessageExportViewModel());
		}

		[HttpPost]
		public ActionResult Index(MessageExportViewModel viewModel)
		{
			try
			{
				viewModel.Error = null;
				int keyID = 0;
				int secondaryKeyID = 0;
				string region = "";

				if (viewModel.RegionIndex == 0)
				{
					viewModel.Error = "A region must be seleted first.";
				}
				else
				{
					region = StaticData.WholeFoodsRegions.ElementAt(viewModel.RegionIndex);
					var storeBusinessUnit = viewModel.StoreIndex;
					var queue = (MessageExportViewModel.QueueName)Enum.Parse(typeof(MessageExportViewModel.QueueName), viewModel.Queues[viewModel.QueueIndex].Value);
					var messageType = viewModel.MessageTypeIndex > 0 ? Enum.Parse(typeof(MessageExportViewModel.MessageTypeName), viewModel.Status[viewModel.MessageTypeIndex - 1].Value).ToString() : String.Empty;
					var eventType = viewModel.EventType != "0" ? viewModel.EventType : String.Empty;
					var status = viewModel.StatusIndex > 0 ? Enum.Parse(typeof(MessageExportViewModel.StatusName), viewModel.Status[viewModel.StatusIndex - 1].Value).ToString() : String.Empty;
					var maxRecords = int.Parse(ConfigurationManager.AppSettings["MaxEntriesForInStockReq"]) + 1;

					if (Request.Form["btnSend"] != null) ResetMessages(region, queue, viewModel);

					if (!string.IsNullOrEmpty(viewModel.KeyID)) int.TryParse(viewModel.KeyID, out keyID);
					if (!string.IsNullOrEmpty(viewModel.SecondaryKeyID)) int.TryParse(viewModel.SecondaryKeyID, out secondaryKeyID);

					using (var db = new DBAdapter(region))
					{
						viewModel.ResultTable = db.ExecuteDataSet("amz.ResetQueueMessages",
														  CommandType.StoredProcedure,
														  new SqlParameter[]{ new SqlParameter("@action", "Get"),
																new SqlParameter("@queue", queue.ToString()),
																new SqlParameter("@maxRecords", maxRecords),
																String.IsNullOrEmpty(messageType) ? null : new SqlParameter("@messageType", messageType.ToString()),
																String.IsNullOrEmpty(eventType) ? null : new SqlParameter("@eventType", eventType.ToString()),
																String.IsNullOrEmpty(status) ? null : new SqlParameter("@status", status.ToString()[0]),
																storeBusinessUnit <= 0 ? null : new SqlParameter("@storeBU", storeBusinessUnit),
																keyID <= 0 ? null : new SqlParameter("@keyID", keyID),
																secondaryKeyID <= 0 ? null : new SqlParameter("@secondaryKeyID", secondaryKeyID),
																viewModel.StartDatetime == null ? null : new SqlParameter("@startDatetime", viewModel.StartDatetime),
																viewModel.EndDatetime == null ? null : new SqlParameter("@endDatetime", viewModel.EndDatetime)}
														 ).Tables[0];
					}

					if (viewModel.QueueIndex == 0)
					{
						var chosenRegion = StaticData.WholeFoodsRegions.ElementAt(viewModel.RegionIndex);
						var queryParam = new GetStoresForRegionParameters { Region = chosenRegion };

						var stores = queryForStores.Search(queryParam).Select(sto => new StoreViewModel(sto));
						IEnumerable<StoreViewModel> storeAll = new StoreViewModel[] { new StoreViewModel { Name = " -- All Stores --", BusinessUnit = "0" } };

						stores = storeAll.Concat(stores);
						viewModel.SetStoreOptions(stores);
					}
				}
			}
			catch (Exception ex)
			{
				viewModel.Error = ex.Message;
				logger.Error(JsonConvert.SerializeObject(new
				{
					Message = "Unexpected error occurred",
					Controller = nameof(MessageExportController),
					ViewModel = viewModel,
					Exception = ex
				}));
			}

			return View(viewModel);
		}

		void ResetMessages(String region, MessageExportViewModel.QueueName queue, MessageExportViewModel ViewModel)
		{
			int id = 0;
			var arrayIDs = (Request.Form["cbIsSelected"] ?? String.Empty).Split(',')
								.Where(x => int.TryParse(x, out id))
								.Select(x => id).Distinct().ToArray();

			if (!arrayIDs.Any()) return;

			var table = new DataTable("IntType");
			table.Columns.Add(new DataColumn("Key", typeof(int)));

			foreach (var val in arrayIDs)
				table.Rows.Add(val);

			using (var db = new DBAdapter(region))
				db.ExecuteNonQuery("amz.ResetQueueMessages",
								   parameters: new SqlParameter[]{ new SqlParameter("@action", "Reset"),
														   new SqlParameter("@queue", queue.ToString()),
														   new SqlParameter("userName", User.Identity.Name),
														   new SqlParameter("@IDs", SqlDbType.Structured){ TypeName = "dbo.IntType", Value = table }});

			ViewModel.RequeueSuccess = true;
		}

		[HttpGet]
		public JsonResult Stores(int regionCode)
		{
			if (StaticData.WholeFoodsRegions.Length > 0 && regionCode < StaticData.WholeFoodsRegions.Length)
			{
				var chosenRegion = StaticData.WholeFoodsRegions.ElementAt(regionCode);
				if (String.IsNullOrWhiteSpace(chosenRegion))
				{
					throw new ArgumentException($"invalid region '{chosenRegion ?? "null"}'", nameof(chosenRegion));
				}
				else
				{
					var stores = queryForStores.Search(new GetStoresForRegionParameters { Region = chosenRegion });
					return Json(stores, JsonRequestBehavior.AllowGet);
				}
			}
			else
			{
				throw new ArgumentOutOfRangeException(nameof(regionCode), $"invalid region code {regionCode}");
			}
		}
	}
}