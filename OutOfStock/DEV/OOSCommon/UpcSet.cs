using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OOSCommon
{
    public class UpcSet
    {
        private int size;

        public UpcSet(int size)
        {
            this.size = size;
        }

        protected virtual int UpcLength()
        {
            return 13;
        }

        public List<string> ToPureSqlStrings(IEnumerable<string> upcs)
        {
            List<string> csvUPCs = new List<string>();
            if (upcs != null && upcs.Any())
            {
                string csvUPC = string.Empty;
                foreach (string upc in upcs)
                {
                    if (csvUPC.Length > size)
                    {
                        csvUPCs.Add(csvUPC);
                        csvUPC = string.Empty;
                    }
                    if (string.IsNullOrWhiteSpace(upc) || upc.Trim().Length != UpcLength() || !char.IsDigit(upc[0])) continue;
                    if (csvUPC.Length > 0)
                        csvUPC += ",";
                    csvUPC += "'" + upc.Trim() + "'";
                }
                if (csvUPC.Length > 0)
                    csvUPCs.Add(csvUPC);
            }
            return csvUPCs;
        }
    }
}
