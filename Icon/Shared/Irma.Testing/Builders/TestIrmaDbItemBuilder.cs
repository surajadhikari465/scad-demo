using Irma.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Irma.Testing.Builders
{
    public class TestIrmaDbItemBuilder
    {
        private IrmaContext context;
        private int item_Key;
        private string item_Description;
        private string sign_Description;
        private string ingredients;
        private int subTeam_No;
        private string sales_Account;
        private decimal package_Desc1;
        private decimal package_Desc2;
        private int package_Unit_ID;
        private short min_Temperature;
        private short max_Temperature;
        private short units_Per_Pallet;
        private System.Nullable<decimal> average_Unit_Weight;
        private byte tie;
        private byte high;
        private decimal yield;
        private System.Nullable<int> brand_ID;
        private System.Nullable<int> category_ID;
        private System.Nullable<int> origin_ID;
        private short shelfLife_Length;
        private System.Nullable<int> shelfLife_ID;
        private int retail_Unit_ID;
        private int vendor_Unit_ID;
        private int distribution_Unit_ID;
        private System.Nullable<int> cost_Unit_ID;
        private System.Nullable<int> freight_Unit_ID;
        private bool deleted_Item;
        private bool wFM_Item;
        private bool not_Available;
        private bool pre_Order;
        private byte remove_Item;
        private bool noDistMarkup;
        private bool organic;
        private bool refrigerated;
        private bool keep_Frozen;
        private bool shipper_Item;
        private bool full_Pallet_Only;
        private System.Nullable<int> user_ID;
        private string pOS_Description;
        private bool retail_Sale;
        private bool food_Stamps;
        private bool discountable;
        private bool price_Required;
        private bool quantity_Required;
        private int itemType_ID;
        private bool hFM_Item;
        private string scaleDesc1;
        private string scaleDesc2;
        private string not_AvailableNote;
        private System.Nullable<int> countryProc_ID;
        private System.DateTime insert_Date;
        private System.Nullable<int> manufacturing_Unit_ID;
        private bool eXEDistributed;
        private System.Nullable<int> classID;
        private System.Nullable<System.DateTime> user_ID_Date;
        private System.Nullable<int> distSubTeam_No;
        private bool costedByWeight;
        private System.Nullable<int> taxClassID;
        private System.Nullable<int> labelType_ID;
        private string scaleDesc3;
        private string scaleDesc4;
        private System.Nullable<int> scaleTare;
        private System.Nullable<int> scaleUseBy;
        private string scaleForcedTare;
        private System.Nullable<bool> qtyProhibit;
        private System.Nullable<int> groupList;
        private System.Nullable<int> prodHierarchyLevel4_ID;
        private System.Nullable<bool> case_Discount;
        private System.Nullable<bool> coupon_Multiplier;
        private System.Nullable<short> misc_Transaction_Sale;
        private System.Nullable<short> misc_Transaction_Refund;
        private System.Nullable<bool> recall_Flag;
        private System.Nullable<byte> manager_ID;
        private System.Nullable<int> ice_Tare;
        private System.Nullable<bool> lockAuth;
        private System.Nullable<decimal> purchaseThresholdCouponAmount;
        private System.Nullable<bool> purchaseThresholdCouponSubTeam;
        private string product_Code;
        private System.Nullable<int> unit_Price_Category;
        private System.Nullable<int> storeJurisdictionID;
        private bool catchweightRequired;
        private bool cOOL;
        private bool bIO;
        private System.Nullable<int> lastModifiedUser_ID;
        private System.Nullable<System.DateTime> lastModifiedDate;
        private bool catchWtReq;
        private System.Nullable<bool> sustainabilityRankingRequired;
        private System.Nullable<int> sustainabilityRankingID;
        private bool ingredient;
        private bool fSA_Eligible;
        private System.Nullable<System.DateTime> taxClassModifiedDate;
        private System.Nullable<bool> useLastReceivedCost;
        private bool giftCard;
        private HashSet<ItemIdentifier> itemIdentifiers;
        private HashSet<ItemSignAttribute> itemSignAttributes;
        private ItemUnit itemUnit3;
        private ItemUnit itemUnit4;
        private SubTeam subTeam;

        public TestIrmaDbItemBuilder()
        {
            this.item_Key = 0;
            this.item_Description = "Test Item Description";
            this.sign_Description = "Test Sign Description";
            this.ingredients = null;
            this.subTeam_No = -1;
            this.sales_Account = null;
            this.package_Desc1 = 1;
            this.package_Desc2 = 1;
            this.package_Unit_ID = 1;
            this.min_Temperature = 0;
            this.max_Temperature = 0;
            this.units_Per_Pallet = 0;
            this.average_Unit_Weight = null;
            this.tie = 8;
            this.high = 10;
            this.yield = 100;
            this.brand_ID = null;
            this.category_ID = null;
            this.origin_ID = null;
            this.shelfLife_Length = 0;
            this.shelfLife_ID = null;
            this.retail_Unit_ID = 1;
            this.vendor_Unit_ID = 1;
            this.distribution_Unit_ID = 1;
            this.cost_Unit_ID = null;
            this.freight_Unit_ID = null;
            this.deleted_Item = false;
            this.wFM_Item = true;
            this.not_Available = false;
            this.pre_Order = false;
            this.remove_Item = 0;
            this.noDistMarkup = false;
            this.organic = false;
            this.refrigerated = false;
            this.keep_Frozen = false;
            this.shipper_Item = false;
            this.full_Pallet_Only = false;
            this.user_ID = null;
            this.pOS_Description = "Test POS Description";
            this.retail_Sale = true;
            this.food_Stamps = false;
            this.discountable = false;
            this.price_Required = false;
            this.quantity_Required = false;
            this.itemType_ID = 0;
            this.hFM_Item = true;
            this.scaleDesc1 = null;
            this.scaleDesc2 = null;
            this.not_AvailableNote = null;
            this.countryProc_ID = null;
            this.insert_Date = DateTime.Now;
            this.manufacturing_Unit_ID = null;
            this.eXEDistributed = false;
            this.classID = 44020;
            this.user_ID_Date = null;
            this.distSubTeam_No = null;
            this.costedByWeight = false;
            this.taxClassID = null;
            this.labelType_ID = null;
            this.scaleDesc3 = null;
            this.scaleDesc4 = null;
            this.scaleTare = null;
            this.scaleUseBy = null;
            this.scaleForcedTare = null;
            this.qtyProhibit = null;
            this.groupList = null;
            this.prodHierarchyLevel4_ID = null;
            this.case_Discount = null;
            this.coupon_Multiplier = null;
            this.misc_Transaction_Sale = null;
            this.misc_Transaction_Refund = null;
            this.recall_Flag = null;
            this.manager_ID = null;
            this.ice_Tare = null;
            this.lockAuth = null;
            this.purchaseThresholdCouponAmount = null;
            this.purchaseThresholdCouponSubTeam = null;
            this.product_Code = null;
            this.unit_Price_Category = null;
            this.storeJurisdictionID = null;
            this.catchweightRequired = false;
            this.cOOL = false;
            this.bIO = false;
            this.lastModifiedUser_ID = null;
            this.lastModifiedDate = null;
            this.catchWtReq = false;
            this.sustainabilityRankingRequired = null;
            this.sustainabilityRankingID = null;
            this.ingredient = false;
            this.fSA_Eligible = false;
            this.taxClassModifiedDate = null;
            this.useLastReceivedCost = null;
            this.giftCard = false;
            this.itemIdentifiers = new HashSet<ItemIdentifier>();
            this.itemSignAttributes = new HashSet<ItemSignAttribute>();
            //this.itemUnit3 = new ItemUnit();
            //this.itemUnit4 = new ItemUnit();
            //this.subTeam = new SubTeam();
        }

        public TestIrmaDbItemBuilder(IrmaContext context) : this()
        {
            this.context = context;

            var subteam_No = -1;
            if(this.context.SubTeam.FirstOrDefault(s => s.SubTeam_No == subteam_No) == null)
            {
                var newSubTeam = new SubTeam { SubTeam_No = subteam_No, Target_Margin = 0, EXEWarehouseSent = false, Retail = false, EXEDistributed = false };
                context.SubTeam.Add(newSubTeam);
            }

            var newTestUnit = this.context.ItemUnit.FirstOrDefault(i => i.Unit_Name == "test unit");
            if(newTestUnit == null)
            {
                newTestUnit = new ItemUnit { Unit_Name = "test unit", Weight_Unit = false, IsPackageUnit = false };
                context.ItemUnit.Add(newTestUnit);
            }

            var newUser = this.context.Users.FirstOrDefault(u => u.UserName == "test user");
            if(newUser == null)
            {
                newUser = new Users
                {
                    UserName = "test user",
                    AccountEnabled = true,
                    PO_Accountant = false,
                    Accountant = false,
                    Distributor = false,
                    FacilityCreditProcessor = false,
                    Buyer = false,
                    Coordinator = false,
                    Item_Administrator = false,
                    Vendor_Administrator = false,
                    Lock_Administrator = false,
                    Warehouse = false,
                    PriceBatchProcessor = false,
                    Inventory_Administrator = false,
                    BatchBuildOnly = false,
                    CostAdmin = false,
                    POApprovalAdmin = false,
                    EInvoicing_Administrator = false,
                    VendorCostDiscrepancyAdmin = false,
                    ApplicationConfigAdmin = false,
                    DataAdministrator = false,
                    JobAdministrator = false,
                    POSInterfaceAdministrator = false,
                    SecurityAdministrator = false,
                    StoreAdministrator = false,
                    SystemConfigurationAdministrator = false,
                    UserMaintenance = false,
                    Shrink = false,
                    ShrinkAdmin = false,
                    POEditor = false,
                    DeletePO = false,
                    TaxAdministrator = false
                };

                context.Users.Add(newUser);
            }

            var testItemBrand = this.context.ItemBrand.FirstOrDefault(c => c.Brand_Name == "test itemBrand");
            if(testItemBrand == null)
            {
                testItemBrand = new ItemBrand { Brand_Name = "test itemBrand" };
                context.ItemBrand.Add(testItemBrand);
            }

            context.SaveChanges();

            this.subTeam_No = subteam_No;
            this.package_Unit_ID 
                = this.retail_Unit_ID 
                = this.vendor_Unit_ID 
                = this.distribution_Unit_ID 
                = newTestUnit.Unit_ID;

            this.brand_ID = testItemBrand.Brand_ID;
        }

        public TestIrmaDbItemBuilder WithItem_Key(int item_Key)
        {
            this.item_Key = item_Key;
            return this;
        }

        public TestIrmaDbItemBuilder WithItem_Description(string item_Description)
        {
            this.item_Description = item_Description;
            return this;
        }

        public TestIrmaDbItemBuilder WithSign_Description(string sign_Description)
        {
            this.sign_Description = sign_Description;
            return this;
        }

        public TestIrmaDbItemBuilder WithIngredients(string ingredients)
        {
            this.ingredients = ingredients;
            return this;
        }

        public TestIrmaDbItemBuilder WithSubTeam_No(int subTeam_No)
        {
            this.subTeam_No = subTeam_No;
            return this;
        }

        public TestIrmaDbItemBuilder WithSales_Account(string sales_Account)
        {
            this.sales_Account = sales_Account;
            return this;
        }

        public TestIrmaDbItemBuilder WithPackage_Desc1(decimal package_Desc1)
        {
            this.package_Desc1 = package_Desc1;
            return this;
        }

        public TestIrmaDbItemBuilder WithPackage_Desc2(decimal package_Desc2)
        {
            this.package_Desc2 = package_Desc2;
            return this;
        }

        public TestIrmaDbItemBuilder WithPackage_Unit_ID(int package_Unit_ID)
        {
            this.package_Unit_ID = package_Unit_ID;
            return this;
        }

        public TestIrmaDbItemBuilder WithMin_Temperature(short min_Temperature)
        {
            this.min_Temperature = min_Temperature;
            return this;
        }

        public TestIrmaDbItemBuilder WithMax_Temperature(short max_Temperature)
        {
            this.max_Temperature = max_Temperature;
            return this;
        }

        public TestIrmaDbItemBuilder WithUnits_Per_Pallet(short units_Per_Pallet)
        {
            this.units_Per_Pallet = units_Per_Pallet;
            return this;
        }

        public TestIrmaDbItemBuilder WithAverage_Unit_Weight(System.Nullable<decimal> average_Unit_Weight)
        {
            this.average_Unit_Weight = average_Unit_Weight;
            return this;
        }

        public TestIrmaDbItemBuilder WithTie(byte tie)
        {
            this.tie = tie;
            return this;
        }

        public TestIrmaDbItemBuilder WithHigh(byte high)
        {
            this.high = high;
            return this;
        }

        public TestIrmaDbItemBuilder WithYield(decimal yield)
        {
            this.yield = yield;
            return this;
        }

        public TestIrmaDbItemBuilder WithBrand_ID(System.Nullable<int> brand_ID)
        {
            this.brand_ID = brand_ID;
            return this;
        }

        public TestIrmaDbItemBuilder WithCategory_ID(System.Nullable<int> category_ID)
        {
            this.category_ID = category_ID;
            return this;
        }

        public TestIrmaDbItemBuilder WithOrigin_ID(System.Nullable<int> origin_ID)
        {
            this.origin_ID = origin_ID;
            return this;
        }

        public TestIrmaDbItemBuilder WithShelfLife_Length(short shelfLife_Length)
        {
            this.shelfLife_Length = shelfLife_Length;
            return this;
        }

        public TestIrmaDbItemBuilder WithShelfLife_ID(System.Nullable<int> shelfLife_ID)
        {
            this.shelfLife_ID = shelfLife_ID;
            return this;
        }

        public TestIrmaDbItemBuilder WithRetail_Unit_ID(int retail_Unit_ID)
        {
            this.retail_Unit_ID = retail_Unit_ID;
            return this;
        }

        public TestIrmaDbItemBuilder WithVendor_Unit_ID(int vendor_Unit_ID)
        {
            this.vendor_Unit_ID = vendor_Unit_ID;
            return this;
        }

        public TestIrmaDbItemBuilder WithDistribution_Unit_ID(int distribution_Unit_ID)
        {
            this.distribution_Unit_ID = distribution_Unit_ID;
            return this;
        }

        public TestIrmaDbItemBuilder WithCost_Unit_ID(System.Nullable<int> cost_Unit_ID)
        {
            this.cost_Unit_ID = cost_Unit_ID;
            return this;
        }

        public TestIrmaDbItemBuilder WithFreight_Unit_ID(System.Nullable<int> freight_Unit_ID)
        {
            this.freight_Unit_ID = freight_Unit_ID;
            return this;
        }

        public TestIrmaDbItemBuilder WithDeleted_Item(bool deleted_Item)
        {
            this.deleted_Item = deleted_Item;
            return this;
        }

        public TestIrmaDbItemBuilder WithWFM_Item(bool wFM_Item)
        {
            this.wFM_Item = wFM_Item;
            return this;
        }

        public TestIrmaDbItemBuilder WithNot_Available(bool not_Available)
        {
            this.not_Available = not_Available;
            return this;
        }

        public TestIrmaDbItemBuilder WithPre_Order(bool pre_Order)
        {
            this.pre_Order = pre_Order;
            return this;
        }

        public TestIrmaDbItemBuilder WithRemove_Item(byte remove_Item)
        {
            this.remove_Item = remove_Item;
            return this;
        }

        public TestIrmaDbItemBuilder WithNoDistMarkup(bool noDistMarkup)
        {
            this.noDistMarkup = noDistMarkup;
            return this;
        }

        public TestIrmaDbItemBuilder WithOrganic(bool organic)
        {
            this.organic = organic;
            return this;
        }

        public TestIrmaDbItemBuilder WithRefrigerated(bool refrigerated)
        {
            this.refrigerated = refrigerated;
            return this;
        }

        public TestIrmaDbItemBuilder WithKeep_Frozen(bool keep_Frozen)
        {
            this.keep_Frozen = keep_Frozen;
            return this;
        }

        public TestIrmaDbItemBuilder WithShipper_Item(bool shipper_Item)
        {
            this.shipper_Item = shipper_Item;
            return this;
        }

        public TestIrmaDbItemBuilder WithFull_Pallet_Only(bool full_Pallet_Only)
        {
            this.full_Pallet_Only = full_Pallet_Only;
            return this;
        }

        public TestIrmaDbItemBuilder WithUser_ID(System.Nullable<int> user_ID)
        {
            this.user_ID = user_ID;
            return this;
        }

        public TestIrmaDbItemBuilder WithPOS_Description(string pOS_Description)
        {
            this.pOS_Description = pOS_Description;
            return this;
        }

        public TestIrmaDbItemBuilder WithRetail_Sale(bool retail_Sale)
        {
            this.retail_Sale = retail_Sale;
            return this;
        }

        public TestIrmaDbItemBuilder WithFood_Stamps(bool food_Stamps)
        {
            this.food_Stamps = food_Stamps;
            return this;
        }

        public TestIrmaDbItemBuilder WithDiscountable(bool discountable)
        {
            this.discountable = discountable;
            return this;
        }

        public TestIrmaDbItemBuilder WithPrice_Required(bool price_Required)
        {
            this.price_Required = price_Required;
            return this;
        }

        public TestIrmaDbItemBuilder WithQuantity_Required(bool quantity_Required)
        {
            this.quantity_Required = quantity_Required;
            return this;
        }

        public TestIrmaDbItemBuilder WithItemType_ID(int itemType_ID)
        {
            this.itemType_ID = itemType_ID;
            return this;
        }

        public TestIrmaDbItemBuilder WithHFM_Item(bool hFM_Item)
        {
            this.hFM_Item = hFM_Item;
            return this;
        }

        public TestIrmaDbItemBuilder WithScaleDesc1(string scaleDesc1)
        {
            this.scaleDesc1 = scaleDesc1;
            return this;
        }

        public TestIrmaDbItemBuilder WithScaleDesc2(string scaleDesc2)
        {
            this.scaleDesc2 = scaleDesc2;
            return this;
        }

        public TestIrmaDbItemBuilder WithNot_AvailableNote(string not_AvailableNote)
        {
            this.not_AvailableNote = not_AvailableNote;
            return this;
        }

        public TestIrmaDbItemBuilder WithCountryProc_ID(System.Nullable<int> countryProc_ID)
        {
            this.countryProc_ID = countryProc_ID;
            return this;
        }

        public TestIrmaDbItemBuilder WithInsert_Date(System.DateTime insert_Date)
        {
            this.insert_Date = insert_Date;
            return this;
        }

        public TestIrmaDbItemBuilder WithManufacturing_Unit_ID(System.Nullable<int> manufacturing_Unit_ID)
        {
            this.manufacturing_Unit_ID = manufacturing_Unit_ID;
            return this;
        }

        public TestIrmaDbItemBuilder WithEXEDistributed(bool eXEDistributed)
        {
            this.eXEDistributed = eXEDistributed;
            return this;
        }

        public TestIrmaDbItemBuilder WithClassID(System.Nullable<int> classID)
        {
            this.classID = classID;
            return this;
        }

        public TestIrmaDbItemBuilder WithUser_ID_Date(System.Nullable<System.DateTime> user_ID_Date)
        {
            this.user_ID_Date = user_ID_Date;
            return this;
        }

        public TestIrmaDbItemBuilder WithDistSubTeam_No(System.Nullable<int> distSubTeam_No)
        {
            this.distSubTeam_No = distSubTeam_No;
            return this;
        }

        public TestIrmaDbItemBuilder WithCostedByWeight(bool costedByWeight)
        {
            this.costedByWeight = costedByWeight;
            return this;
        }

        public TestIrmaDbItemBuilder WithTaxClassID(System.Nullable<int> taxClassID)
        {
            this.taxClassID = taxClassID;
            return this;
        }

        public TestIrmaDbItemBuilder WithLabelType_ID(System.Nullable<int> labelType_ID)
        {
            this.labelType_ID = labelType_ID;
            return this;
        }

        public TestIrmaDbItemBuilder WithScaleDesc3(string scaleDesc3)
        {
            this.scaleDesc3 = scaleDesc3;
            return this;
        }

        public TestIrmaDbItemBuilder WithScaleDesc4(string scaleDesc4)
        {
            this.scaleDesc4 = scaleDesc4;
            return this;
        }

        public TestIrmaDbItemBuilder WithScaleTare(System.Nullable<int> scaleTare)
        {
            this.scaleTare = scaleTare;
            return this;
        }

        public TestIrmaDbItemBuilder WithScaleUseBy(System.Nullable<int> scaleUseBy)
        {
            this.scaleUseBy = scaleUseBy;
            return this;
        }

        public TestIrmaDbItemBuilder WithScaleForcedTare(string scaleForcedTare)
        {
            this.scaleForcedTare = scaleForcedTare;
            return this;
        }

        public TestIrmaDbItemBuilder WithQtyProhibit(System.Nullable<bool> qtyProhibit)
        {
            this.qtyProhibit = qtyProhibit;
            return this;
        }

        public TestIrmaDbItemBuilder WithGroupList(System.Nullable<int> groupList)
        {
            this.groupList = groupList;
            return this;
        }

        public TestIrmaDbItemBuilder WithProdHierarchyLevel4_ID(System.Nullable<int> prodHierarchyLevel4_ID)
        {
            this.prodHierarchyLevel4_ID = prodHierarchyLevel4_ID;
            return this;
        }

        public TestIrmaDbItemBuilder WithCase_Discount(System.Nullable<bool> case_Discount)
        {
            this.case_Discount = case_Discount;
            return this;
        }

        public TestIrmaDbItemBuilder WithCoupon_Multiplier(System.Nullable<bool> coupon_Multiplier)
        {
            this.coupon_Multiplier = coupon_Multiplier;
            return this;
        }

        public TestIrmaDbItemBuilder WithMisc_Transaction_Sale(System.Nullable<short> misc_Transaction_Sale)
        {
            this.misc_Transaction_Sale = misc_Transaction_Sale;
            return this;
        }

        public TestIrmaDbItemBuilder WithMisc_Transaction_Refund(System.Nullable<short> misc_Transaction_Refund)
        {
            this.misc_Transaction_Refund = misc_Transaction_Refund;
            return this;
        }

        public TestIrmaDbItemBuilder WithRecall_Flag(System.Nullable<bool> recall_Flag)
        {
            this.recall_Flag = recall_Flag;
            return this;
        }

        public TestIrmaDbItemBuilder WithManager_ID(System.Nullable<byte> manager_ID)
        {
            this.manager_ID = manager_ID;
            return this;
        }

        public TestIrmaDbItemBuilder WithIce_Tare(System.Nullable<int> ice_Tare)
        {
            this.ice_Tare = ice_Tare;
            return this;
        }

        public TestIrmaDbItemBuilder WithLockAuth(System.Nullable<bool> lockAuth)
        {
            this.lockAuth = lockAuth;
            return this;
        }

        public TestIrmaDbItemBuilder WithPurchaseThresholdCouponAmount(System.Nullable<decimal> purchaseThresholdCouponAmount)
        {
            this.purchaseThresholdCouponAmount = purchaseThresholdCouponAmount;
            return this;
        }

        public TestIrmaDbItemBuilder WithPurchaseThresholdCouponSubTeam(System.Nullable<bool> purchaseThresholdCouponSubTeam)
        {
            this.purchaseThresholdCouponSubTeam = purchaseThresholdCouponSubTeam;
            return this;
        }

        public TestIrmaDbItemBuilder WithProduct_Code(string product_Code)
        {
            this.product_Code = product_Code;
            return this;
        }

        public TestIrmaDbItemBuilder WithUnit_Price_Category(System.Nullable<int> unit_Price_Category)
        {
            this.unit_Price_Category = unit_Price_Category;
            return this;
        }

        public TestIrmaDbItemBuilder WithStoreJurisdictionID(System.Nullable<int> storeJurisdictionID)
        {
            this.storeJurisdictionID = storeJurisdictionID;
            return this;
        }

        public TestIrmaDbItemBuilder WithCatchweightRequired(bool catchweightRequired)
        {
            this.catchweightRequired = catchweightRequired;
            return this;
        }

        public TestIrmaDbItemBuilder WithCOOL(bool cOOL)
        {
            this.cOOL = cOOL;
            return this;
        }

        public TestIrmaDbItemBuilder WithBIO(bool bIO)
        {
            this.bIO = bIO;
            return this;
        }

        public TestIrmaDbItemBuilder WithLastModifiedUser_ID(System.Nullable<int> lastModifiedUser_ID)
        {
            this.lastModifiedUser_ID = lastModifiedUser_ID;
            return this;
        }

        public TestIrmaDbItemBuilder WithLastModifiedDate(System.Nullable<System.DateTime> lastModifiedDate)
        {
            this.lastModifiedDate = lastModifiedDate;
            return this;
        }

        public TestIrmaDbItemBuilder WithCatchWtReq(bool catchWtReq)
        {
            this.catchWtReq = catchWtReq;
            return this;
        }

        public TestIrmaDbItemBuilder WithSustainabilityRankingRequired(System.Nullable<bool> sustainabilityRankingRequired)
        {
            this.sustainabilityRankingRequired = sustainabilityRankingRequired;
            return this;
        }

        public TestIrmaDbItemBuilder WithSustainabilityRankingID(System.Nullable<int> sustainabilityRankingID)
        {
            this.sustainabilityRankingID = sustainabilityRankingID;
            return this;
        }

        public TestIrmaDbItemBuilder WithIngredient(bool ingredient)
        {
            this.ingredient = ingredient;
            return this;
        }

        public TestIrmaDbItemBuilder WithFSA_Eligible(bool fSA_Eligible)
        {
            this.fSA_Eligible = fSA_Eligible;
            return this;
        }

        public TestIrmaDbItemBuilder WithTaxClassModifiedDate(System.Nullable<System.DateTime> taxClassModifiedDate)
        {
            this.taxClassModifiedDate = taxClassModifiedDate;
            return this;
        }

        public TestIrmaDbItemBuilder WithUseLastReceivedCost(System.Nullable<bool> useLastReceivedCost)
        {
            this.useLastReceivedCost = useLastReceivedCost;
            return this;
        }

        public TestIrmaDbItemBuilder WithGiftCard(bool giftCard)
        {
            this.giftCard = giftCard;
            return this;
        }

        public TestIrmaDbItemBuilder WithItemIdentifier(ItemIdentifier itemIdentifierBuilder)
        {
            if(this.itemIdentifiers == null)
            {
                this.itemIdentifiers = new HashSet<ItemIdentifier>();
            }

            this.itemIdentifiers.Add(itemIdentifierBuilder);
            return this;
        }

        public TestIrmaDbItemBuilder WithItemSignAttribute(ItemSignAttribute irmaItemSignAttributeBuilder)
        {
            if(this.itemSignAttributes == null)
            {
                this.itemSignAttributes = new HashSet<ItemSignAttribute>();
            }

            this.itemSignAttributes.Add(irmaItemSignAttributeBuilder);
            return this;
        }

        public TestIrmaDbItemBuilder WithItemUnit3(ItemUnit itemUnit)
        {
            this.itemUnit3 = itemUnit;
            return this;
        }

        public TestIrmaDbItemBuilder WithSubTeam(SubTeam subTeam)
        {
            this.subTeam = subTeam;
            return this;
        }

        public TestIrmaDbItemBuilder WithItemUnit4(ItemUnit itemUnit)
        {
            this.itemUnit4 = itemUnit;
            return this;
        }

        public Item Build()
        {
            Item item = new Item();

            item.Item_Key = this.item_Key;
            item.Item_Description = this.item_Description;
            item.Sign_Description = this.sign_Description;
            item.Ingredients = this.ingredients;
            item.SubTeam_No = this.subTeam_No;
            item.Sales_Account = this.sales_Account;
            item.Package_Desc1 = this.package_Desc1;
            item.Package_Desc2 = this.package_Desc2;
            item.Package_Unit_ID = this.package_Unit_ID;
            item.Min_Temperature = this.min_Temperature;
            item.Max_Temperature = this.max_Temperature;
            item.Units_Per_Pallet = this.units_Per_Pallet;
            item.Average_Unit_Weight = this.average_Unit_Weight;
            item.Tie = this.tie;
            item.High = this.high;
            item.Yield = this.yield;
            item.Brand_ID = this.brand_ID;
            item.Category_ID = this.category_ID;
            item.Origin_ID = this.origin_ID;
            item.ShelfLife_Length = this.shelfLife_Length;
            item.ShelfLife_ID = this.shelfLife_ID;
            item.Retail_Unit_ID = this.retail_Unit_ID;
            item.Vendor_Unit_ID = this.vendor_Unit_ID;
            item.Distribution_Unit_ID = this.distribution_Unit_ID;
            item.Cost_Unit_ID = this.cost_Unit_ID;
            item.Freight_Unit_ID = this.freight_Unit_ID;
            item.Deleted_Item = this.deleted_Item;
            item.WFM_Item = this.wFM_Item;
            item.Not_Available = this.not_Available;
            item.Pre_Order = this.pre_Order;
            item.Remove_Item = this.remove_Item;
            item.NoDistMarkup = this.noDistMarkup;
            item.Organic = this.organic;
            item.Refrigerated = this.refrigerated;
            item.Keep_Frozen = this.keep_Frozen;
            item.Shipper_Item = this.shipper_Item;
            item.Full_Pallet_Only = this.full_Pallet_Only;
            item.User_ID = this.user_ID;
            item.POS_Description = this.pOS_Description;
            item.Retail_Sale = this.retail_Sale;
            item.Food_Stamps = this.food_Stamps;
            item.Discountable = this.discountable;
            item.Price_Required = this.price_Required;
            item.Quantity_Required = this.quantity_Required;
            item.ItemType_ID = this.itemType_ID;
            item.HFM_Item = this.hFM_Item;
            item.ScaleDesc1 = this.scaleDesc1;
            item.ScaleDesc2 = this.scaleDesc2;
            item.Not_AvailableNote = this.not_AvailableNote;
            item.CountryProc_ID = this.countryProc_ID;
            item.Insert_Date = this.insert_Date;
            item.Manufacturing_Unit_ID = this.manufacturing_Unit_ID;
            item.EXEDistributed = this.eXEDistributed;
            item.ClassID = this.classID;
            item.User_ID_Date = this.user_ID_Date;
            item.DistSubTeam_No = this.distSubTeam_No;
            item.CostedByWeight = this.costedByWeight;
            item.TaxClassID = this.taxClassID;
            item.LabelType_ID = this.labelType_ID;
            item.ScaleDesc3 = this.scaleDesc3;
            item.ScaleDesc4 = this.scaleDesc4;
            item.ScaleTare = this.scaleTare;
            item.ScaleUseBy = this.scaleUseBy;
            item.ScaleForcedTare = this.scaleForcedTare;
            item.QtyProhibit = this.qtyProhibit;
            item.GroupList = this.groupList;
            item.ProdHierarchyLevel4_ID = this.prodHierarchyLevel4_ID;
            item.Case_Discount = this.case_Discount;
            item.Coupon_Multiplier = this.coupon_Multiplier;
            item.Misc_Transaction_Sale = this.misc_Transaction_Sale;
            item.Misc_Transaction_Refund = this.misc_Transaction_Refund;
            item.Recall_Flag = this.recall_Flag;
            item.Manager_ID = this.manager_ID;
            item.Ice_Tare = this.ice_Tare;
            item.LockAuth = this.lockAuth;
            item.PurchaseThresholdCouponAmount = this.purchaseThresholdCouponAmount;
            item.PurchaseThresholdCouponSubTeam = this.purchaseThresholdCouponSubTeam;
            item.Product_Code = this.product_Code;
            item.Unit_Price_Category = this.unit_Price_Category;
            item.StoreJurisdictionID = this.storeJurisdictionID;
            item.CatchweightRequired = this.catchweightRequired;
            item.COOL = this.cOOL;
            item.BIO = this.bIO;
            item.LastModifiedUser_ID = this.lastModifiedUser_ID;
            item.LastModifiedDate = this.lastModifiedDate;
            item.CatchWtReq = this.catchWtReq;
            item.SustainabilityRankingRequired = this.sustainabilityRankingRequired;
            item.SustainabilityRankingID = this.sustainabilityRankingID;
            item.Ingredient = this.ingredient;
            item.FSA_Eligible = this.fSA_Eligible;
            item.TaxClassModifiedDate = this.taxClassModifiedDate;
            item.UseLastReceivedCost = this.useLastReceivedCost;
            item.GiftCard = this.giftCard;
            item.ItemIdentifier = this.itemIdentifiers;
            item.ItemSignAttribute = this.itemSignAttributes;
            item.ItemUnit3 = this.itemUnit3;
            item.ItemUnit4 = this.itemUnit4;
            item.SubTeam = this.subTeam;

            return item;
        }

        public static implicit operator Item(TestIrmaDbItemBuilder builder)
        {
            return builder.Build();
        }
    }
}