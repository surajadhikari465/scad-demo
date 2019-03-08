using System.Collections.Generic;
using Icon.Framework;
using InterfaceController.Common;

namespace GlobalEventController.Common
{
    public static class Cache
    {
        public const string InterfaceControllerUserName = "iconcontrolleruser";
        public static Dictionary<string, ScanCode> IdentifierToScanCode = new Dictionary<string, ScanCode>();
        public static Dictionary<string, int> InterfaceControllerUserId = new Dictionary<string, int>
        {
           { Enums.IrmaRegion.FL.ToString(), -1 },
           { Enums.IrmaRegion.MA.ToString(), -1 },
           { Enums.IrmaRegion.MW.ToString(), -1 },
           { Enums.IrmaRegion.NA.ToString(), -1 },
           { Enums.IrmaRegion.NC.ToString(), -1 },
           { Enums.IrmaRegion.NE.ToString(), -1 },
           { Enums.IrmaRegion.PN.ToString(), -1 },
           { Enums.IrmaRegion.RM.ToString(), -1 },
           { Enums.IrmaRegion.SO.ToString(), -1 },
           { Enums.IrmaRegion.SP.ToString(), -1 },
           { Enums.IrmaRegion.SW.ToString(), -1 },
           { Enums.IrmaRegion.UK.ToString(), -1 },
        };
    }
}
