using GlobalEventController.DataAccess.Infrastructure;
using Irma.Framework;
using System.Linq;

namespace GlobalEventController.DataAccess.Queries
{
    public class GetItemNutriFactsQueryHandler : IQueryHandler<GetItemNutriFactsQuery, ItemNutrition>
    {
        private IrmaContext context;

        public GetItemNutriFactsQueryHandler(IrmaContext context)
        {
            this.context = context;
        }

        public ItemNutrition Handle(GetItemNutriFactsQuery parameters)
        {
            IQueryable<ItemNutrition> query = context.ItemNutrition.Where(isc => isc.ItemKey == parameters.ItemKey);
            return query.SingleOrDefault();
        }
    }
}
