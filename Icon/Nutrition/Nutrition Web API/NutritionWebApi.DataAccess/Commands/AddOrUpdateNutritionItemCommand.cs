using NutritionWebApi.Common.Models;
using System.Collections.Generic;

namespace NutritionWebApi.DataAccess.Commands
{
    public class AddOrUpdateNutritionItemCommand
    {
        public List<NutritionItemModel> NutritionItems { get; set; }
    }
}
