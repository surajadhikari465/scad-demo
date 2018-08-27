using MammothWebApi.Models;
using System;

namespace MammothWebApi.Tests.ModelBuilders
{
    internal class TestItemLocaleModelBuilder
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
        private bool? colorAdded;
        private string chicagoBaby;
        private string countryOfProcessing;
        private bool? electronicShelfTag;
        private DateTime? exclusive;
        private string linkedItem;
        private int? numberOfDigitsSentToScale;
        private string origin;
        private string scaleExtraText;
        private string tagUom;
        private decimal msrp;
        private bool defaultScanCode;
        private int? irmaItemKey;

        internal TestItemLocaleModelBuilder()
        {
            this.region = "SW";
            this.businessUnit = 1;
            this.scanCode = "77777777771";
            this.discountCase = true;
            this.discountTm = true;
            this.restrictionAge = null;
            this.restrictedHours = false;
            this.authorized = true;
            this.discontinued = false;
            this.labelType = "NONE";
            this.localItem = false;
            this.retailUnit = "EACH";
            this.signDescription = "Test Sign Description";
            this.locality = "Dallas";
            this.colorAdded = null;
            this.chicagoBaby = null;
            this.countryOfProcessing = null;
            this.electronicShelfTag = null;
            this.exclusive = null;
            this.linkedItem = null;
            this.numberOfDigitsSentToScale = null;
            this.origin = null;
            this.scaleExtraText = null;
            this.tagUom = null;
            this.msrp = 5.99M;
            this.defaultScanCode = true;
            this.irmaItemKey = 678901;
        }

        internal TestItemLocaleModelBuilder WithRegion(string region)
        {
            this.region = region;
            return this;
        }

        internal TestItemLocaleModelBuilder WithBusinessUnit(int bu)
        {
            this.businessUnit = bu;
            return this;
        }

        internal TestItemLocaleModelBuilder WithScanCode(string scanCode)
        {
            this.scanCode = scanCode;
            return this;
        }

        internal TestItemLocaleModelBuilder WithDiscountCase(bool discountCase)
        {
            this.discountCase = discountCase;
            return this;
        }

        internal TestItemLocaleModelBuilder WithDiscountTm(bool discountTm)
        {
            this.discountTm = discountTm;
            return this;
        }

        internal TestItemLocaleModelBuilder WithRestrictionAge(int? ageRestriction)
        {
            this.restrictionAge = ageRestriction;
            return this;
        }

        internal TestItemLocaleModelBuilder WithAuthorized(bool authorized)
        {
            this.authorized = authorized;
            return this;
        }

        internal TestItemLocaleModelBuilder WithDiscontinued(bool discontinued)
        {
            this.discontinued = discontinued;
            return this;
        }

        internal TestItemLocaleModelBuilder WithLabelType(string labelType)
        {
            this.labelType = labelType;
            return this;
        }

        internal TestItemLocaleModelBuilder WithLocalItem(bool localItem)
        {
            this.localItem = localItem;
            return this;
        }

        internal TestItemLocaleModelBuilder WithProductCode(string productCode)
        {
            this.productCode = productCode;
            return this;
        }

        internal TestItemLocaleModelBuilder WithRetailUnit(string retailUnit)
        {
            this.retailUnit = retailUnit;
            return this;
        }

        internal TestItemLocaleModelBuilder WithSignDescription(string signDescription)
        {
            this.signDescription = signDescription;
            return this;
        }

        internal TestItemLocaleModelBuilder WithLocality(string locality)
        {
            this.locality = locality;
            return this;
        }

        internal TestItemLocaleModelBuilder WithRomanceLong(string romanceLong)
        {
            this.romanceLong = romanceLong;
            return this;
        }

        internal TestItemLocaleModelBuilder WithRomanceShort(string romanceShort)
        {
            this.romanceShort = romanceShort;
            return this;
        }

        internal TestItemLocaleModelBuilder WithColorAdded(bool? colorAdded)
        {
            this.colorAdded = colorAdded;
            return this;
        }

        internal TestItemLocaleModelBuilder WithChicagoBaby(string chicagoBaby)
        {
            this.chicagoBaby = chicagoBaby;
            return this;
        }

        internal TestItemLocaleModelBuilder WithCountryOfProcessing(string countryOfProcessing)
        {
            this.countryOfProcessing = countryOfProcessing;
            return this;
        }

        internal TestItemLocaleModelBuilder WithElectronicShelfTag(bool? est)
        {
            this.electronicShelfTag = est;
            return this;
        }

        internal TestItemLocaleModelBuilder WithExclusive(DateTime? exclusive)
        {
            this.exclusive = exclusive;
            return this;
        }

        internal TestItemLocaleModelBuilder WithLinkedItem(string linkedItem)
        {
            this.linkedItem = linkedItem;
            return this;
        }

        internal TestItemLocaleModelBuilder WithNumberOfDigitsSentToScale(int? numOfDigitsSentToScale)
        {
            this.numberOfDigitsSentToScale = numOfDigitsSentToScale;
            return this;
        }

        internal TestItemLocaleModelBuilder WithOrigin(string origin)
        {
            this.origin = origin;
            return this;
        }

        internal TestItemLocaleModelBuilder WithScaleExtraText(string scaleExtraText)
        {
            this.scaleExtraText = scaleExtraText;
            return this;
        }

        internal TestItemLocaleModelBuilder WithTagUom(string tagUom)
        {
            this.tagUom = tagUom;
            return this;
        }

        internal TestItemLocaleModelBuilder WithMsrp(decimal msrp)
        {
            this.msrp = msrp;
            return this;
        }
        internal TestItemLocaleModelBuilder WithDefaultScanCode(bool defaultScanCode)
        {
            this.defaultScanCode = defaultScanCode;
            return this;
        }
        internal TestItemLocaleModelBuilder WithIrmaItemKey(int? irmaItemKey)
        {
            this.irmaItemKey = irmaItemKey;
            return this;
        }

        internal ItemLocaleModel Build()
        {
            var itemLocaleStaging = new ItemLocaleModel
            {
                Authorized = this.authorized,
                BusinessUnitId = this.businessUnit,
                Discontinued = this.discontinued,
                CaseDiscount = this.discountCase,
                TmDiscount = this.discountTm,
                LabelTypeDescription = this.labelType,
                LocalItem = this.localItem,
                Locality = this.locality,
                ProductCode = this.productCode,
                Region = this.region,
                RestrictedHours = this.restrictedHours,
                AgeRestriction = this.restrictionAge,
                RetailUnit = this.retailUnit,
                SignRomanceLong = this.romanceLong,
                SignRomanceShort = this.romanceShort,
                ScanCode = this.scanCode,
                SignDescription = this.signDescription,
                ColorAdded = this.colorAdded,
                ChicagoBaby = this.chicagoBaby,
                CountryOfProcessing = this.countryOfProcessing,
                ElectronicShelfTag = this.electronicShelfTag,
                Exclusive = this.exclusive,
                LinkedItem = this.linkedItem,
                NumberOfDigitsSentToScale = this.numberOfDigitsSentToScale,
                Origin = this.origin,
                ScaleExtraText = this.scaleExtraText,
                TagUom = this.tagUom,
                MSRP = this.msrp,
                DefaultScanCode = this.defaultScanCode,
                IrmaItemKey = this.irmaItemKey
            };

            return itemLocaleStaging;
        }
    }
}
