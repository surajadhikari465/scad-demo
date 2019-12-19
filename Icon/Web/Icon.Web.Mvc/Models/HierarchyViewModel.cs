using Icon.Web.DataAccess.Models;

namespace Icon.Web.Mvc.Models
{
    public class HierarchyViewModel
    {
        public int HierarchyId { get; set; }
        public string HierarchyName { get; set; }

        public HierarchyViewModel(HierarchyModel model)
        {
            this.HierarchyId = model.HierarchyId;
            this.HierarchyName = model.HierarchyName;
        }
    }
}