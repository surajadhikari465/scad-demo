using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;


namespace Icon.Esb.ItemMovementListener.Models
{
    public class LineItemTypeDataModel
    {        
        public string ItemId { get; set; }
        
        public string Quantity { get; set; }
      
        public string Units { get; set; }
     
        public string RegularSalesUnitPrice { get; set; }

        public string ExtendedDiscountAmount { get; set; }
    }
}

