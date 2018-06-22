using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icon.Infor.Listeners.Item.Constants.ItemValidation
{
    public static class MaxValues
    {
        public const decimal RetailSize         = 99999.9999m;
        public const int RetailSizeScale        =     4;
        public const int ShelfLife              =   100;
    }

    public static class MaxLengths
    {
        public const int StandardProperty255    = 255;
        public const int ScanCode               =  13;
        public const int PackageUnit            =   3;
        public const int PosDescription         =  25;
        public const int ProductDescription     =  60;
        public const int CFD                    =  60;
        public const int SelfCheckoutItemTareGroup =  60;
        public const int FlexibleText           = 300;
    }

    public static class RegularExpressions
    {
        public const string ScanCode            = "^[0-9]{1,13}$";
        public const string TaxCode             = "^[0-9]{7}$";
        public const string NumericHierarchyClassId = "^[1-9][0-9]*$";
        public const string SubTeamNumber       = "^[0-9]{4}$";
        public const string ScalePlu            = "^2[0-9]{5}0{5}$";
        public const string IngredientPlu46     = "^460{4}[0-9]{5}$";
        public const string IngredientPlu48     = "^480{4}[0-9]{5}$";
        public const string Empty               = "^$";
        public const string BooleanZeroOrOne    = "^[0|1]?$";
        public const string BooleanYesNo        = "^[Yy][Ee][Ss]$|^[Nn][Oo]$";
        public const string BooleanTrueFalse    = "^[Tt][Rr][Uu][Ee]$|^[Ff][Aa][Ll][Ss][Ee]$";
        public const string BooleanOnOff        = "^[Oo][Nn]$|^[Oo][Ff][Ff]$";
        public const string BooleanCatchall     = "^[YyNnTtFf01]$" + "|" 
            + BooleanYesNo + "|" + BooleanTrueFalse + "|" + BooleanOnOff + "|" + Empty;
    }
}
