using System.Collections.Generic;
using Icon.Common.DataAccess;
using Icon.Web.Common.BulkUpload;
using Icon.Web.DataAccess.Models;

namespace Icon.Web.DataAccess.Queries
{
    public class GetBulkUploadStatusParameters : IQuery<List<BulkUploadStatusModel>> 
    {
        public int RowCount { get; set; }
        public BulkUploadDataType BulkUploadDataType { get; set; }
    }
}