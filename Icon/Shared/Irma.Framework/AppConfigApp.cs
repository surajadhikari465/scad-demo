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
    
    public partial class AppConfigApp
    {
        public System.Guid ApplicationID { get; set; }
        public System.Guid EnvironmentID { get; set; }
        public int TypeID { get; set; }
        public string Name { get; set; }
        public string Configuration { get; set; }
        public bool Deleted { get; set; }
        public System.DateTime LastUpdate { get; set; }
        public int LastUpdateUserID { get; set; }
    
        public virtual AppConfigEnv AppConfigEnv { get; set; }
        public virtual Users Users { get; set; }
    }
}
