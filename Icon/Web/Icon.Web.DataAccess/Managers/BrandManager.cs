using System;
using Icon.Framework;

namespace Icon.Web.DataAccess.Managers
{
    [Flags]
    public enum UpdateOptions { None = 0, Brand = 1, Traits = 2 }

    public class BrandManager
    {
        public HierarchyClass Brand { get; set; }
        public string BrandAbbreviation { get; set; }
        public string Designation { get; set; }
        public string ParentCompany { get; set; }
        public string ZipCode { get; set; }
        public string Locality { get; set; }
        public UpdateOptions Update { get; set; }
    }
}
