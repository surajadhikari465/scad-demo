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
    
    public partial class MessageType
    {
        public MessageType()
        {
            this.MessageQueueHierarchy = new HashSet<MessageQueueHierarchy>();
            this.MessageQueueItemLocale = new HashSet<MessageQueueItemLocale>();
            this.MessageQueuePrice = new HashSet<MessageQueuePrice>();
            this.MessageQueueProductSelectionGroup = new HashSet<MessageQueueProductSelectionGroup>();
            this.MessageQueueBusinessUnitInProcess = new HashSet<MessageQueueBusinessUnitInProcess>();
            this.APIMessageMonitorLog = new HashSet<APIMessageProcessorLogEntry>();
            this.MessageHistory = new HashSet<MessageHistory>();
            this.MessageQueueProduct = new HashSet<MessageQueueProduct>();
            this.MessageQueueLocale = new HashSet<MessageQueueLocale>();
        }
    
        public int MessageTypeId { get; set; }
        public string MessageTypeName { get; set; }
    
        public virtual ICollection<MessageQueueHierarchy> MessageQueueHierarchy { get; set; }
        public virtual ICollection<MessageQueueItemLocale> MessageQueueItemLocale { get; set; }
        public virtual ICollection<MessageQueuePrice> MessageQueuePrice { get; set; }
        public virtual ICollection<MessageQueueProductSelectionGroup> MessageQueueProductSelectionGroup { get; set; }
        public virtual ICollection<MessageQueueBusinessUnitInProcess> MessageQueueBusinessUnitInProcess { get; set; }
        public virtual ICollection<APIMessageProcessorLogEntry> APIMessageMonitorLog { get; set; }
        public virtual ICollection<MessageHistory> MessageHistory { get; set; }
        public virtual ICollection<MessageQueueProduct> MessageQueueProduct { get; set; }
        public virtual ICollection<MessageQueueLocale> MessageQueueLocale { get; set; }
    }
}
