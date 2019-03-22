using System;
using System.Data;
using System.Data.SqlClient;
using Icon.Logging;
using Newtonsoft.Json;
using System.Linq;
using System.Web.Mvc;
using WebSupport.ViewModels;
using WebSupport.DataAccess;

namespace WebSupport.Controllers
{
	public class MessageExportController : Controller
	{
		ILogger logger;

		public MessageExportController(ILogger argLogger)
		{
			logger = argLogger;
		}

		[HttpGet]
		public ActionResult Index()
		{
			return View(new MessageExportViewModel());
		}

		[HttpPost]
		public ActionResult Index(MessageExportViewModel ViewModel)
		{
			try
			{
				ViewModel.Error = null;
				int keyID = 0;
				int secondaryKeyID = 0;

				var queue = (MessageExportViewModel.QueueName)Enum.Parse(typeof(MessageExportViewModel.QueueName), ViewModel.Queues[ViewModel.QueueIndex].Value);
				var region = ViewModel.Regions[ViewModel.RegionIndex];
				var status = (MessageExportViewModel.StatusName)Enum.Parse(typeof(MessageExportViewModel.StatusName), ViewModel.Status[ViewModel.StatusIndex].Value);

				if (Request.Form["btnSend"] != null) ResetMessages(region, queue);

				if (!string.IsNullOrEmpty(ViewModel.KeyID)) int.TryParse(ViewModel.KeyID, out keyID);
				if (!string.IsNullOrEmpty(ViewModel.SecondaryKeyID)) int.TryParse(ViewModel.SecondaryKeyID, out secondaryKeyID);

				using (var db = new DBAdapter(region.Text))
				{
					ViewModel.ResultTable = db.ExecuteDataSet("amz.ResetQueueMessages",
													  CommandType.StoredProcedure,
													  new SqlParameter[]{ new SqlParameter("@action", "Get"),
																new SqlParameter("@queue", queue.ToString()),
																new SqlParameter("@status", status.ToString()[0]),
																keyID <= 0 ? null : new SqlParameter("@keyID", keyID),
																secondaryKeyID <= 0 ? null : new SqlParameter("@secondaryKeyID", secondaryKeyID),
																ViewModel.ReceivedDate == null ? null : new SqlParameter("@insertDate", ViewModel.ReceivedDate)}
													 ).Tables[0];
				}
			}
			catch (Exception ex)
			{
				ViewModel.Error = ex.Message;
				logger.Error(JsonConvert.SerializeObject(new
				{
					Message = "Unexpected error occurred",
					Controller = nameof(MessageExportController),
					ViewModel = ViewModel,
					Exception = ex
				}));
			}

			return View(ViewModel);
		}

		void ResetMessages(SelectListItem region, MessageExportViewModel.QueueName queue)
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

			using (var db = new DBAdapter(region.Text))
				db.ExecuteNonQuery("amz.ResetQueueMessages",
								   parameters: new SqlParameter[]{ new SqlParameter("@action", "Reset"),
														   new SqlParameter("@queue", queue.ToString()),
														   new SqlParameter("userName", User.Identity.Name),
														   new SqlParameter("@IDs", SqlDbType.Structured){ TypeName = "dbo.IntType", Value = table }});
		}
	}
}