using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OOSCommon
{
    public class MovementStellaUpcSpecification
    {
        public static bool IsSatifiedBy(string upc)
        {
            if (UpcSpecification.FormValid(new List<string>{upc}).SequenceEqual(new List<string>{upc}))
            {
                return false;
            }
            return true;
        }


        public static List<string> TranslateUpc(List<string> upcs)
        {
            var validUpcs = UpcSpecification.FormValid(upcs);
            return validUpcs.Select(upc => upc.Substring(1, upc.Length - 1)).ToList();
        }

        public static List<string> InvertUpc(List<string> upcs)
        {
            return upcs.Select(p => p.PadLeft(p.Length+1, '0')).ToList();
        }
    }
}
