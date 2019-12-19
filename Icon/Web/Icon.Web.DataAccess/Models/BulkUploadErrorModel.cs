namespace Icon.Web.DataAccess.Models
{
    public class BulkUploadErrorModel
    {
        public int BulkItemUploadErrorId { get; set; }
        public int RowId { get; set; }
        public string Message { get; set; }
    }
}