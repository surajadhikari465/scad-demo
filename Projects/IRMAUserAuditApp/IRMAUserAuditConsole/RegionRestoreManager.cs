using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using WFM.Helpers;
using WholeFoods.Common.IRMALib;

namespace IRMAUserAuditConsole
{
    class RegionRestoreManager
    {
        #region Members

        private List<string> files = null;
        private string region = "";
        private Log log = null;
        private string restorePath = "";
        private string basePath = "";
        private SpreadsheetManager inputSheet = null;
        private UserRepository repo = null;
        private ConfigRepository configRepo;
        private Guid appId;
        private Guid envId;
        private bool configOk = false;

        #endregion

        public RegionRestoreManager(string _region, string _connectionString, IRMAEnvironmentEnum _env)
        {
            this.region = _region;
            this.repo = new UserRepository(_connectionString);
            this.configRepo = new ConfigRepository(_connectionString);
            region = _region;
            //restoreRoot = Properties.Settings.Default.RestorePath;
            SetupLog();
            try
            {
                SetupConfig(_env);
            }
            catch (Exception ex)
            {
                log.Error("Unable to load config:  " + ex.Message);
            }
        }

        public int RestoreUsers()
        {
            if ((configRepo.ConfigurationGetValue("BasePath", ref basePath)) == false)
            {
                log.Error("Unable to get base path from config!");
                return -1;
            }

            if ((configRepo.ConfigurationGetValue("RestorePath", ref restorePath)) == false)
            {
                log.Error("Unable to get restore path from config!");
                return -2;
            }
            restorePath += "\\" + region.ToUpper() + "\\backup";
            log.Message("Building file list...");
            BuildFileList();
            log.Message(files.Count.ToString() + " files found.");
            if (files.Count < 1)
            {
                log.Error("No files found to restore!");
                return -3;
            }

            int result = 0;
            foreach (string file in files)
            {
                
                if (file.ToLower().EndsWith(".xls"))
                {
                    log.Error("XLS (pre-Excel 2007) files are not supported.  Please save " + file + " as a different format and retry.");
                }
                else if ((!file.ToLower().EndsWith(".xlsx")) && (!file.ToLower().EndsWith(".xlsm")))
                {
                    log.Warning("File not recognized: " + Path.GetFileName(file) + ". Skipping.");
                }
                else
                {
                    log.Message("opening " + file);
                    OpenFile(file);
                    List<object[]> userRows = ParseWorksheet("Users", "A", "AT");
                    List<object[]> slimRows;
                    if (WorksheetExists("SLIM"))
                        slimRows = ParseWorksheet("SLIM", "A", "J");
                    else
                        slimRows = new List<object[]>();

                    foreach (object[] userRow in userRows)
                    {
                        if (userRow.Length > 45)
                        {
                            int userId = Int32.Parse(userRow[0].ToString());

                            User u = new User();

                            // use these for TryParse below
                            int tmp = -1;
                            bool btmp = false;
                            short stmp = -1;

                            u.User_ID = userId;
                            u.AccountEnabled = bool.TryParse(userRow[1].ToString(), out btmp) ? btmp : false;
                            u.CoverPage = userRow[2] != null ? userRow[2].ToString() : "";
                            u.EMail = userRow[3] != null ? userRow[3].ToString() : "";
                            u.Fax_Number = userRow[4] != null ? userRow[4].ToString() : "";
                            u.FullName = userRow[5] != null ? userRow[5].ToString() : "";
                            u.Pager_Email = userRow[6] != null ? userRow[6].ToString() : "";
                            u.Phone_Number = userRow[7] != null ? userRow[7].ToString() : "";
                            u.Printer = userRow[8] != null ? userRow[8].ToString() : "";
                            u.RecvLog_Store_Limit = userRow[9] != null ? (int.TryParse(userRow[9].ToString(), out tmp) ? (int?)tmp : null) : null;
                            u.Telxon_Store_Limit = userRow[10] != null ? (int.TryParse(userRow[10].ToString(), out tmp) ? (int?)tmp : null) : null;
                            u.Title = userRow[11] != null ? (int.TryParse(userRow[11].ToString(), out tmp) ? (int?)tmp : null) : null;
                            u.UserName = userRow[12] != null ? userRow[12].ToString() : "";
                            u.Accountant = userRow[13] != null ? (bool.TryParse(userRow[13].ToString(), out btmp) ? btmp : false) : false;
                            u.ApplicationConfigAdmin = userRow[14] != null ? (bool.TryParse(userRow[14].ToString(), out btmp) ? btmp : false) : false;
                            u.BatchBuildOnly = userRow[15] != null ? (bool.TryParse(userRow[15].ToString(), out btmp) ? btmp : false) : false;
                            u.Buyer = userRow[16] != null ? (bool.TryParse(userRow[16].ToString(), out btmp) ? btmp : false) : false;
                            u.Coordinator = userRow[17] != null ? (bool.TryParse(userRow[17].ToString(), out btmp) ? btmp : false) : false;
                            u.CostAdmin = userRow[18] != null ? (bool.TryParse(userRow[18].ToString(), out btmp) ? btmp : false) : false;
                            u.FacilityCreditProcessor = userRow[19] != null ? (bool.TryParse(userRow[19].ToString(), out btmp) ? btmp : false) : false;
                            u.DataAdministrator = userRow[20] != null ? (bool.TryParse(userRow[20].ToString(), out btmp) ? btmp : false) : false;
                            u.DCAdmin = userRow[21] != null ? (bool.TryParse(userRow[21].ToString(), out btmp) ? btmp : false) : false;
                            u.Distributor = userRow[22] != null ? (bool.TryParse(userRow[22].ToString(), out btmp) ? btmp : false) : false;
                            u.DeletePO = userRow[23] != null ? (bool.TryParse(userRow[23].ToString(), out btmp) ? btmp : false) : false;
                            u.EInvoicing_Administrator = userRow[24] != null ? (bool.TryParse(userRow[24].ToString(), out btmp) ? btmp : false) : false;
                            u.Inventory_Administrator = userRow[25] != null ? (bool.TryParse(userRow[25].ToString(), out btmp) ? btmp : false) : false;
                            u.Item_Administrator = userRow[26] != null ? (bool.TryParse(userRow[26].ToString(), out btmp) ? btmp : false) : false;
                            u.JobAdministrator = userRow[27] != null ? (bool.TryParse(userRow[27].ToString(), out btmp) ? btmp : false) : false;
                            u.Lock_Administrator = userRow[28] != null ? (bool.TryParse(userRow[28].ToString(), out btmp) ? btmp : false) : false;
                            u.SuperUser = userRow[29] != null ? (bool.TryParse(userRow[29].ToString(), out btmp) ? btmp : false) : false;
                            u.PO_Accountant = userRow[30] != null ? (bool.TryParse(userRow[30].ToString(), out btmp) ? btmp : false) : false;
                            u.POApprovalAdmin = userRow[31] != null ? (bool.TryParse(userRow[31].ToString(), out btmp) ? btmp : false) : false;
                            u.POEditor = userRow[32] != null ? (bool.TryParse(userRow[32].ToString(), out btmp) ? btmp : false) : false;
                            u.POSInterfaceAdministrator = userRow[33] != null ? (bool.TryParse(userRow[33].ToString(), out btmp) ? btmp : false) : false;
                            u.PriceBatchProcessor = userRow[34] != null ? (bool.TryParse(userRow[34].ToString(), out btmp) ? btmp : false) : false;
                            u.PromoAccessLevel = userRow[35] != null ? (short.TryParse(userRow[35].ToString(), out stmp) ? (short?)stmp : null) : null;
                            u.SecurityAdministrator = userRow[36] != null ? (bool.TryParse(userRow[36].ToString(), out btmp) ? btmp : false) : false;
                            u.Shrink = userRow[37] != null ? (bool.TryParse(userRow[37].ToString(), out btmp) ? btmp : false) : false;
                            u.ShrinkAdmin = userRow[38] != null ? (bool.TryParse(userRow[38].ToString(), out btmp) ? btmp : false) : false;
                            u.StoreAdministrator = userRow[39] != null ? (bool.TryParse(userRow[39].ToString(), out btmp) ? btmp : false) : false;
                            u.SystemConfigurationAdministrator = userRow[40] != null ? (bool.TryParse(userRow[40].ToString(), out btmp) ? btmp : false) : false;
                            u.TaxAdministrator = userRow[41] != null ? (bool.TryParse(userRow[41].ToString(), out btmp) ? btmp : false) : false;
                            u.UserMaintenance = userRow[42] != null ? (bool.TryParse(userRow[42].ToString(), out btmp) ? btmp : false) : false;
                            u.Vendor_Administrator = userRow[43] != null ? (bool.TryParse(userRow[43].ToString(), out btmp) ? btmp : false) : false;
                            u.VendorCostDiscrepancyAdmin = userRow[44] != null ? (bool.TryParse(userRow[44].ToString(), out btmp) ? btmp : false) : false;
                            u.Warehouse = userRow[45] != null ? (bool.TryParse(userRow[45].ToString(), out btmp) ? btmp : false) : false;


                            // find the user on the SLIM tab
                            object[] slimInfo = (from row in slimRows
                                                 where Int32.Parse(row[0].ToString()) == userId
                                                 select row).SingleOrDefault();
                            SlimAccess sa = new SlimAccess();
                            sa.User_ID = userId;
                            if (slimInfo != null)
                            {
                                sa.Authorizations = bool.TryParse(slimInfo[1].ToString(), out btmp) ? btmp : false;
                                sa.IRMAPush = bool.TryParse(slimInfo[2].ToString(), out btmp) ? btmp : false;
                                sa.ItemRequest = bool.TryParse(slimInfo[3].ToString(), out btmp) ? btmp : false;
                                sa.RetailCost = bool.TryParse(slimInfo[4].ToString(), out btmp) ? btmp : false;
                                sa.ScaleInfo = bool.TryParse(slimInfo[5].ToString(), out btmp) ? btmp : false;
                                sa.StoreSpecials = bool.TryParse(slimInfo[6].ToString(), out btmp) ? btmp : false;
                                sa.UserAdmin = bool.TryParse(slimInfo[7].ToString(), out btmp) ? btmp : false;
                                sa.VendorRequest = bool.TryParse(slimInfo[8].ToString(), out btmp) ? btmp : false; ;
                                sa.WebQuery = bool.TryParse(slimInfo[9].ToString(), out btmp) ? btmp : false;
                            }
                            else
                            {
                                sa.Authorizations = false;
                                sa.IRMAPush = false;
                                sa.ItemRequest = false;
                                sa.RetailCost = false;
                                sa.ScaleInfo = false;
                                sa.StoreSpecials = false;
                                sa.UserAdmin = false;
                                sa.VendorRequest = false;
                                sa.WebQuery = false;
                            }

                            UserRestoreError ure = repo.RestoreUser(u, sa);
                            if (ure != UserRestoreError.None)
                            {
                                log.Error("user restore returned nonzero!  Possible error for user: " + u.FullName);
                                result++;
                            }
                            else
                            {
                                log.Message("restored " + u.FullName);
                            }
                        }
                     
                    }
                }
            }
            return result;
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
            log = new Log(Path.Combine(logDir, region.ToUpper() + "_" + logDate + "_Restore.log"), true);
        }

        public void OpenFile(string filePath)
        {
            inputSheet = null;
            inputSheet = new SpreadsheetManager(filePath);
        }

        private void BuildFileList()
        {
            files = new List<string>();
            //string inputFilePath = Path.Combine(restoreRoot, region);
            DirectoryInfo di = new DirectoryInfo(Path.Combine(basePath, restorePath));
            if (!di.Exists)
            {
                log.Error("input file path not found: " + restorePath);
            }
            else
            {
                foreach (FileInfo fi in di.GetFiles())
                {
                    files.Add(Path.Combine(basePath, restorePath, fi.Name));
                }
            }
        }

        public bool WorksheetExists(string worksheet)
        {
            return inputSheet.WorksheetExists(worksheet);
        }

        public List<object[]> ParseWorksheet(string worksheet, string startColumn, string lastColumn)
        {
            int lastRow = inputSheet.GetLastRow(worksheet);
            int row;
            List<object[]> rows = new List<object[]>();
            for (row = 3; row <= lastRow; row++)
            {
                rows.Add(inputSheet.GetRowData(worksheet, lastColumn, row));
            }
            return rows;
        }

        public List<string> Files
        {
            get { return files; }
        }

        
    }
}
