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
    
    public partial class APIMessageProcessorLogEntry
    {
        public int APIMessageMonitorLogID { get; set; }
        public int MessageTypeID { get; set; }
        public Nullable<System.DateTime> StartTime { get; set; }
        public Nullable<System.DateTime> EndTime { get; set; }
        public Nullable<int> CountProcessedMessages { get; set; }
        public Nullable<int> CountFailedMessages { get; set; }
    
        public virtual MessageType MessageType { get; set; }
    }
}
