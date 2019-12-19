using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using OOSCommon.OOSCollector;

namespace OOSCollector
{
    public partial class OOSCollectorUI : Form
    {
        public OOSCommon.OOSCollector.OOSCollectorWorkflow oosCollectorWorkflow { get; set; }

        public OOSCollectorUI()
        {
            InitializeComponent();
            oosCollectorWorkflow = new OOSCommon.OOSCollector.OOSCollectorWorkflow();
            oosCollectorWorkflow.InitializeComponent();
        }

        private void button_Start_Click(object sender, EventArgs e)
        {
            button_Start.Enabled = false;
            TimeSpan ts = AppConfig.runTime;
            AppConfig.ERunDays rd = AppConfig.runDays;
#if (true)
            oosCollectorWorkflow.OnStart(new string[] {});
#else
            if (checkBox_Reported.Checked)
            {
                textBox_Uploaded.Text = AppConfig.uploadedBasePath;
                textBox_Imported.Text = AppConfig.reportedOOSPostImportMoveToPath;
                Update();
                IScanner scanner = new Scanner(AppConfig.uploadedBasePath,
                    AppConfig.reportedOOSPostImportMoveToPath, AppConfig.reportedOOSPostImportDelete,
                    AppConfig.regionPrefix, AppConfig.storePrefix,
                    AppConfig.oosConnectionString, AppConfig.oosEFConnectionString,
                    AppConfig.isValidationMode, AppConfig.oosLogging);
                ScanAndImportReportedOOS saiReported = new ScanAndImportReportedOOS(
                    scanner, AppConfig.oosLogging,
                    AppConfig.isValidationMode,
                    AppConfig.vimRepository, AppConfig.oosEFConnectionString,
                    AppConfig.movementRepository);
                saiReported.DoScanAndImport();
            }
            textBox_Uploaded.Text = string.Empty;
            textBox_Imported.Text = string.Empty;
            Update();
            if (checkBox_Known.Checked)
            {
                textBox_Uploaded.Text = AppConfig.ftpUrlUNFI;
                textBox_Imported.Text = AppConfig.knownOOSPostImportMoveToPath;
                Update();
                OOSCommon.Import.IOOSImportKnown importKnown = new OOSCommon.Import.OOSImportKnownUNFI();
                OOSCommon.Import.IOOSUpdateKnown updateKnown = new OOSCommon.Import.OOSUpdateKnown(
                    AppConfig.isValidationMode, AppConfig.oosLogging, AppConfig.vimRepository,
                    AppConfig.oosEFConnectionString);
                ScanAndImportKnownOOS saiKnown = new ScanAndImportKnownOOS(
                    AppConfig.oosLogging,
                    AppConfig.isValidationMode,
                    AppConfig.vimRepository, 
                    importKnown, 
                    updateKnown,
                    AppConfig.oosEFConnectionString,
                    AppConfig.ftpUrlUNFI, 
                    AppConfig.knownOOSPostImportMoveToPath,
                    AppConfig.knownOOSPostImportDelete);
                saiKnown.DoScanAndImport();
            }
#endif
            button_Start.Enabled = true;
            if (AppConfig.isAutorunMode)
                Environment.Exit(0);
        }

        private void OOSCollectorUI_Shown(object sender, EventArgs e)
        {
            if (AppConfig.isAutorunMode)
                button_Start_Click(sender, e);
        }

    }
}
