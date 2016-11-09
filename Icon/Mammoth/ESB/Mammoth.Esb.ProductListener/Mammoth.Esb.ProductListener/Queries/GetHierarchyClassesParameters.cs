using Mammoth.Common.DataAccess.CommandQuery;
using Mammoth.Esb.ProductListener.Models;
using System.Collections.Generic;

namespace Mammoth.Esb.ProductListener.Queries
{
    public class GetHierarchyClassesParameters : IQuery<List<HierarchyClassModel>>
    {
        public int HierarchyId { get; set; }
    }
}
