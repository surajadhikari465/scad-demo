using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace WFM.Mobile.OOS
{
    public class SavedItem
    {
        public SavedItem(string upc)
        {
            UPC = upc;
        }

        public string UPC { get; set; }
    }
}
