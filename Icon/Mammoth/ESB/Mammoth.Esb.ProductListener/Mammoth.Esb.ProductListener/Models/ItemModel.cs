using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mammoth.Esb.ProductListener.Models
{
    public class ItemModel
    {
        public GlobalAttributesModel GlobalAttributes { get; set; }
        public SignAttributesModel SignAttributes { get; set; }
        public NutritionAttributesModel NutritionAttributes { get; set; }
        public ExtendedAttributesModel ExtendedAttributes { get; set; }
    }
}
