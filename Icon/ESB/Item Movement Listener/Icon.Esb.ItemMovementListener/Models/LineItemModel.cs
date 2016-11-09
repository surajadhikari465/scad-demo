using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Icon.Esb.ItemMovementListener.Models
{
    public class LineItemModel
    {
        [XmlAttribute]
        public bool VoidFlag { get; set; }

        public string SequenceNumber { get; set; }

        public LineItemTypeDataModel Sale_LineItem { get; set; }

        public LineItemTypeDataModel Return_LineItem { get; set; }

        public string SubTeam { get; set; }
       
        public string Taxed { get; set; }
        
        public string TaxTable { get; set; }       
        public string FoodStamp { get; set; }
        
        public string ScanType { get; set; }
      
        public string EndDateTime { get; set; }
    }
}
