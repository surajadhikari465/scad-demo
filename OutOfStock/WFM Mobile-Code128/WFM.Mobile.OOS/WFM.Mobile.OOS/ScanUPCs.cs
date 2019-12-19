using System;
using System.Collections;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Net;
using System.Runtime.Serialization;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Reflection;
using System.ServiceModel.Channels;
using System.Text;
using System.Windows.Forms;
using System.Xml.Linq;
using HandheldHardware;
using System.Threading;
using System.Xml;
using WFM.Mobile.OOS.OOSBiztalk;


namespace WFM.Mobile.OOS
{
    public partial class ScanUPCs :   HandheldHardware.ScanForm
    {

        private HandheldScanner MyScanner;
        private BackupManager backupManager = new BackupManager("/backup.txt");

        private char[] validChars = {'0', '1', '2', '3', '4', '5', '6', '7', '8', '9'};


        public string RegionCode { get; set; }
        public string ServiceURI { get; set; }
        public bool UserAuthenticated { get; set; }
        public string UserName { get; set; }
        public string UserEmail { get; set; }
        public string PluginName { get; set; }

        private SavedItemList ItemList;


        public ScanUPCs()
        {
            InitializeComponent();
           
            
        }


        private void Config()
        {


            using (Form splash = new Splash())
            {
                Global.AssemblyVersion = Assembly.GetExecutingAssembly().GetName().Version.ToString();
                splash.Show();
                Application.DoEvents();
                Thread.Sleep(1000);

                Global.ServiceURI = ServiceURI;
                Global.OOSWebService = ServiceURI.Split('|')[0];
                Global.BizTalkWebService = ServiceURI.Split('|')[1];
                Global.UserAuthenticated = UserAuthenticated;
                Global.UserName = UserName;
                Global.UserEmail = UserEmail;
                Global.PluginName = PluginName;
                Global.RegionCode = RegionCode;

                Text = string.Format("OOS {0}", Global.AssemblyVersion);

                
                
                LoadConfigData();

                if (Global.StoreAbbrev == null)
                {
                    Form settings = new Settings();
                    settings.ShowDialog();
                    settings.Dispose();
                    if (Global.ForceClose)
                    {
                        Close();
                    }
                    else
                    {
                        LoadConfigData();
                    }
                }
                
                if (MyScanner != null)
                {
                    MyScanner.restoreScannerSettings();
                }
                try
                {
                    ScanForm me = this;
                    MyScanner = new HandheldScanner(ref me);
                    if (MyScanner.HHType == Scanner.UNKNOWN)
                    {
                        MessageBox.Show(
                            "We are unable to determine the scanner hardware you are using. The application will now close.");
                        Application.Exit();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    if (ex.InnerException != null)
                    {
                        MessageBox.Show(ex.InnerException.Message);
                    }
                }



                splash.Close();
            }


            //backup management

            if (!backupManager.BackupExists()) return;
            // load list
            ItemList = backupManager.LoadFromBackup();

            // populate screen
            label_backupdate.Text = ItemList.TimeStamp.ToShortDateString();
            label_backuptime.Text = ItemList.TimeStamp.ToShortTimeString();
            label_backupcount.Text = ItemList.Items.Count.ToString(CultureInfo.InvariantCulture);

            //show screen
            backupPanel.BringToFront();
            backupPanel.Visible = true;
        }

        private void LoadConfigData()
        {
            if (!File.Exists("OOSSettings.xml")) return;

            var xml = new XmlDocument();
            xml.Load("OOSSettings.xml");

            var store = xml.DocumentElement.SelectSingleNode("DefaultStore");
            var region = xml.DocumentElement.SelectSingleNode("DefaultRegion");

            
            if (store == null) throw new ArgumentNullException("DefaultStore", "Cant find DefaultStore in config file");

            var StoreAbbr = store.Attributes["Value"].Value;

            if (region !=null)
                Global.RegionCode = region.Attributes["Value"].Value;
            
            Global.StoreAbbrev = StoreAbbr;
            
            Label_Region.Text = Global.RegionCode;
            Label_Store.Text = Global.StoreAbbrev;
        }

        public override void UpdateUPCText(string scantext)
        {
            
            // scantext must not be empty. and must be numeric.
            if (scantext == string.Empty || !scantext.All(char.IsDigit)) return;
            if (Global.isNSC2(scantext))
            {
                scantext = Global.convertToNSC2(scantext);
            }
            ListBox_ScannedUPCs.Items.Insert(0, scantext);
            ListBox_ScannedUPCs.SelectedItem = scantext;
        }

        

        public override void  UpdateControlsOnScanCompleteEvent()
        {

 	        base.UpdateControlsOnScanCompleteEvent();
            UpdateListDisplay();
            Label_Status.Text = "Ready To Scan Items";
            Label_Status.ForeColor = Color.Green;
        }

        public override void UpdateControlsScanFailedEvent()
        {
            base.UpdateControlsScanFailedEvent();
            Label_Status.Text = "Scan Failed";
            Label_Status.ForeColor = Color.DarkRed;
        }

        private void UpdateListDisplay()
        {
            Label_TotalItems.Text = ListBox_ScannedUPCs.Items.Count.ToString(CultureInfo.InvariantCulture);
        }

        private void menuItem_Exit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void Button_Remove_Click (object sender, EventArgs e)
        {
            if (ListBox_ScannedUPCs.SelectedItem != null) { ListBox_ScannedUPCs.Items.RemoveAt(ListBox_ScannedUPCs.SelectedIndex);}
            UpdateListDisplay();
        }

        private void ScanUPCs_Load(object sender, EventArgs e)
        {
            try
            {
                Config();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                if (ex.InnerException != null)
                {
                    MessageBox.Show(ex.InnerException.Message);
                }
                
            }
        }

        private void menuItem_Upload_Click(object sender, EventArgs e)
        {
            var upcs = new List<string>();
            if (ListBox_ScannedUPCs.Items.Count == 0)
            {
                MessageBox.Show("You have no scanned items to upload.", "No Items", MessageBoxButtons.OK,
                                MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                return;
            }

            if (
                MessageBox.Show("Upload your scanned item list now?", "Upload", MessageBoxButtons.OKCancel,
                                MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1) != DialogResult.OK) return;
            
            
           
            upcs.AddRange(from string item in ListBox_ScannedUPCs.Items select item.PadLeft(13, '0'));

            var scanByStoreObject = new ScanProductsByStoreAbbreviation
                {
                    regionAbbrev = Global.RegionCode,
                    scanDate = DateTime.Now,
                    scanDateSpecified = true,
                    storeAbbrev = Global.StoreAbbrev,
                    upcs = upcs.ToArray()
                };

            try
            {
                
                // exception used for testing
                //throw new Exception("forced exception");

                using (var instance = new BizTalkServiceInstance())
                {
                    instance.Url = Global.BizTalkWebService;
                    instance.ScanProductsByStoreAbbreviation(scanByStoreObject);
                }
                MessageBox.Show(string.Format("{0} item(s) uploaded.", upcs.Count), "Success", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                ListBox_ScannedUPCs.Items.Clear();
                Label_TotalItems.Text = ListBox_ScannedUPCs.Items.Count.ToString(CultureInfo.InvariantCulture);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error",
                MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);

                try
                {
                    backupManager.CreateNewBackupFile();
                    upcs.ForEach(i => backupManager.SaveToBackup(i));
                    backupManager.CloseBackup();
                }
                catch (Exception ex1)
                {
                    MessageBox.Show(ex1.Message, "Backup Failed",
                                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                    
                }
                
            }

        }

        private void menuItem_Settings_Click(object sender, EventArgs e)
        {
            using (var settingsForm = new Settings())
            {
                settingsForm.ShowDialog();
                if (Global.ForceClose)
                {
                    Close();
                }
                else
                {
                    LoadConfigData();
                }
            }

        }

        private void ListBox_ScannedUPCs_SelectedIndexChanged(object sender, EventArgs e)
        {
            Label_TotalItems.Text = ListBox_ScannedUPCs.Items.Count.ToString();
        }

        private void ScanUPCs_KeyPress(object sender, KeyPressEventArgs e)
        {

            if (validChars.Contains(e.KeyChar) && ManualUpcTextBox.Text.Length <=12)
            {
                if (!ManualUpcPanel.Visible)
                {
                    
                    ManualUpcTextBox.Text = e.KeyChar.ToString(CultureInfo.InvariantCulture);
                    ManualUpcPanel.Focus();
                    ManualUpcPanel.Visible = true;
                }
                else
                {
                    var text = ManualUpcTextBox.Text;
                    text = text + e.KeyChar.ToString(CultureInfo.InvariantCulture);
                    ManualUpcTextBox.Text = text;
                    ManualUpcTextBox.Select(ManualUpcTextBox.Text.Length, 0);
                }

                e.Handled = true;

            }
            
            switch (e.KeyChar)
            {
                case (char)27:
                    // ESC
                    ManualClose();
                    e.Handled = true;
                    break;
                case '\r':
                    // ENTER
                    ManualAdd();
                    e.Handled = true;
                    break;
                case '\b':
                    //Backspace
                    ManualDelChar();
                    break;


            }
         
         
        }

        private void ManualAdd()
        {
            if (ManualUpcPanel.Visible)
            {
                if (ManualUpcTextBox.Text.Length > 0)
                {
                    UpdateUPCText(ManualUpcTextBox.Text);
                    ManualUpcTextBox.Text = string.Empty;
                    ManualUpcPanel.Visible = false;
                }
            }
        }

        private void ManualDelChar()
        {
            if (ManualUpcPanel.Visible)
            {
                if (ManualUpcTextBox.Text.Length > 0)
                {
                    ManualUpcTextBox.Text = ManualUpcTextBox.Text.Remove(ManualUpcTextBox.Text.Length - 1, 1);
                    ManualUpcTextBox.Select(ManualUpcTextBox.Text.Length, 0);
                }
            }
        }

        
        private void ManualUpcSave_Click(object sender, EventArgs e)
        {
            ManualAdd();
        }

        private void ManualClose()
        {
            if (ManualUpcPanel.Visible)
            {
                ManualUpcPanel.Text = string.Empty;
                ManualUpcPanel.Visible = false;
            }
        }

        private void ManualUpcClear_Click(object sender, EventArgs e)
        {
            ManualUpcTextBox.Text = string.Empty;
            ManualUpcTextBox.Select(0,0);
        }

        private void ManualUpcPanel_Paint(object sender, PaintEventArgs e)
        {

            e.Graphics.DrawRectangle(new Pen(Color.Blue), 0, 0,
                                     e.ClipRectangle.Width - 1,
                                     e.ClipRectangle.Height - 1
                                    );

        
        }

        private void ManualUpcTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!validChars.Contains(e.KeyChar) || ManualUpcTextBox.Text.Length >= 13) 
                e.Handled = true;
        }

        private void button_backupload_Click(object sender, EventArgs e)
        {
            // move items from backup into the live list. 
            ItemList.Items.ForEach(i => UpdateUPCText(i.UPC));
            // remove the backup file
            backupManager.RemoveBackup();
            ItemList = null;
            // hide the screen
            backupPanel.Visible = false;
            
        }

        private void button_backupremove_Click(object sender, EventArgs e)
        {
            // remove the backup file
            backupManager.RemoveBackup();
            ItemList = null;
            // hide the screen
            backupPanel.Visible = false;
        }

        

     
        

        

      





    }
}