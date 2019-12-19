using Icon.Common.DataAccess;
using Icon.Framework;
using Icon.Web.DataAccess.Extensions;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Icon.Web.DataAccess.Commands
{
    public class AddProductMessagesBySubTeamCommandHandler : ICommandHandler<AddProductMessagesBySubTeamCommand>
    {
        private IconContext context;

        public AddProductMessagesBySubTeamCommandHandler(IconContext context)
        {
            this.context = context;
        }

        public void Execute(AddProductMessagesBySubTeamCommand data)
        {
            // Get list of hierarchyClassIDs that have the traitValue equal to the subTeam
            List<int> leafMerchAssociations = this.context.HierarchyClass
                .Where(hc => hc.HierarchyClassTrait.Any(hct => hct.traitValue == data.NewSubTeam))
                .Select(hc => hc.hierarchyClassID)
                .ToList();

            // Get list of all the items associated to the hierarchyClassIDs found above
            var associatedItems = this.context.Item
                .Where(i => i.ItemHierarchyClass
                    .Any(ihc => leafMerchAssociations.Contains(ihc.hierarchyClassID))
                        && i.ItemTrait.Any(it => it.Trait.traitCode == TraitCodes.ValidationDate))
                .Select(i => new
                    {
                        itemID = i.ItemId
                })
                .ToList();

            SqlParameter items = new SqlParameter("updatedItemIDs", SqlDbType.Structured);
            items.TypeName = "app.UpdatedItemIDsType";
            items.Value = associatedItems.ToDataTable();

            string sql = "EXEC app.GenerateItemUpdateMessages @updatedItemIDs";
            this.context.Database.ExecuteSqlCommand(sql, items);
        }
    }
}
