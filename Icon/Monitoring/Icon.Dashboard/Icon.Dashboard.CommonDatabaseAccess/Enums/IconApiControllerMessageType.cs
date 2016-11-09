using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icon.Dashboard.CommonDatabaseAccess
{
    public static class IconApiControllerMessageType
    {
        public const string Undefined = "Undefined";
        public const string Locale = "Locale";
        public const string Hierarchy = "Hierarchy";
        public const string ItemLocale = "ItemLocale";
        public const string Price = "Price";
        public const string DepartmentSale = "DepartmentSale";
        public const string Product = "Product";
        public const string NotUsed = "NotUsed";
        public const string CCHTaxUpdate = "CCHTaxUpdate";
        public const string ProductSelectionGroup = "ProductSelectionGroup";
        public const string eWIC = "eWIC";
        public const string InforNewItem = "InforNewItem";

        public static List<string> GetAll()
        {
            return new List<string>()
            {
                Undefined,
                Locale,
                Hierarchy,
                ItemLocale,
                Price,
                DepartmentSale,
                Product,
                NotUsed,
                CCHTaxUpdate,
                ProductSelectionGroup,
                eWIC,
                InforNewItem
            };
        }
    }
}
