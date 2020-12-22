using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Mvc;
using Icon.Common.DataAccess;
using Icon.Logging;
using Newtonsoft.Json;
using WebSupport.DataAccess;
using WebSupport.DataAccess.Models;
using WebSupport.DataAccess.Queries;
using WebSupport.DataAccess.TransferObjects;
using WebSupport.Models;
using WebSupport.ViewModels;

namespace WebSupport.Controllers
{
    /// <summary>
    /// Re-Queue Message Controller.
    /// </summary>
    public class ReQueueMessageController : Controller
	{
		private readonly ILogger logger;
		private readonly IQueryHandler<GetStoresForRegionParameters, IList<StoreTransferObject>> queryForStores;
		private readonly IQueryHandler<GetMessageArchiveQueryParameters, IList<ArchivedMessage>> getMessageArchiveQuery;

		/// <summary>
		/// Initializes an instance of ReQueueMessageController
		/// </summary>
		/// <param name="logger">LOgger</param>
		/// <param name="queryForStores">Store per region query</param>
		public ReQueueMessageController(
			ILogger logger,
			IQueryHandler<GetStoresForRegionParameters, IList<StoreTransferObject>> queryForStores,
			IQueryHandler<GetMessageArchiveQueryParameters, IList<ArchivedMessage>> getMessageArchiveQuery)
		{
			this.logger = logger;
			this.queryForStores = queryForStores;
			this.getMessageArchiveQuery = getMessageArchiveQuery;
		}

		/// <summary>
		/// Default view
		/// </summary>
		/// <returns></returns>
		[HttpGet]
		public ActionResult Index()
		{
			return View(new ReQueueMessageViewModel());
		}

		/// <summary>
		/// Process a re-queue request
		/// </summary>
		/// <param name="viewModel"></param>
		/// <returns></returns>
		[HttpPost]
		public ActionResult Index(ReQueueMessageViewModel viewModel)
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
					// Normalizes input
					region = StaticData.WholeFoodsRegions.ElementAt(viewModel.RegionIndex);
					var messageType = viewModel.MessageTypeIndex > 0 ? Enum.Parse(typeof(ReQueueMessageViewModel.MessageTypeName), viewModel.Status[viewModel.MessageTypeIndex - 1].Value).ToString() : String.Empty;
					var eventType = viewModel.EventType != "0" ? viewModel.EventType : String.Empty;
					var status = viewModel.StatusIndex > 0 ? Enum.Parse(typeof(ReQueueMessageViewModel.StatusName), viewModel.Status[viewModel.StatusIndex - 1].Value).ToString() : String.Empty;
					var maxRecords = int.Parse(ConfigurationManager.AppSettings["MaxEntriesForInStockReq"]) + 1;
					if (!string.IsNullOrEmpty(viewModel.KeyID)) int.TryParse(viewModel.KeyID, out keyID);
					if (!string.IsNullOrEmpty(viewModel.SecondaryKeyID)) int.TryParse(viewModel.SecondaryKeyID, out secondaryKeyID);

					// Reset messages if btnSent was pressed
					if (Request.Form["btnSend"] != null) ResetMessages(region, viewModel);

					var results = this.getMessageArchiveQuery.Search(new GetMessageArchiveQueryParameters
									{
										Region = region,
										MaxRecords = maxRecords,
										KeyID = viewModel.KeyID,
										StoresBU = viewModel.StoreIndexes,
										EventType = String.IsNullOrEmpty(eventType) ? null : eventType,
										MessageType = String.IsNullOrEmpty(messageType) ? null : messageType,
										StartDatetime = viewModel.StartDatetime,
										EndDatetime = viewModel.EndDatetime,
										Status = String.IsNullOrEmpty(status) ? null : status.Substring(0, 1)
									});

					// Prepare result
					viewModel.ResultTable = new DataTable();
					viewModel.ResultTable.Columns.Add("ArchiveID", typeof(int));
					viewModel.ResultTable.Columns.Add("Store_BU", typeof(int));
					viewModel.ResultTable.Columns.Add("Key_ID", typeof(int));
					viewModel.ResultTable.Columns.Add("Secondary_Key_ID", typeof(int));
					viewModel.ResultTable.Columns.Add("Event", typeof(string));
					viewModel.ResultTable.Columns.Add("Status", typeof(string));
					viewModel.ResultTable.Columns.Add("Insert_Date", typeof(string));
					viewModel.ResultTable.Columns.Add("Reset_By", typeof(string));
					foreach (var message in results)
					{
						var row = viewModel.ResultTable.NewRow();
						row["ArchiveID"] = message.ArchiveID;
						row["Store_BU"] = message.StoreBU;
						row["Event"] = message.Event;
						row["Key_ID"] = message.KeyID;
						row["Secondary_Key_ID"] = message.SecondaryKeyID;
						row["Status"] = message.Status;
						row["Insert_Date"] = message.InsertDate;
						row["Reset_By"] = message.ResetBy;
						viewModel.ResultTable.Rows.Add(row);
					}

					// Re fill Stores
					var chosenRegion = StaticData.WholeFoodsRegions.ElementAt(viewModel.RegionIndex);
					var queryParam = new GetStoresForRegionParameters { Region = chosenRegion };
					var stores = queryForStores.Search(queryParam).Select(sto => new StoreViewModel(sto));
					viewModel.SetStoreOptions(stores);
				}
			}
			catch (Exception ex)
			{
				viewModel.Error = ex.Message;
				logger.Error(JsonConvert.SerializeObject(new
				{
					Message = "Unexpected error occurred",
					Controller = nameof(ReQueueMessageController),
					ViewModel = viewModel,
					Exception = ex
				}));
			}

			return View(viewModel);
		}

		/// <summary>
		/// Reset Messages
		/// </summary>
		/// <param name="region">Region</param>
		/// <param name="ViewModel">View Model Data</param>
		void ResetMessages(String region, ReQueueMessageViewModel ViewModel)
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
														   new SqlParameter("@queue", "ArchivedMessage"),
														   new SqlParameter("userName", User.Identity.Name),
														   new SqlParameter("@IDs", SqlDbType.Structured){ TypeName = "dbo.IntType", Value = table }});

			ViewModel.RequeueSuccess = true;
		}

		/// <summary>
		/// Returns the list of stores in a region in JSON format
		/// </summary>
		/// <param name="regionCode"></param>
		/// <returns>List of Stores</returns>
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