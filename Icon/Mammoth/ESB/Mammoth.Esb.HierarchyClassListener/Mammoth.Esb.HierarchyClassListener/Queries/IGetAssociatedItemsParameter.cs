using Icon.Common.DataAccess;
using Mammoth.Common.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mammoth.Esb.HierarchyClassListener.Queries
{
    public interface IGetAssociatedItemsParameter : IQuery<IEnumerable<Item>>
    {
        IList<int> HierarchyClassIDs { get; set; }
    }
}
