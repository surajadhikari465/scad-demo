using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharedKernel;

namespace OOS.Model
{
    public class CheckumSpecification
    {
        private CheckumSpecification()
        {}

        public static bool IsSatifiedBy(string upc)
        {
            var sut = new UPC(upc);
            return (LastDigit(sut.Code) == sut.CalculateChecksum());
        }

        private static int LastDigit(IEnumerable<char> code)
        {
            return Convert.ToInt32(code.LastOrDefault()) - Convert.ToInt32('0');
        }
    }
}
