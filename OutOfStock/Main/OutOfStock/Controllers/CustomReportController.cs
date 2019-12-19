using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OOS.Model;
using OOSCommon;
using OutOfStock.Models;
using OOSCommon.DataContext;
using StructureMap;

namespace OutOfStock.Controllers
{
    public class CustomReportController : Controller
    {
        private static IOOSLog logger = ObjectFactory.GetInstance<ILogService>().GetLogger();
        
        const string mimeType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        const string fileDownloadNameFormat = "CustomReport-{0:MMMddyyyy}-{1:MMMddyyyy}.xlsx";
        const string allStoresText = "(all stores)";
        const string allTeamsText = "(all teams)";
        const string allSelectKey = "0";

        private IOOSEntitiesFactory entityFactory; 
        public MultiSelectList GetSelectedList { get; private set; }

        private IUserProfile userProfile;
        private IConfigurator config;
        private ICustomReportExcelModel excelModel;
        private CustomReportViewModel customReportViewModel;

        public CustomReportController(IUserProfile userProfile, IConfigurator config, IOOSEntitiesFactory entityFactory, 
            ICustomReportExcelModel excelModel, CustomReportViewModel customReportViewModel)
        {
            this.userProfile = userProfile;
            this.config = config;
            this.entityFactory = entityFactory;
            this.excelModel = excelModel;
            this.customReportViewModel = customReportViewModel;
        }


       
        public ActionResult Index()
        {
            logger.Trace(string.Format("CustomReportController::Index() Central: {0} Regional: {1} Region: {2} Store: {3}", userProfile.IsCentral().ToString(), userProfile.IsRegionBuyer().ToString(), userProfile.UserRegion().ToString(), userProfile.UserStoreAbbreviation().ToString()));
            ActionResult result = null;
            if (!userProfile.IsStoreLevel())
                result = new RedirectResult("~/Home/Index");
            else
            {
                // Get region or regions list for view
                if (userProfile.IsCentral())
                {
                    ViewBag.MyRegion = null;
                    using (var dbContext = entityFactory.New())
                    {
                        ViewBag.MyRegions = dbContext.REGION
                        .Where(r => r.IS_VISIBLE.Equals("true", StringComparison.OrdinalIgnoreCase))
                        .OrderBy(r => r.REGION_ABBR)
                        .Select(r => r).ToList();
                    }
                    ViewBag.MyStore = null;
                    ViewBag.MyStores = new List<SelectListItem>();
                }
                else
                {

                    ViewBag.MyRegion = userProfile.UserRegion();
                    ViewBag.MyRegions = null;
                    ViewBag.MyStore = null;
                    var storeList = GetStores(userProfile.UserRegion()).ToList();
                    storeList.Insert(0, new SelectListItem() { Text = allStoresText, Value = "0" });
                    ViewBag.MyStores = storeList;
                }

                // Get teams from VIM 
                List<KeyValuePair<string, string>> teamList = OutOfStock.MvcApplication.vimRepository.GetVimTeam()
                    .Where(t => !string.IsNullOrEmpty(t.team_name))
                    .OrderBy(d => d.team_name)
                    .Select(t => new KeyValuePair<string, string>(t.team_name, t.team_name))
                    .ToList();
                teamList.Insert(0, new KeyValuePair<string, string>(allSelectKey, allTeamsText));
                ViewBag.MyTeams = teamList;
                result = View();
            }
            return result;
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public JsonResult LoadStoresByRegion(string regionId)
        {
            IEnumerable<SelectListItem> storeData = null;
            // Parse the regionId to an int and get the store select list enumeration
            int regionIdAsInt = 0;
            if (!string.IsNullOrWhiteSpace(regionId) && int.TryParse(regionId, out regionIdAsInt))
                storeData = GetStores(regionIdAsInt);
            return Json(storeData, JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public JsonResult LoadSubTeamsByTeam(string regionName, string storeNumbers, string teamNames)
        {
            // The primary influence on the subteam list is the set of teams
            var teamHash = new HashSet<string>();
            bool isAllTeamsSelected = false;
            if (!string.IsNullOrWhiteSpace(teamNames))
            {
                string[] teamNameArray = teamNames.Split(new char[] {','} );
                foreach (string item in teamNameArray)
                {
                    if (!string.IsNullOrWhiteSpace(item) && !teamHash.Contains(item))
                    {
                        if (item.Equals(allSelectKey))
                            isAllTeamsSelected = true;
                        else
                            teamHash.Add(item);
                    }
                }
            }
            // The subteam list is influenced by store ... but this use of this dimension is out of scope
            var storeHash = new HashSet<string>();
            var isAllStoresSelected = false;
            if (!string.IsNullOrWhiteSpace(storeNumbers))
            {
                string[] storeNameArray = storeNumbers.Split(new char[] { ',' });
                foreach (string item in storeNameArray)
                {
                    if (!string.IsNullOrWhiteSpace(item) && !storeHash.Contains(item))
                    {
                        if (item.Equals(allSelectKey))
                            isAllStoresSelected = true;
                        else
                            storeHash.Add(item);
                    }
                }
            }
            // The business requirement says to select sub teams by teams.  Thus the other filters are disabled
            List<string> subteamList = OutOfStock.MvcApplication.vimRepository.GetVimSubTeam(
                string.Empty,   //regionName,
                null,           //isAllStoresSelected ? null : storeHash.ToList(),
                isAllTeamsSelected ? null : teamHash.ToList())
                .OrderBy(s => s.subteam_name)
                .Select(s => s.subteam_name)
                .ToList();
            var subteamData = new List<SelectListItem>();
            foreach (string subteamName in subteamList)
            {
                var item = new SelectListItem();
                item.Value = subteamName;
                item.Text = subteamName;
                item.Selected = false;
                subteamData.Add(item);
            }
            return Json(subteamData, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Create(string selectedRegion, string selectedStores,
            string selectedTeams, string selectedSubTeams,
            string selectedStartDate, string selectedEndDate, string clientTodayDate)
        {
            logger.Trace(string.Format("CustomReportController::Index() Central: {0} Regional: {1} Region: {2} Store: {3}", userProfile.IsCentral().ToString(), userProfile.IsRegionBuyer().ToString(), userProfile.UserRegion().ToString(), userProfile.UserStoreAbbreviation().ToString()));
            ActionResult result = null;
            if (!userProfile.IsStoreLevel())
                result = new RedirectResult("~/Home/Index");
            else
            {
                // Selecting "all stores" is the same as selecting no stores
                if (!string.IsNullOrWhiteSpace(selectedStores))
                {
                    string[] storeList = selectedStores.Split(new char[] { ',' });
                    if (storeList.Where(s => s.Equals(allSelectKey)).Any())
                        selectedStores = string.Empty;
                }

                // This provide a missing values and enforces security
                if (!userProfile.IsCentral())
                {
                    selectedRegion = userProfile.UserRegion();
                    if (!userProfile.IsRegionBuyer())
                    {
                        var userStoreAbbrev = userProfile.UserStoreAbbreviation();
                        int? storeId;
                        using(var dbContext = entityFactory.New())
                        {
                            storeId = dbContext.STORE
                            .Where(s => s.STORE_ABBREVIATION.Equals(userStoreAbbrev, StringComparison.OrdinalIgnoreCase))
                            .Select(s => s.ID)
                            .FirstOrDefault();
                        }
                        selectedStores = (storeId.HasValue ? storeId.Value.ToString() : string.Empty);
                    }
                }

                try
                {
                    // Checking for nulls and assigning the default values when no values are selected
                    DateTime? startDate = null;
                    DateTime? endDate = null;
                    {
                        DateTime tempDate;
                        if (DateTime.TryParse(selectedStartDate, out tempDate))
                            startDate = tempDate;
                        if (DateTime.TryParse(selectedEndDate, out tempDate))
                            endDate = tempDate;
                    }
                    if (!endDate.HasValue)
                        endDate = DateTime.Now;
                    if (!startDate.HasValue)
                        startDate = endDate.Value.AddDays(-7);

                    // Get store list
                    Dictionary<int, string> dicStore = null;
                    var idStoreClosedStatus = GetStoreClosedStatus(entityFactory);
                    // Get store list from selection
                    if (!string.IsNullOrWhiteSpace(selectedStores) && selectedStores.Length > 1)
                    {
                        string[] items = selectedStores.Split(new char[] { ',' });
                        if (items != null && items.Length > 0 && !items[0].Equals(allSelectKey))
                        {
                            Dictionary<int, string> dicStoreTemp = items
                                .Where(d => !string.IsNullOrWhiteSpace(d))
                                .ToDictionary(d => Convert.ToInt32(d), d => d);

                            using(var dbContext = entityFactory.New())
                            {
                                dicStore =
                                (from s in dbContext.STORE
                                 where dicStoreTemp.Keys.Contains(s.ID) && s.STATUS_ID != idStoreClosedStatus
                                 orderby s.STORE_ABBREVIATION
                                 select s)
                                .ToDictionary(d => d.ID, d => d.STORE_ABBREVIATION);
                            }
                            
                        }
                    }
                    // Get store list from the entire region
                    else if (!string.IsNullOrWhiteSpace(selectedRegion))
                    {
                        using(var dbContext = entityFactory.New())
                        {
                            dicStore =
                            (from s in dbContext.STORE
                             join r in dbContext.REGION on s.REGION_ID equals r.ID
                             where r.REGION_ABBR.Equals(selectedRegion, StringComparison.OrdinalIgnoreCase) &&
                                s.STATUS_ID != idStoreClosedStatus
                             orderby s.STORE_ABBREVIATION
                             select s)
                            .ToDictionary(s => s.ID, s => s.STORE_ABBREVIATION);
                        }
                        
                    }

                    Dictionary<string, string> dicTeam = null;
                    if (!string.IsNullOrWhiteSpace(selectedTeams) && selectedTeams.Length > 1)
                    {
                        string[] items = selectedTeams.Remove(selectedTeams.Length - 1, 1).Split(new char[] { ',' });
                        if (items != null && items.Length > 0 && !items[0].Equals(allSelectKey))
                            dicTeam = items.Where(d => !string.IsNullOrWhiteSpace(d))
                                .ToDictionary(d => d, d => d);
                    }

                    Dictionary<string, string> dicSubTeam = null;
                    if (!string.IsNullOrWhiteSpace(selectedSubTeams))
                    {
                        string[] items = selectedSubTeams.Remove(selectedSubTeams.Length - 1, 1).Split(new char[] { ',' });
                        if (items != null && items.Length > 0 && !items[0].Equals(allSelectKey))
                            dicSubTeam = items.Where(d => !string.IsNullOrWhiteSpace(d))
                                .ToDictionary(d => d, d => d);
                    }

                    // Make header text in Excel sheet

                    string headerMain = "Custom report for "
                        + (startDate.HasValue ? startDate.Value.ToString("MM/dd/yyyy") : "start of records")
                        + " through "
                        + endDate.GetValueOrDefault(DateTime.Now).ToString("MM/dd/yyyy");
                    string headerStores = "  Stores: ";
                    if (dicStore == null || dicStore.Count < 1)
                    {
                        dicStore = null;
                        headerStores += "All";
                    }
                    else
                    {
                        bool isFirst = true;
                        foreach (KeyValuePair<int, string> item in dicStore)
                        {
                            headerStores += (isFirst ? string.Empty : ", ") + item.Value;
                            isFirst = false;
                        }
                    }
                    string headerTeams = "  Teams: ";
                    if (dicTeam == null || dicTeam.Count < 1)
                    {
                        dicTeam = null;
                        headerTeams += "All";
                    }
                    else
                    {
                        bool isFirst = true;
                        foreach (KeyValuePair<string, string> item in dicTeam)
                        {
                            headerTeams += (isFirst ? string.Empty : ", ") + item.Value;
                            isFirst = false;
                        }
                    }
                    string headerSubTeams = "  SubTeams: ";
                    if (dicSubTeam == null || dicSubTeam.Count < 1)
                    {
                        dicSubTeam = null;
                        headerSubTeams += "All";
                    }
                    else
                    {
                        bool isFirst = true;
                        foreach (KeyValuePair<string, string> item in dicSubTeam)
                        {
                            headerSubTeams += (isFirst ? string.Empty : ", ") + item.Value;
                            isFirst = false;
                        }
                    }

                    DateTime todaysDate;
                    DateTime.TryParse(clientTodayDate, out todaysDate);
                    IEnumerable<CustomReportViewModel> crvm = customReportViewModel.RunQuery(startDate, endDate, dicStore, dicTeam, dicSubTeam, todaysDate);
                    
                    string excelFilename = config.TemporaryDownloadFilePath() + Guid.NewGuid() + ".xlsx";
                    string resultMessage = excelModel.CreateExcelFile(excelFilename, crvm,
                        headerMain, headerStores, headerTeams, headerSubTeams);
                    

                    if (resultMessage.Length > 0)
                    {
                        ViewBag.Message = "Creation results: " + resultMessage;
                        result = View("Index");
                    }
                    else
                    {
                        // Now download the result
                        System.IO.FileStream fileStream = null;
                        for (int retry = 0; ; )
                        {
                            // Attempt to open the file -- Excel may still be holding it open
                            try
                            {
                                fileStream = new System.IO.FileStream(excelFilename, System.IO.FileMode.Open);
                            }
                            catch (System.IO.IOException)
                            {
                                System.Threading.Thread.Sleep(200);
                            }
                            // If we got the file, download it
                            if (fileStream != null)
                            {
                                string fileDownloadName = string.Format(fileDownloadNameFormat, startDate.Value, endDate.Value);
                                result = File(fileStream, mimeType, fileDownloadName);
                                break;
                            }
                            // If we did not get the file, allow 10 attempts before giving up
                            if (retry++ < 10) continue;
                            ViewBag.Message = "Could not download Excel file";
                            result = View("Index");
                            break;
                        }
                    }
                }
                catch (Exception ex)
                {
                    logger.Warn(ex.Message + ", Stack=" + ex.StackTrace);
                    ViewBag.Message = "Sorry, an internal error has occurred.";
                    result = View("Index");
                }
            }
            return result;
        }

        /// <summary>
        /// An action without navigation to cleanup temporary files
        /// </summary>
        /// <returns></returns>
        public ActionResult DoTempFileCleanup()
        {
            logger.Trace("Enter");
            ActionResult result = null;
            if (userProfile.IsCentral())
            {
                ViewBag.Message = "Files cleaned up: " + TempFileCleanup();
            }
            result = View("Index");
            logger.Trace("Exit");
            return result;
        }

        /// <summary>
        /// Called to cleanup files previously downloaded.
        /// Specifically, delete files in ~/App_Data/FilesToDownload/*.xlsx 
        /// for files with create data less than more than 1 hour old
        /// </summary>
        public static int TempFileCleanup()
        {
            int result = 0;
            System.IO.DirectoryInfo directoryInfo = null;
            IEnumerable<System.IO.FileInfo> files = null;
            try
            {
                directoryInfo = new System.IO.DirectoryInfo(
                    OutOfStock.MvcApplication.oosTemporaryDownloadFilePath);
                files =
                    directoryInfo.EnumerateFiles("*.xlsx")
                    .Where(f => f.CreationTimeUtc < DateTime.Now.AddHours(-1))
                    .Select(f => f);
                foreach (System.IO.FileInfo file in files)
                {
                    file.Delete();
                    ++result;
                }
            }
            catch (Exception ex)
            {
                logger.Warn(ex.Message + ", Stack=" + ex.StackTrace);
            }
            return result;
        }

        /// <summary>
        /// Get an alphabetical select list enumeration of stores for the region
        /// </summary>
        /// <param name="regionAbbreviation"></param>
        /// <returns></returns>
        protected IEnumerable<SelectListItem> GetStores(string regionAbbreviation)
        {
            using (var dbContext = entityFactory.New())
            {
                int? regionId = (dbContext.REGION
                .Where(r => r.REGION_ABBR.Equals(regionAbbreviation, StringComparison.OrdinalIgnoreCase))
                .Select(r => r.ID)).FirstOrDefault();
                return regionId.HasValue ? GetStores(regionId.Value) : null;
            }
        }

        /// <summary>
        /// Get an alphabetical select list enumeration of stores for the region
        /// </summary>
        /// <param name="regionId"></param>
        /// <returns></returns>
        protected IEnumerable<SelectListItem> GetStores(int regionId)
        {
            int idStoreClosedStatus = GetStoreClosedStatus(entityFactory);
            using (var dbContext = entityFactory.New())
            {
                IQueryable<STORE> storeQuery = dbContext.STORE
                .Where(m => m.REGION_ID == regionId && m.STATUS_ID != idStoreClosedStatus)
                .OrderBy(m => m.STORE_ABBREVIATION);
                return storeQuery.ToList().OrderBy(x=>x.STORE_NAME).Select(store => new SelectListItem() { Text = string.Format("{0}", store.STORE_NAME), Value = store.ID.ToString() }).ToList();
            }
        }

        /// <summary>
        /// Return the status indicating that a store is closed
        /// </summary>
        /// <returns></returns>
        protected static int GetStoreClosedStatus(IOOSEntitiesFactory entitiesFactory)
        {
            using (var dbContext = entitiesFactory.New())
            {
                if (!idStoreClosedStatus.HasValue)
                {
                    int? idStatus =
                        (from s in dbContext.STATUS
                         where s.STATUS1.Equals("CLOSED", StringComparison.OrdinalIgnoreCase)
                         select s.ID).FirstOrDefault();
                    idStoreClosedStatus = idStatus.GetValueOrDefault(0);
                }
                return idStoreClosedStatus.GetValueOrDefault(0);
            }
        }
        protected static int? idStoreClosedStatus = null;
    }
}
     

