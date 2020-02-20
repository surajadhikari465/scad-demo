using System.Collections.Generic;

namespace Icon.Web.DataAccess.Models
{
   public class BulkItemUploadErrorExportModel
    {
        public BulkItemUploadModel bulkItemUploadModel { get; set; }
        public List<BulkUploadErrorModel> bulkUploadErrorModels { get; set; }
    }
}