using Icon.Common.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.NewItem.Queries
{
    public class GetHierarchyClassTraitQuery : IQuery<Dictionary<int, string>>
    {
        public int TraitId { get; set; }
    }
}
