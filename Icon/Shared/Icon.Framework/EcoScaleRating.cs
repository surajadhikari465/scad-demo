//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Icon.Framework
{
    using System;
    using System.Collections.Generic;
    
    public partial class EcoScaleRating
    {
        public EcoScaleRating()
        {
            this.ItemSignAttribute = new HashSet<ItemSignAttribute>();
        }
    
        public int EcoScaleRatingId { get; set; }
        public string Description { get; set; }
    
        public virtual ICollection<ItemSignAttribute> ItemSignAttribute { get; set; }
    }
}
