using Icon.Framework;

namespace Icon.Web.DataAccess.Managers
{
    public class UpdateCertificationAgencyManager
    {
        public HierarchyClass Agency { get; set; }
        public string BrandAbbreviation { get; set; }
        public string GlutenFree { get; set; }
        public string Kosher { get; set; }
        public string NonGMO { get; set; }
        public string Organic { get; set; }
        public string Vegan { get; set; }
        public string DefaultOrganic { get; set; }
    }
}
