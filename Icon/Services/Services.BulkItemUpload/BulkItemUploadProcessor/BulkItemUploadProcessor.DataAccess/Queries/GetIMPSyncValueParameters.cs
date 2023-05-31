using System;
using Icon.Common.DataAccess;


namespace BulkItemUploadProcessor.DataAccess.Queries
{
    public class GetIMPSyncValueParameters : IQuery<string>
    {
        public string ScanCode { get; set; }
    }
}
