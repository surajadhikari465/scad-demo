using System.Collections.Generic;
using BrandUploadProcessor.Common;
using BrandUploadProcessor.Common.Models;

namespace BrandUploadProcessor.Service.Validation
{
    public class RowObjectValidatorResponse
    {
        public List<RowObject> ValidRows { get; set; } = new List<RowObject>();
        public List<InvalidRowError> InvalidRows { get; set; } = new List<InvalidRowError>();
    }
}