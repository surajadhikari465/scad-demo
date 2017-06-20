using Icon.Common.DataAccess;
using Mammoth.Common.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mammoth.Esb.HierarchyClassListener.Queries
{
    public class GetItemsByBrandIdQuery : IQuery<IEnumerable<Item>>
    {
        public List<int> BrandIds { get; set; }
    }
}
