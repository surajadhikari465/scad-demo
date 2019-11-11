namespace Mammoth.ItemLocale.Controller.DataAccess.Tests
{
    using Dapper;
    using Mammoth.Common.DataAccess.DbProviders;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;
       

    public static class IrmaInsertHelpers
    {
        public static void Insert<T>(this IDbProvider dbProvider, T irmaItem, string table = null) 
            where T : class
        {
            var targetTableName = table ?? typeof(T).Name;

            var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(IsTableColumn)
                .Select(p => p.Name)
                .ToList();

            var columnNames = "(" + string.Join(", ", properties) + ")";
            var parameterNames = "(" + string.Join(", ", properties.Select(p => "@" + p)) + ")";

            var sql = "INSERT INTO "
                + targetTableName
                + " " + columnNames
                + " VALUES " + parameterNames;

            dbProvider.Connection.Execute(sql, irmaItem, dbProvider.Transaction);
        }

        public static TResult Insert<T, TResult>(this IDbProvider dbProvider, IrmaQueryParams<T, TResult> queryParameters)
        {
            var targetTableName = queryParameters.TableName ?? typeof(T).Name;

            var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(IsTableColumn)
                .Select(p => p.Name)
                .OrderBy(p => p)
                .ToList();

            // Remove the primary key from the insert column list.
            if(queryParameters.PrimaryKey != null)
            {
                properties.RemoveAll(p => p == GetPropertyName(queryParameters.PrimaryKey));
            }

            // Remove mapped columns.
            if(queryParameters.ColumnMap != null)
            {
                properties.RemoveAll(queryParameters.ColumnMap.ContainsKey);
            }

            // Check for column mappings
            var columnNames = queryParameters.ColumnMap == null
                ? "(" + string.Join(", ", properties) + ")"
                : "(" + string.Join(", ", properties.Union(queryParameters.ColumnMap.Values)) + ")";

            var parameterNames = queryParameters.ColumnMap == null
                ? "(" + string.Join(", ", properties.Select(p => "@" + p)) + ")"
                : "(" + string.Join(", ", properties.Union(queryParameters.ColumnMap.Keys).Select(p => "@" + p)) + ")";

            var sql = "INSERT INTO "
                + targetTableName
                + " " + columnNames
                + " VALUES " + parameterNames;

            if(queryParameters.PrimaryKey == null && !queryParameters.ForceScopeIdentity)
            {
                dbProvider.Connection.Execute(sql, queryParameters.IrmaObject, dbProvider.Transaction);
                return default(TResult);
            }
            else
            {
                sql += " SELECT SCOPE_IDENTITY()";
                return dbProvider.Connection.Query<TResult>(sql, queryParameters.IrmaObject, dbProvider.Transaction).First();
            }
        }
       
        public static TResult GetLookupId<TResult>(this IDbProvider dbProvider, string idColumn, string table, string descriptionColumn, string value)
        {
            var sql = "SELECT " + idColumn
                + " FROM " + table
                + " WHERE " + descriptionColumn + " = " + "@Value";

            var id = dbProvider.Connection.Query<TResult>(sql, new { Value = value }, dbProvider.Transaction).FirstOrDefault();

            return id;
        }

        private static bool IsTableColumn(PropertyInfo pInfo)
        {
            return pInfo.PropertyType.IsPrimitive
                || pInfo.PropertyType == typeof(decimal)
                || pInfo.PropertyType == typeof(string)
                || pInfo.PropertyType == typeof(DateTime)
                || (pInfo.PropertyType.IsGenericType && pInfo.PropertyType.GetGenericArguments().Any(a => a.IsPrimitive))
                || (pInfo.PropertyType.IsGenericType && pInfo.PropertyType.GetGenericArguments().Any(a => a == typeof(DateTime)));
        }

        private static string GetPropertyName<T, TPropertyType>(Expression<Func<T, TPropertyType>> propertyExpression)
        {
            var memberExpression = propertyExpression.Body as MemberExpression;
            var property = memberExpression.Member as PropertyInfo;
            var getMethod = property.GetGetMethod(true);
            return property.Name;
        }
    }

    public class IrmaQueryParams<T, TResult>
    {
        public T IrmaObject { get; set; }

        public Expression<Func<T, TResult>> PrimaryKey { get; set; }

        public Dictionary<string, string> ColumnMap { get; set; }

        public string TableName { get; set; }

        public bool ForceScopeIdentity { get; set; }

        public IrmaQueryParams(
            T irmaObject,
            Expression<Func<T, TResult>> primaryKey = null,
            Dictionary<string, string> columnMap = null,
            string tableName = null,
            bool forceScopeIdentity = false)
        {
            this.IrmaObject = irmaObject;
            this.PrimaryKey = primaryKey;
            this.ColumnMap = columnMap;
            this.TableName = tableName;
            this.ForceScopeIdentity = forceScopeIdentity;
        }
    }
}
