using System.Collections.Generic;
using NutritionWebApi.Common;
using NutritionWebApi.Common.Models;

namespace NutritionWebApi.DataAccess.Queries
{
    public class GetNutritionItemQuery : IQuery<List<NutritionItemModel>>
    {
        public string Plu { get; set; }
    }
}
