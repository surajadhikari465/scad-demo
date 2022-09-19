using System;
using System.Collections.Generic;

namespace InventoryProducer.Producer
{
    public class StartupOptions
    {
        public static HashSet<string> ValidArgs = new HashSet<string>(StringComparer.InvariantCultureIgnoreCase)
        {
            "spoilage",
            "transfer"
        };
    }
}
