using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icon.Esb.R10Listener.Models
{
    public class MessageQueueResendStatus
    {
        public int MessageQueueId { get; set; }
        public int NumberOfResends { get; set; }

        public override int GetHashCode()
        {
            return MessageQueueId.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            MessageQueueResendStatus other = obj as MessageQueueResendStatus;

            if(other == null)
            {
                return false;
            }
            else
            {
                return this.MessageQueueId == other.MessageQueueId;
            }
        }
    }
}
