using Icon.Common.DataAccess;
using Icon.Framework;
using Icon.Web.DataAccess.Attributes;
using Icon.Web.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;

namespace Icon.Web.DataAccess.Queries
{
    public class GetItemsBySearchQuery : IQueryHandler<GetItemsBySearchParameters, ItemsBySearchResultsModel>
    {
        private readonly IconContext context;

        public GetItemsBySearchQuery(IconContext context)
        {
            this.context = context;
        }

        public ItemsBySearchResultsModel Search(GetItemsBySearchParameters parameters)
        {
            var properties = typeof(GetItemsBySearchParameters)
                .GetProperties()
                .Where(p => p.IsDefined(typeof(SqlParameterAttribute)));

            var sqlParameters = CreateSqlParameters(properties, parameters);

            string sql = string.Format(
                "app.GetItemsBySearchParams {0}",
                string.Join(", ", sqlParameters.Select(p => FormatSqlParameterName(p))));

            SetSortColumnToMatchStoredProcedureColumn(sqlParameters);

            if (!parameters.GetCountOnly)
            {
                var dbResult = this.context.Database.SqlQuery<ItemSearchModel>(sql, sqlParameters);
                return new ItemsBySearchResultsModel { Items = dbResult.ToList() };
            }
            else
            {
                var dbResult = this.context.Database.SqlQuery<int>(sql, sqlParameters);
                return new ItemsBySearchResultsModel { ItemsCount = dbResult.First() };
            }
        }

        private SqlParameter[] CreateSqlParameters(IEnumerable<PropertyInfo> properties, GetItemsBySearchParameters parameters)
        {
            return properties
                .Select(p => new
                {
                    p.GetCustomAttribute<SqlParameterAttribute>().ParamName,
                    p.GetCustomAttribute<SqlParameterAttribute>().SqlDbType,
                    Value = p.GetValue(parameters)
                })
                .Select(p => new SqlParameter(p.ParamName, p.SqlDbType)
                {
                    Value = p.Value == null ? (Object)DBNull.Value : p.Value
                })
                .ToArray();
        }

        private static string FormatSqlParameterName(SqlParameter p)
        {
            return string.Format("@{0} = @{0}", p.ParameterName);
        }

        private void SetSortColumnToMatchStoredProcedureColumn(IEnumerable<SqlParameter> sqlParameters)
        {
            var sortColumn = sqlParameters.First(p => p.ParameterName == "sortColumn");
            if (sortColumn.Value != DBNull.Value)
            {
                sortColumn.Value = SetSortColumnToMatchStoredProcedureColumn(sortColumn.Value.ToString());
            }
        }

        private string SetSortColumnToMatchStoredProcedureColumn(string sortColumn)
        {
            var property = typeof(ItemSearchModel).GetProperties()
                .FirstOrDefault(p => p.Name == sortColumn);

            if(property != null)
            {
                var propertySortColumn = property.GetCustomAttribute<SqlResultAttribute>().SqlSortValue;
                if(string.IsNullOrWhiteSpace(propertySortColumn))
                {
                    throw new ArgumentException("Unable to sort by " + sortColumn + ". No column is registered to sort by that paramter.");
                }
                else
                {
                    return propertySortColumn;
                }
            }

            return null;
        }

        private string SetSortOrderToMatchStoredProcedureOrder(string sortOrder)
        {
            if(sortOrder.Equals("asc", StringComparison.InvariantCultureIgnoreCase))
            {
                return "ASC";
            }
            else if(sortOrder.Equals("desc", StringComparison.InvariantCultureIgnoreCase))
            {
                return "DESC";
            }
            else
            {
                throw new ArgumentException("Unable to use sort order " + sortOrder + ". No order is registered to sort by that parameter.");
            }
        }
    }
}
