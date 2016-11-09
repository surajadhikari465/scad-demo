using Icon.Testing.CustomModels;
using System;

namespace Icon.Testing.Builders
{
    public class TestItemMovementToIrmaBuilder
    {
        private int itemMovementID;
        private string esbMessageID;
        private DateTime transDate;
        private int businessUnitId;
        private string identifier;
        private int itemType; //Values should be 0 or 2 with 0 for sales, 2 for returns
        private bool? itemVoid;
        private int? quantity;
        private decimal? weight;
        private decimal basePrice;
        private decimal? markdownAmount;

        public TestItemMovementToIrmaBuilder()
        {
            this.itemMovementID = 0;
            this.esbMessageID = "TestESBMessageID1";
            this.transDate = DateTime.Now;
            this.businessUnitId = 0;
            this.identifier = "1234567890";
            this.itemType = 0;
            this.basePrice = 10.99m;
        }

        public TestItemMovementToIrmaBuilder WithItemMovementID(int itemMovementID)
        {
            this.itemMovementID = itemMovementID;
            return this;
        }

        public TestItemMovementToIrmaBuilder WithESBMessageID(string esbMessageID)
        {
            this.esbMessageID = esbMessageID;
            return this;
        }

        public TestItemMovementToIrmaBuilder WithTransDate(DateTime transDate)
        {
            this.transDate = transDate;
            return this;
        }

        public TestItemMovementToIrmaBuilder WithBusinessUnitId(int businessUnitId)
        {
            this.businessUnitId = businessUnitId;
            return this;
        }

        public TestItemMovementToIrmaBuilder WithIdentifier(string identifier)
        {
            this.identifier = identifier;
            return this;
        }

        public TestItemMovementToIrmaBuilder WithItemType(int itemType)
        {
            this.itemType = itemType;
            return this;
        }

        public TestItemMovementToIrmaBuilder WithItemVoid(bool itemVoid)
        {
            this.itemVoid = itemVoid;
            return this;
        }

        public TestItemMovementToIrmaBuilder WithQuantity(int quantity)
        {
            this.quantity = quantity;
            return this;
        }

        public TestItemMovementToIrmaBuilder WithWeight(decimal weight)
        {
            this.weight = weight;
            return this;
        }

        public TestItemMovementToIrmaBuilder WithBasePrice(decimal basePrice)
        {
            this.basePrice = basePrice;
            return this;
        }

        public TestItemMovementToIrmaBuilder WithMarkdownAmount(decimal markdownAmount)
        {
            this.markdownAmount = markdownAmount;
            return this;
        }
        public ItemMovementToIrma Build()
        {
            ItemMovementToIrma itemMovementToIrma = new ItemMovementToIrma();

            itemMovementToIrma.ItemMovementID = this.itemMovementID;
            itemMovementToIrma.ESBMessageID = this.esbMessageID;
            itemMovementToIrma.TransDate = this.transDate;
            itemMovementToIrma.BusinessUnitId = this.businessUnitId;
            itemMovementToIrma.Identifier = this.identifier;
            itemMovementToIrma.ItemType = this.itemType;
            itemMovementToIrma.ItemVoid  = this.itemVoid;
            itemMovementToIrma.Quantity = this.quantity;
            itemMovementToIrma.Weight = this.weight;
            itemMovementToIrma.BasePrice = this.basePrice;
            itemMovementToIrma.MarkdownAmount = this.markdownAmount;

            return itemMovementToIrma;
        }

        public static implicit operator ItemMovementToIrma(TestItemMovementToIrmaBuilder builder)
        {
            return builder.Build();
        }
    }
}
