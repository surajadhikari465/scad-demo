using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mammoth.ItemLocale.Controller.DataAccess.Tests
{
    public class StoreItemExtended
    {
        public int StoreItemExtendedID { get; set; }
        public int Store_No { get; set; }
        public int Item_Key { get; set; }
        public int? ItemStatusCode { get; set; }
        public bool? OrderedByInfor { get; set; }
    }
}
