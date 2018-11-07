namespace KitBuilderWebApi.QueryParameters
{
    public class ItemsParameters: BaseParameters
    {
        public string SearchProductDescQuery { get; set; }
        public string ProductDesc { get; set; }
       
        public string ScanCode { get; set; }
        public string SearchScanCodeQuery { get; set; }
    }
}