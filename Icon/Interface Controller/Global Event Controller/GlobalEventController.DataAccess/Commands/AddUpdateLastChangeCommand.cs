using GlobalEventController.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalEventController.DataAccess.Commands
{
    public class AddUpdateLastChangeCommand
    {
        public IconItemLastChangeModel UpdatedItem { get; set; }
        public int BrandId { get; set; }
        public int TaxClassId { get; set; }
        public int ClassId { get; set; }
        public int PackageUnitId { get; set; }
    }
}
