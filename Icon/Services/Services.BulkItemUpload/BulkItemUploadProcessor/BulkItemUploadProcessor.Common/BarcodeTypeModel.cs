namespace BulkItemUploadProcessor.Common
{
    public class BarcodeTypeModel
    {
        public int BarcodeTypeId { get; set; }
        public string BarcodeType { get; set; }
        public string BeginRange { get; set; }
        public string EndRange { get; set; }
        public bool ScalePlu { get; set; }
    }
}