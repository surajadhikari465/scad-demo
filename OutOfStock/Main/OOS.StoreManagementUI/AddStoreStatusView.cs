using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using OOS.Model;
using OOSCommon.DataContext;

namespace OOS.StoreManagementUI
{
    public partial class AddStoreStatusView : Form
    {
        private Form beneathWindow;
        private IOOSEntitiesFactory entitiesFactory;

        public AddStoreStatusView(Form beneathWindow, IOOSEntitiesFactory entitiesFactory)
        {
            this.beneathWindow = beneathWindow;
            this.entitiesFactory = entitiesFactory;

            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            Location = beneathWindow.Location;
            addStoreStatusResult.Text = string.Empty;
        }

        private void addStoreStatus_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            addStoreStatusResult.Text = string.Empty;
            if (!ValidateInput()) return;
            try
            {
                AddStoreStatus();
            }
            catch(Exception error)
            {
                addStoreStatusResult.Text = "Failed while adding store status.\n\r" + error.Message;
            }
        }


        private void AddStoreStatus()
        {
            var status = storeStatus.Text.Trim();
            using (var dbContext = entitiesFactory.New())
            {
                var first = (from c in dbContext.STATUS where c.STATUS1 == status select c.STATUS1).FirstOrDefault();
                if (first == null)
                {
                    dbContext.STATUS.AddObject(new STATUS { STATUS1 = status });
                    dbContext.SaveChanges();
                    addStoreStatusResult.Text = string.Format("Done adding status '{0}'", status);
                    return;
                }
                addStoreStatusResult.Text = string.Format("Status '{0}' already exists", status);
            }

        }

        private bool ValidateInput()
        {
            if (string.IsNullOrWhiteSpace(storeStatus.Text))
            {
                addStoreStatusResult.Text = "Store status cannot be empty";
                return false;
            }
            if (!new Regex("^[A-Za-z]+$").Match(storeStatus.Text.Trim()).Success)
            {
                addStoreStatusResult.Text = "Store status must be upper and lower case letters";
                return false;
            }
            return true;
        }

        private void refresh_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            addStoreStatusResult.Text = string.Empty;
            storeStatus.Text = string.Empty;
        }
    }
}
