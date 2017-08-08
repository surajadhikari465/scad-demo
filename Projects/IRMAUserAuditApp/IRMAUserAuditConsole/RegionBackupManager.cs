using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

using WFM.Helpers;

using WholeFoods.Common.IRMALib;

namespace IRMAUserAuditConsole
{
    class RegionBackupManager
    {

        #region Members

        private List<string> files = null;
        private string region = "";
        private Log log = null;
        private UserRepository repo;
        private ConfigRepository configRepo;
        private bool configOk = false;
        private Guid envId;
        private Guid appId;
        private string basePath = "";
        private string restorePath = "";
        private string backupPath = "Backup";
        private string previousBackupPath = "Backup_PREVIOUS";

        #endregion

        #region ctors

        public RegionBackupManager(string _region, string _connectionString, IRMAEnvironmentEnum _env)
        {
            this.region = _region;
            this.repo = new UserRepository(_connectionString);
            this.configRepo = new ConfigRepository(_connectionString);
            region = _region;
             try
            {
                SetupConfig(_env);
            }
             catch (Exception ex)
             {
                 log.Error("Unable to load config:  " + ex.Message);
             }
            basePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            bool isrestorePath = configRepo.ConfigurationGetValue("RestorePath", ref restorePath);

            backupPath = Path.Combine(restorePath + "\\" + region.ToUpper(), "Backup");
            previousBackupPath = Path.Combine(restorePath + "\\"  + region.ToUpper(), "Backup_PREVIOUS");
            //backupPath = Path.Combine(basePath, "Backup");
            //previousBackupPath = Path.Combine(basePath, "Backup_PREVIOUS");
            SetupLog();
        }

        #endregion

        #region Methods

        public int BackupUsers()
        {
            int result = 0;

            if (!SetupBackupFolders())
            {
                log.Error("Unable to setup backup folders!  Exiting!");
                return -1;
            }

            List<Store> storeList = repo.GetStores();
            foreach (Store store in storeList)
            {
                BackupUsersByStore(store);
            }
            // export users which are not store limited (store_id = null)
            Store allStores = new Store();
            allStores.Store_Name = "All Stores";
            allStores.Store_No = -1;
            allStores.BusinessUnit_ID = null;
            BackupUsersByStore(allStores);

            return result;
        }

        private bool SetupBackupFolders()
        {
            try
            {

                if (!Directory.Exists(backupPath))
                {
                    // create it.
                    Directory.CreateDirectory(backupPath);
                    // if for some ODD reason the previous backup directory exists, even tho
                    // the regular backup path did not...go ahead and delete it
                    if (Directory.Exists(previousBackupPath))
                        Directory.Delete(previousBackupPath, true);
                }
                else
                {
                    // already got it, so rename it
                    if (Directory.Exists(previousBackupPath))
                    {
                        Directory.Delete(previousBackupPath, true);
                    }
                    Directory.Move(backupPath, previousBackupPath);
                    Directory.CreateDirectory(backupPath);
                }
            }
            catch (Exception ex)
            {
                log.Error("error setting up backup folders: " + ex.Message);
                return false;
            }

            return true;
        }

        private void BackupUsersByStore(Store store)
        {
            // setup spreadsheet
            log.Message("backing up " + store.Store_Name);
            string fileName = Path.Combine(basePath, backupPath, Common.CreateStoreFilename(store));
            int year = DateTime.Now.Year;
            SpreadsheetManager ssm = SetupBackupSpreadsheet(fileName);

            // jump to row 3 (header is row 1, then 2 is blank)
            ssm.JumpToRow("Users", 3);
            ssm.JumpToRow("SLIM", 3);

            List<User> storeUsers = repo.GetUsersTableByStore(true, store.Store_No);

            //Console.WriteLine(store.Store_Name + ": " + storeUsers.Count);

            foreach (User u in storeUsers)
            {
                SlimAccess sa = repo.GetUserSlimAccess(u.User_ID);
                if (sa == null)
                {
                    sa = new SlimAccess();
                    sa.Authorizations = false;
                    sa.IRMAPush = false;
                    sa.ItemRequest = false;
                    sa.RetailCost = false;
                    sa.ScaleInfo = false;
                    sa.StoreSpecials = false;
                    sa.User_ID = u.User_ID;
                    sa.UserAdmin = false;
                    sa.VendorRequest = false;
                    sa.WebQuery = false;
                }
                else
                {
                    sa.Authorizations = sa.Authorizations.HasValue ? sa.Authorizations : false;
                    sa.IRMAPush = sa.IRMAPush.HasValue ? sa.IRMAPush : false;
                    sa.ItemRequest = sa.ItemRequest.HasValue ? sa.ItemRequest : false;
                    sa.RetailCost = sa.RetailCost.HasValue ? sa.RetailCost : false;
                    sa.StoreSpecials = sa.StoreSpecials.HasValue ? sa.StoreSpecials : false;
                    sa.UserAdmin = sa.UserAdmin.HasValue ? sa.UserAdmin : false;
                    sa.VendorRequest = sa.VendorRequest.HasValue ? sa.VendorRequest : false;
                    sa.WebQuery = sa.WebQuery.HasValue ? sa.WebQuery : false;
                }
                log.Message("Adding " + u.FullName + "...");
                AddUserToBackupSpreadsheet(ref ssm, u, sa);
            }

            ssm.AutosizeColumns("Users");
            ssm.AutosizeColumns("SLIM");
            log.Message("saving " + fileName);
            ssm.Close(fileName);
        }

        private void AddUserToBackupSpreadsheet(ref SpreadsheetManager ssm, User u, SlimAccess sa)
        {
            List<object> items = new List<object>();
            items.Add(u.User_ID);
            items.Add(u.AccountEnabled);
            items.Add(u.CoverPage);
            items.Add(u.EMail);
            items.Add(u.Fax_Number);
            items.Add(u.FullName);
            items.Add(u.Pager_Email);
            items.Add(u.Phone_Number);
            items.Add(u.Printer);
            items.Add(u.RecvLog_Store_Limit);
            items.Add(u.Telxon_Store_Limit);
            items.Add(u.Title);
            items.Add(u.UserName);
            items.Add(u.Accountant);
            items.Add(u.ApplicationConfigAdmin);
            items.Add(u.BatchBuildOnly);
            items.Add(u.Buyer);
            items.Add(u.Coordinator);
            items.Add(u.CostAdmin);
            items.Add(u.FacilityCreditProcessor);
            items.Add(u.DataAdministrator);
            items.Add(u.DCAdmin);
            items.Add(u.Distributor);
            items.Add(u.DeletePO);
            items.Add(u.EInvoicing_Administrator);
            items.Add(u.Inventory_Administrator);
            items.Add(u.Item_Administrator);
            items.Add(u.JobAdministrator);
            items.Add(u.Lock_Administrator);
            items.Add(u.SuperUser);
            items.Add(u.PO_Accountant);
            items.Add(u.POApprovalAdmin);
            items.Add(u.POEditor);
            items.Add(u.POSInterfaceAdministrator);
            items.Add(u.PriceBatchProcessor);
            items.Add(u.PromoAccessLevel);
            items.Add(u.SecurityAdministrator);
            items.Add(u.Shrink);
            items.Add(u.ShrinkAdmin);
            items.Add(u.StoreAdministrator);
            items.Add(u.SystemConfigurationAdministrator);
            items.Add(u.TaxAdministrator);
            items.Add(u.UserMaintenance);
            items.Add(u.Vendor_Administrator);
            items.Add(u.VendorCostDiscrepancyAdmin);
            items.Add(u.Warehouse);
            ssm.AddRow("Users", items.ToArray());

            // SLIM
            items = new List<object>();
            items.Add(u.User_ID);
            items.Add(sa.Authorizations);
            items.Add(sa.IRMAPush);
            items.Add(sa.ItemRequest);
            items.Add(sa.RetailCost);
            items.Add(sa.ScaleInfo);
            items.Add(sa.StoreSpecials);
            items.Add(sa.UserAdmin);
            items.Add(sa.VendorRequest);
            items.Add(sa.WebQuery);
            ssm.AddRow("SLIM", items.ToArray());

        }

        private SpreadsheetManager SetupBackupSpreadsheet(string fileName)
        {
            SpreadsheetManager ssm = new SpreadsheetManager(fileName);
            ssm.CreateWorksheet("Users");
            ssm.CreateHeader("Users", (new string[] { "User_ID", "AccountEnabled", "CoverPage", "EMail", "Fax_Number", "FullName", "Pager_Email", "Phone_Number", "Printer", 
                "RecvLog_Store_Limit", "Telxon_Store_Limit", "Title", "UserName", "Accountant", "ApplicationConfigAdmin", "BatchBuildOnly", "Buyer", "Coordinator", "CostAdmin", 
                "FacilityCreditProcessor", "DataAdministrator", "DCAdmin", "Distributor", "DeletePO", "EInvoicingAdmin", "Inventory_Administrator", "Item_Administrator", "JobAdministrator",
                "Lock_Administrator", "SuperUser", "PO_Accountant", "POApprovalAdmin", "POEditor", "POSInterfaceAdministrator", "PriceBatchProcessor", "PromoAccessLevel", 
                "SecurityAdministrator", "Shrink", "ShrinkAdmin", "StoreAdministrator", "SystemConfigurationAdministrator", "TaxAdministrator", "UserMaintenance", "Vendor_Administrator",
                "VendorCostDiscrepancyAdmin", "Warehouse"}).ToList());

            // SLIM sheet
            ssm.CreateWorksheet("SLIM");
            ssm.CreateHeader("SLIM", (new string[] { "User_ID", "Authorizations", "IRMAPush", "ItemRequest", "RetailCost", "ScaleInfo", "StoreSpecials", "UserAdmin", "VendorRequest", "WebQuery" }).ToList());


            return ssm;
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

        private void SetupLog()
        {
            string logDir = Path.Combine(Directory.GetCurrentDirectory(), "logs");
            if (!Directory.Exists(logDir))
                Directory.CreateDirectory(logDir);
            string logDate = DateTime.Now.ToString("yyyy_MM_dd");
            log = new Log(Path.Combine(logDir, region.ToUpper() + "_" + logDate + "_Backup.log"), true);
        }

        #endregion
    }
}
