using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OOSCommon
{
    public class StellaUpcSet : UpcSet
    {
        public StellaUpcSet(int size) : base(size) {}

        protected override int UpcLength()
        {
            return 12;
        }
    }
}
