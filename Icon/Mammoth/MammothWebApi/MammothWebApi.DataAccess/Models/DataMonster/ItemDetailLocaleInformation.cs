namespace MammothWebApi.DataAccess.Models.DataMonster
{
    public class ItemDetailLocaleInformation
    {
        public string Region { get; set; }
        public int BusinessUnitId { get; set; }
        public int ItemId { get; set; }
        public bool? Authorized { get; set; }
        public string ExtraText { get; set; }
        public string SignDescription { get; set; }
        public string VendorName { get; set; }
    }
}