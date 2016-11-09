using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MessageGenerationWeb.Excel
{
    public class ItemPriceDeleteSpreadsheetHeader
    {
        public ItemPriceDeleteSpreadsheetHeader()
        {
            ScanCodeColumnIndex = 0;
            BusinessUnitColumnIndex = 1;
            PriceColumnIndex = 2;
            StartDateColumnIndex = 3;
            EndDateColumnIndex = 4;
            UomColumnIndex = 5;
        }

        public int ScanCodeColumnIndex { get; private set; }
        public int BusinessUnitColumnIndex { get; private set; }
        public int PriceColumnIndex { get; private set; }
        public int StartDateColumnIndex { get; private set; }
        public int EndDateColumnIndex { get; private set; }
        public int UomColumnIndex { get; private set; }
    }
}
