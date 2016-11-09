using Icon.Framework;
using System.Collections.Generic;
using System.Linq;

namespace Icon.Testing.Builders
{
    public class TestItemBuilder
    {
        private int itemId;
        private int itemTypeId;
        private int productKey;
        private int localeId;
        private string scanCode;
        private int scanCodeTypeId;
        private string productDescription;
        private string posDescription;
        private string packageUnit;
        private string retailSize;
        private string retailUom;
        private string deliverySystem;
        private string posScaleTare;
        private string foodStampEligible;
        private string validationDate;
        private string modifiedDate;
        private int merchLevelFiveId;
        private int brandId;
        private int taxId;
        private int nationalClassId;
        private Dictionary<int, Trait> traitReferences;
        private Locale posScaleTareLocale;

        public TestItemBuilder()
        {
            this.itemId = 0;
            this.itemTypeId = 1;
            this.productKey = 0;
            this.localeId = 1;
            this.productDescription = "Unit Test Product Description 1";
            this.posDescription = "Unit Test Pos Description 1";
            this.posScaleTare = "0";
            this.packageUnit = "1";
            this.retailSize = "1";
            this.retailUom = "EA";
            this.deliverySystem = "CAP";
            this.foodStampEligible = "1";
            this.scanCode = "8428428428";
            this.scanCodeTypeId = ScanCodeTypes.Upc;
            this.validationDate = null;
            this.modifiedDate = null;
            this.brandId = 0;
            this.taxId = 0;
            this.merchLevelFiveId = 0;
            this.posScaleTareLocale = null;
        }

        /// <summary>
        /// Populate the itemId if you need objects that won't be added to the DbContext.
        /// This would be used for something like Controller Unit Tests.
        /// </summary>
        /// <param name="itemId">ItemId</param>
        /// <returns></returns>
        public TestItemBuilder WithItemId(int itemId)
        {
            this.itemId = itemId;
            return this;
        }

        public TestItemBuilder WithItemType(int itemTypeId)
        {
            this.itemTypeId = itemTypeId;
            return this;
        }

        public TestItemBuilder WithProductDescription(string productDescription)
        {
            this.productDescription = productDescription;
            return this;
        }

        public TestItemBuilder WithPosDescription(string posDescription)
        {
            this.posDescription = posDescription;
            return this;
        }

        public TestItemBuilder WithPackageUnit(string packageUnit)
        {
            this.packageUnit = packageUnit;
            return this;
        }

        public TestItemBuilder WithPosScaleTare(string posScaleTare)
        {
            this.posScaleTare = posScaleTare;
            return this;
        }

        public TestItemBuilder WithFoodStamp(string foodStampEligible)
        {
            this.foodStampEligible = foodStampEligible;
            return this;
        }

        public TestItemBuilder WithScanCode(string scanCode)
        {
            this.scanCode = scanCode;
            return this;
        }

        public TestItemBuilder WithScanCodeType(int scanCodeTypeId)
        {
            this.scanCodeTypeId = scanCodeTypeId;
            return this;
        }

        /// <summary>
        /// Add the validation date trait to the item.
        /// A row will only exist if a value is provided.
        /// </summary>
        /// <param name="validationDate">Validation Date</param>
        /// <returns></returns>
        public TestItemBuilder WithValidationDate(string validationDate)
        {
            this.validationDate = validationDate;
            return this;
        }

        /// <summary>
        /// Add a value to the modified date trait.
        /// A row will exist in ItemTrait for this item regardless of traitValue.
        /// </summary>
        /// <param name="modifiedDate">Modified Date</param>
        /// <returns></returns>
        public TestItemBuilder WithModifiedDate(string modifiedDate)
        {
            this.modifiedDate = modifiedDate;
            return this;
        }

        /// <summary>
        /// Associate the Item to a Brand that already exists
        /// </summary>
        /// <param name="brandId">hierarchyClassID of the Brand</param>
        /// <returns></returns>
        public TestItemBuilder WithBrandAssociation(int brandId)
        {
            this.brandId = brandId;
            return this;
        }

        /// <summary>
        /// Associate the Item that you're building to a Tax Class that already exists
        /// </summary>
        /// <param name="taxId">hierarchyClassID of the Tax Class</param>
        /// <returns></returns>
        public TestItemBuilder WithTaxClassAssociation(int taxId)
        {
            this.taxId = taxId;
            return this;
        }

        public TestItemBuilder WithNationalClassAssociation(int nationalClassId)
        {
            this.nationalClassId = nationalClassId;
            return this;
        }

        /// <summary>
        /// Associate the Item to a sub-brick that already exists.
        /// Use this if you already have a Merchandise sub-brick that has a 'lineage'
        /// e.g. sub-brick has a parent, grand-parent, etc.
        /// </summary>
        /// <param name="merchLevelFiveId">hierarchyClassID of sub-brick</param>
        /// <returns></returns>
        public TestItemBuilder WithSubBrickAssociation(int merchLevelFiveId)
        {
            this.merchLevelFiveId = merchLevelFiveId;
            return this;
        }

        public TestItemBuilder WithTraitReferences(Dictionary<int, Trait> traitReferences)
        {
            this.traitReferences = traitReferences;
            return this;
        }

        public TestItemBuilder WithTraitCodesOnTraits()
        {
            this.traitReferences = new Dictionary<int, Trait>
                {
                    { Traits.ProductDescription, new Trait { traitCode = TraitCodes.ProductDescription } },
                    { Traits.PosDescription, new Trait { traitCode = TraitCodes.PosDescription } },
                    { Traits.PackageUnit, new Trait { traitCode = TraitCodes.PackageUnit } },
                    { Traits.RetailSize, new Trait { traitCode = TraitCodes.RetailSize } },
                    { Traits.RetailUom, new Trait { traitCode = TraitCodes.RetailUom } },
                    { Traits.DeliverySystem, new Trait {traitCode = TraitCodes.DeliverySystem } },
                    { Traits.PosScaleTare, new Trait { traitCode = TraitCodes.PosScaleTare } },
                    { Traits.FoodStampEligible, new Trait { traitCode = TraitCodes.FoodStampEligible } },
                    { Traits.ModifiedDate, new Trait { traitCode = TraitCodes.ModifiedDate } },
                    { Traits.ValidationDate, new Trait { traitCode = TraitCodes.ValidationDate } }
                };
            return this;
        }

        public TestItemBuilder WithPosScaleTareLocale(Locale locale)
        {
            this.posScaleTareLocale = locale;
            return this;
        }

        public Item Build()
        {
            // To be added to as needed for ItemHierarchyClass Associations
            List<ItemHierarchyClass> itemHierarchyClasses = new List<ItemHierarchyClass>();
            HierarchyClass merchSubBrick = new HierarchyClass();

            // Item Base
            Item item = new Item { itemID = itemId };
            item.itemTypeID = this.itemTypeId;
            item.productKey = this.productKey;

            // Item Traits
            List<ItemTrait> itemTraits = new List<ItemTrait>
            {
                new ItemTrait { itemID = item.itemID, localeID = this.localeId, traitID = Traits.ProductDescription, Trait = traitReferences == null ? null : traitReferences[Traits.ProductDescription], traitValue = this.productDescription },
                new ItemTrait { itemID = item.itemID, localeID = this.localeId, traitID = Traits.PosDescription, Trait = traitReferences == null ? null : traitReferences[Traits.PosDescription], traitValue = this.posDescription },
                new ItemTrait { itemID = item.itemID, localeID = this.localeId, traitID = Traits.PackageUnit, Trait = traitReferences == null ? null : traitReferences[Traits.PackageUnit], traitValue = this.packageUnit },
                new ItemTrait { itemID = item.itemID, localeID = this.localeId, traitID = Traits.RetailSize, Trait = traitReferences == null ? null : traitReferences[Traits.RetailSize], traitValue = this.retailSize },
                new ItemTrait { itemID = item.itemID, localeID = this.localeId, traitID = Traits.RetailUom, Trait = traitReferences == null ? null : traitReferences[Traits.RetailUom], traitValue = this.retailUom },
                new ItemTrait { itemID = item.itemID, localeID = this.localeId, traitID = Traits.DeliverySystem, Trait = traitReferences == null ? null : traitReferences[Traits.DeliverySystem], traitValue = this.deliverySystem },
                new ItemTrait { itemID = item.itemID, localeID = this.localeId, traitID = Traits.PosScaleTare, Trait = traitReferences == null ? null : traitReferences[Traits.PosScaleTare], traitValue = this.posScaleTare },
                new ItemTrait { itemID = item.itemID, localeID = this.localeId, traitID = Traits.FoodStampEligible, Trait = traitReferences == null ? null : traitReferences[Traits.FoodStampEligible], traitValue = this.foodStampEligible },
                new ItemTrait { itemID = item.itemID, localeID = this.localeId, traitID = Traits.ModifiedDate, Trait = traitReferences == null ? null : traitReferences[Traits.ModifiedDate], traitValue = this.modifiedDate }
            };

            if (validationDate != null)
            {
                ItemTrait validationDateTrait = new ItemTrait { itemID = item.itemID, localeID = this.localeId, traitID = Traits.ValidationDate, Trait = traitReferences == null ? null : traitReferences[Traits.ValidationDate], traitValue = this.validationDate };
                itemTraits.Add(validationDateTrait);
            }

            // Add ScanCodes
            List<ScanCode> scanCodes = new List<ScanCode>
            {
                new ScanCode { itemID = item.itemID, localeID = this.localeId, scanCodeTypeID = this.scanCodeTypeId, scanCode = this.scanCode }
            };

            // Setup HierarchyClass Associations
            if (this.merchLevelFiveId != 0)
            {
                itemHierarchyClasses.Add(new ItemHierarchyClass { itemID = item.itemID, localeID = this.localeId, hierarchyClassID = this.merchLevelFiveId });
            }

            // Setup Tax Class Association
            if (this.taxId != 0)
            {
                itemHierarchyClasses.Add(new ItemHierarchyClass { itemID = item.itemID, localeID = this.localeId, hierarchyClassID = this.taxId });
            }

            // Setup National Class Association
            if (this.nationalClassId != 0)
            {
                itemHierarchyClasses.Add(new ItemHierarchyClass { itemID = item.itemID, localeID = this.localeId, hierarchyClassID = this.nationalClassId });
            }

            // Setup Brand Association
            if (this.brandId != 0)
            {
                itemHierarchyClasses.Add(new ItemHierarchyClass { itemID = item.itemID, localeID = this.localeId, hierarchyClassID = brandId });
            }

            // Add Navigation Properties to Item
            item.ItemTrait = itemTraits;
            item.ScanCode = scanCodes;
            item.ItemHierarchyClass = itemHierarchyClasses;

            if(posScaleTareLocale != null)
            {
                var trait = item.ItemTrait.First(it => it.traitID == Traits.PosScaleTare);
                trait.Locale = posScaleTareLocale;
                trait.localeID = posScaleTareLocale.localeID;
            }

            return item;
        }

        public static implicit operator Item(TestItemBuilder builder)
        {
            return builder.Build();
        }
    }
}
