using System;

namespace OutOfStock.Classes
{
    public class ReportHeaderMetaData
    {
        public int Id { get; set; }
        public int Store_Id { get; set; }
        public string StoreName { get; set; }
        public string StoreAbbreviation { get; set; }
        public DateTime Created_Date { get; set; }
        public DateTime Last_Updated_Date { get; set; }
    }
}