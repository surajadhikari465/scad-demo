namespace BulkItemUploadProcessor.DataAccess.Commands
{
    public class SetCurrentRecordCommand
    {
        public int BulkUploadId { get; set; }
        public int CurrentRecord { get; set; }
    }
}