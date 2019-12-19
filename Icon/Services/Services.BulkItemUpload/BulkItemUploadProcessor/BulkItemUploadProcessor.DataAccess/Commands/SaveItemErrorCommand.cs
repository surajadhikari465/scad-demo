using System.Collections.Generic;

namespace BulkItemUploadProcessor.DataAccess.Commands
{
    public class SaveErrorsCommand
    {
        public int BulkItemUploadId { get; set; }
        public int RowId { get; set; }
        public List<string> ErrorList { get; set; }
    }
}