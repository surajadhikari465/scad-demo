using Dapper;
using Icon.Common;
using Icon.Common.DataAccess;
using Icon.Web.DataAccess.Infrastructure;
using Icon.Web.DataAccess.Models;
using System;
using System.Data;
using System.Linq;

namespace Icon.Web.DataAccess.Queries
{
    public class GetItemsQueryHandler : IQueryHandler<GetItemsParameters, GetItemsResult>
    {
        private IDbConnection connection;
        private IItemQueryBuilder queryBuilder;

        public GetItemsQueryHandler(IDbConnection connection, IItemQueryBuilder queryBuilder)
        {
            this.connection = connection;
            this.queryBuilder = queryBuilder;
        }

        public GetItemsResult Search(GetItemsParameters parameters)
        {
            int commandTimeOut = 300;

            if (parameters.Top == AppSettingsAccessor.GetIntSetting("maxNumberOfItemsRecordToBeExported", 10000))
            {
                commandTimeOut = AppSettingsAccessor.GetIntSetting("exportItemTimeOutInSeconds", 300);
            }

            if (parameters.ItemAttributeJsonParameters.Count > 0)
            {
                string searchQuery = this.queryBuilder.BuildQuery(parameters);
                var command = new CommandDefinition(searchQuery, null, null, commandTimeOut);
                var recordSets = connection.QueryMultiple(command);
                var items = recordSets.Read<ItemDbModel>();
                var recordCount = recordSets.ReadFirst<int>();

                return new GetItemsResult
                {
                    Items = items,
                    TotalRecordsCount = recordCount,
                    Query = searchQuery.Replace(Environment.NewLine, " ")
                };
            }
            else
            {
                return new GetItemsResult();
            }
        }
    }
}
