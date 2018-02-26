using Icon.Common.DataAccess;
using Mammoth.Common.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mammoth.Esb.HierarchyClassListener.Queries
{
    public class GetItemsByMerchandiseHierarchyIdParameter : IGetAssociatedItemsParameter
    {
        public IList<int> HierarchyClassIDs { get; set; }
        public IList<int> MerchandiseHierarchyIDs
        {
            get { return HierarchyClassIDs; }
            set { HierarchyClassIDs = value; }
        }
}
}
