using Irma.Framework;
using System;

namespace Irma.Testing.Builders
{
    public class TestIconItemChangeQueueBuilder
    {
        private int queueId;
        private int itemKey;
        private string identifier;
        private byte itemChgTypeID;
        private DateTime insertDate;
        private DateTime? processFailedDate;
        private string inProcessBy;

        public TestIconItemChangeQueueBuilder()
        {
            this.queueId = 0;
            this.itemKey = 0;
            this.identifier = "123412341234";
            this.itemChgTypeID = 1;
            this.insertDate = DateTime.Now;
            this.processFailedDate = null;
            this.inProcessBy = null;
        }

        public TestIconItemChangeQueueBuilder WithQueueId(int queueId)
        {
            this.queueId = queueId;
            return this;
        }

        public TestIconItemChangeQueueBuilder WithItemKey(int itemKey)
        {
            this.itemKey = itemKey;
            return this;
        }

        public TestIconItemChangeQueueBuilder WithIdentifier(string identifier)
        {
            this.identifier = identifier;
            return this;
        }

        public TestIconItemChangeQueueBuilder WithItemChgTypeId(byte itemChgTypeID)
        {
            this.itemChgTypeID = itemChgTypeID;
            return this;
        }

        public TestIconItemChangeQueueBuilder WithInsertDate(DateTime insertDate)
        {
            this.insertDate = insertDate;
            return this;
        }

        public TestIconItemChangeQueueBuilder WithProcessFailedDate(DateTime? processFailedDate)
        {
            this.processFailedDate = processFailedDate;
            return this;
        }

        public TestIconItemChangeQueueBuilder WithInProcessBy(string inProcessBy)
        {
            this.inProcessBy = inProcessBy;
            return this;
        }

        public IconItemChangeQueue Build()
        {
            IconItemChangeQueue iconItemChangeQueue = new IconItemChangeQueue();
            iconItemChangeQueue.QID = this.queueId;
            iconItemChangeQueue.Item_Key = this.itemKey;
            iconItemChangeQueue.Identifier = this.identifier;
            iconItemChangeQueue.ItemChgTypeID = this.itemChgTypeID;
            iconItemChangeQueue.InsertDate = this.insertDate;
            iconItemChangeQueue.ProcessFailedDate = this.processFailedDate;
            iconItemChangeQueue.InProcessBy = this.inProcessBy;

            return iconItemChangeQueue;
        }

        public static implicit operator IconItemChangeQueue(TestIconItemChangeQueueBuilder builder)
        {
            return builder.Build();
        }
    }
}
