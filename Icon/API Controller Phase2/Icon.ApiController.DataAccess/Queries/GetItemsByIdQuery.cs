using Icon.RenewableContext;
using Icon.Common.DataAccess;
using Icon.Framework;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Icon.ApiController.DataAccess.Queries
{
    public class GetItemsByIdQuery : IQueryHandler<GetItemsByIdParameters, List<Item>>
    {
        private IRenewableContext<IconContext> globalContext;

        public GetItemsByIdQuery(IRenewableContext<IconContext> globalContext)
        {
            this.globalContext = globalContext;
        }

        public List<Item> Search(GetItemsByIdParameters parameters)
        {
            List<Item> items = globalContext.Context.Item
                .Include(i => i.ItemHierarchyClass.Select(ihc => ihc.HierarchyClass.HierarchyClassTrait))
                .Include(i => i.ItemHierarchyClass.Select(ihc => ihc.HierarchyClass.Hierarchy))
                .Include(i => i.ItemTrait.Select(it => it.Trait))
                .Include(i => i.ScanCode.Select(sc => sc.ScanCodeType))
                .Include(i => i.ItemType)
                .Where(i => parameters.ItemsById.Contains(i.itemID))
                .ToList();

            return items;
        }
    }
}
