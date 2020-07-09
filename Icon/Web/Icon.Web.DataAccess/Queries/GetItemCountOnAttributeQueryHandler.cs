using Icon.Common.DataAccess;
using System.Linq;
using Dapper;
using System.Data;
using Icon.Common.Models;
using System.Collections.Generic;

namespace Icon.Web.DataAccess.Queries
{
    public class GetItemCountOnAttributeQueryHandler : IQueryHandler<EmptyAttributesParameters, IEnumerable<AttributeModel>>
    {
        private IDbConnection connection;

        public GetItemCountOnAttributeQueryHandler(IDbConnection connection)
        {
            this.connection = connection;
        }

        public IEnumerable<AttributeModel> Search(EmptyAttributesParameters parameters)
        {
            var results = connection.QueryMultiple("app.GetAttributesListWithItemCount", commandType: CommandType.StoredProcedure);
            var attributes = results.Read<AttributeModel>().OrderBy(a => a.DisplayName)
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
            var attributesCount = results.Read<AttributeItemCountModel>().ToList();
            var itemAttributeGroupId= results.Read<int>().FirstOrDefault();

            attributes.ForEach(a =>
            {
                a.PickListData = pickListDataGroups.FirstOrDefault(g => g.Key == a.AttributeId);
                a.CharacterSets = attributeCharacterSetGroups.FirstOrDefault(g => g.Key == a.AttributeId);
                a.ItemCount = attributesCount.FirstOrDefault(g => g.AttributeName == a.AttributeName && a.AttributeGroupId == itemAttributeGroupId)?.ItemCount ?? 0;
            });

            return attributes;
        }
    }
}