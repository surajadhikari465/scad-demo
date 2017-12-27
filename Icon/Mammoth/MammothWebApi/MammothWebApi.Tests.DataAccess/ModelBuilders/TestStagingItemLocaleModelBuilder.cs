using MammothWebApi.DataAccess.Models;
using System;

namespace MammothWebApi.Tests.DataAccess.ModelBuilders
{
    internal class TestStagingItemLocaleModelBuilder
    {
        private string region;
        private int businessUnit;
        private string scanCode;
        private bool discountCase;
        private bool discountTm;
        private int? restrictionAge;
        private bool restrictedHours;
        private bool authorized;
        private bool discontinued;
        private string labelType;
        private bool localItem;
        private string productCode;
        private string retailUnit;
        private string signDescription;
        private string romanceLong;
        private string romanceShort;
        private string locality;
        private decimal msrp;
        private bool OrderedByInfor;
        private decimal altRetailSize;
        private string altRetailUom;
        private string defaultScanCode;
        private DateTime timestamp;
        private Guid transactionId;

        internal TestStagingItemLocaleModelBuilder()
        {
            region = "SW";
            businessUnit = 1;
            scanCode = "77777777771";
            discountCase = true;
            discountTm = true;
            restrictionAge = null;
            restrictedHours = false;
            authorized = true;
            discontinued = false;
            labelType = "NONE";
            localItem = false;
            retailUnit = "EACH";
            signDescription = "Test Sign Description";
            locality = "Austin";
            romanceLong = "Sign Romance Long";
            romanceShort = "Sign Romance Short";
            msrp = 5.99M;
            OrderedByInfor = true;
            altRetailSize = 9.8m;
            altRetailUom = "EA";
            defaultScanCode = "123456123";
            timestamp = DateTime.Now;
            transactionId = Guid.NewGuid();
        }

        internal TestStagingItemLocaleModelBuilder WithRegion(string region)
        {
            this.region = region;
            return this;
        }

        internal TestStagingItemLocaleModelBuilder WithBusinessUnit(int businessUnit)
        {
            this.businessUnit = businessUnit;
            return this;
        }

        internal TestStagingItemLocaleModelBuilder WithScanCode(string scanCode)
        {
            this.scanCode = scanCode;
            return this;
        }

        internal TestStagingItemLocaleModelBuilder WithDiscountCase(bool discountCase)
        {
            this.discountCase = discountCase;
            return this;
        }

        internal TestStagingItemLocaleModelBuilder WithDiscountTm(bool discountTm)
        {
            this.discountTm = discountTm;
            return this;
        }

        internal TestStagingItemLocaleModelBuilder WithRestrictionAge(int? restrictionAge)
        {
            this.restrictionAge = restrictionAge;
            return this;
        }

        internal TestStagingItemLocaleModelBuilder WithAuthorized(bool authorized)
        {
            this.authorized = authorized;
            return this;
        }

        internal TestStagingItemLocaleModelBuilder WithDiscontinued(bool discontinued)
        {
            this.discontinued = discontinued;
            return this;
        }

        internal TestStagingItemLocaleModelBuilder WithLabelType(string labelType)
        {
            this.labelType = labelType;
            return this;
        }

        internal TestStagingItemLocaleModelBuilder WithLocalItem(bool localItem)
        {
            this.localItem = localItem;
            return this;
        }

        internal TestStagingItemLocaleModelBuilder WithProductCode(string productCode)
        {
            this.productCode = productCode;
            return this;
        }

        internal TestStagingItemLocaleModelBuilder WithRetailUnit(string retailUnit)
        {
            this.retailUnit = retailUnit;
            return this;
        }

        internal TestStagingItemLocaleModelBuilder WithSignDescription(string signDescription)
        {
            this.signDescription = signDescription;
            return this;
        }

        internal TestStagingItemLocaleModelBuilder WithLocality(string locality)
        {
            this.locality = locality;
            return this;
        }

        internal TestStagingItemLocaleModelBuilder WithRomanceLong(string romanceLong)
        {
            this.romanceLong = romanceLong;
            return this;
        }

        internal TestStagingItemLocaleModelBuilder WithRomanceShort(string romanceShort)
        {
            this.romanceShort = romanceShort;
            return this;
        }

        internal TestStagingItemLocaleModelBuilder WithTimestamp(DateTime timestamp)
        {
            this.timestamp = timestamp;
            return this;
        }

        internal TestStagingItemLocaleModelBuilder WithMsrp(decimal msrp)
        {
            this.msrp = msrp;
            return this;
        }

        internal TestStagingItemLocaleModelBuilder WithOrderedByInfor(bool OrderedByInfor)
        {
            this.OrderedByInfor = OrderedByInfor;
            return this;
        }

        internal TestStagingItemLocaleModelBuilder WithAltRetailSize(decimal altRetailSize)
        {
            this.altRetailSize = altRetailSize;
            return this;
        }

        internal TestStagingItemLocaleModelBuilder WithAltRetailUom(string altRetailUom)
        {
            this.altRetailUom = altRetailUom;
            return this;
        }

        internal TestStagingItemLocaleModelBuilder WithDefaultScanCode(string defaultScanCode)
        {
            this.defaultScanCode = defaultScanCode;
            return this;
        }

        internal TestStagingItemLocaleModelBuilder WithTransactionId(Guid transactionId)
        {
            this.transactionId = transactionId;
            return this;
        }

        internal StagingItemLocaleModel Build()
        {
            var itemLocaleStaging = new StagingItemLocaleModel
            {
                Authorized = authorized,
                BusinessUnitID = businessUnit,
                Discontinued = discontinued,
                Discount_Case = discountCase,
                Discount_TM = discountTm,
                LabelTypeDesc = labelType,
                LocalItem = localItem,
                Locality = locality,
                Product_Code = productCode,
                Region = region,
                Restriction_Hours = restrictedHours,
                Restriction_Age = restrictionAge,
                RetailUnit = retailUnit,
                Sign_RomanceText_Long = romanceLong,
                Sign_RomanceText_Short = romanceShort,
                ScanCode = scanCode,
                Sign_Desc = signDescription,
                Msrp = msrp,
                OrderedByInfor = OrderedByInfor,
                AltRetailSize = altRetailSize,
                AltRetailUOM = altRetailUom,
                DefaultScanCode = defaultScanCode,
                Timestamp = timestamp,
                TransactionId = transactionId
            };

            return itemLocaleStaging;
        }
    }
}
