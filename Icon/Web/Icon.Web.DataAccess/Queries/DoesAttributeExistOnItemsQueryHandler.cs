using Dapper;
using Icon.Common.DataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icon.Web.DataAccess.Queries
{
    public class DoesAttributeExistOnItemsQueryHandler : IQueryHandler<DoesAttributeExistOnItemsParameters, bool>
    {
        private IDbConnection dbConnection;

        public DoesAttributeExistOnItemsQueryHandler(IDbConnection dbConnection)
        {
            this.dbConnection = dbConnection;
        }

        public bool Search(DoesAttributeExistOnItemsParameters parameters)
        {
            return dbConnection.QueryFirst<bool>(
                @"DECLARE @jsonPath NVARCHAR(259) = '$.""' + (SELECT AttributeName FROM Attributes WHERE AttributeId = @AttributeId) + '""'
                IF EXISTS(
                        SELECT TOP 1 1
                        FROM Item i
                        WHERE JSON_VALUE(i.ItemAttributesJson, @jsonPath) IS NOT NULL
                        )
                    SELECT CAST(1 AS BIT)
                ELSE
                    SELECT CAST(0 AS BIT)",
                parameters);
        }
    }
}
