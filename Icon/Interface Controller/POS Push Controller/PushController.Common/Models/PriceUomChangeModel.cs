using Icon.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PushController.Common.Models
{
    public class PriceUomChangeModel : IRMAPush
    {
        public string CurrentPosUom { get; set; }
        public string NewPosUom { get; set; }

        public PriceUomChangeModel()
        {

        }

        public PriceUomChangeModel(IRMAPush irmaPush)
        {
            base.IRMAPushID = irmaPush.IRMAPushID;
            base.Identifier = irmaPush.Identifier;
            base.RegionCode = irmaPush.RegionCode;
            base.BusinessUnit_ID = irmaPush.BusinessUnit_ID;
            base.Price = irmaPush.Price;
            base.Multiple = irmaPush.Multiple;
            base.Sold_By_Weight = irmaPush.Sold_By_Weight;
            NewPosUom = base.Sold_By_Weight ? "LB" : "EA";
        }
    }
}
