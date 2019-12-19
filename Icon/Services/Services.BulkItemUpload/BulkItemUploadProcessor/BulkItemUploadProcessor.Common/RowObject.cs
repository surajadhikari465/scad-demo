using System.Collections.Generic;
using BulkItemUploadProcessor.Common.Models;

namespace BulkItemUploadProcessor.Common
{
    public class RowObject
    {
        public int Row { get; set; }
        public List<ParsedCell> Cells { get; set; }
    }
}