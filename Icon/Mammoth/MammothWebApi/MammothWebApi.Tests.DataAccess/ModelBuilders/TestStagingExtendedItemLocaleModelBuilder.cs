using Mammoth.Common.DataAccess;
using MammothWebApi.DataAccess.Models;
using System;
using System.Collections.Generic;

namespace MammothWebApi.Tests.DataAccess.ModelBuilders
{
    internal class TestStagingExtendedItemLocaleModelBuilder
    {
        private string region;
        private int businessUnit;
        private string scanCode;
        private string colorAdded;
        private string countryOfProcessing;
        private string origin;
        private string electronicShelfTag;
        private string exclusive;
        private string numberOfDigitsSentToScale;
        private string chicagoBaby;
        private string tagUom;
        private string linkedItem;
        private string scaleExtraText;
        private DateTime timestamp;
        private Guid transactionId;

        internal TestStagingExtendedItemLocaleModelBuilder()
        {
            region = "SW";
            businessUnit = 1;
            scanCode = "222222222222";
            colorAdded = "1";
            countryOfProcessing = "Spain";
            origin = "France";
            electronicShelfTag = "1";
            exclusive = DateTime.Now.Date.ToString();
            numberOfDigitsSentToScale = "4";
            chicagoBaby = "hey baby!";
            tagUom = "1";
            linkedItem = "2222";
            scaleExtraText = "Extra!";
            timestamp = DateTime.Now;
            transactionId = Guid.NewGuid();
        }

        internal TestStagingExtendedItemLocaleModelBuilder WithRegion(string region)
        {
            this.region = region;
            return this;
        }

        internal TestStagingExtendedItemLocaleModelBuilder WithBusinessUnit(int bu)
        {
            this.businessUnit = bu;
            return this;
        }

        internal TestStagingExtendedItemLocaleModelBuilder WithScanCode(string scanCode)
        {
            this.scanCode = scanCode;
            return this;
        }

        internal TestStagingExtendedItemLocaleModelBuilder WithTimestamp(DateTime timestamp)
        {
            this.timestamp = timestamp;
            return this;
        }

        internal TestStagingExtendedItemLocaleModelBuilder WithTransactionId(Guid transactionId)
        {
            this.transactionId = transactionId;
            return this;
        }

        internal List<StagingItemLocaleExtendedModel> Build()
        {
            var extendedAttributes = new List<StagingItemLocaleExtendedModel>
            {
                new StagingItemLocaleExtendedModel { ScanCode = scanCode, BusinessUnitId = businessUnit, Region = region, AttributeId = Attributes.ColorAdded, AttributeValue = colorAdded, Timestamp = timestamp, TransactionId = this.transactionId },
                new StagingItemLocaleExtendedModel { ScanCode = scanCode, BusinessUnitId = businessUnit, Region = region, AttributeId = Attributes.CountryOfProcessing, AttributeValue = countryOfProcessing, Timestamp = timestamp, TransactionId = this.transactionId },
                new StagingItemLocaleExtendedModel { ScanCode = scanCode, BusinessUnitId = businessUnit, Region = region, AttributeId = Attributes.Origin, AttributeValue = origin, Timestamp = timestamp, TransactionId = this.transactionId },
                new StagingItemLocaleExtendedModel { ScanCode = scanCode, BusinessUnitId = businessUnit, Region = region, AttributeId = Attributes.ElectronicShelfTag, AttributeValue = electronicShelfTag, Timestamp = timestamp, TransactionId = this.transactionId },
                new StagingItemLocaleExtendedModel { ScanCode = scanCode, BusinessUnitId = businessUnit, Region = region, AttributeId = Attributes.Exclusive, AttributeValue = exclusive, Timestamp = timestamp, TransactionId = this.transactionId },
                new StagingItemLocaleExtendedModel { ScanCode = scanCode, BusinessUnitId = businessUnit, Region = region, AttributeId = Attributes.NumberOfDigitsSentToScale, AttributeValue = numberOfDigitsSentToScale, Timestamp = timestamp, TransactionId = this.transactionId },
                new StagingItemLocaleExtendedModel { ScanCode = scanCode, BusinessUnitId = businessUnit, Region = region, AttributeId = Attributes.ChicagoBaby, AttributeValue = chicagoBaby, Timestamp = timestamp, TransactionId = this.transactionId },
                new StagingItemLocaleExtendedModel { ScanCode = scanCode, BusinessUnitId = businessUnit, Region = region, AttributeId = Attributes.TagUom, AttributeValue = tagUom, Timestamp = timestamp, TransactionId = this.transactionId },
                new StagingItemLocaleExtendedModel { ScanCode = scanCode, BusinessUnitId = businessUnit, Region = region, AttributeId = Attributes.LinkedScanCode, AttributeValue = linkedItem, Timestamp = timestamp, TransactionId = this.transactionId },
                new StagingItemLocaleExtendedModel { ScanCode = scanCode, BusinessUnitId = businessUnit, Region = region, AttributeId = Attributes.ScaleExtraText, AttributeValue = scaleExtraText, Timestamp = timestamp, TransactionId = this.transactionId },
            };

            return extendedAttributes;
        }
    }
}
