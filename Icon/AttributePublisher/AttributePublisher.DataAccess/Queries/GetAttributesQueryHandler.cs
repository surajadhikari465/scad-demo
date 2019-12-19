using AttributePublisher.DataAccess.Models;
using Dapper;
using Icon.Common.DataAccess;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace AttributePublisher.DataAccess.Queries
{
    public class GetAttributesQueryHandler : IQueryHandler<GetAttributesParameters, List<AttributeModel>>
    {
        private IDbConnection dbConnection;

        public GetAttributesQueryHandler(IDbConnection dbConnection)
        {
            this.dbConnection = dbConnection;
        }

        public List<AttributeModel> Search(GetAttributesParameters parameters)
        {
            string sql = @"
                    DELETE TOP(@RecordsPerQuery) q
                        OUTPUT deleted.AttributeId,
                            a.AttributeName,
                            a.TraitCode,
                            a.XmlTraitDescription,
                            dt.DataType,
                            ag.AttributeGroupName
                    FROM esb.MessageQueueAttribute q
                    JOIN dbo.Attributes a ON q.AttributeId = a.AttributeId
                    JOIN dbo.AttributeGroup ag on a.AttributeGroupId = ag.AttributeGroupId
                    JOIN dbo.DataType dt on a.DataTypeId = dt.DataTypeId";

            var attributeModels = dbConnection
                .Query<AttributeModel>(sql, parameters)
                .ToList();

            return attributeModels;
        }
    }
}
