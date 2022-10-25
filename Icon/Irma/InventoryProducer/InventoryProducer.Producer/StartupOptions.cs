using InventoryProducer.Common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace InventoryProducer.Producer
{
    public class StartupOptions
    {
        public static HashSet<string> ValidArgs { get {
                var inventoryTypes = from d in typeof(Constants.ProducerType).GetFields()
                                           select Convert.ToString(d.GetRawConstantValue());
                return new HashSet<string>(inventoryTypes, StringComparer.InvariantCultureIgnoreCase);
            } }
    }
}
