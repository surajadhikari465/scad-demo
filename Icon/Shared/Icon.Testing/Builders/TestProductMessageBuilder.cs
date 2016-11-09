using Icon.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icon.Testing.Builders
{
    public class TestProductMessageBuilder
    {
        private int messageTypeId;
        private int messageStatusId;
        private int? messageHistoryId;
        private int itemId;
        private int localeId;
        private string itemTypeCode;
        private string itemTypeDesc;
        private int scanCodeId;
        private string scanCode;
        private int scanCodeTypeId;
        private string scanCodeTypeDesc;
        private string productDescription;
        private string posDescription;
        private string packageUnit;
        private string retailSize;
        private string retailUom;
        private string foodStampEligible;
        private int brandId;
        private string brandName;
        private int brandLevel;
        private int? brandParentId;
        private int browsingClassId;
        private string browsingClassName;
        private int browsingLevel;
        private int? browsingParentId;
        private int merchandiseClassId;
        private string merchandiseClassName;
        private int merchandiseLevel;
        private int merchandiseParentId;
        private bool prohibitDiscount;
        private int taxClassId;
        private string taxClassName;
        private int taxLevel;
        private int? taxParentId;
        private string financialClassId;
        private string financialClassName;
        private int financialLevel;
        private int? financialParentId;
        private string departementSale;
        private DateTime insertDate;
        private int? inProcessBy;
        private DateTime? processedDate;

        public TestProductMessageBuilder()
        {
            messageTypeId = MessageTypes.Product;
            messageStatusId = MessageStatusTypes.Ready;
            messageHistoryId = null;
            itemId = 1;
            localeId = Locales.WholeFoods;
            itemTypeCode = ItemTypeCodes.RetailSale;
            itemTypeDesc = ItemTypeDescriptions.RetailSale;
            scanCodeId = 123;
            scanCode = "123";
            scanCodeTypeId = ScanCodeTypes.Upc;
            scanCodeTypeDesc = ScanCodeTypeDescriptions.Upc;
            productDescription = "Test Product Description";
            posDescription = "Test POS Description";
            packageUnit = "1";
            retailSize = "1";
            retailUom = UomCodes.Each;
            foodStampEligible = "0";
            brandId = 1;
            brandName = "Test Brand";
            brandLevel = 1;
            brandParentId = null;
            browsingClassId = 1;
            browsingClassName = "Test Browsing";
            browsingLevel = 1;
            browsingParentId = null;
            merchandiseClassId = 1;
            merchandiseClassName = "Test Merch";
            merchandiseLevel = 1;
            merchandiseParentId = 2;
            prohibitDiscount = true;
            taxClassId = 1;
            taxClassName = "000000 Tax";
            taxLevel = 1;
            taxParentId = null;
            financialClassId = "1";
            financialClassName = "Test Financial";
            financialLevel = 1;
            financialParentId = null;
            departementSale = "0";
            insertDate = DateTime.Now;
            inProcessBy = null;
            processedDate = null;
        }

        public TestProductMessageBuilder WithMessageTypeId(int id)
        {
            this.messageTypeId = id;
            return this;
        }

        public TestProductMessageBuilder WithStatusId(int id)
        {
            this.messageStatusId = id;
            return this;
        }

        public TestProductMessageBuilder WithHistoryId(int id)
        {
            this.messageHistoryId = id;
            return this;
        }

        public TestProductMessageBuilder WithItemId(int id)
        {
            this.itemId = id;
            return this;
        }

        public TestProductMessageBuilder WithLocaleId(int id)
        {
            this.localeId = id;
            return this;
        }

        public TestProductMessageBuilder WithItemTypeCode(string itemTypeCode)
        {
            this.itemTypeCode = itemTypeCode;
            return this;
        }

        public TestProductMessageBuilder WithItemTypeDesc(string description)
        {
            this.itemTypeDesc = description;
            return this;
        }

        public TestProductMessageBuilder WithScanCodeId(int id)
        {
            this.scanCodeId = id;
            return this;
        }

        public TestProductMessageBuilder WithScanCode(string scanCode)
        {
            this.scanCode = scanCode;
            return this;
        }

        public TestProductMessageBuilder WithScanCodeTypeId(int id)
        {
            this.scanCodeTypeId = id;
            return this;
        }

        public TestProductMessageBuilder WithScanCodeTypeDesc(string description)
        {
            this.scanCodeTypeDesc = description;
            return this;
        }

        public TestProductMessageBuilder WithProductDescription(string description)
        {
            this.productDescription = description;
            return this;
        }

        public TestProductMessageBuilder WithPosDescription(string description)
        {
            this.posDescription = description;
            return this;
        }

        public TestProductMessageBuilder WithPackageUnit(string packageUnit)
        {
            this.packageUnit = packageUnit;
            return this;
        }

        public TestProductMessageBuilder WithRetailSize(string retailSize)
        {
            this.retailSize = retailSize;
            return this;
        }

        public TestProductMessageBuilder WithRetailUom(string uom)
        {
            this.retailUom = uom;
            return this;
        }

        public TestProductMessageBuilder WithFoodStampEligible(string foodStampEligible)
        {
            this.foodStampEligible = foodStampEligible;
            return this;
        }

        public TestProductMessageBuilder WithBrandId(int id)
        {
            this.brandId = id;
            return this;
        }

        public TestProductMessageBuilder WithBrandName(string name)
        {
            this.brandName = name;
            return this;
        }

        public TestProductMessageBuilder WithBrandLevel(int level)
        {
            this.brandLevel = level;
            return this;
        }

        public TestProductMessageBuilder WithBrandParentId(int? id)
        {
            this.brandParentId = id;
            return this;
        }

        public TestProductMessageBuilder WithBrowsingClassId(int id)
        {
            this.browsingClassId = id;
            return this;
        }

        public TestProductMessageBuilder WithBrowsingClassName(string name)
        {
            this.browsingClassName = name;
            return this;
        }

        public TestProductMessageBuilder WithBrowsingLevel(int level)
        {
            this.browsingLevel = level;
            return this;
        }

        public TestProductMessageBuilder WithBrowsingParentId(int? id)
        {
            this.browsingParentId = id;
            return this;
        }

        public TestProductMessageBuilder WithMerchandiseClassId(int id)
        {
            this.merchandiseClassId = id;
            return this;
        }

        public TestProductMessageBuilder WithMerchandiseClassName(string name)
        {
            this.merchandiseClassName = name;
            return this;
        }

        public TestProductMessageBuilder WithMerchandiseClassLevel(int level)
        {
            this.merchandiseLevel = level;
            return this;
        }

        public TestProductMessageBuilder WithMerchandiseClassParentId(int id)
        {
            this.merchandiseParentId = id;
            return this;
        }

        public TestProductMessageBuilder WithProhibitDiscount(bool prohibitDiscount)
        {
            this.prohibitDiscount = prohibitDiscount;
            return this;
        }

        public TestProductMessageBuilder WithTaxClassId(int id)
        {
            this.taxClassId = id;
            return this;
        }

        public TestProductMessageBuilder WithTaxClassName(string name)
        {
            this.taxClassName = name;
            return this;
        }

        public TestProductMessageBuilder WithTaxLevel(int level)
        {
            this.taxLevel = level;
            return this;
        }

        public TestProductMessageBuilder WithTaxParentId(int? id)
        {
            this.taxParentId = id;
            return this;
        }

        public TestProductMessageBuilder WithFinancialClassId(string id)
        {
            this.financialClassId = id;
            return this;
        }

        public TestProductMessageBuilder WithFinancialClassName(string name)
        {
            this.financialClassName = name;
            return this;
        }

        public TestProductMessageBuilder WithFinancialLevel(int level)
        {
            this.financialLevel = level;
            return this;
        }

        public TestProductMessageBuilder WithFinancialParentId(int? id)
        {
            this.financialParentId = id;
            return this;
        }

        public TestProductMessageBuilder WithDepartmentSale(string departmentSale)
        {
            this.departementSale = departmentSale;
            return this;
        }

        public TestProductMessageBuilder WithInsertDate(DateTime insertDate)
        {
            this.insertDate = insertDate;
            return this;
        }

        public TestProductMessageBuilder WithInProcessBy(int inProcessBy)
        {
            this.inProcessBy = inProcessBy;
            return this;
        }

        public TestProductMessageBuilder WithProcessedDate(DateTime? processedDate)
        {
            this.processedDate = processedDate;
            return this;
        }

        public MessageQueueProduct Build()
        {
            return new MessageQueueProduct
            {
                MessageTypeId = messageTypeId,
                MessageStatusId = messageStatusId,
                MessageHistoryId = messageHistoryId,
                ItemId = itemId,
                LocaleId = localeId,
                ItemTypeCode = itemTypeCode,
                ItemTypeDesc = itemTypeDesc,
                ScanCodeId = scanCodeId,
                ScanCode = scanCode,
                ScanCodeTypeId = scanCodeTypeId,
                ScanCodeTypeDesc = scanCodeTypeDesc,
                ProductDescription = productDescription,
                PosDescription = posDescription,
                PackageUnit = packageUnit,
                RetailSize = retailSize,
                RetailUom = retailUom,
                FoodStampEligible = foodStampEligible,
                BrandId = brandId,
                BrandName = brandName,
                BrandLevel = brandLevel,
                BrandParentId = brandParentId,
                BrowsingClassId = browsingClassId,
                BrowsingClassName = browsingClassName,
                BrowsingLevel = browsingLevel,
                BrowsingParentId = browsingParentId,
                MerchandiseClassId = merchandiseClassId,
                MerchandiseClassName = merchandiseClassName,
                MerchandiseLevel = merchandiseLevel,
                MerchandiseParentId = merchandiseParentId,
                ProhibitDiscount = prohibitDiscount,
                TaxClassId = taxClassId,
                TaxClassName = taxClassName,
                TaxLevel = taxLevel,
                TaxParentId = taxParentId,
                FinancialClassId = financialClassId,
                FinancialClassName = financialClassName,
                FinancialLevel = financialLevel,
                FinancialParentId = financialParentId,
                DepartmentSale = departementSale,
                InsertDate = insertDate,
                InProcessBy = inProcessBy,
                ProcessedDate = processedDate
            };
        }

        public static implicit operator MessageQueueProduct(TestProductMessageBuilder builder)
        {
            return builder.Build();
        }
    }
}
