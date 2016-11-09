using GlobalEventController.DataAccess.Infrastructure;
using Irma.Framework;

namespace GlobalEventController.DataAccess.Queries
{
    public class GetItemNutriFactsQuery : IQuery<ItemNutrition>
    {
        public int ItemKey { get; set; }
    }
}
