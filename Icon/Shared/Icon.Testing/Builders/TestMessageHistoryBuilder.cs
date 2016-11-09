using Icon.Framework;
using System;
using System.Collections.Generic;

namespace Icon.Testing.Builders
{
    public class TestMessageHistoryBuilder
    {
        private int messageTypeId;
        private int messageStatusId;
        private string message;
        private DateTime insertDate;
        private int? inProcessBy;
        private DateTime? processedDate;

        ICollection<MessageQueueProduct> messageQueueProduct;

        public TestMessageHistoryBuilder()
        {
            this.messageTypeId = MessageTypes.Product;
            this.messageStatusId = 1;
            this.message = String.Empty;
            this.insertDate = DateTime.Now;
            this.inProcessBy = null;
            this.processedDate = null;
            this.messageQueueProduct = new List<MessageQueueProduct>();
        }

        public TestMessageHistoryBuilder WithMessageStatusId(int messageStatusId)
        {
            this.messageStatusId = messageStatusId;
            return this;
        }

        public TestMessageHistoryBuilder WithInProcessBy(int? inProcessBy)
        {
            this.inProcessBy = inProcessBy;
            return this;
        }

        public TestMessageHistoryBuilder WithMessageTypeId(int messageTypeId)
        {
            this.messageTypeId = messageTypeId;
            return this;
        }

        public TestMessageHistoryBuilder WithMessage(string message)
        {
            this.message = message;
            return this;
        }

        public MessageHistory Build()
        {
            return new MessageHistory
            {
                MessageTypeId = this.messageTypeId,
                MessageStatusId = this.messageStatusId,
                Message = this.message,
                InsertDate = this.insertDate,
                InProcessBy = this.inProcessBy,
                ProcessedDate = this.processedDate,
                MessageQueueProduct = this.messageQueueProduct
            };
        }

        public static implicit operator MessageHistory(TestMessageHistoryBuilder builder)
        {
            return builder.Build();
        }

        public TestMessageHistoryBuilder WithProductMessageQueue(ICollection<MessageQueueProduct> productMessageQueue)
        {
            this.messageQueueProduct = productMessageQueue;
            return this;
        }
    }
}
