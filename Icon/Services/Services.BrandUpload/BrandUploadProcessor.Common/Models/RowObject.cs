using System.Collections.Generic;

namespace BrandUploadProcessor.Common.Models
{
    public class RowObject
    {
        public int Row { get; set; }
        public List<ParsedCell> Cells { get; set; }
    }
}