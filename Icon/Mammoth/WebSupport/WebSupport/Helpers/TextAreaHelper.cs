using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebSupport.Helpers
{
    public static class TextAreaHelper
    {
        public const int DefaultRows = 20;
        public const int MaxRows = 40;
        public const int DefaultCols = 20;

        public static int GetRowsByListCount(int listCount)
        {
            int textAreaRowSize = DefaultRows;
            if (listCount > DefaultRows)
            {
                textAreaRowSize = (listCount <= MaxRows) ? listCount : MaxRows;
            }
            return textAreaRowSize;
        }
    }
}