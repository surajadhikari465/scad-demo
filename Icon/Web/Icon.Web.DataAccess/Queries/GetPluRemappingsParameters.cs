using Icon.Common.DataAccess;
using Icon.Web.DataAccess.Commands;
using Icon.Web.DataAccess.Infrastructure;
using Icon.Web.DataAccess.Models;
using System.Collections.Generic;

namespace Icon.Web.DataAccess.Queries
{
    public class GetPluRemappingsParameters : IQuery<List<BulkImportPluRemapModel>>
    {
        public List<BulkImportPluModel> ImportedItems { get; set; }
    }
}
