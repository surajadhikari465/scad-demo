using System.Collections.Generic;
using BulkItemUploadProcessor.Common.Models;
using Icon.Common.DataAccess;

namespace BulkItemUploadProcessor.DataAccess.Queries
{
    public class GetBulkUploadsParameters : IQuery<IEnumerable<BulkItemUploadInformation>>
    {
    }
}