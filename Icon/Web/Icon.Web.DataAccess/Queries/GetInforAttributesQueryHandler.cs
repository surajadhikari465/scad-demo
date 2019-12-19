using Dapper;
using System;
using Icon.Common.DataAccess;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Icon.Common.Models;
using Icon.Web.DataAccess.Models;

namespace Icon.Web.DataAccess.Queries
{
    public class GetInforAttributesQueryHandler : IQueryHandler<EmptyQueryParameters<IEnumerable<AttributeInforModel>>, IEnumerable<AttributeInforModel>>
    {
        private IDbConnection connection;

        public GetInforAttributesQueryHandler(IDbConnection connection)
        {
            this.connection = connection;
        }

        public IEnumerable<AttributeInforModel> Search(EmptyQueryParameters<IEnumerable<AttributeInforModel>> parameters)
        {
            var sql = @"SELECT AttributeName, AttributeName as DisplayName from [infor].[HistoricalAttributes] WHERE AttributeType='ITEM'";

            return connection.Query<AttributeInforModel>(sql);
        }
    }
}