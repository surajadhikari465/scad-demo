using Icon.Common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace KitBuilder.ESB.Listeners.Item.Service
{
    public class ItemListenerSettings
    {
        public bool ValidateSequenceId { get; set; }
        public IList<int> ValidScanCodeEnumeration { get; set; }
        

        public static ItemListenerSettings CreateFromConfig()
        {
            return new ItemListenerSettings
            {
            };
        }

        
    }
}