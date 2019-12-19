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
    public partial class StoreSkuCountView : Form
    {
        private Form beneathWindow;
        private IStoreSkuCountReader storeSkuCountReader;
        private IRegionRepository regionRepository;

        public StoreSkuCountView()
        {
            InitializeComponent();
        }

        public StoreSkuCountView(Form storeManagementTasks, IStoreSkuCountReader storeSkuCountReader, IRegionRepository regionRepository) : this()
        {
            this.beneathWindow = storeManagementTasks;
            this.storeSkuCountReader = storeSkuCountReader;
            this.regionRepository = regionRepository;

            StartPosition = FormStartPosition.Manual;
        }

        protected override void OnLoad(EventArgs e)
        {
            Location = beneathWindow.Location;
            var abbrevs = regionRepository.RegionAbbreviations().ToList();
            abbrevs.Insert(0, "---Select a region---");
            regionComboBox.DataSource = abbrevs;
            skuCountViewResult.Text = string.Empty;
            ShowEmptySkuSummary();
        }

        private void ShowEmptySkuSummary()
        {
            var skuCountList = (from summaryKey in new List<SummaryKey>()
                                let store = summaryKey.Store
                                let team = summaryKey.Team
                                select new { Store = store, Team = team, Count = Convert.ToString(new SKUSummary().For(store, team)) }).ToList();
            skuCountDataGridView.DataSource = skuCountList;
        }

        private void refreshLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            TryBindSkuCount();
        }

        private void TryBindSkuCount()
        {
            skuCountViewResult.Text = string.Empty;
            try
            {
                BindSkuCount();
            }
            catch (Exception error)
            {
                skuCountViewResult.Text = "Error while reading SKU Count.\n\r" + error.Message;
                ShowEmptySkuSummary();
            }
        }

        private void BindSkuCount()
        {
            if (regionComboBox.SelectedIndex < 1)
            {
                ShowEmptySkuSummary();
                return;
            }
            var region = regionComboBox.SelectedItem.ToString();
            var skuSummary = storeSkuCountReader.SkuSummary(region);
            var keys = skuSummary.KeyList();
            var skuCountList = (from summaryKey in keys
                                let store = summaryKey.Store
                                let team = summaryKey.Team
                                select new { Store = store, Team = team, Count = Convert.ToString(skuSummary.For(store, team)) }).ToList();
            skuCountDataGridView.DataSource = skuCountList;
        }

        private void regionComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            TryBindSkuCount();
        }
    }
}
