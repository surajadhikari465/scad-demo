using Icon.Common.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infor.Services.NewItem.Queries
{
    public class GetHierarchyClassCodesToHierarchyClassIdsDictionaryQuery : IQuery<Dictionary<string, int>>
    {
        public int HierarchyId { get; set; }
    }
}
