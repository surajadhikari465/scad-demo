using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Windows.Forms;
using System.Configuration;
using KnownUploadClientUI.KnownUploader;

namespace KnownUploadClientUI
{
    public partial class KnownUploadForm : Form
    {
        public KnownUploadForm()
        {
            InitializeComponent();
        }

        private void KnownUploadForm_Load(object sender, EventArgs e)
        {
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                var doc = new KnownUploadDocument
                              {
                                  UploadDate = DateTime.Now,
                                  ItemData = new KnownItemData[0],
                                  VendorRegionMap = new KnownVendorRegionMap[0],
                              };
                new KnownUploaderClient().Upload(doc);
            }
            catch (EndpointNotFoundException ex)
            {
            }
        }
    }
}
