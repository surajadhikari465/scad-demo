using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace OOSImport
{
    public partial class OOSImportUI : Form
    {
        const string storeClosedText = "(closed)";
        public string currentStoreAbbreviation = string.Empty;

        public ImportThread importWorker { get; set; }
        public Thread workerThread { get; set; }

        OOSCommon.DataContext.OOSEntities db
        {
            get
            {
                if (_db == null)
                    _db = new OOSCommon.DataContext.OOSEntities(Program.oosEFConnectionString);
                return _db;
            }
            set { _db = value; }
        } OOSCommon.DataContext.OOSEntities _db = null;

        public OOSImportUI()
        {
            InitializeComponent();
            importWorker = null;
            workerThread = null;

            PopulateRegion();

            comboBox_Format.Items.Clear();
            comboBox_Format.Items.Add("(select format)");
            foreach (ImportBatch.OOSFormat option in Enum.GetValues(typeof(ImportBatch.OOSFormat)))
            {
                if (option != ImportBatch.OOSFormat.None)
                    comboBox_Format.Items.Add(option.ToString());
            }
            comboBox_Format.SelectedIndex = 0;
            dateTimePicker_ScanDate.Text = string.Empty;
            checkBox_Validation.Checked = false;
        }

        private void comboBox_Format_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetEnables();
        }

        private void comboBox_Region_SelectedIndexChanged(object sender, EventArgs e)
        {
            currentStoreAbbreviation = string.Empty;
            string regionAbbreviation = GetSelectedValue(comboBox_Region);
            string[] storeAbbreviations = new string[] { };
            if (!string.IsNullOrWhiteSpace(regionAbbreviation))
            {
                storeAbbreviations =
                    (from s in db.STORE
                     join r in db.REGION on s.REGION_ID equals r.ID
                     where r.REGION_ABBR.Equals(regionAbbreviation, StringComparison.OrdinalIgnoreCase) &&
                        !s.STORE_NAME.Contains(storeClosedText)
                     orderby s.STORE_ABBREVIATION
                     select s.STORE_ABBREVIATION + " " + s.STORE_NAME).ToArray();
            }
            comboBox_Store.Items.Clear();
            comboBox_Store.Items.Add("(select store)");
            comboBox_Store.Items.AddRange(storeAbbreviations);
            comboBox_Store.SelectedIndex = 0;
            SetEnables();
        }

        private void comboBox_Store_SelectedIndexChanged(object sender, EventArgs e)
        {
            currentStoreAbbreviation = GetSelectedValue(comboBox_Store);
            SetEnables();
        }

        private void button_ChooseFiles_Click(object sender, EventArgs e)
        {
            string formatDisplay = (string.IsNullOrWhiteSpace(comboBox_Format.Text) ?
                string.Empty : comboBox_Format.Text);
            string dateDisplay = (!dateTimePicker_ScanDate.Enabled || string.IsNullOrWhiteSpace(dateTimePicker_ScanDate.Text) ?
                string.Empty : dateTimePicker_ScanDate.Text);
            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                foreach (string fileName in openFileDialog1.FileNames)
                {
                    ListViewItem item = new ListViewItem(
                        new string[] { Guid.NewGuid().ToString(), formatDisplay, dateDisplay, currentStoreAbbreviation, fileName });
                    listView_FilesToImport.Items.Add(item);
                }
            }
            SetEnables();
        }

        private void button_ClearQueue_Click(object sender, EventArgs e)
        {
            listView_FilesToImport.Items.Clear();
            SetEnables();
        }

        private void button_Import_Click(object sender, EventArgs e)
        {
            // Start importing
            if (button_Import.Enabled)
            {
                bool isTreadToBeStarted = (workerThread == null);
                if (!isTreadToBeStarted)
                {
                    switch (workerThread.ThreadState)
                    {
                        case ThreadState.Running:
                            workerThread.Abort();
                            break;
                        case ThreadState.Aborted:
                        case ThreadState.Stopped:
                        case ThreadState.Unstarted:
                            isTreadToBeStarted = true;
                            break;
                    }
                }
                if (isTreadToBeStarted)
                    RunImportInThread();
            }
            SetEnables();
        }

        public void OnWorkerProgressChanged(object sender, ImportThreadProgressArgs e)
        {
            // Cross thread -- so you don't get the cross theading exception
            if (this.InvokeRequired)
            {
                this.BeginInvoke((MethodInvoker)delegate
                {
                    OnWorkerProgressChanged(sender, e);
                });
                return;
            }

            // Update UI
            if (!string.IsNullOrWhiteSpace(e.logEntry))
                OOSImport.Program.oosLogging.Log(e.logLevel, e.logEntry);
            switch (e.progressType)
            {
                case ImportThreadProgressArgs.ProgressType.FileStart:
                    // TODO -- Move pointer in list
                    break;
                case ImportThreadProgressArgs.ProgressType.FileComplete:
                case ImportThreadProgressArgs.ProgressType.FileFailed:
                    // TODO -- Note success / failure in list
                    break;
                case ImportThreadProgressArgs.ProgressType.Starting:
                    break;
                case ImportThreadProgressArgs.ProgressType.Stopping:
                    break;
            }
            SetEnables();
        }

        public void RunImportInThread()
        {
            importWorker = new ImportThread(checkBox_Validation.Checked, listView_FilesToImport.Items);
            importWorker.progressChangedEvent += new EventHandler<ImportThreadProgressArgs>(OnWorkerProgressChanged);
            workerThread = new Thread(new ThreadStart(importWorker.StartWork));
            workerThread.Start();
        }

        private void SetEnables()
        {
            ImportBatch.OOSFormat format = ImportBatch.OOSFormat.None;
            if (comboBox_Format.SelectedItem != null)
                Enum.TryParse<ImportBatch.OOSFormat>(comboBox_Format.SelectedItem.ToString(), out format);

            dateTimePicker_ScanDate.Enabled = (format == ImportBatch.OOSFormat.Tagnetics ||
                format == ImportBatch.OOSFormat.UPC);

            button_ChooseFiles.Enabled = (comboBox_Format.SelectedIndex > 0 &&
                comboBox_Store.SelectedIndex > 0 && currentStoreAbbreviation.Length > 0 &&
                (!dateTimePicker_ScanDate.Enabled || !string.IsNullOrWhiteSpace(dateTimePicker_ScanDate.Text)));

            button_Import.Enabled = button_ClearQueue.Enabled = (listView_FilesToImport.Items.Count > 0);
            button_Import.Text = (workerThread != null && workerThread.IsAlive ? "Abort" : "Import");
        }

        private string GetSelectedValue(ComboBox comboBox)
        {
            string result = string.Empty;
            if (comboBox.SelectedItem != null && !((string)comboBox.SelectedItem).StartsWith("("))
            {
                int ixBlank = ((string)comboBox.SelectedItem).IndexOf(' ');
                if (ixBlank > 0)
                    result = ((string)comboBox.SelectedItem).Substring(0, ixBlank);
                else
                    result = ((string)comboBox.SelectedItem).Trim();
            }
            return result;
        }

        private void PopulateRegion()
        {
            string[] regionAbbreviations =
                (from r in db.REGION orderby r.REGION_ABBR select r.REGION_ABBR).ToArray();
            comboBox_Region.Items.Clear();
            comboBox_Region.Items.Add("(select region)");
            comboBox_Region.Items.AddRange(regionAbbreviations);
            comboBox_Region.SelectedIndex = 0;
        }

    }

}
