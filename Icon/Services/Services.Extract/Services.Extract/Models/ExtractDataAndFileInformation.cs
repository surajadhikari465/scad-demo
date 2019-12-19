using System.Collections.Generic;
using System.IO;

namespace Services.Extract.Models
{
    public class ExtractDataAndFileInformation
    {
        public IEnumerable<dynamic> Data { get; set; }
        public FileInfo FileInformation { get; set; }
    }
}