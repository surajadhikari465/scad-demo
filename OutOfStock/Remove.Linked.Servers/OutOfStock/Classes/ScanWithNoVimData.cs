namespace OutOfStock.Classes
{
    public class ScanWithNoVimData
    {

        public int ReportHeaderId { get; set; }
        public string BusinessUnit { get; set; }
        public string StoreName { get; set; }
        public string StoreAbbreviation { get; set; }
        public int StoreId { get; set; }
        public string UPC { get; set; }
        public int scanCount { get; set; }


        public ScanWithNoVimData(int scanCount, string upc, int storeId, string storeAbbreviation, string storeName, int reportHeaderId, string businessUnit)
        {
            this.scanCount = scanCount;
            UPC = upc;
            StoreId = storeId;
            StoreAbbreviation = storeAbbreviation;
            StoreName = storeName;
            ReportHeaderId = reportHeaderId;
            BusinessUnit = businessUnit;
        }



        public ScanWithNoVimData() {}
        
    }
}