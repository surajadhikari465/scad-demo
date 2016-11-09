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
    
    public partial class MessageHistory
    {
        public MessageHistory()
        {
            this.MessageQueueHierarchy = new HashSet<MessageQueueHierarchy>();
            this.MessageQueueLocale = new HashSet<MessageQueueLocale>();
            this.MessageQueueProduct = new HashSet<MessageQueueProduct>();
            this.MessageQueueItemLocale = new HashSet<MessageQueueItemLocale>();
            this.MessageQueuePrice = new HashSet<MessageQueuePrice>();
            this.MessageQueueProductSelectionGroup = new HashSet<MessageQueueProductSelectionGroup>();
            this.R10MessageResponse = new HashSet<R10MessageResponse>();
            this.MessageResendStatus = new HashSet<MessageResendStatus>();
        }
    
        public int MessageHistoryId { get; set; }
        public int MessageTypeId { get; set; }
        public int MessageStatusId { get; set; }
        public string Message { get; set; }
        public System.DateTime InsertDate { get; set; }
        public Nullable<int> InProcessBy { get; set; }
        public Nullable<System.DateTime> ProcessedDate { get; set; }
    
        public virtual MessageStatus MessageStatus { get; set; }
        public virtual MessageType MessageType { get; set; }
        public virtual ICollection<MessageQueueHierarchy> MessageQueueHierarchy { get; set; }
        public virtual ICollection<MessageQueueLocale> MessageQueueLocale { get; set; }
        public virtual ICollection<MessageQueueProduct> MessageQueueProduct { get; set; }
        public virtual ICollection<MessageQueueItemLocale> MessageQueueItemLocale { get; set; }
        public virtual ICollection<MessageQueuePrice> MessageQueuePrice { get; set; }
        public virtual ICollection<MessageQueueProductSelectionGroup> MessageQueueProductSelectionGroup { get; set; }
        public virtual ICollection<R10MessageResponse> R10MessageResponse { get; set; }
        public virtual ICollection<MessageResendStatus> MessageResendStatus { get; set; }
    }
}
