using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icon.Infor.Listeners.Item.Constants
{
    public static class CustomRegexPatterns
    {
        public const string BooleanValue = "^[0|1]?$";
        public const string ScanCode = "^[0-9]{1,13}$";
        public const string TaxCode = "^[0-9]{7}$";
        public const string NumericHierarchyClassId = "^[1-9][0-9]*$";
        public const string SubTeamNumber = "^[0-9]{4}$";
        public const string ScalePlu = "^2[0-9]{5}0{5}$";
        public const string IngredientPlu46 = "^460{4}[0-9]{5}$";
        public const string IngredientPlu48 = "^480{4}[0-9]{5}$";
    }
}
