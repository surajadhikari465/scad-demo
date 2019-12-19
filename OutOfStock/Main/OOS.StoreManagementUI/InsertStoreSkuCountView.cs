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
    public partial class InsertStoreSkuCountView : Form
    {
        private Form beneathWindow;
        private IRegionRepository regionRepository;
        private IStoreSkuCountReader skuCountReader;
        private ISkuCountRepository skuCountRepository;

        public InsertStoreSkuCountView(Form beneathWindow, IRegionRepository regionRepository, IStoreSkuCountReader skuCountReader, ISkuCountRepository skuCountRepository)
        {
            this.beneathWindow = beneathWindow;
            this.regionRepository = regionRepository;
            this.skuCountReader = skuCountReader;
            this.skuCountRepository = skuCountRepository;

            InitializeComponent();
        }


        protected override void OnLoad(EventArgs e)
        {
            Location = beneathWindow.Location;
            insertSkuCountResult.Text = string.Empty;
            ResetView();
        }

        private void ResetView()
        {
            insertSkuCountResult.Text = string.Empty;
            var abbrevs = RegionAbbreviations();
            abbrevs.Insert(0, "---Select a region---");
            regionComboBox.DataSource = abbrevs;

            storeComboBox.DataSource = new List<string> { "---Select a store---" };
            teamComboBox.DataSource = new List<string> { "---Select a team---" };
            skuCount.Text = string.Empty;
        }

        private List<string> RegionAbbreviations()
        {
            try
            {
                return regionRepository.RegionAbbreviations().ToList();
            }
            catch (Exception e)
            {
                insertSkuCountResult.Text = "RegionAbbreviations error:" + e.Message;
            }
            return new List<string>();
        }


        private void refresh_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            ResetView();
        }

        private void insertSku_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            insertSkuCountResult.Text = string.Empty;
            if (!ValidateInput()) return;

            var region = regionComboBox.SelectedItem.ToString();
            var store = storeComboBox.SelectedItem.ToString();
            var team = teamComboBox.SelectedItem.ToString();
            var count = Convert.ToInt32(skuCount.Text.Trim());

            try
            {
                skuCountRepository.Insert(store, team, count);
                insertSkuCountResult.Text = string.Format("Done inserting store '{0}' in region '{1}' \n\rfor team '{2}' SKU Count '{3}'", store, region, team, count);
                BindAvailableTeams();
            }
            catch (Exception error)
            {
                insertSkuCountResult.Text = "Error while updating SKU Count:\n\r" + error.Message;
            }


        }

        private bool ValidateInput()
        {
            if (regionComboBox.SelectedIndex <= 0)
            {
                insertSkuCountResult.Text = "Region not selected.";
                return false;
            }
            if (storeComboBox.SelectedIndex <= 0)
            {
                insertSkuCountResult.Text = "Store not selected.";
                return false;
            }
            if (teamComboBox.SelectedIndex <= 0)
            {
                insertSkuCountResult.Text = "Team not selected.\n\rPossibly there are no more teams available to insert";
                return false;
            }
            if (string.IsNullOrWhiteSpace(skuCount.Text))
            {
                insertSkuCountResult.Text = "SKU Count must be entered.";
                return false;
            }
            try
            {
                Convert.ToInt32(skuCount.Text.Trim());
            }
            catch (Exception)
            {
                insertSkuCountResult.Text = string.Format("SKU '{0}' could not be converted to an integer.", skuCount.Text.Trim());
                return false;
            }
            return true;

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

            BindAvailableTeams();
        }

        private void BindAvailableTeams()
        {
            var teams = AvailableTeams();
            teams.Insert(0, "---Select a team---");
            teamComboBox.DataSource = teams;
        }

        private List<string> AvailableTeams()
        {
            var region = regionComboBox.SelectedItem.ToString();
            var store = storeComboBox.SelectedItem.ToString();
            var teamsForStore = TeamsForStore(store, region);
            var teams = DefaultTeamList().Where(p => !teamsForStore.Contains(p)).ToList();
            return teams;
        }

        private List<string> TeamsForStore(string store, string region)
        {
            var skuSummary = skuCountReader.SkuSummary(region);
            var storeName = StoreNameFor(store, region);
            var teamsForStore = (from c in skuSummary.KeyList() where c.Store == storeName select c.Team).Distinct().OrderBy(p => p);
            return teamsForStore.ToList();
        }

        private string StoreNameFor(string store, string region)
        {
            var regionalStores = regionRepository.ForAbbrev(region).GetStores();
            var storeName = regionalStores.Find(p => p.Abbrev == store).Name;
            return storeName;
        }

        private List<string> DefaultTeamList()
        {
            return new List<string>{"Grocery", "Whole Body"};
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
    }
}
