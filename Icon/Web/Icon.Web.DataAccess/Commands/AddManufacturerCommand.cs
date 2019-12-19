using Icon.Framework;

namespace Icon.Web.DataAccess.Commands
{
    public class AddManufacturerCommand
    {
        public HierarchyClass Manufacturer { get; set; }
        public string ZipCode { get; set; }
        public string ArCustomerId { get; set; }
    }
}
