using System.Collections.Generic;
using Icon.Common.DataAccess;
using Icon.Web.DataAccess.Models;

namespace Icon.Web.DataAccess.Queries
{
    public class GetBulkItemUploadStatusParameters : IQuery<List<BulkItemUploadStatusModel>> {
     public int RowCount { get; set; }
    }
}