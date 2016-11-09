using Mammoth.Common.DataAccess;
using Mammoth.Framework;
using System;

namespace Mammoth.Common.Testing.Builders
{
    public class TestMessageQueueItemLocaleBuilder
    {
        private int messageQueueId;
        private int messageTypeId;
        private int messageStatusId;
        private int? messageHistoryId;
        private int messageActionId;
        private DateTime insertDate;
        private string regionCode;
        private int businessUnitId;
        private int itemId;
        private string itemTypeCode;
        private string itemTypeDesc;
        private string localeName;
        private string scanCode;
        private bool caseDiscount;
        private bool tmDiscount;
        private int? ageRestriction;
        private bool restrictedHours;
        private bool authorized;
        private bool discontinued;
        private string labelTypeDescription;
        private bool localItem;
        private string productCode;
        private string retailUnit;
        private string signDescription;
        private string locality;
        private string signRomanceLong;
        private string signRomanceShort;
        private bool? colorAdded;
        private string countryOfProcessing;
        private string origin;
        private bool? electronicShelfTag;
        private DateTime? exclusive;
        private int? numberOfDigitsSentToScale;
        private string chicagoBaby;
        private string tagUom;
        private string linkedItem;
        private string scaleExtraText;
        private decimal? msrp;
        private int? inProcessBy;
        private DateTime? processedDate;

        public TestMessageQueueItemLocaleBuilder()
        {
            this.messageQueueId = 0;
            this.messageTypeId = MessageTypes.ItemLocale;
            this.messageStatusId = MessageStatusTypes.Ready;
            this.messageHistoryId = null;
            this.messageActionId = MessageActions.AddOrUpdate;
            this.insertDate = DateTime.Now;
            this.regionCode = "FL";
            this.businessUnitId = 0;
            this.itemId = 0;
            this.itemTypeCode = "TST";
            this.itemTypeDesc = "test item type";
            this.localeName = "test locale name";
            this.scanCode = "12345";
            this.caseDiscount = false;
            this.tmDiscount = false;
            this.ageRestriction = null;
            this.restrictedHours = false;
            this.authorized = false;
            this.discontinued = false;
            this.labelTypeDescription = null;
            this.localItem = false;
            this.productCode = null;
            this.retailUnit = null;
            this.signDescription = null;
            this.locality = null;
            this.signRomanceLong = null;
            this.signRomanceShort = null;
            this.colorAdded = null;
            this.countryOfProcessing = null;
            this.origin = null;
            this.electronicShelfTag = null;
            this.exclusive = null;
            this.numberOfDigitsSentToScale = null;
            this.chicagoBaby = null;
            this.tagUom = null;
            this.linkedItem = null;
            this.scaleExtraText = null;
            this.msrp = null;
            this.inProcessBy = null;
            this.processedDate = null;
        }

        public TestMessageQueueItemLocaleBuilder WithMessageQueueId(int messageQueueId)
        {
            this.messageQueueId = messageQueueId;
            return this;
        }

        public TestMessageQueueItemLocaleBuilder WithMessageTypeId(int messageTypeId)
        {
            this.messageTypeId = messageTypeId;
            return this;
        }

        public TestMessageQueueItemLocaleBuilder WithMessageStatusId(int messageStatusId)
        {
            this.messageStatusId = messageStatusId;
            return this;
        }

        public TestMessageQueueItemLocaleBuilder WithMessageHistoryId(System.Nullable<int> messageHistoryId)
        {
            this.messageHistoryId = messageHistoryId;
            return this;
        }

        public TestMessageQueueItemLocaleBuilder WithMessageActionId(int messageActionId)
        {
            this.messageActionId = messageActionId;
            return this;
        }

        public TestMessageQueueItemLocaleBuilder WithInsertDate(System.DateTime insertDate)
        {
            this.insertDate = insertDate;
            return this;
        }

        public TestMessageQueueItemLocaleBuilder WithRegionCode(string regionCode)
        {
            this.regionCode = regionCode;
            return this;
        }

        public TestMessageQueueItemLocaleBuilder WithBusinessUnitId(int businessUnitId)
        {
            this.businessUnitId = businessUnitId;
            return this;
        }

        public TestMessageQueueItemLocaleBuilder WithItemId(int itemId)
        {
            this.itemId = itemId;
            return this;
        }

        public TestMessageQueueItemLocaleBuilder WithItemTypeCode(string itemTypeCode)
        {
            this.itemTypeCode = itemTypeCode;
            return this;
        }

        public TestMessageQueueItemLocaleBuilder WithItemTypeDesc(string itemTypeDesc)
        {
            this.itemTypeDesc = itemTypeDesc;
            return this;
        }

        public TestMessageQueueItemLocaleBuilder WithLocaleName(string localeName)
        {
            this.localeName = localeName;
            return this;
        }

        public TestMessageQueueItemLocaleBuilder WithScanCode(string scanCode)
        {
            this.scanCode = scanCode;
            return this;
        }

        public TestMessageQueueItemLocaleBuilder WithCaseDiscount(bool caseDiscount)
        {
            this.caseDiscount = caseDiscount;
            return this;
        }

        public TestMessageQueueItemLocaleBuilder WithTmDiscount(bool tmDiscount)
        {
            this.tmDiscount = tmDiscount;
            return this;
        }

        public TestMessageQueueItemLocaleBuilder WithAgeRestriction(System.Nullable<int> ageRestriction)
        {
            this.ageRestriction = ageRestriction;
            return this;
        }

        public TestMessageQueueItemLocaleBuilder WithRestrictedHours(bool restrictedHours)
        {
            this.restrictedHours = restrictedHours;
            return this;
        }

        public TestMessageQueueItemLocaleBuilder WithAuthorized(bool authorized)
        {
            this.authorized = authorized;
            return this;
        }

        public TestMessageQueueItemLocaleBuilder WithDiscontinued(bool discontinued)
        {
            this.discontinued = discontinued;
            return this;
        }

        public TestMessageQueueItemLocaleBuilder WithLabelTypeDescription(string labelTypeDescription)
        {
            this.labelTypeDescription = labelTypeDescription;
            return this;
        }

        public TestMessageQueueItemLocaleBuilder WithLocalItem(bool localItem)
        {
            this.localItem = localItem;
            return this;
        }

        public TestMessageQueueItemLocaleBuilder WithProductCode(string productCode)
        {
            this.productCode = productCode;
            return this;
        }

        public TestMessageQueueItemLocaleBuilder WithRetailUnit(string retailUnit)
        {
            this.retailUnit = retailUnit;
            return this;
        }

        public TestMessageQueueItemLocaleBuilder WithSignDescription(string signDescription)
        {
            this.signDescription = signDescription;
            return this;
        }

        public TestMessageQueueItemLocaleBuilder WithLocality(string locality)
        {
            this.locality = locality;
            return this;
        }

        public TestMessageQueueItemLocaleBuilder WithSignRomanceLong(string signRomanceLong)
        {
            this.signRomanceLong = signRomanceLong;
            return this;
        }

        public TestMessageQueueItemLocaleBuilder WithSignRomanceShort(string signRomanceShort)
        {
            this.signRomanceShort = signRomanceShort;
            return this;
        }

        public TestMessageQueueItemLocaleBuilder WithColorAdded(System.Nullable<bool> colorAdded)
        {
            this.colorAdded = colorAdded;
            return this;
        }

        public TestMessageQueueItemLocaleBuilder WithCountryOfProcessing(string countryOfProcessing)
        {
            this.countryOfProcessing = countryOfProcessing;
            return this;
        }

        public TestMessageQueueItemLocaleBuilder WithOrigin(string origin)
        {
            this.origin = origin;
            return this;
        }

        public TestMessageQueueItemLocaleBuilder WithElectronicShelfTag(System.Nullable<bool> electronicShelfTag)
        {
            this.electronicShelfTag = electronicShelfTag;
            return this;
        }

        public TestMessageQueueItemLocaleBuilder WithExclusive(System.Nullable<System.DateTime> exclusive)
        {
            this.exclusive = exclusive;
            return this;
        }

        public TestMessageQueueItemLocaleBuilder WithNumberOfDigitsSentToScale(System.Nullable<int> numberOfDigitsSentToScale)
        {
            this.numberOfDigitsSentToScale = numberOfDigitsSentToScale;
            return this;
        }

        public TestMessageQueueItemLocaleBuilder WithChicagoBaby(string chicagoBaby)
        {
            this.chicagoBaby = chicagoBaby;
            return this;
        }

        public TestMessageQueueItemLocaleBuilder WithTagUom(string tagUom)
        {
            this.tagUom = tagUom;
            return this;
        }

        public TestMessageQueueItemLocaleBuilder WithLinkedItem(string linkedItem)
        {
            this.linkedItem = linkedItem;
            return this;
        }

        public TestMessageQueueItemLocaleBuilder WithScaleExtraText(string scaleExtraText)
        {
            this.scaleExtraText = scaleExtraText;
            return this;
        }

        public TestMessageQueueItemLocaleBuilder WithMsrp(decimal? msrp)
        {
            this.msrp = msrp;
            return this;
        }

        public TestMessageQueueItemLocaleBuilder WithInProcessBy(System.Nullable<int> inProcessBy)
        {
            this.inProcessBy = inProcessBy;
            return this;
        }

        public TestMessageQueueItemLocaleBuilder WithProcessedDate(System.Nullable<System.DateTime> processedDate)
        {
            this.processedDate = processedDate;
            return this;
        }

        public TestMessageQueueItemLocaleBuilder PopulateAllAttributes()
        {
            this.messageQueueId = 0;
            this.messageTypeId = MessageTypes.ItemLocale;
            this.messageStatusId = MessageStatusTypes.Ready;
            this.messageHistoryId = null;
            this.messageActionId = MessageActions.AddOrUpdate;
            this.insertDate = DateTime.Now;
            this.regionCode = "FL";
            this.businessUnitId = 0;
            this.itemId = 0;
            this.itemTypeCode = "TST";
            this.itemTypeDesc = "test item type";
            this.localeName = "test locale name";
            this.scanCode = "12345";
            this.caseDiscount = true;
            this.tmDiscount = true;
            this.ageRestriction = 18;
            this.restrictedHours = true;
            this.authorized = true;
            this.discontinued = true;
            this.labelTypeDescription = "test LabelTypeDescription";
            this.localItem = true;
            this.productCode = "test ProductCode";
            this.retailUnit = "OZ";
            this.signDescription = "test SignDescription";
            this.locality = "test Locality";
            this.signRomanceLong = "test RomanceLong";
            this.signRomanceShort = "test RomanceShort";
            this.colorAdded = true;
            this.countryOfProcessing = "test CountryOfProcessing";
            this.origin = "test Origin";
            this.electronicShelfTag = true;
            this.exclusive = DateTime.Now;
            this.numberOfDigitsSentToScale = 1;
            this.chicagoBaby = "test ChicagoBaby";
            this.tagUom = "test TagUom";
            this.linkedItem = "test LinkedItem";
            this.scaleExtraText = "test ScaleExtraText";
            this.inProcessBy = null;
            this.processedDate = null;

            return this;
        }

        public MessageQueueItemLocale Build()
        {
            MessageQueueItemLocale messageQueueItemLocale = new MessageQueueItemLocale();

            messageQueueItemLocale.MessageQueueId = this.messageQueueId;
            messageQueueItemLocale.MessageTypeId = this.messageTypeId;
            messageQueueItemLocale.MessageStatusId = this.messageStatusId;
            messageQueueItemLocale.MessageHistoryId = this.messageHistoryId;
            messageQueueItemLocale.MessageActionId = this.messageActionId;
            messageQueueItemLocale.InsertDate = this.insertDate;
            messageQueueItemLocale.RegionCode = this.regionCode;
            messageQueueItemLocale.BusinessUnitId = this.businessUnitId;
            messageQueueItemLocale.ItemId = this.itemId;
            messageQueueItemLocale.ItemTypeCode = this.itemTypeCode;
            messageQueueItemLocale.ItemTypeDesc = this.itemTypeDesc;
            messageQueueItemLocale.LocaleName = this.localeName;
            messageQueueItemLocale.ScanCode = this.scanCode;
            messageQueueItemLocale.CaseDiscount = this.caseDiscount;
            messageQueueItemLocale.TmDiscount = this.tmDiscount;
            messageQueueItemLocale.AgeRestriction = this.ageRestriction;
            messageQueueItemLocale.RestrictedHours = this.restrictedHours;
            messageQueueItemLocale.Authorized = this.authorized;
            messageQueueItemLocale.Discontinued = this.discontinued;
            messageQueueItemLocale.LabelTypeDescription = this.labelTypeDescription;
            messageQueueItemLocale.LocalItem = this.localItem;
            messageQueueItemLocale.ProductCode = this.productCode;
            messageQueueItemLocale.RetailUnit = this.retailUnit;
            messageQueueItemLocale.SignDescription = this.signDescription;
            messageQueueItemLocale.Locality = this.locality;
            messageQueueItemLocale.SignRomanceLong = this.signRomanceLong;
            messageQueueItemLocale.SignRomanceShort = this.signRomanceShort;
            messageQueueItemLocale.ColorAdded = this.colorAdded;
            messageQueueItemLocale.CountryOfProcessing = this.countryOfProcessing;
            messageQueueItemLocale.Origin = this.origin;
            messageQueueItemLocale.ElectronicShelfTag = this.electronicShelfTag;
            messageQueueItemLocale.Exclusive = this.exclusive;
            messageQueueItemLocale.NumberOfDigitsSentToScale = this.numberOfDigitsSentToScale;
            messageQueueItemLocale.ChicagoBaby = this.chicagoBaby;
            messageQueueItemLocale.TagUom = this.tagUom;
            messageQueueItemLocale.LinkedItem = this.linkedItem;
            messageQueueItemLocale.ScaleExtraText = this.scaleExtraText;
            messageQueueItemLocale.Msrp = this.msrp;
            messageQueueItemLocale.InProcessBy = this.inProcessBy;
            messageQueueItemLocale.ProcessedDate = this.processedDate;

            return messageQueueItemLocale;
        }

        public static implicit operator MessageQueueItemLocale(TestMessageQueueItemLocaleBuilder builder)
        {
            return builder.Build();
        }
    }
}
