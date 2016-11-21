using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icon.Infor.Listeners.HierarchyClass.Constants
{
    public static class HierarchyConstants
    {
        /// <summary>
        /// Name of the all the levels for each hierarchy. Used to validate messages sent to Icon have correct names before performing updates.
        /// </summary>
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

        /// <summary>
        /// Name of the levels in the Brand hierarchy. Used to validate messages sent to Icon have correct names before performing updates.
        /// </summary>
        public static HashSet<string> BrandHierarchyLevelNames = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            Icon.Framework.HierarchyLevelNames.Brand
        };

        /// <summary>
        /// Name of the levels in the National hierarchy. Used to validate messages sent to Icon have correct names before performing updates.
        /// </summary>
        public static HashSet<string> NationalHierarchyLevelNames = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            Icon.Framework.HierarchyLevelNames.NationalFamily,
            Icon.Framework.HierarchyLevelNames.NationalCategory,
            Icon.Framework.HierarchyLevelNames.NationalSubCategory,
            Icon.Framework.HierarchyLevelNames.NationalClass
        };

        /// <summary>
        /// Name of the levels in the Merchandise hierarchy. Used to validate messages sent to Icon have correct names before performing updates.
        /// </summary>
        public static HashSet<string> MerchandiseHierarchyLevelNames = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            Icon.Framework.HierarchyLevelNames.Segment,
            Icon.Framework.HierarchyLevelNames.Family,
            Icon.Framework.HierarchyLevelNames.Class,
            Icon.Framework.HierarchyLevelNames.Gs1Brick,
            Icon.Framework.HierarchyLevelNames.SubBrick
        };

        /// <summary>
        /// Name of the levels in the Tax hierarchy. Used to validate messages sent to Icon have correct names before performing updates.
        /// </summary>
        public static HashSet<string> TaxHierarchyLevelNames = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            Icon.Framework.HierarchyLevelNames.Tax
        };

        /// <summary>
        /// Name of the levels in the Financial hierarchy. Used to validate messages sent to Icon have correct names before performing updates.
        /// </summary>
        public static HashSet<string> FinancialHierarchyLevelNames = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            Icon.Framework.HierarchyLevelNames.Financial
        };

        /// <summary>
        /// Regex pattern used to validate the name of a Hierarchy Class under the Brand hierarchy.
        /// </summary>
        public const string BrandNamePattern = @"^[\x20-\x21\x23-\x2A\x2C-\x5A\x61-\x7A\x9C]{0,35}$";

        /// <summary>
        /// Regex pattern used to validate the name of a Hierarchy Class under the Tax hierarchy.
        /// </summary>
        public const string TaxNamePattern = @"^[\d]{7} [^<>]{1,247}$";

        /// <summary>
        /// This constant is one of the possible values that represents whether a hierarchy class can have items attached to it. 
        /// The ESB requires that items attached is populated in HierarchyClasses and that the value is either 1 or 0.
        /// </summary>
        public const string ItemsAttachedTrue = "1";

        /// <summary>
        /// This constant is one of the possible values that represents whether a hierarchy class can have items attached to it. 
        /// The ESB requires that items attached is populated in HierarchyClasses and that the value is either 1 or 0.
        /// </summary>
        public const string ItemsAttachedFalse = "0";
    }
}
