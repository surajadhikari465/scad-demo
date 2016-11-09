namespace Testing.Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    public class DapperSqlFactory
    {
        /// <summary>
        /// Store the templates with key of your item type and value of your template.
        /// </summary>
        private Dictionary<string, Type> sqlBuilderTemplates;

        public DapperSqlFactory(Assembly templateAssembly)
        {
            string interfaceName = typeof(ISqlBuilderTemplate<>).Name;

            // Get all the types that have the ISqlBuilderTemplate and store them in the template dictionary.
            this.sqlBuilderTemplates = templateAssembly.GetTypes()
                .Where(t => t.GetInterface(interfaceName) != null)
                .ToDictionary(
                    t => t.GetInterface(interfaceName).GetGenericArguments().First().Name);
        }

        public string BuildInsertSql<T>(bool includeScopeIdentity = false) where T : class
        {
            string itemTypeName = typeof(T).Name;

            var builderTemplate = this.sqlBuilderTemplates.ContainsKey(itemTypeName)
                ? Activator.CreateInstance(this.sqlBuilderTemplates[itemTypeName]) as ISqlBuilderTemplate<T>
                : null;

            string tableName = builderTemplate == null || string.IsNullOrEmpty(builderTemplate.TableName)
                ? itemTypeName
                : builderTemplate.TableName;

            var allColumns = GetColumns<T>(builderTemplate);

            var columnNames = builderTemplate == null || builderTemplate.PropertyToColumnMapping == null
                ? "(" + string.Join(", ", allColumns) + ")"
                : "(" + string.Join(", ", allColumns.Union(builderTemplate.PropertyToColumnMapping.Values)) + ")";

            var parameterNames = builderTemplate == null || builderTemplate.PropertyToColumnMapping == null
                ? "(" + string.Join(", ", allColumns.Select(p => "@" + p)) + ")"
                : "(" + string.Join(", ", allColumns.Union(builderTemplate.PropertyToColumnMapping.Keys).Select(p => "@" + p)) + ")";

            var sql = "INSERT INTO "
                + tableName
                + " " + columnNames
                + " VALUES " + parameterNames
                + (includeScopeIdentity ? " SELECT SCOPE_IDENTITY()" : string.Empty);

            return sql;
        }

        private static List<string> GetColumns<T>(ISqlBuilderTemplate<T> template) where T : class
        {
            var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(IsTableColumn)
                .Select(p => p.Name)
                .ToList();

            if (template == null) return properties;

            if (!string.IsNullOrEmpty(template.IdentityColumn))
            {
                properties.RemoveAll(p => p == template.IdentityColumn);
            }

            if (template.PropertyToColumnMapping != null)
            {
                properties.RemoveAll(template.PropertyToColumnMapping.ContainsKey);
            }

            return properties;
        }

        // This method is checking to see if the property on your class is a 'basic' type that woudl translate into a column on 
        // the corresponding table.
        private static bool IsTableColumn(PropertyInfo pInfo)
        {
            return pInfo.PropertyType.IsPrimitive
                || (pInfo.PropertyType.IsGenericType && pInfo.PropertyType.GetGenericArguments().Any(a => a.IsPrimitive))
                || pInfo.PropertyType == typeof(decimal)
                || pInfo.PropertyType == typeof(decimal?)
                || pInfo.PropertyType == typeof(string)
                || pInfo.PropertyType == typeof(DateTime)
                || pInfo.PropertyType == typeof(DateTime?);
        }
    }
}
