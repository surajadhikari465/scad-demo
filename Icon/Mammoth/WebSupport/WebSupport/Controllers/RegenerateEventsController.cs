using System;
using System.Linq;
using System.Data;
using System.Data.SqlClient;
using System.Web.Mvc;
using Icon.Logging;
using Newtonsoft.Json;
using WebSupport.DataAccess;
using WebSupport.ViewModels;
using WebSupport.Models;
using WebSupport.DataAccess.Queries;
using System.Collections.Generic;
using WebSupport.DataAccess.TransferObjects;
using Icon.Common.DataAccess;
using System.Configuration;

namespace WebSupport.Controllers
{
    public class RegenerateEventsController : Controller
    {
        private ILogger logger = null;
		private IQueryHandler<GetStoresForRegionParameters, IList<StoreTransferObject>> queryForStores;
		const string UNEXPECTED_ERROR = "Unexpected error occurred";

        public RegenerateEventsController(
			ILogger argLogger,
			IQueryHandler<GetStoresForRegionParameters, IList<StoreTransferObject>> queryForStores)
		{
            logger = argLogger;
			this.queryForStores = queryForStores;
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
			string region = "";

			if (view.RegionIndex == 0)
			{
				view.Error = "A region must be seleted first.";
			}
			else
			{
				region = StaticData.WholeFoodsRegions.ElementAt(view.RegionIndex);
				var storeBusinessUnit = view.StoreIndex;
				var maxRecords = int.Parse(ConfigurationManager.AppSettings["MaxEntriesForInStockReq"]) + 1;
				using (var db = new DBAdapter(region))
				{
					
					var dataSet = db.ExecuteDataSet("amz.GetInStockEventsByType",
													CommandType.StoredProcedure,
													new SqlParameter[]{ new SqlParameter("@eventCode", view.EventType),
																	new SqlParameter("@maxRecords", maxRecords),
																	storeBusinessUnit <= 0 ? null : new SqlParameter("@storeBU", storeBusinessUnit),
																	new SqlParameter("@dateFrom", SqlDbType.Date) { Value = view.StartDatetime },
																	new SqlParameter("@dateTo", SqlDbType.Date) { Value = view.EndDatetime }});

					view.ResultTable = dataSet.Tables.Count == 0 ? null : dataSet.Tables[0];
				}

				var queryParam = new GetStoresForRegionParameters { Region = region };

				var stores = queryForStores.Search(queryParam).Select(sto => new StoreViewModel(sto));
				IEnumerable<StoreViewModel> storeAll = new StoreViewModel[] { new StoreViewModel { Name = " -- All Stores --", BusinessUnit = "0" } };

				stores = storeAll.Concat(stores);
				view.SetStoreOptions(stores);
			}
        }

        void Submit(RegenerateEventViewModel view)
        {
			string region = StaticData.WholeFoodsRegions.ElementAt(view.RegionIndex);

			int id = 0;
            var tvp = Icon.Common.Extensions.ToTvp((Request.Form["cbIsSelected"] ?? String.Empty).Split(',')
                                .Where(x => int.TryParse(x, out id))
                                .Distinct()
                                .Select(x => new{ Key = x }),
                                "@IDs", "dbo.IntType");

            if(tvp == null || (tvp.Value as DataTable).Rows.Count == 0)
            {
                ViewData["FYI"] = "No events has been selected to re-generate to queue. Please select one or more events and try again.";
            }
            else
            {
                using(var db = new DBAdapter(region))
                {
                    int.TryParse(db.ExecuteScalar("amz.RequeueMessages",
                                                  CommandType.StoredProcedure,
                                                  new SqlParameter("@eventCode", view.EventType),
                                                  tvp).ToString(), out id);

                    ViewData["FYI"] = $"Event code {view.EventType}: {id.ToString()} events have been submitted.";
               }

				view.EventsGenerationSuccess = true;
			}
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