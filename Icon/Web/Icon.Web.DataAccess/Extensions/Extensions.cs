using Icon.Framework;
using Icon.Web.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;

namespace Icon.Web.DataAccess.Extensions
{
    public static class Extensions
    {
        /// <summary>
        /// Convert a List to a DataTable
        /// </summary>
        /// <typeparam name="T">any type</typeparam>
        /// <param name="data">list of type T"/></param>
        /// <returns>DataTable</returns>
        public static DataTable ToDataTable<T>(this IList<T> data)
        {
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(typeof(T));
            DataTable table = new DataTable();

            foreach (PropertyDescriptor prop in properties)
            {
                table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
            }

            foreach (T item in data)
            {
                DataRow row = table.NewRow();
                foreach (PropertyDescriptor prop in properties)
                {
                    row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                }
                table.Rows.Add(row);
            }

            return table;
        }

        /// <summary>
        /// Convert boolean to a string of "1" or "0"
        /// </summary>
        /// <param name="value">any boolean</param>
        /// <returns>"1" or "0"</returns>
        public static string BoolToString(this bool value)
        {
            return value ? "1" : "0";
        }

        /// <summary>
        /// Converts a boolean to a string of "Y" or "N".
        /// </summary>
        /// <param name="value">any boolean.</param>
        /// <returns>"Y" for true or "N" for false.</returns>
        public static string BoolToYesOrNo(this bool value)
        {
            return value ? "Y" : "N";
        }

        /// <summary>
        /// Converts a boolean to a string of "Y" or "N".
        /// </summary>
        /// <param name="value">any boolean.</param>
        /// <returns>"Y" for true or "N" for false.</returns>
        public static string OneOrZeroToYesOrNo(this string value)
        {
            if (value != "1" && value != "0")
            {
                return "N";
            }
            else
            {
                return value == "1" ? "Y" : "N";
            }
        }

        public static bool ContainsDuplicateName(this IEnumerable<HierarchyClass> hierarchyClasses,
            string name, int? level, int hierarchyId, int hierarchyClassId, HierarchyClass subTeamHierarchyClass, int? hierarchyParentClassId, bool IsUpdate)
        {
            IEnumerable<HierarchyClass> duplicates = hierarchyClasses.Where(hc =>
                hc.hierarchyClassName.ToLower() == name.ToLower()
                && hc.hierarchyLevel == level
                && hc.hierarchyID == hierarchyId
                && hc.hierarchyParentClassID == hierarchyParentClassId);

            if (level == HierarchyLevels.SubBrick && hierarchyId == Hierarchies.Merchandise && subTeamHierarchyClass != null)
            {
                duplicates = duplicates.Where(hc => hc.HierarchyClassTrait.Any(ihc => ihc.traitValue == subTeamHierarchyClass.hierarchyClassName && ihc.Trait.traitCode == TraitCodes.MerchFinMapping));
            }

            if (IsUpdate)
            {
                duplicates = duplicates.Where(hc => hc.hierarchyClassID != hierarchyClassId);
            }

            return duplicates.Any();
        }

        public static bool ContainsDuplicateBrandName(this IEnumerable<HierarchyClass> hierarchyClasses, string brandName)
        {
            return hierarchyClasses.Any(hc =>
                hc.Hierarchy.hierarchyName == HierarchyNames.Brands && hc.hierarchyClassName.ToLower() == brandName.ToLower());
        }

        public static bool ContainsDuplicateAgencyName(this IEnumerable<HierarchyClass> hierarchyClasses, string agencyName)
        {
            return hierarchyClasses.Any(hc =>
                hc.Hierarchy.hierarchyName == HierarchyNames.CertificationAgencyManagement && hc.hierarchyClassName == agencyName);
        }

        public static bool ContainsDuplicateTrimmedBrandName(this IEnumerable<HierarchyClass> hierarchyClasses, string trimmedBrandName)
        {
            var trimmedBrandNames = hierarchyClasses
                .Where(hc => hc.Hierarchy.hierarchyName == HierarchyNames.Brands && hc.hierarchyClassName.Length >= Constants.IrmaBrandNameMaxLength)
                .Select(hc => hc.hierarchyClassName.Substring(0, Constants.IrmaBrandNameMaxLength));

            return trimmedBrandNames.Any(b => b.ToLower() == trimmedBrandName.ToLower());
        }

        public static bool ContainsDuplicateNationalClass(this IEnumerable<HierarchyClass> hierarchyClasses,
            string name, int? level, int hierarchyId, int hierarchyClassId, string nationalClassCode, int? hierarchyParentClassId, bool IsUpdate)
        {
            IEnumerable<HierarchyClass> duplicates = null;

            duplicates = hierarchyClasses.Where(hc =>
            hc.hierarchyClassName.ToLower() == name.ToLower()
            && hc.hierarchyLevel == level
            && hc.hierarchyID == hierarchyId
            && hc.hierarchyParentClassID == hierarchyParentClassId);            

            if (IsUpdate)
            {
                duplicates = duplicates.Where(hc => hc.hierarchyClassID != hierarchyClassId);
            }

            return duplicates.Any();
        }

        public static string ParsePeopleSoftNumber(this string hierarchyClassName)
        {
            if (String.IsNullOrWhiteSpace(hierarchyClassName))
            {
                throw new ArgumentException("Hierarchy class name cannot be null, empty, or whitespace when parsing it for its PeopleSoft number.", "hierarchyClassName");
            }
            else
            {
                return hierarchyClassName.Split('(')[1].Trim(')');
            }
        }

        public static void AddHierarchyClassTrait(this HierarchyClass hierarchyClass, IconContext context, string traitCode, string traitValue)
        {
            var hierarchyClassTrait = new HierarchyClassTrait
            {
                traitID = context.Trait.Single(t => t.traitCode == traitCode).traitID,
                traitValue = traitValue,
                hierarchyClassID = hierarchyClass.hierarchyClassID
            };

            context.HierarchyClassTrait.Add(hierarchyClassTrait);
            context.SaveChanges();
        }

        public static void AddHierarchyClassTrait(this HierarchyClass hierarchy, IconContext context, int currentID, string value, bool isForceValue = false)
        {
            if(string.IsNullOrWhiteSpace(value) && !isForceValue) return;
            context.HierarchyClassTrait.Add(new HierarchyClassTrait() { traitID = currentID, traitValue = value?.Trim(), hierarchyClassID = hierarchy.hierarchyClassID });
        }

        public static void UpdateHierarchyClassTrait(this HierarchyClassTrait hierarchyClassTrait, IconContext context, string traitValue, bool removeIfNullOrEmpty, bool saveChanges = true)
        {
            if (removeIfNullOrEmpty && String.IsNullOrWhiteSpace(traitValue))
            {
                context.HierarchyClassTrait.Remove(hierarchyClassTrait);
            }
            else
            {
                hierarchyClassTrait.traitValue = traitValue.Trim();
            }

            if(saveChanges)
            {
                context.SaveChanges();
            }
        }

        public static bool ContainsDuplicatePluCategoryName(this IEnumerable<PLUCategory> pluCategoryList, int? pluCategoryId, string pluCategoryName, long beginRange, long endRange)
        {
            if (pluCategoryId == null)
            {
                return pluCategoryList.Any(pc => pc.PluCategoryName.Equals(pluCategoryName, StringComparison.CurrentCultureIgnoreCase));
            }
            else
            {
                return pluCategoryList.Any(pc =>
                    pc.PluCategoryID != pluCategoryId && pc.PluCategoryName.Equals(pluCategoryName, StringComparison.CurrentCultureIgnoreCase));
            }
        }

        public static bool ContainsDuplicatePluCategoryRange(this IEnumerable<PLUCategory> pluCategoryList, int? pluCategoryId, string pluCategoryName, long beginRange, long endRange)
        {
            if (pluCategoryId == null)
            {
                return pluCategoryList.Any(pc => endRange >= Convert.ToInt64(pc.BeginRange) && beginRange <= Convert.ToInt64(pc.EndRange));
            }
            else
            {
                return pluCategoryList.Any(pc =>
                    pc.PluCategoryID != pluCategoryId && endRange >= Convert.ToInt64(pc.BeginRange) && beginRange <= Convert.ToInt64(pc.EndRange));
            }
        }

        public static void SortPluCategoriesByBeginRange(this List<PLUCategory> pluCategoryList)
        {
            pluCategoryList.Sort(delegate(PLUCategory x, PLUCategory y)
                {
                    return Convert.ToInt64(x.BeginRange) > Convert.ToInt64(y.BeginRange) ? 1 : Convert.ToInt64(x.BeginRange) < Convert.ToInt64(y.BeginRange) ? -1 : 0;
                });
        }
    }
}
