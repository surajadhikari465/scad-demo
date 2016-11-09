using Icon.Esb.R10Listener.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icon.Esb.R10Listener.Infrastructure.Cache
{
    public interface IMessageQueueResendStatusCache
    {
        MessageQueueResendStatus Get(int messageTypeId, int messageQueueId);
        void AddOrUpdate(int messageTypeId, MessageQueueResendStatus resendStatus);
    }
}
