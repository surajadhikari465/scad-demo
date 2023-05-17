﻿using Icon.Services.ItemPublisher.Infrastructure.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Icon.Services.ItemPublisher.Repositories.Entities
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

        public DateTime InsertDateUTC { get; private set; }

        public string Machine { get; private set; }

        public MessageQueueItemArchive()
        {
        }

        public MessageQueueItemArchive(List<MessageQueueItemModel> messageQueueItem, Guid messageId, string message, Dictionary<string, string> messageHeaders, string errorMessage, List<string> warningMessages, DateTime insertDateUTC, string machine, bool success)
        {
            this.Message = message;
            this.MessageId = messageId;
            this.MessageHeader = JsonConvert.SerializeObject(messageHeaders);
            this.ErrorMessage = errorMessage;
            this.WarningMessage = string.Join(Environment.NewLine, warningMessages.ToArray());
            this.MessageQueueItemJson = JsonConvert.SerializeObject(messageQueueItem);
            this.ErrorOccurred = !success;
            this.InsertDateUTC = insertDateUTC;
            this.Machine = machine;
        }
    }
}