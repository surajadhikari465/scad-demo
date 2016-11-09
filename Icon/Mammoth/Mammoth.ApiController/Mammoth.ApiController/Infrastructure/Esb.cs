using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mammoth.ApiController.Infrastructure
{
    public static class Esb
    {
        public const string BusinessUnitTraitCode = "BU";
        public const string BusinessUnitTraitDescription = "PS Business Unit ID";

        public static class ItemPrice
        {
            public static class Descriptions
            {
                public const string Reg = "Regular Price";
                public const string Tpr = "Temporary Price Reduction";
            }

            public static class Ids
            {
                public const int Reg = 1;
                public const int Tpr = 2;
            }
        }
    }
}
