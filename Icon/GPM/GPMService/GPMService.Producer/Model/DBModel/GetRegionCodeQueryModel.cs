namespace GPMService.Producer.Model.DBModel
{
    internal class GetRegionCodeQueryModel
    {
        public string Region { get; set; }
        public int BusinessUnitID { get; set; }
        public int LocaleID { get; set; }
        public string StoreName { get; set; }
        public string StoreAbbrev { get; set; }
    }
}
