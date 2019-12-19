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
    public partial class StoreManagementTasks : Form
    {
        private IStoreSkuCountReader skuCountReader;
        private IRegionRepository regionRepository;
        private IStoreRepository storeRepository;
        private IOOSEntitiesFactory entitiesFactory;
        private IStoreValidator storeValidator;
        private ISkuCountRepository skuCountRepository;

        public StoreManagementTasks(IStoreSkuCountReader skuCountReader, 
                                    IRegionRepository regionRepository, 
                                    IStoreRepository storeRepository, 
                                    IOOSEntitiesFactory entitiesFactory,
                                    IStoreValidator storeValidator,
                                    ISkuCountRepository skuCountRepository)
        {
            this.skuCountReader = skuCountReader;
            this.regionRepository = regionRepository;
            this.storeRepository = storeRepository;
            this.entitiesFactory = entitiesFactory;
            this.storeValidator = storeValidator;
            this.skuCountRepository = skuCountRepository;

            InitializeComponent();
        }

        private void StoreManagement_Load(object sender, EventArgs e)
        {

        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            var view = new StoreSkuCountView(this, skuCountReader, regionRepository);
            view.Show();
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            var view = new InsertStoreSkuCountView(this, regionRepository, skuCountReader, skuCountRepository);
            view.Show();
        }

        private void addStoreLinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            var view = new AddStoreView(this, storeRepository, regionRepository, entitiesFactory, storeValidator);
            view.Show();
        }

        private void changeStoreStatusLinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            var view = new UpdateStoreView(this, regionRepository, storeRepository, entitiesFactory);
            view.Show();
        }

        private void linkLabel3_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            var view = new UpdateStoreSkuCountView(this, regionRepository, skuCountRepository, skuCountReader);
            view.Show();
        }

        private void addStoreStatus_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            var view = new AddStoreStatusView(this, entitiesFactory);
            view.Show();
        }
    }
}
