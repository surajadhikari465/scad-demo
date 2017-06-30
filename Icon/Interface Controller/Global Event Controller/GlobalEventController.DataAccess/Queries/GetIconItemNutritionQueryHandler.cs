using GlobalEventController.DataAccess.Infrastructure;
using Icon.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace GlobalEventController.DataAccess.Queries
{
    public class GetIconItemNutritionQueryHandler : IQueryHandler<GetIconItemNutritionQuery, List<ItemNutrition>>
    {
        private Icon.DbContextFactory.IDbContextFactory<IconContext> contextFactory;

        public GetIconItemNutritionQueryHandler(Icon.DbContextFactory.IDbContextFactory<IconContext> contextFactory)
        {
            this.contextFactory = contextFactory;
        }

        public List<ItemNutrition> Handle(GetIconItemNutritionQuery parameters)
        {
            using (var context = contextFactory.CreateContext())
            {
                List<ItemNutrition> nutritionItems = context.ItemNutrition
                    .Where(nt => parameters.ScanCodes.Contains(nt.Plu))
                    .ToList();

                return nutritionItems;
            }
        }
    }
}
