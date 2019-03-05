namespace KitBuilderWebApi.Models
{
    public class VenueInfo
    {
        public int? VenueID               { get; set; }
        public int? StoreBU               { get; set; }
        public string VenueDisplayName    { get; set; }
        public int MainItemID             { get; set; }
        public string KitStatus           { get; set; }
        public string MainItemScanCode    { get; set; }
        public string MainItemDescription { get; set; }
    }
}