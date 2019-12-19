using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Xml;
using WFM.Mobile.OOS.OOSService;


namespace WFM.Mobile.OOS
{
    public partial class Settings : Form
    {
        private delegate void NetworkCheckDelegate(bool value);

        private readonly ScanOutOfStock oos = new ScanOutOfStock();
        private bool isLoading = true;
        private bool NetworkTestResult = false;
        
        public Settings()
        {
            InitializeComponent();
        }

        private void Settings_Load(object sender, EventArgs e)
        {
            ThreadPool.QueueUserWorkItem(NetworkCheck);
            Cursor.Current = Cursors.WaitCursor;


            textBox_OOSURI.Text = Global.OOSWebService;
            textBox_BizTalkURI.Text = Global.BizTalkWebService;
//            label_ServerTime.Text = GetServerTime().ToString("MM/dd/yy H:mm:ss zzz");

            if (!RefreshRegions())
            {
                MessageBox.Show(
                    "Unable to load Region and Store information. Please check your network connectivity and try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                Global.ForceClose = true;
                Close();
            }
        

        Cursor.Current = Cursors.Default;

        }

       

        private void NetworkCheck(object state)
        {
            NetworkCheck();
        }

        private void UpdateNetworkCheck(bool value)
        {
            picNetworkTest.Image = value ? imageList1.Images[1] : imageList1.Images[0];
        }

        private void RefreshStores( string region)
        {
            var results = oos.StoreAbbreviationsFor(region).ToList();
            results.Insert(0,"");            
            ComboBox_Stores.BeginUpdate();
            ComboBox_Stores.DataSource = results;
            ComboBox_Stores.EndUpdate();

            if (Global.StoreAbbrev == string.Empty) return;

            var index = 0;
            for (index = 0; index < ComboBox_Stores.Items.Count; index++)
            {
                var item = ComboBox_Stores.Items[index].ToString();
                if (item != Global.StoreAbbrev) continue;
                ComboBox_Stores.SelectedIndex = index;
                break;
            }

        }

        private bool RefreshRegions()
        {

            var retval = false;

            try
            {
                oos.Url = Global.OOSWebService;
                var results = oos.RegionAbbreviations();
               // var regionstext = string.Join(",", results.ToArray());
                //MessageBox.Show("regions returned: " + regionstext);
                
                //MessageBox.Show(oos.Url);
                //MessageBox.Show("global region " + Global.RegionCode);
                ComboBox_Regions.BeginUpdate();
                ComboBox_Regions.DataSource = results;
                ComboBox_Regions.EndUpdate();
                isLoading = false;

                
                var x = 0;
                foreach (var item in ComboBox_Regions.Items)
                {
                    if (item.ToString() == Global.RegionCode)
                    {
                        ComboBox_Regions.SelectedIndex = x;
                    }
                    x++;
                }

                retval = true;
            }
            catch (Exception)
            {

                retval = false;
            }

            return retval;

        }

        private void NetworkCheck()
        {
            try
            {
                NetworkTestResult = Global.TcpSocketTest();
                Invoke(new NetworkCheckDelegate(UpdateNetworkCheck), NetworkTestResult);
            }
            catch (ObjectDisposedException)
            {
                
            }
        }

        private void ComboBox_Regions_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isLoading) return;
            RefreshStores(ComboBox_Regions.SelectedItem.ToString());
        }



        private void menuItem_Save_Click(object sender, EventArgs e)
        {
            label_Info.Text = "... Saving ...";

            panel_Info.Visible = true;
            panel_Info.Refresh();

            menuItem_Empty.Enabled = false;
            

            var retval = SaveSettings();

            menuItem_Empty.Enabled = true;
            panel_Info.Visible = false;
            panel_Info.Refresh();


            if (retval)
            {
                Close();
            }
            
        }

        private bool SaveSettings()
        {
            var retval = false;

            if (ComboBox_Stores.SelectedItem.ToString() == string.Empty)
            {
                MessageBox.Show("You must select a Store", "Notice", MessageBoxButtons.OK, MessageBoxIcon.Exclamation,
                                MessageBoxDefaultButton.Button1);
                retval = false;

            }
            else
            {

                Global.StoreAbbrev = ComboBox_Stores.SelectedItem.ToString();
                

                var xml = new XmlDocument();
                var root = xml.CreateElement("OOSSettings");
                xml.AppendChild(root);
                var defaultRegion = xml.CreateElement("DefaultRegion");
                defaultRegion.SetAttribute("Value", Global.RegionCode);
                root.AppendChild(defaultRegion);


                var defaultStore = xml.CreateElement("DefaultStore");
                defaultStore.SetAttribute("Value", Global.StoreAbbrev);
                root.AppendChild(defaultStore);
                xml.Save("OOSSettings.xml");
                retval = true;

            }

            Thread.Sleep(500);
            return retval;

        }

        private void MenuItem_ExitOOS_Click(object sender, EventArgs e)
        {
            Global.ForceClose = true;  // set a flag to close the application.
            Close();                    // close this form.
        }


       


      
    }
}