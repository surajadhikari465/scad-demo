using Irma.Framework;

namespace Irma.Testing.Builders
{
    public class TestIrmaStoreBuilder
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
        private System.Collections.Generic.ICollection<Irma.Framework.IConPOSPushPublish> iConPOSPushPublish;
        private System.Collections.Generic.ICollection<Irma.Framework.Price> price;
        private System.Collections.Generic.ICollection<Irma.Framework.Sales_SumByItem> sales_SumByItem;
        private Irma.Framework.Users users;
        private System.Collections.Generic.ICollection<Irma.Framework.MammothItemLocaleChangeQueue> itemLocaleChangeQueue;
        private System.Collections.Generic.ICollection<Irma.Framework.StoreSubTeam> storeSubTeam;
        private System.Collections.Generic.ICollection<Irma.Framework.Users> users1;
        private System.Collections.Generic.ICollection<Irma.Framework.Users> users2;
        private System.Collections.Generic.ICollection<Irma.Framework.MammothPriceChangeQueue> priceChangeQueue;

        public TestIrmaStoreBuilder()
        {
            this.store_No = 0;
            this.store_Name = null;
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
            this.iConPOSPushPublish = null;
            this.price = null;
            this.sales_SumByItem = null;
            this.users = null;
            this.itemLocaleChangeQueue = null;
            this.storeSubTeam = null;
            this.users1 = null;
            this.users2 = null;
            this.priceChangeQueue = null;
        }

        public TestIrmaStoreBuilder WithStore_No(int store_No)
        {
            this.store_No = store_No;
            return this;
        }

        public TestIrmaStoreBuilder WithStore_Name(string store_Name)
        {
            this.store_Name = store_Name;
            return this;
        }

        public TestIrmaStoreBuilder WithPhone_Number(string phone_Number)
        {
            this.phone_Number = phone_Number;
            return this;
        }

        public TestIrmaStoreBuilder WithMega_Store(bool mega_Store)
        {
            this.mega_Store = mega_Store;
            return this;
        }

        public TestIrmaStoreBuilder WithDistribution_Center(bool distribution_Center)
        {
            this.distribution_Center = distribution_Center;
            return this;
        }

        public TestIrmaStoreBuilder WithManufacturer(bool manufacturer)
        {
            this.manufacturer = manufacturer;
            return this;
        }

        public TestIrmaStoreBuilder WithWFM_Store(bool wFM_Store)
        {
            this.wFM_Store = wFM_Store;
            return this;
        }

        public TestIrmaStoreBuilder WithInternal(bool internalStore)
        {

            this._internal = internalStore;
            return this;

        }

        public TestIrmaStoreBuilder WithTelnetUser(string telnetUser)
        {
            this.telnetUser = telnetUser;
            return this;
        }

        public TestIrmaStoreBuilder WithTelnetPassword(string telnetPassword)
        {
            this.telnetPassword = telnetPassword;
            return this;
        }

        public TestIrmaStoreBuilder WithBatchID(int batchID)
        {
            this.batchID = batchID;
            return this;
        }

        public TestIrmaStoreBuilder WithBatchRecords(int batchRecords)
        {
            this.batchRecords = batchRecords;
            return this;
        }

        public TestIrmaStoreBuilder WithBusinessUnit_ID(System.Nullable<int> businessUnit_ID)
        {
            this.businessUnit_ID = businessUnit_ID;
            return this;
        }

        public TestIrmaStoreBuilder WithZone_ID(System.Nullable<int> zone_ID)
        {
            this.zone_ID = zone_ID;
            return this;
        }

        public TestIrmaStoreBuilder WithUNFI_Store(string uNFI_Store)
        {
            this.uNFI_Store = uNFI_Store;
            return this;
        }

        public TestIrmaStoreBuilder WithLastRecvLogDate(System.Nullable<System.DateTime> lastRecvLogDate)
        {
            this.lastRecvLogDate = lastRecvLogDate;
            return this;
        }

        public TestIrmaStoreBuilder WithLastRecvLog_No(System.Nullable<int> lastRecvLog_No)
        {
            this.lastRecvLog_No = lastRecvLog_No;
            return this;
        }

        public TestIrmaStoreBuilder WithRecvLogUser_ID(System.Nullable<int> recvLogUser_ID)
        {
            this.recvLogUser_ID = recvLogUser_ID;
            return this;
        }

        public TestIrmaStoreBuilder WithEXEWarehouse(System.Nullable<short> eXEWarehouse)
        {
            this.eXEWarehouse = eXEWarehouse;
            return this;
        }

        public TestIrmaStoreBuilder WithRegional(bool regional)
        {
            this.regional = regional;
            return this;
        }

        public TestIrmaStoreBuilder WithLastSalesUpdateDate(System.Nullable<System.DateTime> lastSalesUpdateDate)
        {
            this.lastSalesUpdateDate = lastSalesUpdateDate;
            return this;
        }

        public TestIrmaStoreBuilder WithStoreAbbr(string storeAbbr)
        {
            this.storeAbbr = storeAbbr;
            return this;
        }

        public TestIrmaStoreBuilder WithPLUMStoreNo(System.Nullable<int> pLUMStoreNo)
        {
            this.pLUMStoreNo = pLUMStoreNo;
            return this;
        }

        public TestIrmaStoreBuilder WithTaxJurisdictionID(System.Nullable<int> taxJurisdictionID)
        {
            this.taxJurisdictionID = taxJurisdictionID;
            return this;
        }

        public TestIrmaStoreBuilder WithPOSSystemId(System.Nullable<int> pOSSystemId)
        {
            this.pOSSystemId = pOSSystemId;
            return this;
        }

        public TestIrmaStoreBuilder WithPSI_Store_No(System.Nullable<int> pSI_Store_No)
        {
            this.pSI_Store_No = pSI_Store_No;
            return this;
        }

        public TestIrmaStoreBuilder WithStoreJurisdictionID(System.Nullable<int> storeJurisdictionID)
        {
            this.storeJurisdictionID = storeJurisdictionID;
            return this;
        }

        public TestIrmaStoreBuilder WithUseAvgCostHistory(System.Nullable<bool> useAvgCostHistory)
        {
            this.useAvgCostHistory = useAvgCostHistory;
            return this;
        }

        public TestIrmaStoreBuilder WithGeoCode(string geoCode)
        {
            this.geoCode = geoCode;
            return this;
        }

        public TestIrmaStoreBuilder WithIConPOSPushPublish(System.Collections.Generic.ICollection<Irma.Framework.IConPOSPushPublish> iConPOSPushPublish)
        {
            this.iConPOSPushPublish = iConPOSPushPublish;
            return this;
        }

        public TestIrmaStoreBuilder WithPrice(System.Collections.Generic.ICollection<Irma.Framework.Price> price)
        {
            this.price = price;
            return this;
        }

        public TestIrmaStoreBuilder WithSales_SumByItem(System.Collections.Generic.ICollection<Irma.Framework.Sales_SumByItem> sales_SumByItem)
        {
            this.sales_SumByItem = sales_SumByItem;
            return this;
        }

        public TestIrmaStoreBuilder WithUsers(Irma.Framework.Users users)
        {
            this.users = users;
            return this;
        }

        public TestIrmaStoreBuilder WithItemLocaleChangeQueue(System.Collections.Generic.ICollection<Irma.Framework.MammothItemLocaleChangeQueue> itemLocaleChangeQueue)
        {
            this.itemLocaleChangeQueue = itemLocaleChangeQueue;
            return this;
        }

        public TestIrmaStoreBuilder WithStoreSubTeam(System.Collections.Generic.ICollection<Irma.Framework.StoreSubTeam> storeSubTeam)
        {
            this.storeSubTeam = storeSubTeam;
            return this;
        }

        public TestIrmaStoreBuilder WithUsers1(System.Collections.Generic.ICollection<Irma.Framework.Users> users1)
        {
            this.users1 = users1;
            return this;
        }

        public TestIrmaStoreBuilder WithUsers2(System.Collections.Generic.ICollection<Irma.Framework.Users> users2)
        {
            this.users2 = users2;
            return this;
        }

        public TestIrmaStoreBuilder WithPriceChangeQueue(System.Collections.Generic.ICollection<Irma.Framework.MammothPriceChangeQueue> priceChangeQueue)
        {
            this.priceChangeQueue = priceChangeQueue;
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
            store.IConPOSPushPublish = this.iConPOSPushPublish;
            store.Price = this.price;
            store.Sales_SumByItem = this.sales_SumByItem;
            store.Users = this.users;
            store.ItemLocaleChangeQueue = this.itemLocaleChangeQueue;
            store.StoreSubTeam = this.storeSubTeam;
            store.Users1 = this.users1;
            store.Users2 = this.users2;
            store.PriceChangeQueue = this.priceChangeQueue;
            return store;
        }

        public static implicit operator Store(TestIrmaStoreBuilder builder)
        {
            return builder.Build();
        }
    }
}