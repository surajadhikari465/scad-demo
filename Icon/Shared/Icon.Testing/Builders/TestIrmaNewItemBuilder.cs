using Icon.Framework;
using Icon.Testing.CustomModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icon.Testing.Builders
{
    public class TestIrmaNewItemBuilder
    {
        private string regionCode;
        private bool processedByController;
        private string irmaTaxClass;
        private int queueId;
        private int irmaItemKey;
        private string identifier;
        private int iconItemId;
        private IRMAItem irmaItem;
        private string failureReason;

        public TestIrmaNewItemBuilder()
        {
            this.regionCode = "FL";
            this.processedByController = false;
            this.irmaTaxClass = "Test Irma Tax Class Description 1";
            this.queueId = 0;
            this.irmaItemKey = 12345;
            this.identifier = "123412341234";
            this.iconItemId = 678;
            this.irmaItem = new IRMAItem
            {
                regioncode = regionCode,
                identifier = identifier,
                defaultIdentifier = true,
                insertDate = DateTime.Now,
                brandName = "Test Brand Name 1",
                itemDescription = "Test Irma Item Description",
                posDescription = "Test Irma Item POS Desc",
                packageUnit = 12,
                retailSize = 2,
                retailUom = "EACH",
                foodStamp = true,
                departmentSale = false,
                posScaleTare = 0.0m,
                giftCard = false
            };
        }

        public TestIrmaNewItemBuilder WithQueueId(int queueId)
        {
            this.queueId = queueId;
            return this;
        }

        public TestIrmaNewItemBuilder WithItemKey(int irmaItemKey)
        {
            this.irmaItemKey = irmaItemKey;
            return this;
        }
        public TestIrmaNewItemBuilder WithIdentifier(string identifier)
        {
            this.identifier = identifier;
            this.irmaItem.identifier = identifier;
            return this;
        }

        public TestIrmaNewItemBuilder WithProcessedByController(bool processedByController)
        {
            this.processedByController = processedByController;
            return this;
        }

        public TestIrmaNewItemBuilder WithFailureReason(string failureReason)
        {
            this.failureReason = failureReason;
            return this;
        }
        public IrmaNewItem Build()
        {
            IrmaNewItem irmaNewItem = new IrmaNewItem();
            irmaNewItem.QueueId = this.queueId;
            irmaNewItem.RegionCode = this.regionCode;
            irmaNewItem.ProcessedByController = this.processedByController;
            irmaNewItem.IrmaTaxClass = this.irmaTaxClass;
            irmaNewItem.IrmaItemKey = this.irmaItemKey;
            irmaNewItem.Identifier  = this.identifier;
            irmaNewItem.IconItemId = this.iconItemId;
            irmaNewItem.IrmaItem = this.irmaItem ;
            irmaNewItem.FailureReason = this.failureReason;

            return irmaNewItem;
        }

        public static implicit operator IrmaNewItem(TestIrmaNewItemBuilder builder)
        {
            return builder.Build();
        }
    }
}
