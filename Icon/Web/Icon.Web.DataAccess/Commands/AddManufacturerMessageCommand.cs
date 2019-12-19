using Icon.Framework;

namespace Icon.Web.DataAccess.Commands
{
    public class AddManufacturerMessageCommand
    {
        public HierarchyClass Manufacturer { get; set; }
        public int Action { get; set; }
        public string ZipCode { get; set; }
        public string ArCustomerId { get; set; }
    }
}
