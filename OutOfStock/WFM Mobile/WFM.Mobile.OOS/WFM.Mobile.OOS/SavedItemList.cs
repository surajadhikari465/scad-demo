using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace WFM.Mobile.OOS
{
    public  class SavedItemList
    {
        public SavedItemList()
        {
            Items = new List<SavedItem>();
        }

        public DateTime TimeStamp { get; set; }
        public List<SavedItem> Items { get; set; }
    }
}
