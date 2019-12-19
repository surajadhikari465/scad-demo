using Icon.Common.DataAccess;
using System.Collections.Generic;

namespace BulkItemUploadProcessor.DataAccess.Queries
{
    public class GetScanCodesThatExistParameters : IQuery<HashSet<string>>
    {
        public List<string> ScanCodes { get; set; }
    }
}