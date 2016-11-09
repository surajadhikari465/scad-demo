using GlobalEventController.DataAccess.Infrastructure;
using Icon.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalEventController.DataAccess.Queries
{
    public class GetIconItemNutritionQuery : IQuery<List<ItemNutrition>>
    {
        public List<string> ScanCodes { get; set; }
    }
}
