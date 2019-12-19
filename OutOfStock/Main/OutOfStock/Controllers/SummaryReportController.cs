using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.DirectoryServices;
using System.IO;
using System.Web.Configuration;
using OOS.Model;
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

        public ActionResult Index()
        {
            MvcApplication.oosLog.Trace("SummaryReportController::Index() Enter");
            ActionResult result = null;
            if (!OOSUser.isStoreLevel)
                result = new RedirectResult("~/Home/Index");
            else
            {
                var dtEnd = DateTime.Now;
                var dtStart = dtEnd.AddDays(-7);
                IEnumerable<SummaryReportViewModel> viewModel = GetModel(dtStart, dtEnd, OOSUser.userRegion, null, false);
                result = View("Index", viewModel);
            }
            MvcApplication.oosLog.Trace("SummaryReportController::Index() Exit");
            return result;
        }

        [HttpPost]
        public ActionResult Create(string selectedStartDate, string selectedEndDate, string clientTodayDate, string selectedRegion, string[] regionListSelected, string hideStoresNotReporting)
        {
            MvcApplication.oosLog.Trace("SummaryReportController::Create() Enter");
            ActionResult result = null;
            var startDate = Convert.ToDateTime(selectedStartDate);
            var endDate = Convert.ToDateTime(selectedEndDate);

            DateTime todaysDate;
            DateTime.TryParse(clientTodayDate, out todaysDate);
            var inclusiveDate = new InclusiveTimeZoneEndDateSpecification(todaysDate, endDate).InclusiveEndDate;
            IEnumerable<SummaryReportViewModel> viewModel = GetModel(startDate, inclusiveDate, selectedRegion, regionListSelected, hideStoresNotReporting != null);

            if (viewModel.Count() > 0)
            {
                ViewBag.Message = "Summary for " + startDate.ToShortDateString() + " to " + endDate.ToShortDateString() + " in " + ViewBag.selectedRegion;
            }
            
            result = View("Index", viewModel);
            MvcApplication.oosLog.Trace("SummaryReportController::Create() Exit");
            return result; 
        }

        private IEnumerable<SummaryReportViewModel> GetModel(DateTime dtStart, DateTime dtEnd, string selectedPriorRegion, string[] regionListSelected, bool isHideStoresNotReporting)
        {
            IEnumerable<SummaryReportViewModel> viewModel = null;

            string selectedRegion = string.Empty;
            if (!OOSUser.isCentral)
                selectedRegion = OOSUser.userRegion;
            else if (regionListSelected != null && regionListSelected.Length > 0 &&
                regionListSelected[0].Length == 2)
                selectedRegion = regionListSelected[0];
            else
                selectedRegion = selectedPriorRegion;

            
            viewModel = GetSummaryReportFor(dtStart, dtEnd, selectedRegion);

            // Regional buyer (which includes central) includes all stores
            //string storeAbbreviation = (OOSUser.isRegionalBuyer ? string.Empty : OOSUser.userStore);
            //bool isClosedExcluded = true;
            //viewModel = SummaryReportViewModel.RunQuery(
            //    selectedRegion,
            //    storeAbbreviation,
            //    dtStart,
            //    dtEnd, isClosedExcluded, isHideStoresNotReporting);

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
