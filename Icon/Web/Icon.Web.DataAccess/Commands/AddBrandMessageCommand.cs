using Icon.Framework;

namespace Icon.Web.DataAccess.Commands
{
    public class AddBrandMessageCommand
    {
        public HierarchyClass Brand { get; set; }
        public int Action { get; set; }
        public string BrandAbbreviation { get; set; }
        public string Designation { get; set; }
        public string ZipCode { get; set; }
        public string Locality { get; set; }
    }
}