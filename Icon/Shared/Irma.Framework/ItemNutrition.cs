//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Irma.Framework
{
    using System;
    using System.Collections.Generic;
    
    public partial class ItemNutrition
    {
        public int ItemNutritionId { get; set; }
        public int ItemKey { get; set; }
        public Nullable<int> NutriFactsID { get; set; }
        public Nullable<int> Scale_Ingredient_ID { get; set; }
        public Nullable<int> Scale_Allergen_ID { get; set; }
        public Nullable<int> Item_ExtraText_ID { get; set; }
    
        public virtual Item Item { get; set; }
        public virtual NutriFacts NutriFacts { get; set; }
    }
}
