using System.Collections.Generic;
using Mammoth.Common.DataAccess.CommandQuery;
using MammothWebApi.DataAccess.Models;

namespace MammothWebApi.DataAccess.Queries
{
    public class GetItemNutritionAttributesByItemIdQuery : IQuery<IEnumerable<ItemNutritionAttributes>>
    {
        public List<int> ItemIds { get; set; }
    }
}