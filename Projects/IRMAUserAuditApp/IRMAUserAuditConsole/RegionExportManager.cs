using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using log4net;
using WholeFoods.Common.IRMALib;
using WFM.Helpers;
using System.Configuration;

namespace IRMAUserAuditConsole
{
    public class RegionExportManager
    {
        #region Members

        private string region = "";
        private Log log;
        private UserRepository repo;
        private ConfigRepository configRepo;
        private bool configOk = false;
        private Guid envId;
        private Guid appId;
        private string fiscalYearString = "";
        private string query = @"SELECT * FROM Users WHERE accountenabled = 1
                                          AND(Accountant = 1
                                          OR CostAdmin = 1
                                          OR DCAdmin = 1
                                          OR Inventory_Administrator = 1
                                          OR Item_Administrator = 1
                                          OR POApprovalAdmin = 1
                                          OR POEditor = 1
                                          OR PO_Accountant = 1
                                          OR POApprovalAdmin = 1
                                          OR TaxAdministrator = 1
                                          OR VendorCostDiscrepancyAdmin = 1
                                          OR Distributor = 1
                                          OR DataAdministrator = 1
                                          OR ApplicationConfigAdmin = 1
                                          OR POSInterfaceAdministrator = 1
                                          OR StoreAdministrator = 1
                                          OR SecurityAdministrator = 1
                                          OR SuperUser = 1
                                          OR SystemConfigurationAdministrator = 1
                                          OR UserMaintenance = 1
                                          OR JobAdministrator = 1
                                          )";
        #endregion

        #region Properties

        public bool ConfigOk { get { return configOk; } }

        #endregion

        #region Constructor

        public RegionExportManager(string _region, string _connectionString, IRMAEnvironmentEnum _environment)
        {
            this.region = _region;
            this.repo = new UserRepository(_connectionString);
            this.configRepo = new ConfigRepository(_connectionString);
            this.fiscalYearString = Common.CreateFiscalYearString();
            SetupLog();

            try
            {
                SetupConfig(_environment);
            }
            catch (Exception ex)
            {
                log.Error("Unable to load config:  " + ex.Message);
            }
        }

        #endregion

        #region Methods


        public int Export(string folderName)
        {
            string basePath = "";
            string masterFilePath = "";
     
            if ((configRepo.ConfigurationGetValue("BasePath", ref basePath)) == false)
            {
                log.Error("Unable to get base path from config!  Aborting.");
                // can't find the base path.
                return -1;
            }
            if ((configRepo.ConfigurationGetValue("MasterFilePath", ref masterFilePath)) == false)
            {
                log.Error("Unable to get master file path from config!  Aborting.");
                // can't find the master file path.
                return -1;
            }

            if (SetupFolders(basePath) == false)
            {
                log.Error("Unable to setup folders!  Aborting.");
                return -2;
            }

            string regionPath = Path.Combine(basePath, region, folderName);
            string masterFileRegionPath = Path.Combine(masterFilePath, region, folderName);

            if (!Directory.Exists(regionPath))
            {
                try
                {
                    Directory.CreateDirectory(regionPath);
                }
                catch (Exception ex)
                {
                    log.Error("Unable to create region path:" + ex.Message);
                    return -3;
                }
            }

            if (!Directory.Exists(masterFileRegionPath))
            {
                try
                {
                    Directory.CreateDirectory(masterFileRegionPath);
                }
                catch (Exception ex)
                {
                    log.Error("Unable to create master File  path:" + ex.Message);
                    return -3;
                }
            }

            string exportBy = "Region";
            if ((configRepo.ConfigurationGetValue("ExportBy", ref exportBy)) == false)
            {
                log.Warning("Unable to get ExportBy value from config!  Defaulting to \"Store\"");
                exportBy = "Store";
            }

            if ((exportBy.Trim().ToLower() != "store") && (exportBy.Trim().ToLower() != "region"))
            {
                log.Warning("Unexpected value for ExportBy: " + exportBy + ".  Defaulting to \"Store\"");
                exportBy = "Store";
            }

            if (exportBy.ToLower() == "store")
            {
                List<Store> storeList = repo.GetStores();
                foreach (Store store in storeList)
                {
                    ExportUsersByStore(regionPath, store);
                }
                // export users which are not store limited (store_id = null)
                Store allStores = new Store();
                allStores.Store_Name = "All Stores";
                allStores.Store_No = -1;
                allStores.BusinessUnit_ID = null;
                ExportUsersByStore(regionPath, allStores);
            }
            else
            {
                ExportUsersByRegion(regionPath, masterFileRegionPath);
            }

            return 0;
        }

        private List<string> GetRolesListForExport(string delimiter = ";")
        {
            var appSettingsData = ConfigurationManager.AppSettings["UserRolesForExport"];
            var rolesList = appSettingsData
                .Split(new string[] { delimiter }, StringSplitOptions.RemoveEmptyEntries)
                .Select(each => each.Trim().ToUpper())
                .Distinct()
                .ToList();
            return rolesList;
        }

        public string createFilename(string fileName)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(fileName.ToUpper());
            //sb.Append(DateTime.Now.ToString("s"));
            sb.Append(".xlsx");
            sb.Replace(":", "_");
            sb.Replace(" ", "_");
            sb.Replace("/", "-");
            sb.Replace("\\", "-");
            return sb.ToString();
        }

        private void writeTotalRecordsFile(int totalRecords,String query, string path)
        {
            System.IO.StreamWriter file = new System.IO.StreamWriter(path);
            string lines = "Total Number Of Records:" + totalRecords + "\r\n" + "query:"+ query;
            file.WriteLine(lines);
            file.Close();
        }
        private void deleteFiles(string folderName)
        {
            System.IO.DirectoryInfo di = new DirectoryInfo(folderName);
            foreach (FileInfo file in di.GetFiles())
            {
                file.Delete();
            }
            foreach (DirectoryInfo dir in di.GetDirectories())
            {
                dir.Delete(true);
            }
        }
        private void ExportUsersByRegion(string regionPath, string masterFileRegionPath)
        {
            string masterFile = "Master";
            log.Message("setting up region spreadsheet for " + region.ToUpper());
           
            var userRolesDictionary = GetRolesListForExport();
            var users= repo.GetUsers(userRolesDictionary);

            string masterFileName = Path.Combine(masterFileRegionPath, createFilename(masterFile));
            string totalRecordsFilePAth = Path.Combine(masterFileRegionPath,"TotalRecords.txt");
            deleteFiles(regionPath);
            deleteFiles(masterFileRegionPath);

            writeTotalRecordsFile(users.Count(), query, totalRecordsFilePAth);

            SpreadsheetManager master = Common.SetupStoreSpreadsheet(masterFileName, repo.GetStoreNames(), repo.GetTitles());
            master.JumpToRow("Users", 3);
            foreach (UserInfo ui in users.OrderBy(u => u.Location))
            {
                log.Message("Adding " + ui.FullName + "...");
                Common.AddUserToSpreadsheet(ref master, ui);
            }
            master.AutosizeColumns("Users");
            log.Message("saving " + masterFileName);
            master.Close(masterFileName);

            var groupByTitle = users.GroupBy(ur => ur.Title).ToList();

            foreach (var group in groupByTitle)
            {
                string fileName = Path.Combine(regionPath, createFilename(group.First().Title));
                SpreadsheetManager ssm = Common.SetupStoreSpreadsheet(fileName, repo.GetStoreNames(), repo.GetTitles());
                // header is row 1, then skip row 2:
                ssm.JumpToRow("Users", 3);

                foreach (var userInfo in group)
                {
                    log.Message("Adding " + userInfo.FullName + "...");
                    Common.AddUserToSpreadsheet(ref ssm, userInfo);
                }
                ssm.AutosizeColumns("Users");
                log.Message("saving " + fileName);
                ssm.Close(fileName);
            }
        }

        private void ExportUsersByStore(string regionPath, Store store)
        {
            // setup spreadsheet
            log.Message("setting up " + store.Store_Name);
            string fileName = Path.Combine(regionPath, Common.CreateStoreFilename(store));
            fiscalYearString = Common.CreateFiscalYearString();

            // Common.CreateFolder(fiscalYearString + "\\" + region.ToUpper());
            SpreadsheetManager ssm = Common.SetupStoreSpreadsheet(fileName, repo.GetStoreNames(), repo.GetTitles());

            // jump to row 3 (header is row 1, then 2 is blank)
            ssm.JumpToRow("Users", 3);
            var userRolesDictionary = GetRolesListForExport();
            List<UserInfo> storeUsers = repo.GetUsersByStore(store.Store_No, userRolesDictionary);

            foreach (UserInfo ui in storeUsers)
            {
                log.Message("Adding " + ui.FullName + "...");
                Common.AddUserToSpreadsheet(ref ssm, ui);
            }

            ssm.AutosizeColumns("Users");
            log.Message("saving " + fileName);
            ssm.Close(fileName);

        }

        private void SetupConfig(IRMAEnvironmentEnum _env)
        {
            var environments = configRepo.GetEnvironmentList();
            envId = environments.SingleOrDefault(env => env.Name.ToLower().Replace(" ", "") == _env.ToString().ToLower()).EnvironmentID;
            if (envId != null)
            {
                // we have found an Env.  Now look for the app:
                appId = configRepo.GetApplicationList(envId).SingleOrDefault(app => app.Name.ToLower() == "user audit").ApplicationID;
                if (appId != null)
                {
                    // we have the app!  
                    configOk = configRepo.LoadConfig(appId, envId);
                    if (!configOk)
                        throw new Exception("Unable to load config!");
                }
                else
                {
                    log.Error("Unable to find application in environment!");
                    throw new Exception("could not find config application for environment!");
                }
            }
            else
            {
                log.Error("Unable to find Environment!");
                throw new Exception("Could not find config environment!");
            }
        }
        private bool SetupFolders(string basePath)
        {
            if (!Directory.Exists(basePath))
            {
                try
                {
                    Directory.CreateDirectory(basePath);
                }
                catch (Exception ex)
                {
                    log.Error("Unable to create base path " + basePath + ": " + ex.Message);
                    return false;
                }
            }

            if (!Directory.Exists(Path.Combine(basePath, region)))
            {
                try
                {
                    Directory.CreateDirectory(Path.Combine(basePath, region));
                }
                catch (Exception ex)
                {
                    log.Error("Unable to create region folder " + Path.Combine(basePath, region) + ": " + ex.Message);
                    return false;
                }
            }
            log.Message("Folders ready.");
            return true;
        }

        private void SetupLog()
        {
            string logDir = Path.Combine(Directory.GetCurrentDirectory(), "logs");
            if (!Directory.Exists(logDir))
                Directory.CreateDirectory(logDir);

            string logDate = DateTime.Now.ToString("yyyy_MM_dd");
            log = new Log(Path.Combine(logDir, region.ToUpper() + "_" + logDate + "_Export.log"), true);
        }

        private string CreateRegionPath()
        {
            return Path.Combine("");
        }

        #endregion
    }
}
