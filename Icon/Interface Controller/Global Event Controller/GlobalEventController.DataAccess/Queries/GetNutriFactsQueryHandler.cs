using GlobalEventController.DataAccess.Infrastructure;
using Irma.Framework;
using System.Collections.Generic;
using System.Linq;

namespace GlobalEventController.DataAccess.Queries
{
    public class GetNutriFactsQueryHandler : IQueryHandler<GetNutriFactsQuery, List<NutriFacts>>
    {
        private IrmaContext context;

        public GetNutriFactsQueryHandler(IrmaContext context)
        {
            this.context = context;
        }

        public List<NutriFacts> Handle(GetNutriFactsQuery parameters)
        {
            IQueryable<NutriFacts> query = context.NutriFacts.Where(nc => nc.ItemNutrition.Any(inc => inc.Item.ItemIdentifier.Any(iii => iii.Identifier == parameters.ScanCode && iii.Deleted_Identifier == 0 && iii.Default_Identifier == 1)));

            return query.ToList();
        }
    }
}
