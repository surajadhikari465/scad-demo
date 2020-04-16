using System;
using System.Collections.Generic;

namespace Icon.Web.Mvc.Utility
{
    public static class ItemNonAttributeFieldsDefaultOrder
    {
       public static Dictionary<float, string>  OrderForNonAttributefields()
        {
            //using float for non attribute fields . attribut display order runs from 1 to number of attributes in database. so merge attributes display order and non attribute display order in 
            //one dictionary we are using floats here.
            Dictionary<float, string> defaultFields = new Dictionary<float, string>();
            defaultFields.Add((float)0.5, "ItemId");
            defaultFields.Add((float)1.5, "BarcodeType");
            defaultFields.Add((float)2.1, "ItemType");
            defaultFields.Add((float)2.2, "ScanCode");
            defaultFields.Add((float)2.5, "Brand");
            defaultFields.Add((float)8.2, "Merchandise");
            defaultFields.Add((float)8.1, "Financial");
            defaultFields.Add((float)8.5, "National");
            defaultFields.Add((float)8.8, "Tax");
            defaultFields.Add((float)83.1, "Manufacturer");

            return defaultFields;
        }
    }
}