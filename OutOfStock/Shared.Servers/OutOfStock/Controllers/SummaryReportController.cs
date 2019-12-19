using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Data.Objects;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Management;
using System.Web.Mvc;
using System.DirectoryServices;
using System.IO;
using System.Web.Configuration;
//using Microsoft.ApplicationInsights;
using OOS.Model;
using OutOfStock.Classes;
using OutOfStock.Models;
using OOSCommon.DataContext;
using OutOfStock.Service;

namespace OutOfStock.Controllers
{
    public class SummaryReportController : Controller
    {
        const int defaultUPCCount = 90000;
        const string percentFormat = "{0:P5}";

        protected string userStore = OOSUser.userStore;
        private SummaryReportService service;

        //
        // GET: /SummaryReport/

        public SummaryReportController(SummaryReportService service)
        {
            this.service = service;
        }



        public ActionResult GridTest()
        {
            Response.Redirect("/SummaryReport/Index");
            return null;
        }


        public ActionResult Index()
        {
            //var telemetry = new TelemetryClient();
            var sw = new Stopwatch();
            sw.Start();
            var user = OOSUser.GetUserName();
           // MvcApplication.oosLog.Trace(string.Format("Summary Report for {0}", user));
            ActionResult result = null;
            if (!OOSUser.HasValidLocationInformation)
            {
                
                // application insights
                var properties = new Dictionary<string, string> {{"user", "test"}};
                var metrics = new Dictionary<string, double> {};
               // telemetry.TrackEvent("InvalidLoactionInfo", properties, metrics);
                

                result = View("~/Views/Shared/InvalidLocationInformation.cshtml");
            }
            else
            {
                if (!OOSUser.isStoreLevel)
                    result = new RedirectResult("~/Home/Index");
                else
                {
                    var dtEnd = DateTime.Now;
                    var dtStart = dtEnd.AddDays(-7);
                    IEnumerable<SummaryReportViewModel> viewModel = GetModel(dtStart, dtEnd, OOSUser.userRegion, null, false);
                  //  telemetry.TrackPageView("SummaryReport");
                    result = View("Index", viewModel);
                }
            }

            sw.Stop();

           
            MvcApplication.oosLog.Trace(string.Format("Summary Report for {0} took {1} millisecond(s). Parameters: From={2} To={3} Region={4}", user, sw.ElapsedMilliseconds, DateTime.Now.AddDays(-7).ToShortDateString(), DateTime.Now.ToShortDateString(), OOSUser.userRegion));

            return result;
        }

        public JsonResult GetSummaryPageData(string region, string start, string end)
        {
            IEnumerable<SummaryReportData_Result> data = null;
            var s = DateTime.Parse(start);
            var e = DateTime.Parse(end);
            using (var ctx = new OOSEntities())
            {
                data = ctx.ExecuteFunction<SummaryReportData_Result>("SummaryReportData", new[] { new ObjectParameter("region", region), new ObjectParameter("start", s), new ObjectParameter("end", e) }).ToList();

            }
            var results = (from d in data
                let index = d.index
                let name = d.type == "store" ? d.STORE_NAME : d.type == "team" ? d.PS_Team : d.type == "subteam" ? d.ps_subteam : ""
                let displayPercent =    d.OOSPercent
                let collapsed = d.type != "store" ? "collapsed" : ""
                let hasChildren = d.haschilren == 1 ? "hasChildren" : ""
                where index != null
                select new SummaryPageItem()
                {
                    Id = (int) index,
                    HasChildren = hasChildren,
                    Name =  name,
                    OosCount = d.cnt.HasValue ? d.cnt.Value : 0,
                    OosPercent = (double) d.OOSPercent ,
                    Parent = d.parent.HasValue ? d.parent.Value : 0,
                    Type = d.type,
                    Collapsed = collapsed,
                    scanCount = d.scanCount.HasValue ? d.scanCount.Value : 0,
                    type=d.type

                }).ToList();
            return Json(results, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetSummaryHeader(string region, string start, string end)
        {
            IEnumerable<SummaryStoreHeaders_Result> results = null;
            var s = DateTime.Parse(start);
            var e = DateTime.Parse(end);
            using (var ctx = new OOSEntities())
            {
                results = ctx.ExecuteFunction<SummaryStoreHeaders_Result>("SummaryStoreHeaders", new [] {new ObjectParameter("region", region),new ObjectParameter("start", s),new ObjectParameter("end", e) }).ToList();

            }
            return Json(results,JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetSummaryDetails(int psbu, string start, string end)
        {
            IEnumerable<SummaryStoreDetail_Result> results = null;
            DateTime s = DateTime.Parse(start);
            DateTime e = DateTime.Parse(end);

            using (var ctx = new OOSEntities(MvcApplication.oosEFConnectionString))
            {
                results = ctx.ExecuteFunction<SummaryStoreDetail_Result>("SummaryStoreDetail", new[] { new ObjectParameter("psbu", psbu), new ObjectParameter("start", s), new ObjectParameter("end", e) }).ToList();
                

            }
            return Json(results, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Create(string selectedStartDate, string selectedEndDate, string clientTodayDate, string selectedRegion, string[] regionListSelected, string hideStoresNotReporting)
        {
           // var telemetry = new TelemetryClient();
            var sw = new Stopwatch();
            sw.Start();
            var user = OOSUser.GetUserName();
            //MvcApplication.oosLog.Trace(string.Format("Summary Report for {0}",user));
            ActionResult result = null;
            var startDate = Convert.ToDateTime(selectedStartDate);
            var endDate = Convert.ToDateTime(selectedEndDate);

            DateTime todaysDate;
            DateTime.TryParse(clientTodayDate, out todaysDate);
            //var inclusiveDate = new InclusiveTimeZoneEndDateSpecification(todaysDate, endDate).InclusiveEndDate;
            IEnumerable<SummaryReportViewModel> viewModel = GetModel(startDate, endDate, selectedRegion, regionListSelected, hideStoresNotReporting != null);

            if (viewModel.Any())
            {
                ViewBag.Message = string.Format("Summary for {0} to {1} in {2}", startDate, endDate,
                    ViewBag.selectedRegion);
            }
            
            result = View("Index", viewModel);
            sw.Stop();

            MvcApplication.oosLog.Trace(string.Format("Summary Report for {0} took {1} millisecond(s). Parameters: From={2} To={3} Region={4}", user, sw.ElapsedMilliseconds, startDate.ToShortDateString(), endDate.ToShortDateString(), ViewBag.selectedRegion));

            var properties = new Dictionary<string, string> { {"User", user},{"StartDate", startDate.ToShortDateString()},{"EndDate", endDate.ToShortDateString()},{"Region", ViewBag.selectedRegion},{ "Page", "SummaryReport" } };
            var metrics = new Dictionary<string, double> {{ "timeElapsed", sw.ElapsedMilliseconds } };
          //  telemetry.TrackEvent("SummaryReport", properties,metrics);

            return result; 
        }

        private IEnumerable<SummaryReportViewModel> GetModel(DateTime dtStart, DateTime dtEnd, string selectedPriorRegion, string[] regionListSelected, bool isHideStoresNotReporting)
        {
            IEnumerable<SummaryReportViewModel> viewModel = null;

            var selectedRegion = string.Empty;
            if (!OOSUser.isCentral)
                selectedRegion = OOSUser.userRegion;
            else if (regionListSelected != null && regionListSelected.Length > 0 &&
                regionListSelected[0].Length == 2)
                selectedRegion = regionListSelected[0];
            else
                selectedRegion = selectedPriorRegion;

            
            viewModel = GetSummaryReportFor(dtStart, dtEnd, selectedRegion);

            var byTeamAverage =
                new Dictionary<SummaryReportViewModel.TeamEnum, SummaryReportViewModel.SummaryReportViewModelByTeam>();
            foreach (SummaryReportViewModel.TeamEnum teamEnum in Enum.GetValues(typeof(SummaryReportViewModel.TeamEnum)))
            {
                System.Diagnostics.Debug.WriteLine("Team: ",teamEnum.ToString());
                int storeOOSAverageCount = 0;
                int storeUPCAverageCount = 0;
                double teamStoreCount = viewModel.Sum(d => d.storeTimesScanned > 0 && d.storeByTeam.Keys.Contains(teamEnum) && d.storeByTeam[teamEnum].storeOOSCount > 0 ? 1 : 0);
                System.Diagnostics.Debug.WriteLine("Team Store Count: ", teamStoreCount.ToString());
                if (teamStoreCount > 0)
                {

                    int tmp = viewModel.Sum(d => d.storeByTeam[teamEnum].storeOOSCount);
                    System.Diagnostics.Debug.WriteLine(string.Format("viewModel.Sum(d => d.storeByTeam[teamEnum].storeOOSCount: {0}", tmp.ToString()));

                    storeOOSAverageCount = (int)
                        ((double)viewModel.Sum(d => d.storeTimesScanned == 0 ? 0 : d.storeByTeam[teamEnum].storeOOSCount) / teamStoreCount);
                    storeUPCAverageCount = (int)
                        ((double)viewModel.Sum(d => d.storeTimesScanned == 0 ? 0 : d.storeByTeam[teamEnum].storeUPCCount) / teamStoreCount);
                    System.Diagnostics.Debug.WriteLine("storeOOSAverageCount: ", storeOOSAverageCount.ToString());
                    System.Diagnostics.Debug.WriteLine("storeUPCAverageCount: ", storeUPCAverageCount.ToString());
                }
                byTeamAverage.Add(teamEnum, new SummaryReportViewModel.SummaryReportViewModelByTeam(
                    storeOOSAverageCount, storeUPCAverageCount));
            }
            int noTimesScanned = 0;
            {
                double storeCount = viewModel.Count(p => p.storeTimesScanned > 0);
                if (storeCount > 0)
                    noTimesScanned = (int)(viewModel.Sum(d => d.storeTimesScanned));

                        System.Diagnostics.Debug.WriteLine("storeCount: ", storeCount.ToString());
            }

            System.Diagnostics.Debug.WriteLine("noTimesScanned: ", noTimesScanned.ToString());
            ViewBag.byTeamAverage = byTeamAverage;
            ViewBag.NoTimesScanned = noTimesScanned;
            ViewBag.defaultUPCCount = defaultUPCCount;
            ViewBag.percentFormat = percentFormat;
            ViewBag.isRegionSelectable = OOSUser.isCentral;
            ViewBag.regions = null;
            if (!OOSUser.isCentral)
                ViewBag.regions = null;
            else
            {
                OOSEntities db = new OOSEntities(MvcApplication.oosEFConnectionString);
                ViewBag.regions = db.REGION
                    .Where(r => r.IS_VISIBLE.Equals("true", StringComparison.OrdinalIgnoreCase))
                    .OrderBy(r => r.REGION_ABBR)
                    .Select(r => r).ToList();
            }

            ViewBag.selectedRegion = selectedRegion;
            ViewBag.hideStoresNotReporting = isHideStoresNotReporting;

            return viewModel;
        }

        private IEnumerable<SummaryReportViewModel> GetSummaryReportFor(DateTime dtStart, DateTime dtEnd, string region)
        {
            var summaryReport = service.SummaryReportFor(dtStart, dtEnd, region);
            return summaryReport.OrderBy(p => p.storeName);
        }

    }
}
