using Esb.Core.EsbServices;
using Icon.Framework;
using System;

namespace Icon.Infor.Listeners.HierarchyClass.Models
{
    public class VimMessageHistory
    {
        public int MessageHistoryId { get; set; }
        public Guid EsbMessageId { get; set; }
        public int MessageTypeId { get; set; }
        public int MessageStatusId { get; set; }
        public string Message { get; set; }

        public VimMessageHistory() { }

        public VimMessageHistory(EsbServiceResponse response)
        {
            this.EsbMessageId = Guid.Parse(response.Message.MessageId);
            this.MessageTypeId = MessageTypes.Hierarchy;
            this.MessageStatusId = GetMessageStatus(response.Status);
            this.Message = response.Message.Text;
        }

        private int GetMessageStatus(EsbServiceResponseStatus status)
        {
            switch(status)
            {
                case EsbServiceResponseStatus.Sent: return MessageStatusTypes.Sent;
                case EsbServiceResponseStatus.Failed: return MessageStatusTypes.Failed;
                default: throw new ArgumentException($"No message status type is defined for the response status '{status}'.");
            }
        }
    }
}
