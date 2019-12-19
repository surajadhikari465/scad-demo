using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulkItemUploadProcessor.Common
{
    public class InvalidRowError
    {
        public int RowId { get; set; }
        public string Error { get; set; }
    }
}
