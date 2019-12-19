using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharedKernel;

namespace OOS.Model
{
    public class OffShelfScan
    {
        private UPC upc;

        public OffShelfScan(string upc)
        {
            if (string.IsNullOrEmpty(upc))
                throw new InvalidUPCException(string.Format("'{0}' is not a valid UPC", upc));

            this.upc = new UPC(upc);
        }

        public string Upc { get { return upc.Code; } }
    }
}
