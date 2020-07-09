using Dapper;
using Icon.Common.DataAccess;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Icon.Common.Models;

namespace Icon.Web.DataAccess.Queries
{
    public class GetAttributesQueryHandler : IQueryHandler<EmptyQueryParameters<IEnumerable<AttributeModel>>, IEnumerable<AttributeModel>>
    {
        private IDbConnection connection;
        
        public GetAttributesQueryHandler(IDbConnection connection)
        {
            this.connection = connection;
        }

        public IEnumerable<AttributeModel> Search(EmptyQueryParameters<IEnumerable<AttributeModel>> parameters)
        {
            var results = connection.QueryMultiple("app.GetItemAttributesList", commandType: CommandType.StoredProcedure);
            var attributes = results.Read<AttributeModel>()
                .ToList();
            var pickListDataGroups = results.Read<PickListModel>()
                .GroupBy(p => p.AttributeId);
            var attributeCharacterSetGroups = results.Read<AttributeCharacterSetModel, CharacterSetModel, AttributeCharacterSetModel>(
                (attributeCharacterSetModel, characterSetModel) =>
                {
                    attributeCharacterSetModel.CharacterSetModel = characterSetModel;
                    return attributeCharacterSetModel;
                },
                "CharacterSetId")
                .GroupBy(c => c.AttributeId);
            
            attributes.ForEach(a =>
            {
                a.PickListData = pickListDataGroups.FirstOrDefault(g => g.Key == a.AttributeId);
                a.CharacterSets = attributeCharacterSetGroups.FirstOrDefault(g => g.Key == a.AttributeId);                
            });

            return attributes;
        }       
    }
}