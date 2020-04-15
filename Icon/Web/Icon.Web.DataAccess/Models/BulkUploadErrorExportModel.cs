using System.Collections.Generic;

namespace Icon.Web.DataAccess.Models
{
    public class BulkUploadErrorExportModel
    {
        public BulkUploadModel BulkUploadModel { get; set; }
        public List<BulkUploadErrorModel> bulkUploadErrorModels { get; set; }
    }
}
