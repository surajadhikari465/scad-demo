namespace OutOfStock.Controllers
{
    public class StoreUpdateData
    {
        public string StoreAbbr { get; set; }
        public string StoreName { get; set; }
        public string StoreNumber { get; set; }
        public int RegionId { get; set; }
        public int StoreId { get; set; }
        public bool Hidden { get; set; }
        public string View { get; set; }
    }
}