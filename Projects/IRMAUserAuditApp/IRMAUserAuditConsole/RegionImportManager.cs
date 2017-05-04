using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using WholeFoods.Common.IRMALib;

using WFM.Helpers;

namespace IRMAUserAuditConsole
{
    enum ImportResultType : int { Unchanged = 1, Updated, Deleted, Errored };

    class RegionImportManager
    {

        #region Members

        private SpreadsheetManager inputSheet = null;
        private SpreadsheetManager resultSheet = null;
        private Log log = null;
        private string resultsPath = "";
        private int updateCount = 0;
        private int deleteCount = 0;
        private int unchangedCount = 0;
        private int erroredCount = 0;
        private List<string> files = null;
        private string region = "";
        private string resultsFileName = "";
        private string rootPath = "";
        private Repository repo;
        private ConfigRepository configRepo;
        private IRMAEnvironment environment;
        private Guid appId;
        private Guid envId;
        private string fiscalYearString = "";
        private bool configOk = false;

        #endregion

        #region ctors
        public RegionImportManager(string _region, string _connString, IRMAEnvironment _env)
        {
            this.region = _region;
            this.repo = new Repository(_connString);
            this.configRepo = new ConfigRepository(_connString);
            region = _region;
            environment = _env;
            SetupLog();
            SetupConfig(_env);
        }
        /*
        public RegionImportManager(string _region, int quarter)
        {
#if DEBUG
            rootPath = Properties.Settings.Default.LocalRootPath;
#else
            rootPath = Properties.Settings.Default.RootPath;
#endif
            region = _region;

            resultsPath = Path.Combine(rootPath, Properties.Settings.Default.TempFilePath);

            SetupLog();
            // TODO:  this needs to not select the current quarter, but the previous one
            // since we are importing.  For now, accept this UGLY hack.  :)
            SetupResultsSheet(quarter);
            BuildFileList(quarter);
        
        }
        */

        #endregion

        #region Methods

        public int Import()
        {
            string basePath = "";
            if ((configRepo.ConfigurationGetValue("BasePath", ref basePath)) == false)
            {
                log.Error("Unable to get base path from config!  Aborting.");
                // can't find the base path.
                return -1;
            }

            fiscalYearString = Common.CreateFiscalYearString();

            string regionPath = Path.Combine(basePath, region, fiscalYearString);
            if (!Directory.Exists(regionPath))
            {
                log.Error("Cannot find import path: " + regionPath + "! Please verify and try again.");
                return -2;
            }

            SetupResultsSheet(regionPath);
            BuildFileList(regionPath);

            if (files.Count < 1)
            {
                log.Error("No files found to import in " + regionPath + "!  Exiting...");
                return -3;
            }

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
                    List<object[]> userRows = ParseWorksheet("Users", "A", "I");
                    List<object[]> slimRows;
                    if (WorksheetExists("SLIM"))
                        slimRows = ParseWorksheet("SLIM", "A", "I");
                    else
                        slimRows = new List<object[]>();

                    foreach (object[] userRow in userRows)
                    {
                        try
                        {
                            if (userRow.Length > 8)
                            {
                                int number;
                                if (Int32.TryParse(userRow[0].ToString(), out number))
                                {
                                    bool deleteUser = ((string)userRow[8]).ToLower() == "yes" ? true : false;
                                    bool updateUser = ((string)userRow[7]).ToLower() == "yes" ? true : false;
                                    int userId = Int32.Parse(userRow[0].ToString());
                                    UserInfo ui = repo.GetUserInfo(userId);
                                    if (ui == null)
                                    {
                                        log.Warning("User not found: " + userRow[0].ToString());
                                        Console.WriteLine("User not found: " + userRow[0].ToString());
                                        List<object> row = new List<object>(userRow);
                                        row.Add("User Not Found. (May already be Inactive)");
                                        AddResultRow(ImportResultType.Errored, row.ToArray());
                                        continue;
                                    }

                                    //check for slimUpdate
                                    bool updateSlim = false;
                                    // find the user on the SLIM tab
                                    object[] slimInfo = (from row in slimRows
                                                         //    where Int32.Parse(row[0].ToString()) == userId
                                                         where row.Length > 0 && row[0].ToString() == userId.ToString()
                                                         select row).SingleOrDefault();
                                    // get the SLIM fields.
                                    if (slimInfo != null && slimInfo.Count() > 8)
                                    {
                                        updateSlim = ((string)slimInfo[8]).ToLower() == "yes" ? true : false;
                                        ui.HasSlimAccess = true;
                                        if (updateSlim)
                                        {
                                            ui.WebQueryEnabled = slimInfo[3] as string;
                                            ui.ItemRequestEnabled = slimInfo[4] as string;
                                            ui.ISSEnabled = slimInfo[5] as string;
                                        }
                                    }

                                    // get previous store from history.
                                    Store currentStore = repo.GetUsersPreviousStore(userRow[0].ToString());

                                    bool storeMismatch = false;
                                    if ((currentStore != null) && (currentStore.Store_No != ui.StoreId))
                                        storeMismatch = true;

                                    if ((currentStore == null) && (ui.StoreId != null))
                                        storeMismatch = true;

                                    if (storeMismatch)
                                    {
                                        log.Error("User " + ui.FullName + " does not belong to current store.  NO UPDATE performed");
                                        Console.WriteLine("User " + ui.FullName + " does not belong to current store.  NO UPDATE performed");
                                        List<object> row = new List<object>(userRow);
                                        row.Add("User not in this store.  Not updated.");
                                        AddResultRow(ImportResultType.Errored, row.ToArray());
                                        continue;
                                    }

                                    if (deleteUser)
                                    {
                                        try
                                        {
                                            repo.DeleteUser(userId, ref log);
                                        }
                                        catch (Exception ex)
                                        {
                                            log.Error("Delete user " + ui.FullName + ": " + ex.Message);
                                        }
                                        AddResultRow(ImportResultType.Deleted, userRow);
                                        log.Message("user " + ui.FullName + " deleted.");
                                    }
                                    else if (updateUser)
                                    {
                                        //update fields
                                        ui.StoreLimit = userRow[4] as string;
                                        ui.Title = userRow[3] as string;
                                                        
                                        // get the SLIM fields.
                                        if (slimInfo != null && slimInfo.Count() > 8)
                                        {
                                            updateSlim = ((string)slimInfo[8]).ToLower() == "yes" ? true : false;
                                            ui.HasSlimAccess = true;
                                            if (updateSlim)
                                            {
                                                ui.WebQueryEnabled = slimInfo[3] as string;
                                                ui.ItemRequestEnabled = slimInfo[4] as string;
                                                ui.ISSEnabled = slimInfo[5] as string;
                                            }
                                        }

                                        // ready to go!
                                        UserUpdateError uue = repo.UpdateUser(ui, updateSlim, ref log);
                                        if (uue != UserUpdateError.None)
                                        {
                                            // add error message to Errors tab
                                            List<object> row = new List<object>(userRow);

                                            if (uue == UserUpdateError.TitleNotFound)
                                            {
                                                log.Error("User Title not found!  Must EXACTLY match Title_Desc in Title table!");
                                                row.Add("User Title not found!  Must EXACTLY match Title_Desc in Title table!");
                                                Console.WriteLine("!! ERROR !!: User Title not found!  Must EXACTLY match Title_Desc in Title table!");
                                            }
                                            else if (uue == UserUpdateError.StoreNotFound)
                                            {
                                                log.Error("Store not found!  Must EXACTLY match Store_Name in Stores table!");
                                                row.Add("Store not found!  Must EXACTLY match Store_Name in Stores table!");
                                                Console.WriteLine("!! ERROR !!: Store not found!  Must EXACTLY match Store_Name in Stores table!");
                                            }
                                            else if (uue == UserUpdateError.Other)
                                            {
                                                log.Error("Unknown error updating user!  User not updated!");
                                                row.Add("Unknown error updating user!  User NOT updated!");
                                                Console.WriteLine("!! ERROR !!: Unknown error updating user!  User not updated!");
                                            }

                                            AddResultRow(ImportResultType.Errored, row.ToArray());
                                        }

                                        AddResultRow(ImportResultType.Updated, userRow);
                                        log.Message("user " + ui.FullName + " updated.");

                                        // so we don't update twice.
                                        updateSlim = false;
                                    }
                                    else if (updateSlim)
                                    {
                                        // get the SLIM fields.
                                        if (slimInfo != null && slimInfo.Count() > 8)
                                        {
                                            updateSlim = ((string)slimInfo[8]).ToLower() == "yes" ? true : false;
                                            ui.HasSlimAccess = true;
                                            if (updateSlim)
                                            {
                                                ui.WebQueryEnabled = slimInfo[3] as string;
                                                ui.ItemRequestEnabled = slimInfo[4] as string;
                                                ui.ISSEnabled = slimInfo[5] as string;
                                            }
                                        }
                                        // just update SLIM:
                                        UserUpdateError uue = repo.UpdateSlim(ui, ref log);
                                        if (uue != UserUpdateError.None)
                                        {
                                            // add error message to Errors tab
                                            List<object> row = new List<object>(userRow);

                                            if (uue == UserUpdateError.TitleNotFound)
                                            {
                                                log.Error("User Title not found!  Must EXACTLY match Title_Desc in Title table!");
                                                row.Add("User Title not found!  Must EXACTLY match Title_Desc in Title table!");
                                                Console.WriteLine("!! ERROR !!: User Title not found!  Must EXACTLY match Title_Desc in Title table!");
                                            }
                                            else if (uue == UserUpdateError.StoreNotFound)
                                            {
                                                log.Error("Store not found!  Must EXACTLY match Store_Name in Stores table!");
                                                row.Add("Store not found!  Must EXACTLY match Store_Name in Stores table!");
                                                Console.WriteLine("!! ERROR !!: Store not found!  Must EXACTLY match Store_Name in Stores table!");
                                            }
                                            else if (uue == UserUpdateError.Other)
                                            {
                                                log.Error("Unknown error updating user!  User not updated!");
                                                row.Add("Unknown error updating user!  User NOT updated!");
                                                Console.WriteLine("!! ERROR !!: Unknown error updating user!  User not updated!");
                                            }

                                            AddResultRow(ImportResultType.Errored, row.ToArray());
                                        }

                                        AddResultRow(ImportResultType.Updated, userRow);
                                        log.Message("user " + ui.FullName + " updated SLIM fields.");
                                    }
                                    else
                                    {
                                        // no change.
                                        AddResultRow(ImportResultType.Unchanged, userRow);
                                        log.Message("skipping user " + ui.FullName);
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            log.Error(ex.Source + " " +  ex.Message );
                            List<object> errorrow = new List<object>(userRow);
                            errorrow.Add(ex.Source + " " +  ex.Message );
                            AddResultRow(ImportResultType.Errored, errorrow.ToArray());
                            Console.WriteLine("!! ERROR !!: Unknown error updating user!  User not updated!");
                        }
                    }
                }
            }
            log.Message(Results());
            this.Close();
            return 0;
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

        private void SetupLog()
        {
            string logDir = Path.Combine(Directory.GetCurrentDirectory(), "logs");
            if (!Directory.Exists(logDir))
                Directory.CreateDirectory(logDir);
            string logDate = DateTime.Now.ToString("yyyy_MM_dd");
            log = new Log(Path.Combine(logDir, region.ToUpper() + "_" + logDate + "_Import.log"), true);
        }

        private void OpenFile(string filePath)
        {
            inputSheet = null;
            inputSheet = new SpreadsheetManager(filePath);
        }

        public List<object[]> ParseWorksheet(string worksheet, string startColumn, string lastColumn)
        {
            int lastRow = inputSheet.GetLastRow(worksheet);
            int row;
            List<object[]> rows = new List<object[]>();
            for (row = 1; row <= lastRow; row++)
            {
                rows.Add(inputSheet.GetRowData(worksheet, lastColumn, row));
            }
            return rows;
        }

        public bool WorksheetExists(string worksheet)
        {
            return inputSheet.WorksheetExists(worksheet);
        }

        private void BuildFileList(string path)
        {
            files = new List<string>();

            DirectoryInfo di = new DirectoryInfo(path);
            if (!di.Exists)
            {
                log.Error("input file path not found: " + path);
            }
            else
            {
                foreach (FileInfo fi in di.GetFiles())
                {
                    //if (fi.Name.EndsWith(".xlsx"))
                    files.Add(Path.Combine(path, fi.Name));
                }
            }
            
        }

        private string CreateResultsFilename(string path)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(region.ToUpper());
            sb.Append("_Import_Results.xlsx");
            string resultPath = Path.Combine(path, sb.ToString());
            if (File.Exists(resultPath))
                File.Delete(resultPath);
            return resultPath;
        }

        private void SetupResultsSheet(string path)
        {
            
            resultsFileName = CreateResultsFilename(path);
            resultSheet = new SpreadsheetManager(resultsFileName);
            resultSheet.CreateWorksheet("Updated");
            resultSheet.CreateWorksheet("Deleted");
            resultSheet.CreateWorksheet("Unchanged");
            resultSheet.CreateWorksheet("Errored");
            List<string> columns = new List<string> { "User_ID", "UserName", "FullName", "Title", "StoreLimit", "Override Allow", "Override Deny", "User Edited?", "Delete User?" };
            resultSheet.CreateHeader("Updated", columns);
            resultSheet.CreateHeader("Deleted", columns);
            resultSheet.CreateHeader("Unchanged", columns);
            resultSheet.CreateHeader("Errored", columns);
            resultSheet.JumpToRow("Updated", 3);
            resultSheet.JumpToRow("Deleted", 3);
            resultSheet.JumpToRow("Unchanged", 3);
            resultSheet.JumpToRow("Errored", 3);
        }

        private void AddResultRow(ImportResultType type, object[] items)
        {
            switch (type)
            {
                case ImportResultType.Deleted:
                    resultSheet.AddRow("Deleted", items);
                    deleteCount++;
                    break;
                case ImportResultType.Updated:
                    resultSheet.AddRow("Updated", items);
                    updateCount++;
                    break;
                case ImportResultType.Errored:
                    resultSheet.AddRow("Errored", items);
                    erroredCount++;
                    break;
                default:
                    resultSheet.AddRow("Unchanged", items);
                    unchangedCount++;
                    break;
            }
        }

        public void Close()
        {
            resultSheet.Close();
        }

        public string Results()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Results: \n\t");
            sb.Append(updateCount);
            if(updateCount == 1)
                sb.Append(" user updated\n\t");
            else
                sb.Append(" users updated\n\t");

            sb.Append(deleteCount);
            if (deleteCount == 1)
                sb.Append(" user deleted\n\t");
            else
                sb.Append(" users deleted\n\t");

            sb.Append(erroredCount);
            if (erroredCount == 1)
                sb.Append(" user errored\n\t");
            else
                sb.Append(" users errored\n\t");

            //sb.Append("and ");
            sb.Append(unchangedCount);
            if (unchangedCount == 1)
                sb.Append(" user unchanged.");
            else
                sb.Append(" users unchanged.");

            return sb.ToString();

        }

        //public void MoveResultsFile(string toPath)
        //{
        //    File.Move(resultsFileName, Path.Combine(toPath, Path.GetFileName(resultsFileName)));
        //}

        #endregion

        #region Properties

        public List<string> Files
        {
            get { return files; }
        }

        public string ResultsFileName
        {
            get { return resultsFileName; }
        }

        public int UpdateCount { get { return this.updateCount; } }
        public int DeleteCount { get { return this.deleteCount; } }
        public int UnchangedCount { get { return this.unchangedCount; } }
        public int ErroredCount { get { return this.erroredCount; } }

        #endregion
    }
}
