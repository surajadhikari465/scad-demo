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
    
    public partial class AddressUsage
    {
        public AddressUsage()
        {
            this.LocaleAddress = new HashSet<LocaleAddress>();
        }
    
        public int addressUsageID { get; set; }
        public string addressUsageCode { get; set; }
        public string addressUsageDesc { get; set; }
    
        public virtual ICollection<LocaleAddress> LocaleAddress { get; set; }
    }
}
