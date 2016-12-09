using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infor.Services.NewItem.Constants
{
    public static class CustomRegexPatterns
    {
        public const string ScalePlu = "^2[0-9]{5}0{5}$";
        public const string IngredientPlu46 = "^460{4}[0-9]{5}$";
        public const string IngredientPlu48 = "^480{4}[0-9]{5}$";
    }
}
