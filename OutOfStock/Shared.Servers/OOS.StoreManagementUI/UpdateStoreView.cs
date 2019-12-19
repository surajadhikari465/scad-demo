using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using OOS.Model;

namespace OOS.StoreManagementUI
{
    public partial class UpdateStoreView : Form
    {
        private Form beneathWindow;
        private IRegionRepository regionRepository;
        private IOOSEntitiesFactory entitiesFactory;
        private IStoreRepository storeRepository;

        public UpdateStoreView(Form beneathWindow, IRegionRepository regionRepository, IStoreRepository storeRepository, IOOSEntitiesFactory entitiesFactory)
        {
            this.beneathWindow = beneathWindow;
            this.regionRepository = regionRepository;
            this.storeRepository = storeRepository;
            this.entitiesFactory = entitiesFactory;

            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            Location = beneathWindow.Location;
            ResetView();
        }

        private void ResetView()
        {
            updateStoreResultLabel.Text = string.Empty;
            var abbrevs = RegionAbbreviations();
            abbrevs.Insert(0, "---Select a region---");
            regionComboBox.DataSource = abbrevs;

            var emptyStores = new List<string> { "---Select a store---" };
            storeComboBox.DataSource = emptyStores;

            var statuses = QueryValidStoreStatus();
            statuses.Insert(0, "---Select a status---");
            statusComboBox.DataSource = statuses;
        }

        private List<string> RegionAbbreviations()
        {
            try
            {
                return regionRepository.RegionAbbreviations().ToList();
            }
            catch (Exception e)
            {
                updateStoreResultLabel.Text = "RegionAbbreviations error:" + e.Message;
            }
            return new List<string>();
        }

        private List<string> QueryValidStoreStatus()
        {
            try
            {
                using (var dbContext = entitiesFactory.New())
                {
                    return (from c in dbContext.STATUS select c.STATUS1).Distinct().OrderBy(p => p).ToList();
                }
            }
            catch (Exception e)
            {
                updateStoreResultLabel.Text = "QueryValidStoreStatus error:" + e.Message;
            }
            return new List<string>();
        }


        private void regionComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            var comboBox = sender as ComboBox;
            if (comboBox == null) return;
            if (comboBox.SelectedIndex == 0) return;

            storeComboBox.DataSource = StoresInRegion(comboBox.SelectedItem.ToString());
        }

        private List<string> StoresInRegion(string regionAbbrev)
        {
            var stores = regionRepository.ForAbbrev(regionAbbrev).GetStores().Select(p => p.Abbrev).ToList();
            stores.Insert(0, "---Select a store---");
            return stores;
        }

        private void updateStoreLinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            updateStoreResultLabel.Text = string.Empty;
            if (!ValidateInput()) return;

            var abbreviation = storeComboBox.SelectedItem.ToString().Trim();
            var status = statusComboBox.SelectedItem.ToString().Trim();
            var store = storeRepository.ForAbbrev(abbreviation);
            var region = regionComboBox.SelectedItem.ToString().Trim();
            try
            {
                store.Status = status;
                storeRepository.Update(store);
                updateStoreResultLabel.Text = string.Format("Done updating store '{0}' in region '{1}' w/ status '{2}'", abbreviation, region, status);
            }
            catch(Exception error)
            {
                updateStoreResultLabel.Text = string.Format("Error while updating store '{0}' in region '{1}' w/ status '{2}' \n\r{3}", abbreviation, region, status, error.Message);
            }
        }

        private bool ValidateInput()
        {
            if (regionComboBox.SelectedIndex <= 0)
            {
                updateStoreResultLabel.Text = "Region not selected.";
                return false;
            }
            if (storeComboBox.SelectedIndex <= 0)
            {
                updateStoreResultLabel.Text = "Store not selected.";
                return false;
            }
            if (statusComboBox.SelectedIndex <= 0)
            {
                updateStoreResultLabel.Text = "Store status not selected.";
                return false;
            }
            return true;

        }

        private void refreshView_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            ResetView();
        }
    }
}
