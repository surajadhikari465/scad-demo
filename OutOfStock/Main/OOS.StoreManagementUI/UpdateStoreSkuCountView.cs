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
    public partial class UpdateStoreSkuCountView : Form
    {
        private Form beneathWindow;
        private IRegionRepository regionRepository;
        private ISkuCountRepository skuCountRepository;
        private IStoreSkuCountReader skuCountReader;

        public UpdateStoreSkuCountView(Form beneathWindow, IRegionRepository regionRepository, ISkuCountRepository skuCountRepository, IStoreSkuCountReader skuCountReader)
        {
            this.beneathWindow = beneathWindow;
            this.regionRepository = regionRepository;
            this.skuCountRepository = skuCountRepository;
            this.skuCountReader = skuCountReader;

            InitializeComponent();
        }


        protected override void OnLoad(EventArgs e)
        {
            Location = beneathWindow.Location;
            updateSkuResultLabel.Text = string.Empty;
            ResetView();
        }

        private void ResetView()
        {
            updateSkuResultLabel.Text = string.Empty;
            var abbrevs = RegionAbbreviations();
            abbrevs.Insert(0, "---Select a region---");
            regionComboBox.DataSource = abbrevs;

            storeComboBox.DataSource = new List<string> { "---Select a store---" };
            teamComboBox.DataSource = new List<string> { "---Select a team---" };
            skuCountTexBox.Text = string.Empty;
        }

        private List<string> RegionAbbreviations()
        {
            try
            {
                return regionRepository.RegionAbbreviations().ToList();
            }
            catch (Exception e)
            {
                updateSkuResultLabel.Text = "RegionAbbreviations error:" + e.Message;
            }
            return new List<string>();
        }

        private void updateStoreSku_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            updateSkuResultLabel.Text = string.Empty;
            if (!ValidateInput()) return;

            var region = regionComboBox.SelectedItem.ToString();
            var store = storeComboBox.SelectedItem.ToString();
            var team = teamComboBox.SelectedItem.ToString();
            var count = Convert.ToInt32(skuCountTexBox.Text.Trim());
            
            try
            {
                skuCountRepository.Modify(store, team, count);
                updateSkuResultLabel.Text = string.Format("Done updating store '{0}' in region '{1}' \n\rfor team '{2}' SKU Count '{3}'", store, region, team, count);
            }
            catch(Exception error)
            {
                updateSkuResultLabel.Text = "Error while updating SKU Count:\n\r" + error.Message;
            }
        }

        private bool ValidateInput()
        {
            if (regionComboBox.SelectedIndex <= 0)
            {
                updateSkuResultLabel.Text = "Region not selected.";
                return false;
            }
            if (storeComboBox.SelectedIndex <= 0)
            {
                updateSkuResultLabel.Text = "Store not selected.";
                return false;
            }
            if (teamComboBox.SelectedIndex <= 0)
            {
                updateSkuResultLabel.Text = "Team not selected.";
                return false;
            }
            if (string.IsNullOrWhiteSpace(skuCountTexBox.Text))
            {
                updateSkuResultLabel.Text = "SKU Count must be entered.";
                return false;
            }
            try
            {
                Convert.ToInt32(skuCountTexBox.Text.Trim());
            }
            catch(Exception)
            {
                updateSkuResultLabel.Text = string.Format("SKU '{0}' could not be converted to an integer.", skuCountTexBox.Text.Trim());
                return false;
            }
            return true;
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

        private void refresh_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            ResetView();
        }

        private void storeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            var comboBox = sender as ComboBox;
            if (comboBox == null) return;
            if (comboBox.SelectedIndex <= 0)
            {
                teamComboBox.DataSource = new List<string> { "---Select a team---" };
                return;
            }

            var teams = TeamsForStore();
            teams.Insert(0, "---Select a team---");
            teamComboBox.DataSource = teams;
        }

        private List<string> TeamsForStore()
        {
            var region = regionComboBox.SelectedItem.ToString();
            var store = storeComboBox.SelectedItem.ToString();
            var skuSummary = skuCountReader.SkuSummary(region);
            var regionalStores = regionRepository.ForAbbrev(region).GetStores();
            var storeName = regionalStores.Find(p => p.Abbrev == store).Name;
            var teams = (from c in skuSummary.KeyList() where c.Store == storeName select c.Team).Distinct().OrderBy(p => p);
            return teams.ToList();
        }
    }
}
