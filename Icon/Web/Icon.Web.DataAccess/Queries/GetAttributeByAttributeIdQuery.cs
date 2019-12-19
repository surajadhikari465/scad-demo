using Icon.Common.DataAccess;
using System.Linq;
using Dapper;
using System.Data;
using Icon.Common.Models;

namespace Icon.Web.DataAccess.Queries
{
    public class GetAttributeByAttributeIdQuery : IQueryHandler<GetAttributeByAttributeIdParameters, AttributeModel>
    {
        private IDbConnection connection;


        public GetAttributeByAttributeIdQuery(IDbConnection connection)
        {
            this.connection = connection;
        }

        public AttributeModel Search(GetAttributeByAttributeIdParameters parameters)
        {
            return connection.Query<AttributeModel>(@"
                           SELECT  [AttributeId]
                                  ,[DisplayName]
                                  ,[AttributeName]
                                  ,[AttributeGroupId]
                                  ,[HasUniqueValues]
                                  ,[Description]
                                  ,[DefaultValue]
                                  ,[IsRequired]
                                  ,[SpecialCharactersAllowed]
                                  ,[TraitCode]
                                  ,[DataTypeId]
                                  ,[DisplayOrder]
                                  ,[InitialValue]
                                  ,[IncrementBy]
                                  ,[InitialMax]
                                  ,[DisplayType]
                                  ,[MaxLengthAllowed]
                                  ,[MinimumNumber]
                                  ,[MaximumNumber]
                                  ,[NumberOfDecimals]
                                  ,[IsPickList]
                                 FROM [dbo].[Attributes]
                                 WHERE AttributeId = @AttributeId",
                                new { AttributeId = parameters.AttributeId }).FirstOrDefault();
        }
    }
}
