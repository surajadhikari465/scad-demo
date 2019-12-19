namespace Icon.Web.DataAccess.Models
{
    public class BulkItemUploadModel
    {
        public string FileName { get; set; }
        public int FileModeType { get; set; }
        public byte[] FileContent { get; set; }
        public string UploadedBy { get; set; }

    }
}