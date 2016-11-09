using System.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using Icon.Framework;

namespace RegionalEventController.Common
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

        public static int MapToTaxId(this string taxCode)
        {
            int taxId;
            if (Cache.taxCodeToTaxId.TryGetValue(taxCode, out taxId))
            {
                return taxId;
            }
            else
            {
                throw new InvalidOperationException("The taxCode to taxID mapping dictionary has not been initialized.");
            }
        }

        public static int? MapToNationalClassId(this int nationalClassCode)
        {
            int nationalClassId;
            if (Cache.nationalClassCodeToNationalClassId.TryGetValue(nationalClassCode, out nationalClassId))
            {
                return nationalClassId;
            }
            else
            {
                return null;
            }
        }

        public static string PrependedPosDescription(this IRMAItem irmaItem)
        {
            string brandAbbreviation;
            if (Cache.brandNameToBrandAbbreviationMap.TryGetValue(irmaItem.brandName.ToLower(), out brandAbbreviation))
            {
                var prependedPosDescription = brandAbbreviation + " " + irmaItem.posDescription;
                return prependedPosDescription.Length > FieldLengthConstants.PosDescriptionMaxLength ? prependedPosDescription.Substring(0, 25) : prependedPosDescription;
            }
            else
            {
                return irmaItem.posDescription;
            }
        }
    }
}
