namespace MammothWebApi.DataAccess.Models.DataMonster
{
    public class ItemLocaleInformation
    {
        public string Region { get; set; }
        public int BusinessUnitId { get; set; }
        public int ItemId { get; set; }
        public bool? Authorized { get; set; }
        public string Sign_Desc { get; set; }
        public string SupplierName { get; set; }

    }
}