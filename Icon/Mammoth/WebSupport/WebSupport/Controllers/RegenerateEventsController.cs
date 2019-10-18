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
    public class RegenerateEventsController : Controller
    {
        ILogger logger = null;
        const string UNEXPECTED_ERROR = "Unexpected error occurred";

        public RegenerateEventsController(ILogger argLogger)
        {
            logger = argLogger;
        }

        [HttpGet]
        public ActionResult Index()
        {
            return View(new RegenerateEventViewModel());
        }

        [HttpPost]
        public ActionResult Index(RegenerateEventViewModel ViewModel)
        {
            try
            {
                ViewModel.Error = null;

                if(Request.Form["ActionGet"] != null)
                {
                   Get(ViewModel);
                }
                else if(Request.Form["ActionSubmit"] != null)
                {
                   Submit(ViewModel);
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

        void Get(RegenerateEventViewModel view)
        {
            using(var db = new DBAdapter(view.Region))
            {
                var dataSet = db.ExecuteDataSet("amz.GetInStockEventsByType",
                                                CommandType.StoredProcedure,
                                                new SqlParameter[]{ new SqlParameter("@eventCode", view.EventType),
                                                                    new SqlParameter("@dateFrom", SqlDbType.Date) { Value = view.DateFrom },
                                                                    new SqlParameter("@dateTo", SqlDbType.Date) { Value = view.DateTo }});

                view.ResultTable = dataSet.Tables.Count == 0 ? null : dataSet.Tables[0];
            }
        }

        void Submit(RegenerateEventViewModel view)
        {
            int id = 0;
            var tvp = Icon.Common.Extensions.ToTvp((Request.Form["cbIsSelected"] ?? String.Empty).Split(',')
                                .Where(x => int.TryParse(x, out id))
                                .Distinct()
                                .Select(x => new{ Key = x }),
                                "@IDs", "dbo.IntType");

            if(tvp == null || (tvp.Value as DataTable).Rows.Count == 0)
            {
                ViewData["FYI"] = "No events has been selected to re-queue. Please select one or more events and try again.";
            }
            else
            {
                using(var db = new DBAdapter(view.Region))
                {
                    int.TryParse(db.ExecuteScalar("amz.RequeueMessages",
                                                  CommandType.StoredProcedure,
                                                  new SqlParameter("@eventCode", view.EventType),
                                                  tvp).ToString(), out id);

                    ViewData["FYI"] = $"Event code {view.EventType}: {id.ToString()} events have been submitted.";
               }
            }
        }
    }
}