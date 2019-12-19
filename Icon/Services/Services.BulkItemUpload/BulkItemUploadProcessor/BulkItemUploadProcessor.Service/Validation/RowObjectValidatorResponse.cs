using BulkItemUploadProcessor.Common;
using System.Collections.Generic;

namespace BulkItemUploadProcessor.Service.Validation
{
    public class RowObjectValidatorResponse
    {
        public List<RowObject> ValidRows { get; set; } = new List<RowObject>();
        public List<InvalidRowError> InvalidRows { get; set; } = new List<InvalidRowError>();
    }
}
