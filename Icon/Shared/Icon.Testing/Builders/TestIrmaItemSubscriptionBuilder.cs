using Icon.Framework;
using System;

namespace Icon.Testing.Builders
{
    public class TestIrmaItemSubscriptionBuilder
    {
        private int irmaItemSubscriptionId;
        private string regionCode;
        private string identifier;
        private DateTime insertDate;
        private DateTime? deleteDate;

        public TestIrmaItemSubscriptionBuilder()
        {
            this.regionCode = "SP";
            this.identifier = "1234567890";
            this.insertDate = DateTime.Now;
            this.deleteDate = null;
        }

        public TestIrmaItemSubscriptionBuilder WithIrmaItemSubscriptionId(int irmaItemSubscriptionId)
        {
            this.irmaItemSubscriptionId = irmaItemSubscriptionId;
            return this;
        }

        public TestIrmaItemSubscriptionBuilder WithRegionCode(string regionCode)
        {
            this.regionCode = regionCode;
            return this;
        }

        public TestIrmaItemSubscriptionBuilder WithIdentifier(string identifier)
        {
            this.identifier = identifier;
            return this;
        }
        public TestIrmaItemSubscriptionBuilder WithInsertDate(DateTime insertDate)
        {
            this.insertDate = insertDate;
            return this;
        }

        public TestIrmaItemSubscriptionBuilder WithDeleteDate(DateTime deleteDate)
        {
            this.deleteDate = deleteDate;
            return this;
        }

        public IRMAItemSubscription Build()
        {
            return new IRMAItemSubscription
            {
                IRMAItemSubscriptionID = this.irmaItemSubscriptionId,
                regioncode = this.regionCode,
                identifier = this.identifier,
                insertDate = this.insertDate,
                deleteDate = this.deleteDate
            };
        }

        public static implicit operator IRMAItemSubscription(TestIrmaItemSubscriptionBuilder builder)
        {
            return builder.Build();
        }
    }
}
