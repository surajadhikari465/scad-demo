using System;
using Irma.Framework;

namespace Irma.Testing.Builders
{
    public class TestStoreBuilder
    {
        private int store_No;
        private string store_Name;
        private string phone_Number;
        private bool mega_Store;
        private bool distribution_Center;
        private bool manufacturer;
        private bool wFM_Store;
        private bool _internal;
        private string telnetUser;
        private string telnetPassword;
        private int batchID;
        private int batchRecords;
        private System.Nullable<int> businessUnit_ID;
        private System.Nullable<int> zone_ID;
        private string uNFI_Store;
        private System.Nullable<System.DateTime> lastRecvLogDate;
        private System.Nullable<int> lastRecvLog_No;
        private System.Nullable<int> recvLogUser_ID;
        private System.Nullable<short> eXEWarehouse;
        private bool regional;
        private System.Nullable<System.DateTime> lastSalesUpdateDate;
        private string storeAbbr;
        private System.Nullable<int> pLUMStoreNo;
        private System.Nullable<int> taxJurisdictionID;
        private System.Nullable<int> pOSSystemId;
        private System.Nullable<int> pSI_Store_No;
        private System.Nullable<int> storeJurisdictionID;
        private System.Nullable<bool> useAvgCostHistory;
        private string geoCode;

        public TestStoreBuilder()
        {
            this.store_No = 73245732;
            this.store_Name = "Test Store";
            this.phone_Number = null;
            this.mega_Store = false;
            this.distribution_Center = false;
            this.manufacturer = false;
            this.wFM_Store = false;
            this._internal = false;
            this.telnetUser = null;
            this.telnetPassword = null;
            this.batchID = 0;
            this.batchRecords = 0;
            this.businessUnit_ID = null;
            this.zone_ID = null;
            this.uNFI_Store = null;
            this.lastRecvLogDate = null;
            this.lastRecvLog_No = null;
            this.recvLogUser_ID = null;
            this.eXEWarehouse = null;
            this.regional = false;
            this.lastSalesUpdateDate = null;
            this.storeAbbr = null;
            this.pLUMStoreNo = null;
            this.taxJurisdictionID = null;
            this.pOSSystemId = null;
            this.pSI_Store_No = null;
            this.storeJurisdictionID = null;
            this.useAvgCostHistory = null;
            this.geoCode = null;
        }

        public TestStoreBuilder WithStore_No(int store_No)
        {
            this.store_No = store_No;
            return this;
        }

        public TestStoreBuilder WithBusinessUnitId(int businessUnitId)
        {
            this.businessUnit_ID = businessUnitId;
            return this;
        }

        public TestStoreBuilder WithStoreJurisdictionId(int id)
        {
            this.storeJurisdictionID = id;
            return this;
        }

        public TestStoreBuilder WithStore_Name(string store_Name)
        {
            this.store_Name = store_Name;
            return this;
        }

        public TestStoreBuilder WithPhone_Number(string phone_Number)
        {
            this.phone_Number = phone_Number;
            return this;
        }

        public TestStoreBuilder WithMega_Store(bool mega_Store)
        {
            this.mega_Store = mega_Store;
            return this;
        }

        public TestStoreBuilder WithDistribution_Center(bool distribution_Center)
        {
            this.distribution_Center = distribution_Center;
            return this;
        }

        public TestStoreBuilder WithManufacturer(bool manufacturer)
        {
            this.manufacturer = manufacturer;
            return this;
        }

        public TestStoreBuilder WithWFM_Store(bool wFM_Store)
        {
            this.wFM_Store = wFM_Store;
            return this;
        }

        public Store Build()
        {
            Store store = new Store();

            store.Store_No = this.store_No;
            store.Store_Name = this.store_Name;
            store.Phone_Number = this.phone_Number;
            store.Mega_Store = this.mega_Store;
            store.Distribution_Center = this.distribution_Center;
            store.Manufacturer = this.manufacturer;
            store.WFM_Store = this.wFM_Store;
            store.Internal = this._internal;
            store.TelnetUser = this.telnetUser;
            store.TelnetPassword = this.telnetPassword;

            store.BatchID = this.batchID;

            store.BatchRecords = this.batchRecords;

            store.BusinessUnit_ID = this.businessUnit_ID;

            store.Zone_ID = this.zone_ID;

            store.UNFI_Store = this.uNFI_Store;

            store.LastRecvLogDate = this.lastRecvLogDate;

            store.LastRecvLog_No = this.lastRecvLog_No;

            store.RecvLogUser_ID = this.recvLogUser_ID;

            store.EXEWarehouse = this.eXEWarehouse;

            store.Regional = this.regional;

            store.LastSalesUpdateDate = this.lastSalesUpdateDate;

            store.StoreAbbr = this.storeAbbr;

            store.PLUMStoreNo = this.pLUMStoreNo;

            store.TaxJurisdictionID = this.taxJurisdictionID;

            store.POSSystemId = this.pOSSystemId;

            store.PSI_Store_No = this.pSI_Store_No;

            store.StoreJurisdictionID = this.storeJurisdictionID;

            store.UseAvgCostHistory = this.useAvgCostHistory;

            store.GeoCode = this.geoCode;

            return store;
        }

        public static implicit operator Store(TestStoreBuilder builder)
        {
            return builder.Build();
        }
    }
}