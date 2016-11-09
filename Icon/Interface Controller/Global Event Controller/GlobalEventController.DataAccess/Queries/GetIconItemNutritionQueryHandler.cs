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
        private readonly ContextManager contextManager;

        public GetIconItemNutritionQueryHandler(ContextManager contextManager)
        {
            this.contextManager = contextManager;
        }

        public List<ItemNutrition> Handle(GetIconItemNutritionQuery parameters)
        {
            List<ItemNutrition> nutritionItems = contextManager.IconContext.ItemNutrition                
                .Where(nt => parameters.ScanCodes.Contains(nt.Plu))
                .ToList();
           
                return nutritionItems;
        }
    }
}
