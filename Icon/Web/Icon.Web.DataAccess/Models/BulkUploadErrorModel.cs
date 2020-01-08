namespace Icon.Web.DataAccess.Models
{
    public class BulkUploadErrorModel
    {
        public int BulkUploadErrorId { get; set; }
        public int RowId { get; set; }
        public string Message { get; set; }
    }
}