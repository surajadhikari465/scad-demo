using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icon.Infor.Listeners.Item.Models
{
    public class ItemHierarchyClassModel
    {
        public int ItemId { get; set; }
        public int HierarchyId { get; set; }
        public string HierarchyClassId { get; set; }

        public ItemHierarchyClassModel(int itemId, int hierarchyId, string hierarchyClassId)
        {
            this.ItemId = itemId;
            this.HierarchyId = hierarchyId;
            this.HierarchyClassId = hierarchyClassId;
        }
    }
}
