namespace BulkItemUploadProcessor.DataAccess.Commands
{
    public class SetTotalRecordCountCommand
    {
        public int BulkItemUploadId { get; set; }
        public int TotalRecordCount { get; set; }
    }
}