using Icon.Common.DataAccess;
using System.Collections.Generic;
using System.Linq;
using Dapper;
using System.Data;
using Icon.Common.Models;

namespace Icon.Web.DataAccess.Queries
{
    public class GetCharacterSetsByAttributeQuery : IQueryHandler<GetCharacterSetsByAttributeParameters, List<AttributeCharacterSetModel>>
    {
        private IDbConnection connection;


        public GetCharacterSetsByAttributeQuery(IDbConnection connection)
        {
            this.connection = connection;
        }

        public List<AttributeCharacterSetModel> Search(GetCharacterSetsByAttributeParameters parameters)
        {
            return connection.Query<AttributeCharacterSetModel, CharacterSetModel,AttributeCharacterSetModel>(@"
                           SELECT  AttributeCharacterSetID
                                  ,AttributeId
                                  ,AttributeCharacterSets.CharacterSetId   
                                  ,Name
                                  ,RegEx  
                                 FROM dbo.AttributeCharacterSets
                                 INNER JOIN dbo.CharacterSets ON dbo.AttributeCharacterSets.CharacterSetId = dbo.CharacterSets.CharacterSetId
                                 WHERE AttributeId = @AttributeId",         
                                  (attributeCharacterSetModel, characterSetModel) => {
                                      attributeCharacterSetModel.CharacterSetModel = characterSetModel;
                                      return attributeCharacterSetModel;
                                  }, new { AttributeId = parameters.AttributeId },
                                 splitOn: "CharacterSetId").ToList();
        }
    }
}
