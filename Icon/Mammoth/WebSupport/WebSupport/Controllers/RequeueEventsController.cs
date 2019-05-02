using System;
using System.Linq;
using System.Data;
using System.Data.SqlClient;
using System.Web.Mvc;
using Icon.Logging;
using Newtonsoft.Json;
using WebSupport.DataAccess;
using WebSupport.ViewModels;

namespace WebSupport.Controllers
{
    public class RequeueEventsController : Controller
    {
        ILogger logger = null;
        const string UNEXPECTED_ERROR = "Unexpected error occurred";

        public RequeueEventsController(ILogger argLogger)
		{
			logger = argLogger;
		}

        [HttpGet]
        public ActionResult Index()
        {
            return View(new RequeueEventViewModel());
        }

        [HttpPost]
        public ActionResult Index(RequeueEventViewModel ViewModel)
        {
            try
            {
                ViewModel.Error = null;
                var region = ViewModel.Region; //ViewModel.Regions[ViewModel.RegionID].Text;

                if(Request.Form["btnSend"] != null) RequeueMessages(ViewModel);

                using(var db = new DBAdapter(region))
                {
                    var dataSet = db.ExecuteDataSet("amz.RequeueMessages",
                                                    CommandType.StoredProcedure,
                                                    new SqlParameter[]{ new SqlParameter("@action", "Get"),
                                                                        new SqlParameter("@eventCode", ViewModel.EventType),
                                                                        new SqlParameter("@dateFrom", SqlDbType.Date) { Value = ViewModel.DateFrom },
                                                                        new SqlParameter("@dateTo", SqlDbType.Date) { Value = ViewModel.DateTo }}
                                                   );

                    ViewModel.ResultTable = dataSet.Tables.Count == 0 ? new DataTable() : dataSet.Tables[0];
                }
            }
            catch(Exception ex)
            {
                ViewModel.Error = ex.Message;
                logger.Error(JsonConvert.SerializeObject(new
                {
                    Message = UNEXPECTED_ERROR,
                    Controller = nameof(MessageExportController),
                    ViewModel,
                    Exception = ex
                }));
            }

            return View(ViewModel);
        }

        void RequeueMessages(RequeueEventViewModel view)
		{
            int id = 0;
			var arrayIDs = (Request.Form["cbIsSelected"] ?? String.Empty).Split(',')
								.Where(x => int.TryParse(x, out id))
								.Select(x => id).Distinct().ToArray();

            if(!arrayIDs.Any()) return;

            var table = new DataTable("IntType");
			table.Columns.Add(new DataColumn("Key", typeof(int)));

            foreach(var val in arrayIDs)
				table.Rows.Add(val);


			using (var db = new DBAdapter(view.Region))
		        db.ExecuteNonQuery("amz.RequeueMessages",
                                   CommandType.StoredProcedure,
				                   new SqlParameter[]{ new SqlParameter("@action", "Requeue"),
                                                       new SqlParameter("@eventCode", view.EventType),
				                                       new SqlParameter("@IDs", SqlDbType.Structured){ TypeName = "dbo.IntType", Value = table }});
        }
    }
}