using Icon.Common.DataAccess;
using Mammoth.Common.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mammoth.Esb.HierarchyClassListener.Queries
{
    public class GetItemsByBrandIdParameter : IGetAssociatedItemsParameter//, IQuery<IEnumerable<int>>
    {
        public IList<int> HierarchyClassIDs { get; set; }
        public IList<int> BrandIds
        {
            get { return HierarchyClassIDs; }
            set { HierarchyClassIDs = value; }
        }
    }
}
