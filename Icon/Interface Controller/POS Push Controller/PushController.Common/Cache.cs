using Icon.Framework;
using PushController.Common.Models;
using System;
using System.Collections.Generic;

namespace PushController.Common
{
    public static class Cache
    {
        public static Dictionary<int, Locale> businessUnitToLocale = new Dictionary<int, Locale>();
        public static Dictionary<string, ScanCodeModel> identifierToScanCode = new Dictionary<string, ScanCodeModel>();
        public static Dictionary<Tuple<string, int>, string> scanCodeByBusinessUnitToLinkedScanCode = new Dictionary<Tuple<string, int>, string>();
        public static Dictionary<int, UOM> uomIdToUom = new Dictionary<int, UOM>();
        public static Dictionary<string, Boolean> regionCodeToGPMInstanceDataFlag = new Dictionary<string, Boolean>();

        public static void ClearAll()
        {
            businessUnitToLocale.Clear();
            identifierToScanCode.Clear();
            scanCodeByBusinessUnitToLinkedScanCode.Clear();
            uomIdToUom.Clear();
            regionCodeToGPMInstanceDataFlag.Clear();
        }
    }
}
