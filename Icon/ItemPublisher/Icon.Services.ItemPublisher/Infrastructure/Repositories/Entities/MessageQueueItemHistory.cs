using Icon.Web.Common.Lib;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Icon.Services.NewItem.Repositories.Entities
{
    public class MessageQueueItemArchive
    {
        public int MessageQueueItemArchiveId { get; private set; }

        public string MessageQueueItemJson { get; private set; }

        public string Message { get; private set; }

        public Guid MessageId { get; private set; }

        public string MessageHeader { get; private set; }

        public string ErrorMessage { get; private set; }

        public bool ErrorOccurred { get; private set; }

        public string WarningMessage { get; private set; }


        public MessageQueueItemArchive()
        {
        }

        public MessageQueueItemArchive(List<MessageQueueItemModel> messageQueueItem, Guid messageId, string esbMessage, Dictionary<string, string> esbMessageHeaders, string errorMessage, List<string> warningMessages, bool success)
        {
            this.Message = esbMessage;
            this.MessageId = messageId;
            this.MessageHeader = JsonConvert.SerializeObject(esbMessageHeaders);
            this.ErrorMessage = errorMessage;
            this.WarningMessage = string.Join(Environment.NewLine, warningMessages.ToArray());
            this.MessageQueueItemJson = JsonConvert.SerializeObject(messageQueueItem);
            this.ErrorOccurred = !success;
        }
    }
}