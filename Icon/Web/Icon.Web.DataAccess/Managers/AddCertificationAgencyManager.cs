using Icon.Framework;

namespace Icon.Web.DataAccess.Managers
{
    public class AddCertificationAgencyManager
    {
        public HierarchyClass Agency { get; set; }
        public string GlutenFree { get; set; }
        public string Kosher { get; set; }
        public string NonGMO { get; set; }
        public string Organic { get; set; }
        public string Vegan { get; set; }
    }
}
