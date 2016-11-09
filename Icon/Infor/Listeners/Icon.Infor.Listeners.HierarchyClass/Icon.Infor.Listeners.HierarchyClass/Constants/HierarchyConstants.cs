using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icon.Infor.Listeners.HierarchyClass.Constants
{
    public static class HierarchyConstants
    {
        public static HashSet<string> HierarchyLevelNames = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            Icon.Framework.HierarchyLevelNames.Segment,
            Icon.Framework.HierarchyLevelNames.Family,
            Icon.Framework.HierarchyLevelNames.Class,
            Icon.Framework.HierarchyLevelNames.Gs1Brick,
            Icon.Framework.HierarchyLevelNames.SubBrick,
            Icon.Framework.HierarchyLevelNames.Brand,
            Icon.Framework.HierarchyLevelNames.Tax,
            Icon.Framework.HierarchyLevelNames.Parent,
            Icon.Framework.HierarchyLevelNames.Sub,
            Icon.Framework.HierarchyLevelNames.SubSub,
            Icon.Framework.HierarchyLevelNames.Financial,
            Icon.Framework.HierarchyLevelNames.NationalFamily,
            Icon.Framework.HierarchyLevelNames.NationalCategory,
            Icon.Framework.HierarchyLevelNames.NationalSubCategory,
            Icon.Framework.HierarchyLevelNames.NationalClass,
            Icon.Framework.HierarchyLevelNames.CertificationAgencyManagement
        };

        public static HashSet<string> BrandHierarchyLevelNames = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            Icon.Framework.HierarchyLevelNames.Brand
        };

        public static HashSet<string> NationalHierarchyLevelNames = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            Icon.Framework.HierarchyLevelNames.NationalFamily,
            Icon.Framework.HierarchyLevelNames.NationalCategory,
            Icon.Framework.HierarchyLevelNames.NationalSubCategory,
            Icon.Framework.HierarchyLevelNames.NationalClass
        };

        public static HashSet<string> MerchandiseHierarchyLevelNames = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            Icon.Framework.HierarchyLevelNames.Segment,
            Icon.Framework.HierarchyLevelNames.Family,
            Icon.Framework.HierarchyLevelNames.Class,
            Icon.Framework.HierarchyLevelNames.Gs1Brick,
            Icon.Framework.HierarchyLevelNames.SubBrick
        };

        public static HashSet<string> TaxHierarchyLevelNames = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            Icon.Framework.HierarchyLevelNames.Tax
        };

        public static HashSet<string> FinancialHierarchyLevelNames = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            Icon.Framework.HierarchyLevelNames.Financial
        };

        public const string BrandNamePattern = @"^[\x20-\x21\x23-\x2A\x2C-\x5A\x61-\x7A\x9C]{0,35}$";
    }
}
