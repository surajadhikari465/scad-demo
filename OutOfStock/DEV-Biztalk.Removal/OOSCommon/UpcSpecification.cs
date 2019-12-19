using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OOSCommon
{
    public class UpcSpecification
    {
        public static bool IsSatisfiedBy(string upc)
        {
            string vimUpc;
            var upcCheck = Utility.UPCCheck(upc, out vimUpc);
            switch (upcCheck)
            {
                case Utility.eUPCCheck.Good:
                    return true;
                default:
                    return false;
            }
        }

        public static List<string> FormValid(List<string> upcs)
        {
            var goodUpcs = new List<string>();
            foreach (var upc in upcs)
            {
                string vimUpc;
                var upcCheck = Utility.UPCCheck(upc, out vimUpc);
                switch (upcCheck)
                {
                    case Utility.eUPCCheck.Good:
                        goodUpcs.Add(vimUpc);
                        break;
                    default:
                        break;
                }
            }
            return goodUpcs;
        }

        public static Utility.eUPCCheck GetUpcCheckValue(string upc)
        {
            string vimUpc;
            return Utility.UPCCheck(upc, out vimUpc);
        }
    }
}
