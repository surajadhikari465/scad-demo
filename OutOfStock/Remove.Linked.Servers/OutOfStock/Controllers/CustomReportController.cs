using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Objects.SqlClient;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
//using Microsoft.ApplicationInsights;
using OOS.Model;
using OOSCommon;
using OOSCommon.Movement;
using OutOfStock.Classes;
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
       // private IMovementRepository movementRepository;

        private IUserProfile userProfile;
        private IConfigurator config;
        private ICustomReportExcelModel excelModel;
        
        
        public CustomReportController(IUserProfile userProfile, IConfigurator config, IOOSEntitiesFactory entityFactory )
        {
            this.userProfile = userProfile;
            this.config = config;
            this.entityFactory = entityFactory;
            this.excelModel = new ReportExcelModel(logger);

        }



       
        public ActionResult Index()
        {
            
           // telemetry.TrackPageView("CustomReport");

            logger.Trace(string.Format("CustomReportController::Index() Central: {0} Regional: {1} Region: {2} Store: {3}", userProfile.IsCentral().ToString(), userProfile.IsRegionBuyer().ToString(), userProfile.UserRegion().ToString(), userProfile.UserStoreAbbreviation().ToString()));
            ActionResult result = null;

            if (!OOSUser.HasValidLocationInformation)
            {
                result = View("~/Views/Shared/InvalidLocationInformation.cshtml");
            }
            else
            {
                if (!userProfile.IsStoreLevel())
                    result = new RedirectResult("~/Home/Index");
                else
                {
                    try
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
                            var storeList = (List<SelectListItem>) GetStores(userProfile);
                            storeList.Insert(0, new SelectListItem() {Text = allStoresText, Value = "0"});
                            ViewBag.MyStores = storeList;
                        }
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex.Message);
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
            //var isAllStoresSelected = false;
            if (!string.IsNullOrWhiteSpace(storeNumbers))
            {
                string[] storeNameArray = storeNumbers.Split(new char[] { ',' });
                foreach (string item in storeNameArray)
                {
                    if (!string.IsNullOrWhiteSpace(item) && !storeHash.Contains(item))
                    {
                        if (!item.Equals(allSelectKey))
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

            // remove commans from  subteam names. to handle subteams like "Tea, Coffe, Housewares". otherwise stored procs break.
            subteamList = subteamList.Select(s => s.Replace(",", "")).ToList();

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
            string selectedStartDate, string selectedEndDate, string clientTodayDate, int isGroupedByUpc)
        {
            var excelFilename = config.TemporaryDownloadFilePath() + Guid.NewGuid() + ".xlsx";
            
            logger.Trace(
                $"CustomReportController::Index() Central: {userProfile.IsCentral()} Regional: {userProfile.IsRegionBuyer()} Region: {userProfile.UserRegion()} Store: {userProfile.UserStoreAbbreviation()}");
            ActionResult result = null;
            if (!userProfile.IsStoreLevel())
                result = new RedirectResult("~/Home/Index");
            else
            {

                // Selecting "all stores" is the same as selecting no stores
                if (!string.IsNullOrWhiteSpace(selectedStores))
                {
                    // check if "all stores" w/ index 0 was selected.
                    var zeroselected = from s in selectedStores.Split('|')
                                       where s.Equals("0")
                                       select s;

                    // if so, set selectedStores = empty to force loading of all stores.
                    if (zeroselected.Any())
                        selectedStores = string.Empty;
                }

                if (!userProfile.IsCentral())
                    selectedRegion = userProfile.UserRegion();

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

                    // ################
                    //  Get store list
                    // ################
                    Dictionary<int, string> StoreInfo = null;
                    //var idStoreClosedStatus = GetStoreClosedStatus(entityFactory);
                    
                   
                    if (!string.IsNullOrWhiteSpace(selectedStores) && selectedStores.Length > 1)
                    {
                        // get an array of integers from the comma separated list of store ids. exclude null values
                        var items = selectedStores.Split('|').Where(s => s != "" ).Select(s => int.Parse(s)).ToArray();
                        if (items != null && items.Length > 0 && !items[0].Equals(0))
                        {
                            // get StoreAbbreviations for all Ids. exclude closed stores.
                            using(var dbContext = entityFactory.New())
                            {
                                StoreInfo = (from s in dbContext.STORE join st in dbContext.STATUS on s.STATUS_ID equals st.ID
                                               where items.Contains(s.ID)
                                               && st.STATUS1 != "Closed"
                                               select s).ToDictionary(d => d.ID, d => d.STORE_ABBREVIATION);
                            }
                            
                        }
                    }
                    // Get store list from the entire region
                    else if (!string.IsNullOrWhiteSpace(selectedRegion))
                    {
                        using(var dbContext = entityFactory.New())
                        {
                            StoreInfo =
                            (from s in dbContext.STORE
                             join r in dbContext.REGION on s.REGION_ID equals r.ID
                             join st in dbContext.STATUS on s.STATUS_ID equals st.ID
                             where r.REGION_ABBR.Equals(selectedRegion, StringComparison.OrdinalIgnoreCase) &&
                                st.STATUS1 != "Closed" 
                             orderby s.STORE_ABBREVIATION
                             select s)
                            .ToDictionary(s => s.ID, s => s.STORE_ABBREVIATION);
                        }
                        
                    }

                    // ################
                    //  Get team list
                    // ################
                    Dictionary<string, string> TeamInfo = null;
                    if (!string.IsNullOrWhiteSpace(selectedTeams) && selectedTeams.Length > 1)
                    {
                        string[] items = selectedTeams.Remove(selectedTeams.Length - 1, 1).Split(new char[] { '|' });
                        if (items != null && items.Length > 0 && !items[0].Equals(allSelectKey))
                            TeamInfo = items.Where(d => !string.IsNullOrWhiteSpace(d))
                                .ToDictionary(d => d, d => d);
                    }

                    Dictionary<string, string> SubTeamInfo = null;
                    if (!string.IsNullOrWhiteSpace(selectedSubTeams))
                    {
                        var items = selectedSubTeams.Remove(selectedSubTeams.Length - 1, 1).Split(new char[] { '|' });
                        if (items != null && items.Length > 0 && !items[0].Equals(allSelectKey))
                            SubTeamInfo = items.Where(d => !string.IsNullOrWhiteSpace(d))
                                .ToDictionary(d => d, d => d);
                    }


             
                    
                    // ################################
                    //  Make header text in Excel sheet
                    // ################################

                    var reportHeaderInfo = new ReportHeaderDataModel
                        {
                            Main = string.Format("Custom report for {0} through {1}",
                                                 (startDate.HasValue
                                                      ? startDate.Value.ToString("MM/dd/yyyy")
                                                      : "start of records"),
                                                 (endDate.GetValueOrDefault(DateTime.Now).ToString("MM/dd/yyyy"))
                                ),
                            Stores = "  Stores: "
                        };

                    if (StoreInfo == null || StoreInfo.Count < 1)
                    {
                        StoreInfo = null;
                        reportHeaderInfo.Stores += "All";
                    }
                    else
                    {
                            reportHeaderInfo.Stores  = string.Join(", ", StoreInfo.Values.ToArray());// += (isFirst ? string.Empty : ", ") + item.Value;
                     }

                    reportHeaderInfo.Teams = "  Teams: ";
                    if (TeamInfo == null || TeamInfo.Count < 1)
                    {
                        TeamInfo = null;
                        reportHeaderInfo.Teams += "All";
                    }
                    else
                    {
                        reportHeaderInfo.Teams = string.Join(", ", TeamInfo.Values.ToArray());// += (isFirst ? string.Empty : ", ") + item.Value;
                    }

                    reportHeaderInfo.SubTeams = "  SubTeams: ";
                    if (SubTeamInfo == null || SubTeamInfo.Count < 1)
                    {
                        SubTeamInfo = null;
                        reportHeaderInfo.SubTeams += "All";
                    }
                    else
                    {
                        reportHeaderInfo.SubTeams = string.Join(", ", SubTeamInfo.Values.ToArray());// += (isFirst ? string.Empty : ", ") + item.Value;
                    }



                    // ###########
                    //  Run Query
                    // ###########

                    DateTime todaysDate;
                    DateTime.TryParse(clientTodayDate, out todaysDate);


                    ICustomReportViewModel crvm;
                    IEnumerable<ICustomReportExcelModel> reportData;
                    IEnumerable<ScanWithNoVimData> scansMissingVimData;


                    var properties = new Dictionary<string, string> { { "User", userProfile.UserName }, { "StartDate", startDate.Value.ToShortDateString() }, { "EndDate", endDate.Value.ToShortDateString() }, { "Region", userProfile.UserRegion()},  { "Page", "CustomReport" } };
                    var metrics = new Dictionary<string, double> { };


                    if (SubTeamInfo != null)
                        properties.Add("Subteams", string.Join(",", SubTeamInfo.Select(st => st.Value)));
                    
                    if (StoreInfo !=null)
                        properties.Add("Stores", string.Join(",", StoreInfo.Select(st => st.Value)));
               


                    crvm = StoreInfo.Count > 1
                        ? (ICustomReportViewModel) new MultiStoreReportViewModel()
                        : new SingleStoreReportViewModel();


                    var runQueryParameters = new RunQueryParameters(startDate, endDate, StoreInfo, TeamInfo, SubTeamInfo, todaysDate, isGroupedByUpc);
                    

                    reportData = crvm.RunQuery(runQueryParameters, ref entityFactory);
                    scansMissingVimData = crvm.GetScanswithNoVimData(startDate, endDate, StoreInfo, TeamInfo,
                        SubTeamInfo, todaysDate, ref entityFactory);

                    var resultMessage = excelModel.CreateExcelFile(excelFilename, crvm.Columns, ref reportData, ref reportHeaderInfo, ref scansMissingVimData);

                    if (resultMessage.Length > 0)
                    {
                        ViewBag.Message = "Creation results: " + resultMessage;
                        result = View("Index");
                    }
                    else
                    {
                        // Now download the result
                        System.IO.FileStream fileStream = null;
                        for (var retry = 0; ; )
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
                                var fileDownloadName = string.Format(fileDownloadNameFormat, startDate.Value, endDate.Value);
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
            //logger.Trace("Enter");
            if (userProfile.IsCentral())
            {
                ViewBag.Message = "Files cleaned up: " + TempFileCleanup();
            }
            ActionResult result = View("Index");
            //logger.Trace("Exit");
            return result;
        }

        /// <summary>
        /// Called to cleanup files previously downloaded.
        /// Specifically, delete files in ~/App_Data/FilesToDownload/*.xlsx 
        /// for files with create data less than more than 1 hour old
        /// </summary>
        public static int TempFileCleanup()
        {
            var result = 0;
            try
            {
                var directoryInfo = new System.IO.DirectoryInfo(OutOfStock.MvcApplication.oosTemporaryDownloadFilePath);
                var files = directoryInfo.EnumerateFiles("*.xlsx")
                     .Where(f => f.CreationTimeUtc < DateTime.Now.AddHours(-1))
                     .Select(f => f);

                foreach (var file in files)
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

        protected IEnumerable<SelectListItem> GetStores(IUserProfile user)
        {
            var isCentral = user.IsCentral();
            var isRegional = user.IsRegionBuyer();
            var region = user.UserRegion();
            var usersStore = user.UserStoreAbbreviation();

            List<SelectListItem> stores = null;

            using (var dbContext = entityFactory.New())
            {

                if (isCentral || isRegional)
                {
                    // non regional admin from a non-store (regional office)  all stores.

                    stores = (from s in dbContext.STORE
                        join r in dbContext.REGION on s.REGION_ID equals r.ID
                        join st in dbContext.STATUS on s.STATUS_ID equals st.ID
                        where !st.STATUS1.Equals("CLOSED", StringComparison.OrdinalIgnoreCase)
                              && r.REGION_ABBR.Equals(region, StringComparison.OrdinalIgnoreCase)
                        orderby s.STORE_NAME
                        select new SelectListItem()
                        {
                            Text = s.STORE_NAME,
                            Value = SqlFunctions.StringConvert((double) s.ID)
                        }).ToList();

                }
                else
                {
                    // store user. get only their store.
                    stores = (from s in dbContext.STORE
                        join r in dbContext.REGION on s.REGION_ID equals r.ID
                        join st in dbContext.STATUS on s.STATUS_ID equals st.ID
                        where !st.STATUS1.Equals("CLOSED", StringComparison.OrdinalIgnoreCase)
                              && r.REGION_ABBR.Equals(region, StringComparison.OrdinalIgnoreCase)
                              && s.STORE_ABBREVIATION.Equals(usersStore, StringComparison.OrdinalIgnoreCase)
                        orderby s.STORE_NAME
                        select new SelectListItem()
                        {
                            Text = s.STORE_NAME,
                            Value = SqlFunctions.StringConvert((double) s.ID)
                        }).ToList();

                    // if no stores were found because they were from a non-store (regional office) show all stores.
                    if (!stores.Any())
                        stores = ( from s in dbContext.STORE
                                 join r in dbContext.REGION on s.REGION_ID equals r.ID
                                 join st in dbContext.STATUS on s.STATUS_ID equals st.ID
                                 where !st.STATUS1.Equals("CLOSED", StringComparison.OrdinalIgnoreCase)
                                       && r.REGION_ABBR.Equals(region, StringComparison.OrdinalIgnoreCase)
                                 orderby s.STORE_NAME
                                 select new SelectListItem()
                                 {
                                     Text = s.STORE_NAME,
                                     Value = SqlFunctions.StringConvert((double)s.ID)
                                 }).ToList();
                }

                return stores.ToList();
            }
        }


        /// <summary>
        /// Get an alphabetical select list enumeration of stores for the region
        /// </summary>
        /// <param name="regionId"></param>
        /// <returns></returns>
        protected IEnumerable<SelectListItem> GetStores(int regionId)
        {
            //int idStoreClosedStatus = GetStoreClosedStatus(entityFactory);
            IEnumerable<SelectListItem> stores = null;

            using (var dbContext = entityFactory.New())
            {
                stores = (from s in dbContext.STORE
                    join r in dbContext.REGION on s.REGION_ID equals r.ID
                    join st in dbContext.STATUS on s.STATUS_ID equals st.ID
                    where !st.STATUS1.Equals("CLOSED", StringComparison.OrdinalIgnoreCase)
                    && s.REGION_ID == regionId
                    orderby s.STORE_NAME
                    select new SelectListItem()
                    {
                        Text = s.STORE_NAME,
                        Value = SqlFunctions.StringConvert((double) s.ID)
                    }).ToList();

            }
            return stores;

            //IQueryable<STORE> storeQuery = dbContext.STORE
            //.Where(m => m.REGION_ID == regionId && m.STATUS_ID != idStoreClosedStatus)
            //.OrderBy(m => m.STORE_ABBREVIATION);
            //return storeQuery.ToList().OrderBy(x=>x.STORE_NAME).Select(store => new SelectListItem() { Text = string.Format("{0}", store.STORE_NAME), Value = store.ID.ToString(CultureInfo.InvariantCulture) }).ToList();
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
     

