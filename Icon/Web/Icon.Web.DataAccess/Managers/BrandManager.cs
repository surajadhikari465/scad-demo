using Icon.Framework;
using Icon.Web.Common;

namespace Icon.Web.DataAccess.Managers
{
    public class BrandManager
    {
        public HierarchyClass Brand { get; set; }
        public string BrandAbbreviation { get; set; }
        public string Designation { get; set; }
        public string ParentCompany { get; set; }
        public string ZipCode { get; set; }
        public string Locality { get; set; }
        public Enums.WriteAccess WriteAccess { get; set; }
    }
}
