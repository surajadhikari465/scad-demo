namespace BulkItemUploadProcessor.DataAccess.Commands
{
    public class SetCurrentRecordCommand
    {
        public int BulkItemUploadId { get; set; }
        public int CurrentRecord { get; set; }
    }
}