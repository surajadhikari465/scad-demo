using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using OOS.Model;
using OOS.Model.Feed;

namespace OOS.StoreManagementUI
{
    public partial class AddStoreView : Form
    {
        private IStoreRepository storeRepository;
        private IRegionRepository regionRepository;
        private Form beneathWindow;
        private IOOSEntitiesFactory entitiesFactory;
        private IStoreValidator storeValidator;

        public AddStoreView(Form beneathWindow, IStoreRepository storeRepository, IRegionRepository regionRepository, IOOSEntitiesFactory entitiesFactory, IStoreValidator storeValidator)
        {
            this.beneathWindow = beneathWindow;
            this.storeRepository = storeRepository;
            this.regionRepository = regionRepository;
            this.entitiesFactory = entitiesFactory;
            this.storeValidator = storeValidator;

            InitializeComponent();
        }


        protected override void OnLoad(EventArgs e)
        {
            Location = beneathWindow.Location;
            addStoreResultLabel.Text = string.Empty;
            var abbrevs = RegionAbbreviations();
            abbrevs.Insert(0, "---Select a region---");
            addStoreInRegionComboBox.DataSource = abbrevs;

            var statuses = QueryValidStoreStatus();
            statuses.Insert(0, "---Select a status---");
            storeStatusComboBox.DataSource = statuses;
        }

        private List<string> RegionAbbreviations()
        {
            try 
            { 
                return regionRepository.RegionAbbreviations().ToList(); 
            }
            catch(Exception e)
            {
                addStoreResultLabel.Text = "RegionAbbreviations error:" + e.Message;
            }
            return new List<string>();
        }

        private List<string> QueryValidStoreStatus()
        {
            try
            {
                using (var dbContext = entitiesFactory.New())
                {
                    return (from c in dbContext.STATUS
                            where !c.STATUS1.Equals("Closed", StringComparison.OrdinalIgnoreCase)
                            select c.STATUS1).Distinct().ToList();
                }
            }
            catch(Exception e)
            {
                addStoreResultLabel.Text = "QueryValidStoreStatus error:" + e.Message;
            }
            return new List<string>();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            addStoreResultLabel.Text = string.Empty;
            if (!ValidateInput()) return;

            try
            {
                var regionAbbrev = addStoreInRegionComboBox.SelectedItem.ToString();
                var storeAbbrev = storeAbbreviationTextBox.Text.Trim();
                if (StoreExistsInRegion(storeAbbrev, regionAbbrev)) return;

                var name = storeNameTextBox.Text.Trim();
                var status = storeStatusComboBox.SelectedItem.ToString();
                AddStore(storeAbbrev, name, regionAbbrev, status);

                if (storeRepository.ForAbbrev(storeAbbrev) == null)
                {
                    addStoreResultLabel.Text = string.Format("Failed adding '{0}' '{1}'store in '{2}' region with '{3}' status.\n\rPossibly the business unit number is not setup in VIM.", storeAbbrev, name, regionAbbrev, status);
                    return;
                }
                addStoreResultLabel.Text = string.Format("Done adding '{0}' '{1}'store in '{2}' region with '{3}' status.", storeAbbrev, name, regionAbbrev, status);

            }
            catch(Exception failure)
            {
                addStoreResultLabel.Text = "Error while processing:\n\r" + failure.Message;
            }

        }

        private bool ValidateInput()
        {
            if (addStoreInRegionComboBox.SelectedIndex <= 0)
            {
                addStoreResultLabel.Text = "Region is not selected.";
                return false;
            }
            if (storeStatusComboBox.SelectedIndex <= 0)
            {
                addStoreResultLabel.Text = "Store status is not selected.";
                return false;
            }
            if (string.IsNullOrWhiteSpace(storeAbbreviationTextBox.Text.Trim()))
            {
                addStoreResultLabel.Text = "Store abbreviation is empty or is white space.";
                return false;
            }
            if (storeAbbreviationTextBox.Text.Trim().Length != 3)
            {
                addStoreResultLabel.Text = "Store is not 3 characters long.";
                return false;
            }
            if (string.IsNullOrWhiteSpace(storeNameTextBox.Text.Trim()))
            {
                addStoreResultLabel.Text = "Store name is empty or is white space";
                return false;
            }
            return true;
        }

        private bool StoreExistsInRegion(string store, string region)
        {
            try
            {
                storeValidator.ValidateStoreInRegion(region, store);
                addStoreResultLabel.Text = string.Format("Store '{0}' already exists in region '{1}'", store, region);
                return true;
            }
            catch (InvalidStoreException)
            {
                return false;
            }
            catch (Exception e)
            {
                addStoreResultLabel.Text = string.Format("Failed while validating store {0} in region {1}", store, region);
                return true;
            }
        }

        private void AddStore(string storeAbbrev, string name, string regionAbbrev, string status)
        {
            storeRepository.Add(new Store(-1) { Abbrev = storeAbbrev, Name = name, RegionId = regionRepository.ForAbbrev(regionAbbrev).Id, Status = status });
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            addStoreResultLabel.Text = string.Empty;
            storeAbbreviationTextBox.Text = string.Empty;
            storeNameTextBox.Text = string.Empty;
            storeStatusComboBox.SelectedIndex = 0;
            addStoreInRegionComboBox.SelectedIndex = 0;
        }


    }
}
