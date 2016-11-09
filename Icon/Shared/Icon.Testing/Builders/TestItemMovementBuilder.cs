using Icon.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icon.Testing.Builders
{
    public class TestItemMovementBuilder
    {
        private int itemMovementID;
        private string esbMessageID;
        private int transactionSequenceNumber;
        private int businessUnitID;
        private int lineItemNumber;
        private int registerNumber;
        private string identifier;
        private DateTime transDate;
        private int? quantity;
        private bool itemVoid;
        private int itemType;
        private decimal basePrice;
        private decimal? weight;
        private decimal? markDownAmount;
        private DateTime insertDate;
        private string inProcessBy;
        private DateTime? processFailedDate;

        public TestItemMovementBuilder()
        {
            this.itemMovementID = 0;
            this.esbMessageID = "TestMsg";
            this.transactionSequenceNumber = 0;
            this.businessUnitID = 0;
            this.lineItemNumber = 0;
            this.registerNumber = 0;
            this.identifier = "123412341234";
            this.transDate = DateTime.Now;
            this.quantity = 1;
            this.itemVoid = false;
            this.itemType = 1;
            this.basePrice = 0;
            this.weight = null;
            this.markDownAmount = null;
            this.insertDate = DateTime.Now;
            this.inProcessBy = null;
            this.processFailedDate = null;
        }

        public TestItemMovementBuilder WithItemMovementID(int itemMovementID)
        {
            this.itemMovementID = itemMovementID;
            return this;
        }

        public TestItemMovementBuilder WithESBMessageID(string esbMessageID)
        {
            this.esbMessageID = esbMessageID;
            return this;
        }

        public TestItemMovementBuilder WithTransactionSequenceNumber(int transactionSequenceNumber)
        {
            this.transactionSequenceNumber = transactionSequenceNumber;
            return this;
        }

        public TestItemMovementBuilder WithBusinessUnitID(int businessUnitID)
        {
            this.businessUnitID = businessUnitID;
            return this;
        }

        public TestItemMovementBuilder WithRegisterNumber(int registerNumber)
        {
            this.registerNumber = registerNumber;
            return this;
        }
        public TestItemMovementBuilder WithLineItemNumber(int lineItemNumber)
        {
            this.lineItemNumber = lineItemNumber;
            return this;
        }

        public TestItemMovementBuilder WithIdentifier(string identifier)
        {
            this.identifier = identifier;
            return this;
        }

        public TestItemMovementBuilder WithTransDate(DateTime transDate)
        {
            this.transDate = transDate;
            return this;
        }

        public TestItemMovementBuilder WithQuantity(int quantity)
        {
            this.quantity = quantity;
            return this;
        }
        public TestItemMovementBuilder WithItemVoid(bool itemVoid)
        {
            this.itemVoid = itemVoid;
            return this;
        }

        public TestItemMovementBuilder WithItemType(int itemType)
        {
            this.itemType = itemType;
            return this;
        }

        public TestItemMovementBuilder WithBasePrice(decimal basePrice)
        {
            this.basePrice = basePrice;
            return this;
        }

        public TestItemMovementBuilder WithWeight(decimal weight)
        {
            this.weight = weight;
            return this;
        }

        public TestItemMovementBuilder WithMarkDownAmount(decimal markDownAmount)
        {
            this.markDownAmount = markDownAmount;
            return this;
        }
        public TestItemMovementBuilder WithInsertDate(DateTime insertDate)
        {
            this.insertDate = insertDate;
            return this;
        }

        public TestItemMovementBuilder WithInProcessBy(string inProcessBy)
        {
            this.inProcessBy = inProcessBy;
            return this;
        }
        public TestItemMovementBuilder WithProcessFailedDate(DateTime? processFailedDate)
        {
            this.processFailedDate = processFailedDate;
            return this;
        }

        public ItemMovement Build()
        {
            ItemMovement itemMovement = new ItemMovement();
            itemMovement.ItemMovementID = this.itemMovementID;
            itemMovement.ESBMessageID = this.esbMessageID;
            itemMovement.TransactionSequenceNumber = this.transactionSequenceNumber;
            itemMovement.RegisterNumber = this.registerNumber;
            itemMovement.BusinessUnitID = this.businessUnitID;
            itemMovement.RegisterNumber = this.registerNumber;
            itemMovement.LineItemNumber = this.lineItemNumber;
            itemMovement.Identifier = this.identifier;
            itemMovement.TransDate = this.transDate;
            itemMovement.Quantity = this.quantity;
            itemMovement.ItemVoid = this.itemVoid;
            itemMovement.ItemType = this.itemType;
            itemMovement.BasePrice = this.basePrice;
            itemMovement.Weight = this.weight;
            itemMovement.MarkDownAmount = this.markDownAmount;
            itemMovement.InsertDate = this.insertDate;
            itemMovement.ProcessFailedDate = this.processFailedDate;
            itemMovement.InProcessBy = this.inProcessBy;

            return itemMovement;
        }

        public static implicit operator ItemMovement(TestItemMovementBuilder builder)
        {
            return builder.Build();
        }
    }
}

