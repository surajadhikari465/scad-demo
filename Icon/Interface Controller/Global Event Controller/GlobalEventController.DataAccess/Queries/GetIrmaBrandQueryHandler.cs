using GlobalEventController.DataAccess.Infrastructure;
using Icon.DbContextFactory;
using Irma.Framework;
using System.Linq;

namespace GlobalEventController.DataAccess.Queries
{
    /// <summary>
    /// Queries the IRMA database to find the IRMA Brand corresponding with the supplied
    ///   Icon Brands Hierarchy Class ID. Joins the ItemBrand & ValidatedBrand tables
    ///   and searches using the IconBrandId parameter to find the IRMA ItemBrand which
    ///   is cross-referenced with the Icon ID parameter (if any). Returns a default (null)
    ///   object if no matching record is found.
    /// </summary>
    public class GetIrmaBrandQueryHandler : IQueryHandler<GetIrmaBrandQuery, ItemBrand>
    {
        private IDbContextFactory<IrmaContext> contextFactory;

        public GetIrmaBrandQueryHandler(IDbContextFactory<IrmaContext> contextFactory)
        {
            this.contextFactory = contextFactory;
        }

        public ItemBrand Handle(GetIrmaBrandQuery parameters)
        {
            using (var context = contextFactory.CreateContext())
            {
                var queryResult = (from vb in context.ValidatedBrand
                                   join ib in context.ItemBrand on vb.IrmaBrandId equals ib.Brand_ID
                                   where vb.IconBrandId == parameters.IconBrandId
                                   select new { ItemBrand = ib, ResultItemCount = ib.Item.Count }
                    ).SingleOrDefault();
                if (queryResult == null) return null;
                parameters.ResultItemCount = queryResult.ResultItemCount;
                return queryResult.ItemBrand;
            }
        }
    }
}
