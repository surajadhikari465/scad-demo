using System.Collections.Generic;

namespace BrandUploadProcessor.DataAccess.Commands
{
    public class PublishBrandUpdatesCommand
    {
        public List<int> BrandIds { get; set; }
        public List<string> Regions { get; set; }
    }
}