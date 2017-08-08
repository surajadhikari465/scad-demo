using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WholeFoods.Common.IRMALib;
using WholeFoods.Common.IRMALib.Dates;

namespace IRMAUserAuditConsole
{
    public class UserAudit
    {
        #region Properties
        public ILog logger { get; set; }
        public AuditOptions Options { get; set; }
        public IConfigRepository Config { get; set; }
        public IDateRepository Dates { get; set; }

        public Guid AppId { get; set; }
        public Guid EnvId { get; set; }
        
        private DateTime AuditRunTime { get; set; }
        private bool backupSuccessful = false;
        #endregion

        #region constructors
        public UserAudit(){}

        public UserAudit(AuditOptions options,
            ILog logger,
            DateTime? auditRunTime = null, 
            IConfigRepository configRepo = null,
            IDateRepository dateRepo =  null) : this()
        {
            this.Options = options;
            this.logger = logger;
            this.AuditRunTime = auditRunTime.HasValue ? auditRunTime.Value : DateTime.Now;
            this.Config = configRepo ?? new ConfigRepository(Options.ConnectionString);
            this.Dates = dateRepo ?? new DateRepository(Options.ConnectionString); ;
        }
        #endregion

        #region public methods
        public static AuditOptions GetOptionsFromProgramArguments(string[] args)
        {
            var options = new AuditOptions();

            if (args.Length < 1)
            {
                options.ErrorMessage = "No arguments specified.  Halting.";
            }
            else
            {
                string region = args[0].ToUpper();
                string environment = "";
                string conString = "";
                if (args.Length > 1)
                {
                    environment = args[1].ToUpper();
                    conString = args[2];
                }
                else
                {
                    environment = "TEST";
                    options.WarningMessage = $"No environment specified.  Defaulting to {environment}";
                }

                options.SetOptions(region, environment, conString);                
            }
            return options;
        }

        public void RunAudit()
        {
            RunAudit(Options, Config);
        }

        public void RunAudit(AuditOptions options, IConfigRepository configRepo)
        {
            if (LoadConfig(options, configRepo))
            {
                var action = DetermineAuditAction(configRepo);
                if (action != UserAuditFunctionEnum.None)
                {
                    ExecuteAuditAction(action);
                }
            }
            else
            {
                logger.Error("Unable to load config!  Halting.  Make sure USER AUDIT exists in AppConfigApp.");
            }
        }

        public bool LoadConfig()
        {
            return LoadConfig(this.Options, this.Config);
        }

        public bool LoadConfig(AuditOptions options, IConfigRepository configRepo)
        {
            try
            {
                var environments = configRepo.GetEnvironmentList();
                EnvId = environments
                    .SingleOrDefault(e => e.Name.ToLower().Trim() == options.Environment.ToString().ToLower())
                    .EnvironmentID;
                if (EnvId != null)
                {
                    // we have found an Env.  Now look for the app:
                    AppId = configRepo.GetApplicationList(EnvId)
                        .SingleOrDefault(app => app.Name.ToLower() == "user audit")
                        .ApplicationID;
                    if (AppId != null)
                    {
                        // we have the app!  
                        bool configOk = configRepo.LoadConfig(AppId, EnvId);
                        if (!configOk)
                        {
                            throw new Exception("Unable to load config!");
                        }
                    }
                    else
                    {
                        logger.Error("Unable to find application in environment!");
                        throw new Exception("could not find config application for environment!");
                    }
                }
                else
                {
                    logger.Error("Unable to find Environment!");
                    throw new Exception("Could not find config environment!");
                }
            }
            catch (Exception ex)
            {
                logger.Error("Unable to load config! " + ex.Message);
                return false;
            }
            return true;
        }
        
        public UserAuditFunctionEnum DetermineAuditAction()
        {
            return DetermineAuditAction(this.Config);
        }

        public UserAuditFunctionEnum DetermineAuditAction(IConfigRepository configRepo)
        {
            string action = "__none__";

            string exportDates = configRepo.ConfigurationGetValue("ExportDates");
            string importDates = configRepo.ConfigurationGetValue("ImportDates");
            string delimiter = configRepo.ConfigurationGetValue("delimiter") ?? ";";

            if (String.IsNullOrWhiteSpace(exportDates) && String.IsNullOrWhiteSpace(importDates))
            {
                logger.Error("Unable to load Next Export or Import Run Date from config!");
                action = "error";
            }

            if (!string.IsNullOrWhiteSpace(exportDates))
            {
                var exportDatesList = exportDates.Split(new string[] { delimiter }, StringSplitOptions.RemoveEmptyEntries);
                if (exportDatesList.Any(dt => DateTime.Parse(dt).Date == AuditRunTime.Date))
                {
                    action = "export";
                }
                }
            if (!string.IsNullOrWhiteSpace(importDates))
            {
                var importDatesList = importDates.Split(new string[] { delimiter }, StringSplitOptions.RemoveEmptyEntries);
                if (importDatesList.Any(dt => DateTime.Parse(dt).Date == AuditRunTime.Date))
                {
                    action = "import";
                }
            }

            return AuditOptions.ConvertStringToFunction(action);
        }

        public void GetAppAndEnvironmentIds(IRMAEnvironmentEnum _env, IConfigRepository configRepo)
        {
            var environments = configRepo.GetEnvironmentList();
            EnvId = environments.SingleOrDefault(env => env.Name.ToLower().Replace(" ", "") == _env.ToString().ToLower()).EnvironmentID;
            if (EnvId != null)
            {
                // we have found an Env.  Now look for the app:
                AppId = configRepo.GetApplicationList(EnvId).SingleOrDefault(app => app.Name.ToLower() == "user audit").ApplicationID;
                if (AppId != null)
                {
                    // we have the app!  
                    bool configOk = configRepo.LoadConfig(AppId, EnvId);
                    if (!configOk)
                        throw new Exception("Unable to load config!");
                }
                else
                {
                    logger.Error("Unable to find application in environment!");
                    throw new Exception("could not find config application for environment!");
                }
            }
            else
            {
                logger.Error("Unable to find Environment!");
                throw new Exception("Could not find config environment!");
            }
        }
        public void ExecuteAuditAction(UserAuditFunctionEnum action)
        {
            switch (action)
            {
                case UserAuditFunctionEnum.Import:
                    Import(Options);
                    bwImport_RunWorkerCompleted(AuditRunTime);
                    break;
                case UserAuditFunctionEnum.Export:
                    Export(Options);
                    bwExport_RunWorkerCompleted(AuditRunTime);
                    break;
                case UserAuditFunctionEnum.Backup:
                    break;
                case UserAuditFunctionEnum.Restore:
                    Restore(Options);
                    bwRestore_RunWorkerCompleted(AuditRunTime);
                    break;
                case UserAuditFunctionEnum.None:
                    logger.Info("No action scheduled today.  Exiting.");
                    break;
                default:
                    logger.Warn($"Unexpected action '{action}'.  Exiting.");
                    break;
            }
        }

        public void Export(AuditOptions opts)
        {
            logger.Info("connecting to " + opts.Region + " " + opts.Environment.ToString() + " environment...");
            RegionExportManager rem = new RegionExportManager(opts.Region, opts.ConnectionString, opts.Environment);
            logger.Info("Connected.");
            if (!rem.ConfigOk)
            {
                logger.Error("Unable to load config!  Exiting...");
                return;
            }

            string fiscalYear = "FY" + FiscalYear.Year().ToString() + "_" + FiscalYear.Quarter().ToString();

            logger.Info("exporting...");
            int result = rem.Export();

            if (result != 0)
            {
                logger.Error("Export returned non-zero result!  Check log for possible errors.");
            }
        }   

        public void Import(AuditOptions opts)
        {
            int result = Backup(opts);
            if (result != 0)
            {
                logger.Error("Backup returned nonzero result!  Stopping since unsafe to import without current backup.");
                logger.Error("Check backup log for more info.");
            }
            else
            {
                RegionImportManager rim = new RegionImportManager(opts.Region, opts.ConnectionString, opts.Environment);
                result = rim.Import();
                if (result != 0)
                    logger.Error("Import return non-zero result!  Check log for possible errors!!");
            }

            backupSuccessful = true;
        }

        public int Backup(AuditOptions opts)
        {
            logger.Info("connecting to " + opts.Region + "...");
            RegionBackupManager rbm = new RegionBackupManager(opts.Region, opts.ConnectionString, opts.Environment);
            logger.Info("Backing up...");
            int result = rbm.BackupUsers();
            return result;
        }

        public void Restore(AuditOptions opts)
        {
            logger.Info("connecting to " + opts.Region + " " + opts.Environment.ToString() + " environment...");
            RegionRestoreManager rrm = new RegionRestoreManager(opts.Region, opts.ConnectionString, opts.Environment);
            logger.Info("Connected.");

            int result = rrm.RestoreUsers();
            if (result != 0)
                logger.Error("Restore returned with errors!  Check log for details!");
            else
                logger.Info("Users restored.");
        }

        public void SetNextRunDateAction(DateTime dateOfCurrentRun, string completedAction)
        {
            DateTime nextRunDate = dateOfCurrentRun;
            int currentYear = Dates.FiscalYear(dateOfCurrentRun);
            int currentQuarter = Dates.FiscalQuarter(dateOfCurrentRun);

            string nextRunAction = "";
            switch (completedAction)
            {
                case "import":
                    // next export will occur 5 weeks before the last weekday of the next quarter
                    nextRunAction = "export";
                    currentQuarter++;
                    if (currentQuarter > 4)
                    {
                        currentQuarter = 1;
                        currentYear++;
                    }
                    DateTime? qStart = Dates.GetQuarterStart(currentQuarter, currentYear);
                    if (qStart.HasValue)
                    {
                        nextRunDate = Dates.LastWeekdayInQuarter(qStart.Value).AddDays(-35);
                    }
                    else
                    {
                        logger.Error("Unable to set next run date!  Is it October 2015 already?");
                        return;
                    }
                    break;
                case "export":
                    // backup/import will occur 3 weeks from the export date
                    nextRunAction = "import";
                    nextRunDate = dateOfCurrentRun.AddDays(21);
                    break;
            }

            if (Config.UpdateKeyValue(AppId, EnvId, "NextRunDate", nextRunDate.ToString("G"), 0))
            {
                // only set run action if settig the date was successful
                if (!Config.UpdateKeyValue(AppId, EnvId, "NextRunAction", nextRunAction, 0))
                {
                    logger.Error("Unable to set next run action!");
                }
            }
            else
            {
                logger.Error("Unable to set next run date!");
            }
        }
        
        #endregion

        #region private methods
        private void bwImport_RunWorkerCompleted(DateTime completionDateTime)
        {
            if (backupSuccessful)
            {
                SetNextRunDateAction(completionDateTime, "import");
            }
        }

        private void bwExport_RunWorkerCompleted(DateTime completionDateTime)
        {
            SetNextRunDateAction(completionDateTime, "export");
        }

        private void bwRestore_RunWorkerCompleted(DateTime completionDateTime)
        {
            logger.Warn("Restore complete! You MUST set next run date/action MANUALLY or imports/exports will NOT function!");
        }
        #endregion
    }
}
