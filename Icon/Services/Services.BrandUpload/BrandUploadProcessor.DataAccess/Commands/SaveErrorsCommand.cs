using System.Collections.Generic;

namespace BrandUploadProcessor.DataAccess.Commands
{
    public class SaveErrorsCommand
    {
        public int BulkUploadId { get; set; }
        public int RowId { get; set; }
        public List<string> ErrorList { get; set; }
    }
}