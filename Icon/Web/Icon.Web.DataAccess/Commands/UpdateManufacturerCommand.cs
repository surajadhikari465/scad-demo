using Icon.Framework;

namespace Icon.Web.DataAccess.Commands
{
    public class UpdateManufacturerCommand
    {
        public HierarchyClass Manufacturer { get; set; }
        public string ZipCode { get; set; }
        public string ArCustomerId { get; set; }
    }
}
