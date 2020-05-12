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
                            a.Description,
                            a.TraitCode,
                            a.MaxLengthAllowed,
                            a.MinimumNumber,
                            a.MaximumNumber,
                            a.NumberOfDecimals,                            
                            a.XmlTraitDescription,
                            awc.CharacterSetRegexPattern,
                            ag.AttributeGroupId,
                            ag.AttributeGroupName,
                            dt.DataType,
                            a.IsPickList
                    FROM esb.MessageQueueAttribute q
                    JOIN dbo.Attributes a ON q.AttributeId = a.AttributeId
                    JOIN dbo.AttributeGroup ag on a.AttributeGroupId = ag.AttributeGroupId
                    JOIN dbo.DataType dt on a.DataTypeId = dt.DataTypeId
                    JOIN dbo.AttributesWebConfiguration awc on awc.AttributeId = a.AttributeId";

            var attributeModels = dbConnection
                .Query<AttributeModel>(sql, parameters)
                .ToList();

            foreach (var attributeModel in attributeModels)
            {
                if(attributeModel.IsPickList)
                {
                    attributeModel.PickListValues = dbConnection.Query<string>(
                        @"SELECT PickListValue 
                        FROM PickListData 
                        WHERE AttributeId = @AttributeId", 
                        attributeModel).ToList();
                }
            }

            return attributeModels;
        }
    }
}
