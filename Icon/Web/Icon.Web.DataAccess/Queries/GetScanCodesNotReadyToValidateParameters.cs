using Icon.Common.DataAccess;
using Icon.Web.DataAccess.Infrastructure;
using Icon.Web.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icon.Web.DataAccess.Queries
{
    public class GetScanCodesNotReadyToValidateParameters : IQuery<List<string>>
    {
        public List<BulkImportItemModel> Items { get; set; }
    }
}
