namespace BulkItemUploadProcessor.DataAccess.Commands
{
    public class SetTotalRecordCountCommand
    {
        public int BulkUploadId { get; set; }
        public int TotalRecordCount { get; set; }
    }
}