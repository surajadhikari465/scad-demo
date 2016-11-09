using Icon.Framework;
using System;

namespace Icon.Testing.Builders
{
    public class TestAplBuilder
    {
        private string agencyId;
        private string scanCode;
        private string itemDescription;
        private string unitOfMeasure;
        private decimal? packageSize;
        private decimal? benefitQuantity;
        private string benefitDescription;
        private decimal? itemPrice;
        private string priceType;

        public TestAplBuilder()
        {
            scanCode = "222222222";
            itemDescription = "Test APL Description";
            unitOfMeasure = "EA";
            packageSize = 1m;
            benefitQuantity = 1m;
            benefitDescription = "TEST";
            itemPrice = 1.99m;
            priceType = "RG";
        }

        public TestAplBuilder Empty()
        {
            agencyId = String.Empty;
            scanCode = String.Empty;
            itemDescription = null;
            unitOfMeasure = null;
            packageSize = null;
            benefitQuantity = null;
            benefitDescription = null;
            itemPrice = null;
            priceType = null;

            return this;
        }

        public TestAplBuilder WithAgencyId(string agencyId)
        {
            this.agencyId = agencyId;
            return this;
        }

        public TestAplBuilder WithScanCode(string scanCode)
        {
            this.scanCode = scanCode;
            return this;
        }

        public TestAplBuilder WithItemDescription(string itemDescription)
        {
            this.itemDescription = itemDescription;
            return this;
        }

        public AuthorizedProductList Build()
        {
            if (String.IsNullOrEmpty(agencyId))
            {
                throw new InvalidOperationException("WithAgencyId() must be called.");
            }

            var apl = new AuthorizedProductList
            {
                AgencyId = agencyId,
                ScanCode = scanCode,
                ItemDescription = itemDescription,
                UnitOfMeasure = unitOfMeasure,
                PackageSize = packageSize,
                BenefitQuantity = benefitQuantity,
                BenefitUnitDescription = benefitDescription,
                ItemPrice = itemPrice,
                PriceType = priceType
            };

            return apl;
        }

        public static implicit operator AuthorizedProductList(TestAplBuilder builder)
        {
            return builder.Build();
        }
    }
}
