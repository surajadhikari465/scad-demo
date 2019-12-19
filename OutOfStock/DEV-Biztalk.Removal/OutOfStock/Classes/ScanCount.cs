using System;

namespace OutOfStock.Classes
{
    public class ScanCount
    {
        public int Id { get; set; }
        public DateTime Last_Updated_Date { get; set; }
        public int ItemsScanned { get; set; }
    }
}