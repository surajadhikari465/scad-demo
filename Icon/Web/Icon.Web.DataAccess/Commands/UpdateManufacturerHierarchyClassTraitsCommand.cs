using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Icon.Framework;

namespace Icon.Web.DataAccess.Commands
{
    public class UpdateManufacturerHierarchyClassTraitsCommand
    {
        public HierarchyClass Manufacturer { get; set; }
        public string ZipCode { get; set; }
        public string ArCustomerId { get; set; }
    }
}
