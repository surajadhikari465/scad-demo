using Icon.Framework;

namespace Icon.Web.DataAccess.Managers
{
    public class UpdateBrandManager
    {
        public HierarchyClass Brand { get; set; }
        public string BrandAbbreviation { get; set; }
        public string Designation { get; set; }
        public string ParentCompany { get; set; }
        public string ZipCode { get; set; }
        public string Locality { get; set; }
    }
}
