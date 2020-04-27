using System.Collections.Generic;

namespace BrandUploadProcessor.Common.Models
{
    public class RowObjectDictionary
    {
        public int Row { get; set; }
        public Dictionary<int, string> Cells { get; set; }
        public RowObject RowObject { get; set; }
    }
}