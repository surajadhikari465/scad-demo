using Icon.RenewableContext;
using Icon.Common.DataAccess;
using Icon.Framework;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Icon.DbContextFactory;

namespace Icon.ApiController.DataAccess.Queries
{
    public class GetItemsByIdQuery : IQueryHandler<GetItemsByIdParameters, List<Item>>
    {
        private IDbContextFactory<IconContext> iconContextFactory;

        public GetItemsByIdQuery(IDbContextFactory<IconContext> iconContextFactory)
        {
            this.iconContextFactory = iconContextFactory;
        }

        public List<Item> Search(GetItemsByIdParameters parameters)
        {
            using (var context = iconContextFactory.CreateContext())
            {
                List<Item> items = context.Item
                    .Include(i => i.ItemHierarchyClass.Select(ihc => ihc.HierarchyClass.HierarchyClassTrait))
                    .Include(i => i.ItemHierarchyClass.Select(ihc => ihc.HierarchyClass.Hierarchy))
                    .Include(i => i.ItemTrait.Select(it => it.Trait))
                    .Include(i => i.ScanCode.Select(sc => sc.ScanCodeType))
                    .Include(i => i.ItemType)
                    .Where(i => parameters.ItemsById.Contains(i.ItemId))
                    .ToList();

                return items;
            }
        }
    }
}
