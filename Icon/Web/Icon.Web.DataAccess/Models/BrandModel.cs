namespace Icon.Web.DataAccess.Models
{
    public class BrandModel : HierarchyClassModel
    {
        public string BrandAbbreviation { get; set; }
        public string Designation { get; set; }
        public string Locality { get; set; }
        public string ZipCode { get; set; }
        public string ParentCompany { get; set; }
    }
}