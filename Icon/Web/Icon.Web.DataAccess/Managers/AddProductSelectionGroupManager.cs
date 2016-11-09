using Icon.Web.DataAccess.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icon.Web.DataAccess.Managers
{
    public class AddProductSelectionGroupManager
    {
        public string ProductSelectionGroupName { get; set; }
        public int ProductSelectionGroupTypeId { get; set; }
        public int? TraitId { get; set; }
        public string TraitValue { get; set; }
        public int? MerchandiseHierarchyClassId { get; set; }
    }
}
