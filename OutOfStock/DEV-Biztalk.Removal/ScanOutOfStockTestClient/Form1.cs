using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Windows.Forms;

namespace ScanOutOfStockTestClient
{
    public partial class Form1 : Form
    {
        private BasicHttpBinding binding = new BasicHttpBinding();

        private const string defaultRemoteAddress = "http://localhost:54264/Webservice/ScanOutOfStock.svc";
        private EndpointAddress endpointAddress;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            var uri = ConfigurationManager.AppSettings["uri"];
            endpointAddress = new EndpointAddress(uri ?? defaultRemoteAddress);

        }

        private void button2_Click(object sender, EventArgs e)
        {
            regionListView.Items.Clear();
            validationMsg.Text = string.Empty;
            try
            {
                var regions = new ScanOutOfStock.ScanOutOfStockClient(binding, endpointAddress).RegionNames();
                regionListView.Items.AddRange(regions);
            }
            catch(EndpointNotFoundException ex)
            {
                validationMsg.Text = ex.Message;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            validationMsg.Text = string.Empty;
            try
            {
                var echo = new ScanOutOfStock.ScanOutOfStockClient(binding, endpointAddress).Ping("Echo Ping");
                PingBox.Text = echo;
            }
            catch (EndpointNotFoundException ex)
            {
                validationMsg.Text = ex.Message;
            }
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void ClearCommand_Click(object sender, EventArgs e)
        {
            validationMsg.Text = string.Empty;
            PingBox.Text = string.Empty;
            storeListView.Items.Clear();
            regionListView.Items.Clear();
        }

        private void getStoresCommand_Click(object sender, EventArgs e)
        {
            validationMsg.Text = string.Empty;
            storeListView.Items.Clear();
            var region = regionListView.SelectedItem ?? string.Empty;

            try
            {
                var stores = new ScanOutOfStock.ScanOutOfStockClient(binding, endpointAddress).StoreNamesFor(region.ToString());
                storeListView.Items.AddRange(stores);               
            }
            catch (EndpointNotFoundException ex)
            {
                validationMsg.Text = ex.Message;
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void regionListView_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void storeListView_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
