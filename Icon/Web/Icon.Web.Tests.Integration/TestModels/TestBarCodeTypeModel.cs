namespace Icon.Web.Tests.Integration.TestModels
{
    public class TestBarcodeTypeModel
    {
        public int BarcodeTypeId { get; set; }
        public string BarcodeType { get; set; }
        public string BeginRange { get; set; }
        public string EndRange { get; set; }
        public bool ScalePLU { get; set; }
    }
}