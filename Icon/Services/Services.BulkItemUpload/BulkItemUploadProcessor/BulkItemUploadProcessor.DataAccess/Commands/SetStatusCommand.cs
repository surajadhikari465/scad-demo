using BulkItemUploadProcessor.Common;

namespace BulkItemUploadProcessor.DataAccess.Commands
{
    public class SetStatusCommand
    {
        public int BulkItemUploadId { get; set; }
        public Enums.FileStatusEnum FileStatus { get; set; }

        public string Message { get; set; }
        public int PercentageProcessed { get; set; }
    }
}