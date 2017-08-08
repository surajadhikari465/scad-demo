using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using log4net;

namespace IRMAUserAuditConsole
{
    public class Program
    {
        #region Members

        private const string version = "2.1";

        //private UserAudit userAudit;
        //private OptionsManager opts;
        //private Repository repo = null;
        //private ConfigRepository configRepo = null;
        //private DateRepository dateRepo = null;
        //private Guid appId;
        //private Guid envId;
        //private bool backupSuccessful = false;
        ////private List<string> regions = new List<string>();
        ////private List<string> regionsOneSheet = new List<string>();
        ////private List<string> backupRegions = new List<string>();
        ////private List<string> stores;
        ////private List<string> titles;
        ////private List<string> yesNo;

        //// root folder is now a UNC path to the sharepoint machine.
        //// it's stored in the app config.
        ////private string rootFolder = "";
        ////private string fiscalYearFolder = "";
        ////private string backupRoot = "";
        ////int quarter = 1;

        DateTime currentDateTime = DateTime.Now;

        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        //private bool allLogsMoved = true;

        //static Mutex singleton;
        #endregion

        static void Main(string[] args)
        {
            log4net.Config.XmlConfigurator.Configure();
            Program p = new Program();

            // make sure we are only running one instance...
            //bool created;
            //singleton = new Mutex(true, "WHOLEFOODS_USER_AUDIT_PROCESS_THINGY", out created);
            //if (!created)
            //{
            //    p.logger.Error("App already running. Exiting...");
            //    return;
            //}

            var options = UserAudit.GetOptionsFromProgramArguments(args);
            if (options.IsError)
            {
                p.logger.Error(options.ErrorMessage);
                ShowUsage(p.logger);
            }
            else
            {
                if (options.IsWarning)
                {
                    p.logger.Warn(options.WarningMessage);
                }
                var userAudit = new UserAudit(options, p.logger);
                userAudit.RunAudit();
            }
        }

        private static void ShowUsage(ILog logger)
        {
            logger.Error("Usage:");
            logger.Error("\tIRMAUserAuditConsole <region> [environment]");
            logger.Error("\twhere <region> is the two letter region code (SP, CE, SO, etc)");
            logger.Error("\tand [environment] is one of: test, dev, qa, prod.  This is optional");
            logger.Error("\tand will default to TEST if not specified.");
            Console.WriteLine();
        }

        //#region Main Methods

        //private string CheckConfig()
        //{
        //    string action = "__none__";
        //    string exportDates = "";
        //    string importDates = "";
        //    string delimiter = ";";
        //    Boolean isExportRequired = false;
        //    Boolean isImportRequired = false;

        //    if ((configRepo.ConfigurationGetValue("ExportDates", ref exportDates)) == false && (configRepo.ConfigurationGetValue("ImportDates", ref importDates)) == false)
        //    {
        //        logger.Error("Unable to load Next Export or Import Run Date from config!");
        //        return action;
        //    }
        //    configRepo.ConfigurationGetValue("delimiter", ref delimiter);

        //    if (!string.IsNullOrEmpty(exportDates))
        //    {
        //        isExportRequired = anyActionRequired(exportDates, delimiter.ToCharArray());
        //    }
        //    if (!string.IsNullOrEmpty(importDates))
        //    {
        //        isImportRequired = anyActionRequired(importDates, delimiter.ToCharArray());
        //    }

        //    return Action(isExportRequired, isImportRequired);
        // }

        //private bool LoadConfig()
        //{
        //    configRepo = new ConfigRepository(opts.ConnectionString);
        //    try
        //    {
        //        SetupConfig(opts.Environment);
        //    }
        //    catch (Exception ex)
        //    {
        //        logger.Error("Unable to load config! " + ex.Message);
        //        return false;
        //    }
        //    return true;
        //}

        //private void SetupConfig(IRMAEnvironment _env)
        //{
        //    var environments = configRepo.GetEnvironmentList();
        //    envId = environments.SingleOrDefault(env => env.Name.ToLower().Replace(" ", "") == _env.ToString().ToLower()).EnvironmentID;
        //    if (envId != null)
        //    {
        //        // we have found an Env.  Now look for the app:
        //        appId = configRepo.GetApplicationList(envId).SingleOrDefault(app => app.Name.ToLower() == "user audit").ApplicationID;
        //        if (appId != null)
        //        {
        //            // we have the app!  
        //            bool configOk = configRepo.LoadConfig(appId, envId);
        //            if (!configOk)
        //                throw new Exception("Unable to load config!");
        //        }
        //        else
        //        {
        //            logger.Error("Unable to find application in environment!");
        //            throw new Exception("could not find config application for environment!");
        //        }
        //    }
        //    else
        //    {
        //        logger.Error("Unable to find Environment!");
        //        throw new Exception("Could not find config environment!");
        //    }
        //}

        //private void SetNextRunDateAction(string completedAction)
        //{
        //    DateTime nextRunDate = currentDateTime;
        //    int currentYear = dateRepo.FiscalYear(currentDateTime);
        //    int currentQuarter = dateRepo.FiscalQuarter(currentDateTime);

        //    string nextRunAction = "";
        //    switch (completedAction)
        //    {
        //        case "import":
        //            // next export will occur 5 weeks before the last weekday of the next quarter
        //            nextRunAction = "export";
        //            currentQuarter++;
        //            if (currentQuarter > 4)
        //            {
        //                currentQuarter = 1;
        //                currentYear++;
        //            }
        //            DateTime? qStart = dateRepo.GetQuarterStart(currentQuarter, currentYear);
        //            if (qStart.HasValue)
        //            {
        //                nextRunDate = dateRepo.LastWeekdayInQuarter(qStart.Value).AddDays(-35);
        //            }
        //            else
        //            {
        //                logger.Error("Unable to set next run date!  Is it October 2015 already?");
        //                return;
        //            }
        //            break;
        //        case "export":
        //            // backup/import will occur 3 weeks from the export date
        //            nextRunAction = "import";
        //            nextRunDate = currentDateTime.AddDays(21);
        //            break;
        //    }

        //    if (configRepo.UpdateKeyValue(appId, envId, "NextRunDate", nextRunDate.ToString("G"), 0))
        //    {
        //        // only set run action if settig the date was successful
        //        if (!configRepo.UpdateKeyValue(appId, envId, "NextRunAction", nextRunAction, 0))
        //        {
        //            logger.Error("Unable to set next run action!");
        //        }
        //    }
        //    else
        //    {
        //        logger.Error("Unable to set next run date!");
        //    }
        //}


        //private void DoImport()
        //{
        //    RegionImportManager rim = new RegionImportManager(opts.Region, opts.ConnectionString, opts.Environment);
        //    int result = rim.Import();
        //    if (result != 0)
        //        logger.Error("Import return non-zero result!  Check log for possible errors!!");
        //}

        //#endregion

        //#region Background Process

        //private void RunRestore()
        //{

        //    logger.Info("connecting to " + opts.Region + " " + opts.Environment.ToString() + " environment...");
        //    RegionRestoreManager rrm = new RegionRestoreManager(opts.Region, opts.ConnectionString, opts.Environment);
        //    logger.Info("Connected.");

        //    int result = rrm.RestoreUsers();
        //    if (result != 0)
        //        logger.Error("Restore returned with errors!  Check log for details!");
        //    else
        //        logger.Info("Users restored.");

        //}

        //private void bwImport_RunWorkerCompleted()
        //{
        //    if (backupSuccessful)
        //        SetNextRunDateAction("import");
        //}

        //private void bwExport_RunWorkerCompleted()
        //{
        //    SetNextRunDateAction("export");
        //}

        //private void bwRestore_RunWorkerCompleted()
        //{
        //    logger.Warn("Restore complete! You MUST set next run date/action MANUALLY or imports/exports will NOT function!");
        //}

        //private void RunImport()
        //{

        //    int result = RunBackup();
        //    if (result != 0)
        //    {
        //        logger.Error("Backup returned nonzero result!  Stopping since unsafe to import without current backup.");
        //        logger.Error("Check backup log for more info.");
        //    }
        //    else
        //    {
        //        DoImport();
        //    }

        //    backupSuccessful = true;
        //}

        //private void RunExport()
        //{

        //    logger.Info("connecting to " + opts.Region + " " + opts.Environment.ToString() + " environment...");
        //    RegionExportManager rem = new RegionExportManager(opts.Region, opts.ConnectionString, opts.Environment);
        //    logger.Info("Connected.");
        //    if (!rem.ConfigOk)
        //    {
        //        logger.Error("Unable to load config!  Exiting...");
        //        return;
        //    }

        //    string fiscalYear = "FY" + FiscalYear.Year().ToString() + "_" + FiscalYear.Quarter().ToString();

        //    logger.Info("exporting...");
        //    int result = rem.Export();

        //    if (result != 0)
        //    {
        //        logger.Error("Export returned non-zero result!  Check log for possible errors.");
        //    }

        //}

        //private int RunBackup()
        //{
        //    logger.Info("connecting to " + opts.Region + "...");
        //    RegionBackupManager rbm = new RegionBackupManager(opts.Region, opts.ConnectionString, opts.Environment);
        //    logger.Info("Backing up...");
        //    int result = rbm.BackupUsers();
        //    return result;
        //}

        //#endregion
    }
}
