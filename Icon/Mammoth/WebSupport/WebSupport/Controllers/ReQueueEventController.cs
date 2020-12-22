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
using WebSupport.Models;
using WebSupport.ViewModels;

namespace WebSupport.Controllers
{
    /// <summary>
    /// Re-Queue Event Controller
    /// </summary>
    public class ReQueueEventController : Controller
	{
		private readonly ILogger logger;
		private readonly IQueryHandler<GetMessageArchiveEventsQueryParameters, IList<ArchivedMessage>> getMessageArchiveEventsQuery;

		/// <summary>
		/// Initializes an instance of ReQueueEventController
		/// </summary>
		/// <param name="logger">Logger</param>
		/// <param name="getMessageArchiveEventsQuery">GetMessageArchiveEventsQuery</param>
		public ReQueueEventController(ILogger logger,
			IQueryHandler<GetMessageArchiveEventsQueryParameters, IList<ArchivedMessage>> getMessageArchiveEventsQuery)
		{
			this.logger = logger;
			this.getMessageArchiveEventsQuery = getMessageArchiveEventsQuery;
		}

		/// <summary>
		/// Defaule view
		/// </summary>
		/// <returns></returns>
		[HttpGet]
		public ActionResult Index()
		{
			return View(new EventReQueueViewModel());
		}

		/// <summary>
		/// Process a re-queue request
		/// </summary>
		/// <param name="viewModel"></param>
		/// <returns></returns>
		[HttpPost]
		public ActionResult Index(EventReQueueViewModel viewModel)
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
					// Normalize input
					region = StaticData.WholeFoodsRegions.ElementAt(viewModel.RegionIndex);
					var queue = (EventReQueueViewModel.QueueName)Enum.Parse(typeof(EventReQueueViewModel.QueueName), viewModel.Queues[viewModel.QueueIndex].Value);
					var eventType = viewModel.EventType != "0" ? viewModel.EventType : String.Empty;
					var status = viewModel.StatusIndex > 0 ? Enum.Parse(typeof(EventReQueueViewModel.StatusName), viewModel.Status[viewModel.StatusIndex - 1].Value).ToString() : String.Empty;
					var maxRecords = int.Parse(ConfigurationManager.AppSettings["MaxEntriesForInStockReq"]) + 1;
					if (!string.IsNullOrEmpty(viewModel.KeyID)) int.TryParse(viewModel.KeyID, out keyID);
					if (!string.IsNullOrEmpty(viewModel.SecondaryKeyID)) int.TryParse(viewModel.SecondaryKeyID, out secondaryKeyID);

					// Reset messages if btnSent was pressed
					if (Request.Form["btnSend"] != null) ResetMessages(region, queue, viewModel);

					var results = this.getMessageArchiveEventsQuery.Search(new GetMessageArchiveEventsQueryParameters
										{
											Region = region,
											MaxRecords = maxRecords,
											KeyID = viewModel.KeyID,
											SecondaryKeyID = viewModel.SecondaryKeyID,
											StartDatetime = viewModel.StartDatetime,
											EndDatetime = viewModel.EndDatetime,
											EventType = string.IsNullOrWhiteSpace(eventType) ? null : eventType,
											Queue = queue.ToString(),
											Status = String.IsNullOrWhiteSpace(status) ? null : status.ToString()

										});

					// Fill DataTable with results
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
				}
			}
			catch (Exception ex)
			{
				viewModel.Error = ex.Message;
				logger.Error(JsonConvert.SerializeObject(new
				{
					Message = "Unexpected error occurred",
					Controller = nameof(ReQueueEventController),
					ViewModel = viewModel,
					Exception = ex
				}));
			}

			return View(viewModel);
		}

		/// <summary>
		///  Reset Events
		/// </summary>
		/// <param name="region">Region</param>
		/// <param name="queue">Queue to update</param>
		/// <param name="ViewModel">View Model data</param>
		void ResetMessages(String region, EventReQueueViewModel.QueueName queue, EventReQueueViewModel ViewModel)
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
	}
}