//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Testing.Core.Models.Irma
{
    using System;
    using System.Collections.Generic;
    
    public partial class StoreItem
    {
        public int StoreItemAuthorizationID { get; set; }
        public int Store_No { get; set; }
        public int Item_Key { get; set; }
        public bool Authorized { get; set; }
        public bool POSDeAuth { get; set; }
        public bool ScaleAuth { get; set; }
        public bool ScaleDeAuth { get; set; }
        public bool Refresh { get; set; }
        public Nullable<bool> ECommerce { get; set; }
    }
}
