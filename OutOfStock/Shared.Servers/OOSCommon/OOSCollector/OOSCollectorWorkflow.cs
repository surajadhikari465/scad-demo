using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
//using System.ServiceProcess;
using System.Text;
using OOSCommon;
using OOSCommon.DataContext;


namespace OOSCommon.OOSCollector
{
    public class OOSCollectorWorkflow
    {
        public const double minimumIntervalMilliseconds = 5000;     // 5 seconds in milliseconds
        public System.Timers.Timer timerServicePeriod { get; set; }
        public System.ComponentModel.BackgroundWorker backgroundWorkerImport { get; set; }

        public OOSCollectorWorkflow()
        {
        }

        public void InitializeComponent()
        {
            if (OOSCommon.OOSCollector.AppConfig.oosLogging == null)
            {
                OOSCommon.OOSCollector.AppConfig.oosLogging = new OOSCommon.OOSLog(
                        OOSCommon.OOSCollector.AppConfig.oosNLogLoggerName,
                        OOSCommon.OOSCollector.AppConfig.nLogBasePath, null, null);
            }

            // Initialize background worker
            backgroundWorkerImport = new BackgroundWorker();
            backgroundWorkerImport.DoWork +=
                new DoWorkEventHandler(backgroundWorkerImport_DoWork);
            backgroundWorkerImport.RunWorkerCompleted +=
                new RunWorkerCompletedEventHandler(backgroundWorkerImport_RunWorkerCompleted);

            // Initialize timer
            this.timerServicePeriod = new System.Timers.Timer();
            ((ISupportInitialize)(this.timerServicePeriod)).BeginInit();
            this.timerServicePeriod.Enabled = false;
            this.timerServicePeriod.Elapsed +=
                new System.Timers.ElapsedEventHandler(this.timerServicePeriod_Elapsed);
            ((ISupportInitialize)(this.timerServicePeriod)).EndInit();
        }

        public void OnStart(string[] args)
        {
            OOSCommon.OOSCollector.AppConfig.oosLogging.Info("Service OnStart");
            if (OOSCommon.OOSCollector.AppConfig.isRunOnTimer)
                StartTimerForNextRun();
            else
                RunImport();
        }

        public void OnStop()
        {
            OOSCommon.OOSCollector.AppConfig.oosLogging.Info("Service OnStop");
            if (timerServicePeriod != null)
            {
                try { timerServicePeriod.Stop(); }
                catch (Exception) { }
            }
            System.Threading.Thread.Sleep(10);
            if (backgroundWorkerImport != null)
            {
                try { backgroundWorkerImport.CancelAsync(); }
                catch (Exception) { }
            }
            System.Threading.Thread.Sleep(10);
        }

        private void backgroundWorkerImport_DoWork(object sender, DoWorkEventArgs e)
        {
            OOSCommon.OOSCollector.AppConfig.oosLogging.Trace("Enter");
            // Reported OOS
            try
            {
                OOSCommon.OOSCollector.IScanner scanner =
                    new OOSCommon.OOSCollector.Scanner(
                        OOSCommon.OOSCollector.AppConfig.uploadedBasePath,
                        OOSCommon.OOSCollector.AppConfig.reportedOOSPostImportMoveToPath,
                        OOSCommon.OOSCollector.AppConfig.reportedOOSPostImportDelete,
                        OOSCommon.OOSCollector.AppConfig.regionPrefix,
                        OOSCommon.OOSCollector.AppConfig.storePrefix,
                        OOSCommon.OOSCollector.AppConfig.oosConnectionString,
                        OOSCommon.OOSCollector.AppConfig.oosEFConnectionString,
                        OOSCommon.OOSCollector.AppConfig.isValidationMode,
                        OOSCommon.OOSCollector.AppConfig.oosLogging);
                OOSCommon.OOSCollector.ScanAndImportReportedOOS saiReported =
                    new OOSCommon.OOSCollector.ScanAndImportReportedOOS(
                        scanner,
                        OOSCommon.OOSCollector.AppConfig.oosLogging,
                        OOSCommon.OOSCollector.AppConfig.isValidationMode,
                        OOSCommon.OOSCollector.AppConfig.vimRepository,
                        OOSCommon.OOSCollector.AppConfig.oosEFConnectionString,
                        OOSCommon.OOSCollector.AppConfig.movementRepository);
                saiReported.DoScanAndImport();
            }
            catch (Exception ex)
            {
                OOSCommon.OOSCollector.AppConfig.oosLogging.Warn("Exception importing reported: Message=\"" + ex.Message + "\"" + (ex.InnerException == null ? string.Empty :
                    ", Inner=\"" + ex.InnerException.Message + "\""));
            }
            OOSCommon.OOSCollector.AppConfig.oosLogging.Trace("Middle: Reported complete.  Starting known.");
            // Known OOS
            try
            {
                OOSCommon.Import.IOOSImportKnown importKnown =
                    new OOSCommon.Import.OOSImportKnownUNFI();
                OOSCommon.Import.IOOSUpdateKnown updateKnown =
                    new OOSCommon.Import.OOSUpdateKnown(
                        OOSCommon.OOSCollector.AppConfig.isValidationMode,
                        OOSCommon.OOSCollector.AppConfig.oosLogging,
                        new EntityFactory(new BasicConfigurator()),
                        OOSCommon.OOSCollector.AppConfig.vimRepository,
                        OOSCommon.OOSCollector.AppConfig.oosEFConnectionString);
                OOSCommon.OOSCollector.ScanAndImportKnownOOS saiKnown =
                    new OOSCommon.OOSCollector.ScanAndImportKnownOOS(
                        OOSCommon.OOSCollector.AppConfig.oosLogging,
                        OOSCommon.OOSCollector.AppConfig.isValidationMode,
                        OOSCommon.OOSCollector.AppConfig.vimRepository,
                        importKnown,
                        updateKnown,
                        OOSCommon.OOSCollector.AppConfig.oosEFConnectionString,
                        OOSCommon.OOSCollector.AppConfig.ftpUrlUNFI,
                        OOSCommon.OOSCollector.AppConfig.knownOOSPostImportMoveToPath,
                        OOSCommon.OOSCollector.AppConfig.knownOOSPostImportDelete);
                saiKnown.DoScanAndImport();
            }
            catch (Exception ex)
            {
                OOSCommon.OOSCollector.AppConfig.oosLogging.Warn("Exception importing known: Message=\"" + ex.Message + "\"" + (ex.InnerException == null ? string.Empty :
                    ", Inner=\"" + ex.InnerException.Message + "\""));
            }
            OOSCommon.OOSCollector.AppConfig.oosLogging.Trace("Exit");
            e.Cancel = true;
        }

        private void backgroundWorkerImport_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            OOSCommon.OOSCollector.AppConfig.oosLogging.Trace("Enter: RunWorkerCompleted");
            if (OOSCommon.OOSCollector.AppConfig.isRunOnTimer)
                StartTimerForNextRun();
            OOSCommon.OOSCollector.AppConfig.oosLogging.Trace("Exit");
        }

        private void timerServicePeriod_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            OOSCommon.OOSCollector.AppConfig.oosLogging.Trace("Enter");
            timerServicePeriod.Stop();
            RunImport();
            OOSCommon.OOSCollector.AppConfig.oosLogging.Trace("Exit");
        }

        protected void RunImport()
        {
            OOSCommon.OOSCollector.AppConfig.oosLogging.Trace("Enter");
            if (backgroundWorkerImport.IsBusy)
                OOSCommon.OOSCollector.AppConfig.oosLogging.Warn("OOS Collector background thread is busy");
            else
            {
                backgroundWorkerImport.RunWorkerAsync();
                OOSCommon.OOSCollector.AppConfig.oosLogging.Info("OOS Collector background thread started");
            }
            OOSCommon.OOSCollector.AppConfig.oosLogging.Trace("Exit");
        }

        protected void StartTimerForNextRun()
        {
            timerServicePeriod.Stop();
            TimeSpan tsTarget = OOSCommon.OOSCollector.AppConfig.runTime;
            OOSCommon.OOSCollector.AppConfig.ERunDays days =
                OOSCommon.OOSCollector.AppConfig.runDays;
            DateTime dtNow = DateTime.Now;
            DateTime dtNext = dtNow;
            // If it is past time, move it to tomorrow
            double totalMinutesUntilTrigger = tsTarget.TotalMinutes - ((60 * dtNow.Hour) + dtNow.Minute);
            if (totalMinutesUntilTrigger <= 1)
                totalMinutesUntilTrigger += 24 * 60;
            dtNext = dtNext.AddMinutes(totalMinutesUntilTrigger);
            for (int ix = 0; ix < 6; ++ix)
            {
                OOSCommon.OOSCollector.AppConfig.ERunDays dow =
                    OOSCommon.OOSCollector.AppConfig.ToERunDays(dtNext.DayOfWeek);
                if ((dow & days) != OOSCommon.OOSCollector.AppConfig.ERunDays.none)
                {
                    timerServicePeriod.Interval = Math.Max(dtNext.Subtract(dtNow).TotalMilliseconds, minimumIntervalMilliseconds);
                    timerServicePeriod.Start();
                    OOSCommon.OOSCollector.AppConfig.oosLogging.Info("OOS Collector scheduled to run next at " +
                        dtNext.ToString("MM/dd HH:mm"));
                    break;
                }
                dtNext.AddDays(1);
            }
            if (!timerServicePeriod.Enabled)
                OOSCommon.OOSCollector.AppConfig.oosLogging.Warn("Timer not started. Servive configuration error.");
        }

    }
}
