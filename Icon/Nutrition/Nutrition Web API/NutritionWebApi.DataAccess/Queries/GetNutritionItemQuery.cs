using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NutritionWebApi.Common;
using NutritionWebApi.Common.Models;

namespace NutritionWebApi.DataAccess.Queries
{
    public class GetNutritionItemQuery : IQuery<List<NutritionItemModel>>
    {
        public string Plu { get; set; }
    }
}
