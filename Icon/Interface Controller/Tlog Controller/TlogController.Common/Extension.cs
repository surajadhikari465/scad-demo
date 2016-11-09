using System.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;

namespace TlogController.Common
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

        public static string MapToRegionCode(this int businessUnit)
        {
            string regionCode;
            if (Cache.BusinessUnitToRegionCode.TryGetValue(businessUnit, out regionCode))
            {
                return regionCode;
            }
            else
            {
                throw new InvalidOperationException("The business unit to region code mapping dictionary has not been initialized.");
            }
        }

        public static List<List<T>> Split<T>(this List<T> items, int sliceSize = 30)
        {
            List<List<T>> list = new List<List<T>>();
            for (int i = 0; i < items.Count; i += sliceSize)
                list.Add(items.GetRange(i, Math.Min(sliceSize, items.Count - i)));
            return list;
        } 
    }
}