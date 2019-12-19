using System.Collections.Generic;
using Icon.Common.DataAccess;
using Icon.Web.DataAccess.Models;

namespace Icon.Web.DataAccess.Queries
{
    public class GetBulkUploadErrorsPrameters : IQuery<List<BulkUploadErrorModel>>
    {
        public int BulkItemUploadId { get; set; }
    }
}