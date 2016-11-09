using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalEventController.Common
{
    public class IrmaItemModel
    {
        public int Item_Key { get; set; }
        public string Identifier { get; set; }
        public string Description { get; set; }
        public int SubTeamNo { get; set; }
        public string SubTeamName { get; set; }
        public decimal RetailPack { get; set; }
        public decimal RetailSize { get; set; }
        public string RetailUomAbbreviation { get; set; }
        public string IconRetailUomAbbreviation { get; set; }
        public string RetailUnitAbbreviation { get; set; }
        public bool RetailUomIsWeightedUnit { get; set; }
        public bool IsDefaultIdentifier { get; set; }
    }
}
