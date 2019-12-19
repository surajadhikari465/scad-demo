using System.Collections.Generic;
using Icon.Common.DataAccess;

namespace BulkItemUploadProcessor.DataAccess.Commands
{
    public class PublishItemUpdatesCommand
    {
        public List<string> ScanCodes { get; set; }
    }
}