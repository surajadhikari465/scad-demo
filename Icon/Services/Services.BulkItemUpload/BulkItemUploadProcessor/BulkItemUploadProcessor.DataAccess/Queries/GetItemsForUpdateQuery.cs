using BulkItemUploadProcessor.Common.Models;
using Icon.Common.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulkItemUploadProcessor.DataAccess.Queries
{
    public class GetItemsForUpdateQuery : IQuery<IEnumerable<UpdateItemModel>>
    {
        public List<string> ScanCodes { get; set; }
    }
}
