using Dapper;
using Icon.Common.DataAccess;
using System.Data;
using System.Linq;
using Icon.Common.Models;

namespace Icon.Web.DataAccess.Queries
{
    public class GetAttributeByNameGroupQueryHandler : IQueryHandler<GetAttributeByNameGroupParameters, AttributeModel>
    {
        private IDbConnection connection;
        
        public GetAttributeByNameGroupQueryHandler(IDbConnection connection)
        {
            this.connection = connection;
        }

        public AttributeModel Search(GetAttributeByNameGroupParameters parameters)
        {
            var results = connection.QueryMultiple("app.GetAttributebyNameGroup", param: new { attributeName = parameters.AttributeName, attributeGroupName = parameters.AttributeGroupName }, commandType: CommandType.StoredProcedure);
            var attribute = results.Read<AttributeModel>().FirstOrDefault();

            var pickListData = results.Read<PickListModel>();

            var attributeCharacterSetGroups = results.Read<AttributeCharacterSetModel, CharacterSetModel, AttributeCharacterSetModel>(
                (attributeCharacterSetModel, characterSetModel) =>
                {
                    attributeCharacterSetModel.CharacterSetModel = characterSetModel;
                    return attributeCharacterSetModel;
                },
                "CharacterSetId");

                attribute.PickListData = pickListData;
                attribute.CharacterSets = attributeCharacterSetGroups;                

            return attribute;
        }       
    }
}