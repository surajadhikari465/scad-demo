using MammothWebApi.Service.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MammothWebApi.Tests.ModelBuilders
{
    internal class TestItemLocaleServiceModelBuilder
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
        private bool? forceTare;
        private bool? sendtoCFS;
        private string wrappedTareWeight;
        private string unwrappedTareWeight;
        private bool? scaleItem;
        private string useBy;
        private int? shelfLife;
        private bool? defaultScanCode;

        internal TestItemLocaleServiceModelBuilder()
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
            this.forceTare = null;
            this.sendtoCFS = null;
            this.wrappedTareWeight = null;
            this.unwrappedTareWeight = null;
            this.scaleItem = null;
            this.useBy = null;
            this.shelfLife = null;
            this.defaultScanCode = false;
    }

        internal TestItemLocaleServiceModelBuilder WithRegion(string region)
        {
            this.region = region;
            return this;
        }

        internal TestItemLocaleServiceModelBuilder WithBusinessUnit(int bu)
        {
            this.businessUnit = bu;
            return this;
        }

        internal TestItemLocaleServiceModelBuilder WithScanCode(string scanCode)
        {
            this.scanCode = scanCode;
            return this;
        }

        internal TestItemLocaleServiceModelBuilder WithDiscountCase(bool discountCase)
        {
            this.discountCase = discountCase;
            return this;
        }

        internal TestItemLocaleServiceModelBuilder WithDiscountTm(bool discountTm)
        {
            this.discountTm = discountTm;
            return this;
        }

        internal TestItemLocaleServiceModelBuilder WithRestrictionAge(int? ageRestriction)
        {
            this.restrictionAge = ageRestriction;
            return this;
        }

        internal TestItemLocaleServiceModelBuilder WithAuthorized(bool authorized)
        {
            this.authorized = authorized;
            return this;
        }

        internal TestItemLocaleServiceModelBuilder WithDiscontinued(bool discontinued)
        {
            this.discontinued = discontinued;
            return this;
        }

        internal TestItemLocaleServiceModelBuilder WithLabelType(string labelType)
        {
            this.labelType = labelType;
            return this;
        }

        internal TestItemLocaleServiceModelBuilder WithLocalItem(bool localItem)
        {
            this.localItem = localItem;
            return this;
        }

        internal TestItemLocaleServiceModelBuilder WithProductCode(string productCode)
        {
            this.productCode = productCode;
            return this;
        }

        internal TestItemLocaleServiceModelBuilder WithRetailUnit(string retailUnit)
        {
            this.retailUnit = retailUnit;
            return this;
        }

        internal TestItemLocaleServiceModelBuilder WithSignDescription(string signDescription)
        {
            this.signDescription = signDescription;
            return this;
        }

        internal TestItemLocaleServiceModelBuilder WithLocality(string locality)
        {
            this.locality = locality;
            return this;
        }

        internal TestItemLocaleServiceModelBuilder WithRomanceLong(string romanceLong)
        {
            this.romanceLong = romanceLong;
            return this;
        }

        internal TestItemLocaleServiceModelBuilder WithRomanceShort(string romanceShort)
        {
            this.romanceShort = romanceShort;
            return this;
        }

        internal TestItemLocaleServiceModelBuilder WithColorAdded(bool? colorAdded)
        {
            this.colorAdded = colorAdded;
            return this;
        }

        internal TestItemLocaleServiceModelBuilder WithCountryOfProcessing(string countryOfProcessing)
        {
            this.countryOfProcessing = countryOfProcessing;
            return this;
        }

        internal TestItemLocaleServiceModelBuilder WithOrigin(string origin)
        {
            this.origin = origin;
            return this;
        }

        internal TestItemLocaleServiceModelBuilder WithElectronicShelfTag(bool? electronicShelfTag)
        {
            this.electronicShelfTag = electronicShelfTag;
            return this;
        }

        internal TestItemLocaleServiceModelBuilder WithExclusive(DateTime? exclusive)
        {
            this.exclusive = exclusive;
            return this;
        }

        internal TestItemLocaleServiceModelBuilder WithNumberOfDigitsSentToScale(int? numberOfDigits)
        {
            this.numberOfDigitsSentToScale = numberOfDigits;
            return this;
        }

        internal TestItemLocaleServiceModelBuilder WithChicagoBaby(string chicagoBaby)
        {
            this.chicagoBaby = chicagoBaby;
            return this;
        }

        internal TestItemLocaleServiceModelBuilder WithTagUom(string tagUom)
        {
            this.tagUom = tagUom;
            return this;
        }

        internal TestItemLocaleServiceModelBuilder WithLinkedItem(string linkedItem)
        {
            this.linkedItem = linkedItem;
            return this;
        }

        internal TestItemLocaleServiceModelBuilder WithScaleExtraText(string scaleExtraText)
        {
            this.scaleExtraText = scaleExtraText;
            return this;
        }

        internal TestItemLocaleServiceModelBuilder WithForceTare(bool? forceTare)
        {
            this.forceTare = forceTare;
            return this;
        }

        internal TestItemLocaleServiceModelBuilder WithCfsSendToScale(bool? cfsSendToScale)
        {
            this.sendtoCFS = cfsSendToScale;
            return this;
        }

        internal TestItemLocaleServiceModelBuilder WithWrappedTareWeight(string wrappedTareWeight)
        {
            this.wrappedTareWeight = wrappedTareWeight;
            return this;
        }

        internal TestItemLocaleServiceModelBuilder WithUnwrappedTareWeight(string unwrappedTareWeight)
        {
            this.unwrappedTareWeight = unwrappedTareWeight;
            return this;
        }

        internal TestItemLocaleServiceModelBuilder WithScaleItem(bool? isScaleItem)
        {
            this.scaleItem = isScaleItem;
            return this;
        }

        internal TestItemLocaleServiceModelBuilder WithUseBy(string useBy)
        {
            this.useBy = useBy;
            return this;
        }

        internal TestItemLocaleServiceModelBuilder WithShelfLife(int? shelfLife)
        {
            this.shelfLife = shelfLife;
            return this;
        }

        internal TestItemLocaleServiceModelBuilder WithDefaultScanCode(bool? defaultScanCode)
        {
            this.defaultScanCode = defaultScanCode;
            return this;
        }

        internal ItemLocaleServiceModel Build()
        {
            var itemLocaleStaging = new ItemLocaleServiceModel
            {
                Authorized = this.authorized,
                BusinessUnitId = this.businessUnit,
                Discontinued = this.discontinued,
                CaseDiscount = this.discountCase,
                TMDiscount = this.discountTm,
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
                ForceTare = this.forceTare,
                SendtoCFS = this.sendtoCFS,
                WrappedTareWeight = this.wrappedTareWeight,
                UnwrappedTareWeight = this.unwrappedTareWeight,
                ScaleItem = this.scaleItem,
                UseBy = this.useBy,
                ShelfLife = this.shelfLife,
                DefaultScanCode = this.defaultScanCode
            };

            return itemLocaleStaging;
        }
    }
}
