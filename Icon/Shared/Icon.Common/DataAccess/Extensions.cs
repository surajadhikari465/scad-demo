using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;

namespace Icon.Common
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

        public static DataTable ToDataTable<T>(this IEnumerable<T> data)
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
        
        public static SqlParameter ToTvp<T>(this IEnumerable<T> enumerable, string parameterName, string typeName)
        {
            return new SqlParameter(parameterName, SqlDbType.Structured)
            {
                TypeName = typeName,
                Value = enumerable.ToDataTable()
            };
        }
    }
}
