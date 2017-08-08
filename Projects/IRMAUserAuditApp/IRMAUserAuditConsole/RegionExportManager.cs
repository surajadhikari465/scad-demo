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

        private List<string> files = null;
        private string region = "";
        private string backupPath;
        private Log log;
        private Repository repo;
        private ConfigRepository configRepo;
        private bool configOk = false;
        private Guid envId;
        private Guid appId;

        private string fiscalYearString = "";

        #endregion

        #region Properties

        public bool ConfigOk { get { return configOk; } }

        #endregion

        #region Constructor

        public RegionExportManager(string _region, string _connectionString, IRMAEnvironment _environment)
        {
            this.region = _region;
            this.repo = new Repository(_connectionString);
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


        public int Export()
        {
            string basePath = "";
            if ((configRepo.ConfigurationGetValue("BasePath", ref basePath)) == false)
            {
                log.Error("Unable to get base path from config!  Aborting.");
                // can't find the base path.
                return -1;
            }

            if (SetupFolders(basePath) == false)
            {
                log.Error("Unable to setup folders!  Aborting.");
                return -2;
            }

            string regionPath = Path.Combine(basePath, region, fiscalYearString);
            if (!Directory.Exists(regionPath))
            {
                try
                {
                    Directory.CreateDirectory(regionPath);
                }
                catch (Exception ex)
                {
                    log.Error("Unable to create fiscal year path:" + ex.Message);
                    return -3;
                }
            }

            string exportBy = "Store";
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
                ExportUsersByRegion(regionPath);
            }

            return 0;
        }
        private Dictionary<string, Boolean> getRolesListForExport()
        {
            string userRoles = ConfigurationManager.AppSettings["UserRolesForExport"];
            List<string> userRolesList = userRoles.Split(new char[] { ';' }).ToList();
            Dictionary<string, Boolean> userRolesDictionary = new Dictionary<string, Boolean>();
            foreach (string userRole in userRolesList)
            {
                if (!userRolesDictionary.ContainsKey(userRole))
                {
                    userRolesDictionary.Add(userRole, true);
                }
            }
            return userRolesDictionary;
        }

        private void ExportUsersByRegion(string regionPath)
        {
            log.Message("setting up region spreadsheet for " + region.ToUpper());
            string fileName = Path.Combine(regionPath, Common.CreateRegionFilename(region));
            int year = DateTime.Now.Year;

            SpreadsheetManager ssm = Common.SetupStoreSpreadsheet(fileName, repo.GetStoreNames(), repo.GetTitles());
            // header is row 1, then skip row 2:
            ssm.JumpToRow("Users", 3);
            ssm.JumpToRow("SLIM", 3);

            Dictionary<string, Boolean> userRolesDictionary = getRolesListForExport();
            List<UserInfo> users = repo.GetUsers(userRolesDictionary);
            foreach (UserInfo ui in users.OrderBy(u => u.StoreLimit))
            {
                log.Message("Adding " + ui.FullName + "...");
                Common.AddUserToSpreadsheet(ref ssm, ui);
            }

            ssm.AutosizeColumns("Users");
            ssm.AutosizeColumns("SLIM");
            log.Message("saving " + fileName);
            ssm.Close(fileName);

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
            ssm.JumpToRow("SLIM", 3);
            Dictionary<string, Boolean> userRolesDictionary = getRolesListForExport();
            List<UserInfo> storeUsers = repo.GetUsersByStore(store.Store_No, userRolesDictionary);

            //Console.WriteLine(store.Store_Name + ": " + storeUsers.Count);

            foreach (UserInfo ui in storeUsers)
            {
                log.Message("Adding " + ui.FullName + "...");
                Common.AddUserToSpreadsheet(ref ssm, ui);
            }

            ssm.AutosizeColumns("Users");
            ssm.AutosizeColumns("SLIM");
            log.Message("saving " + fileName);
            ssm.Close(fileName);

        }

        private void SetupConfig(IRMAEnvironment _env)
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
