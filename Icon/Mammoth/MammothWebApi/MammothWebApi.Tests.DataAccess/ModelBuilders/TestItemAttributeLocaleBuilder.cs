using MammothWebApi.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MammothWebApi.Tests.DataAccess.ModelBuilders
{
    internal class TestItemAttributeLocaleBuilder
    {
        private int itemId;
        private string region;
        private int businessUnit;
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
        private decimal altRetailSize;
        private string altRetailUom;
        private bool defaultScanCode;
        private DateTime addedDate;
        private bool scaleItem;

        internal TestItemAttributeLocaleBuilder()
        {
            this.itemId = 1;
            this.region = "SW";
            this.businessUnit = 1;
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
            this.altRetailSize = 82.89m;
            this.altRetailUom = "LB";
            this.defaultScanCode = true;
            this.addedDate = DateTime.Now;
            this.scaleItem = false;
        }

        internal TestItemAttributeLocaleBuilder WithItemId(int itemId)
        {
            this.itemId = itemId;
            return this;
        }

        internal TestItemAttributeLocaleBuilder WithRegion(string region)
        {
            this.region = region;
            return this;
        }

        internal TestItemAttributeLocaleBuilder WithBusinessUnit(int bu)
        {
            this.businessUnit = bu;
            return this;
        }

        internal TestItemAttributeLocaleBuilder WithDiscountCase(bool discountCase)
        {
            this.discountCase = discountCase;
            return this;
        }

        internal TestItemAttributeLocaleBuilder WithDiscountTm(bool discountTm)
        {
            this.discountTm = discountTm;
            return this;
        }

        internal TestItemAttributeLocaleBuilder WithRestrictionAge(int? ageRestriction)
        {
            this.restrictionAge = ageRestriction;
            return this;
        }

        internal TestItemAttributeLocaleBuilder WithRestrictedHours(bool restrictedHours)
        {
            this.restrictedHours = restrictedHours;
            return this;
        }

        internal TestItemAttributeLocaleBuilder WithAuthorized(bool authorized)
        {
            this.authorized = authorized;
            return this;
        }

        internal TestItemAttributeLocaleBuilder WithDiscontinued(bool discontinued)
        {
            this.discontinued = discontinued;
            return this;
        }

        internal TestItemAttributeLocaleBuilder WithLabelType(string labelType)
        {
            this.labelType = labelType;
            return this;
        }

        internal TestItemAttributeLocaleBuilder WithLocalItem(bool localItem)
        {
            this.localItem = localItem;
            return this;
        }

        internal TestItemAttributeLocaleBuilder WithProductCode(string productCode)
        {
            this.productCode = productCode;
            return this;
        }

        internal TestItemAttributeLocaleBuilder WithRetailUnit(string retailUnit)
        {
            this.retailUnit = retailUnit;
            return this;
        }

        internal TestItemAttributeLocaleBuilder WithSignDescription(string signDescription)
        {
            this.signDescription = signDescription;
            return this;
        }

        internal TestItemAttributeLocaleBuilder WithLocality(string locality)
        {
            this.locality = locality;
            return this;
        }

        internal TestItemAttributeLocaleBuilder WithRomanceLong(string romanceLong)
        {
            this.romanceLong = romanceLong;
            return this;
        }

        internal TestItemAttributeLocaleBuilder WithRomanceShort(string romanceShort)
        {
            this.romanceShort = romanceShort;
            return this;
        }

        internal TestItemAttributeLocaleBuilder WithAddedDate(DateTime addedDate)
        {
            this.addedDate = addedDate;
            return this;
        }

        internal TestItemAttributeLocaleBuilder WithAltRetailSize(decimal altRetailSize)
        {
            this.altRetailSize = altRetailSize;
            return this;
        }

        internal TestItemAttributeLocaleBuilder WithAltRetailUom(string altRetailUom)
        {
            this.altRetailUom = altRetailUom;
            return this;
        }

        internal TestItemAttributeLocaleBuilder WithDefaultScanCode(bool defaultScanCode)
        {
            this.defaultScanCode = defaultScanCode;
            return this;
        }

        internal TestItemAttributeLocaleBuilder WithScaleItem(bool scaleItem)
        {
            this.scaleItem = scaleItem;
            return this;
        }

        internal ItemAttributes_Locale Build()
        {
            ItemAttributes_Locale itemLocale = new ItemAttributes_Locale
            {
                Authorized = this.authorized,
                BusinessUnitID = this.businessUnit,
                Discontinued = this.discontinued,
                Discount_Case = this.discountCase,
                Discount_TM = this.discountTm,
                ItemID = this.itemId,
                LabelTypeDesc = this.labelType,
                LocalItem = this.localItem,
                Locality = this.locality,
                Product_Code = this.productCode,
                Restriction_Age = this.restrictionAge,
                Restriction_Hours = this.restrictedHours,
                RetailUnit = this.retailUnit,
                Region = this.region,
                ScaleItem = this.scaleItem,
                Sign_Desc = this.signDescription,
                Sign_RomanceText_Long = this.romanceLong,
                Sign_RomanceText_Short = this.romanceShort,
                AltRetailSize = altRetailSize,
                AltRetailUOM = altRetailUom,
                DefaultScanCode = defaultScanCode,
                AddedDate = this.addedDate
            };

            return itemLocale;
        }
    }
}
