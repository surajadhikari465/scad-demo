using System;

namespace Icon.Dvs.Model
{
    public class DvsMessage
    {
        public string MessageContent { get; }
        public DvsSqsMessage SqsMessage { get; }

        public DvsMessage(DvsSqsMessage queueMessage, string messageContent)
        {
            this.SqsMessage = queueMessage;
            this.MessageContent = messageContent;
        }
    }
}

