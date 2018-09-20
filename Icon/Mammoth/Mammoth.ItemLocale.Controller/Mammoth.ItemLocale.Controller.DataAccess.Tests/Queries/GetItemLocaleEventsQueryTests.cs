using Dapper;
using Irma.Framework;
using Irma.Testing;
using Mammoth.Common;
using Mammoth.Common.DataAccess;
using Mammoth.Common.DataAccess.DbProviders;
using Mammoth.ItemLocale.Controller.DataAccess.Models;
using Mammoth.ItemLocale.Controller.DataAccess.Queries;
using Mammoth.ItemLocale.Controller.DataAccess.Tests.TestInfrastructure;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Mammoth.ItemLocale.Controller.DataAccess.Tests.Queries
{
    [TestClass]
    public class GetItemLocaleEventsQueryTests
    {
        private GetItemLocaleDataQuery query;
        private GetItemLocaleDataParameters parameters;
        private SqlDbProvider dbProvider;
        private int storeNumber;

        protected string region
        {
            get
            {
                var regionForTesting = AppSettingsAccessor.GetStringSetting("RegionForTesting", false);
                return regionForTesting ?? "FL";
            }
        }

        protected string irmaDatabaseForRegion
        {
            get
            {
                return $"ItemCatalog_{region}";
            }
        }

        protected string irmaConnectionString
        {
            get
            {
                return ConfigurationManager.ConnectionStrings[irmaDatabaseForRegion].ConnectionString;
            }
        }

        [TestInitialize]
        public void Initialize()
        {
            dbProvider = new SqlDbProvider();
            dbProvider.Connection = new SqlConnection(irmaConnectionString);
            dbProvider.Connection.Open();
            dbProvider.Transaction = dbProvider.Connection.BeginTransaction(IsolationLevel.Snapshot);
            query = new GetItemLocaleDataQuery(dbProvider);
            parameters = new GetItemLocaleDataParameters { Instance = 777, Region = region };

            IEnumerable<int> validStores = this.dbProvider.Connection
                .Query<int>(@"DECLARE @ExcludedStoreNo varchar(250);
                            SET @ExcludedStoreNo = (SELECT dbo.fn_GetAppConfigValue('LabAndClosedStoreNo','IRMA Client'));
                            SELECT Store_No FROM Store 
                            WHERE 
                                Store_No NOT IN (SELECT Key_Value as BusinessUnitId FROM dbo.fn_Parse_List(@ExcludedStoreNo, '|'))
                                AND (WFM_Store = 1 OR Mega_Store = 1 )
                                AND (Internal = 1 AND BusinessUnit_ID IS NOT NULL);",
                                transaction: this.dbProvider.Transaction);
            this.storeNumber = validStores.First();
        }

        [TestCleanup]
        public void Cleanup()
        {
            dbProvider.Transaction.Rollback();
            dbProvider.Transaction.Dispose();
            dbProvider.Connection.Dispose();
        }

        [TestMethod]
        public void GetItemLocaleEvents_HasItemNutritionExtraTextOnly_ShouldReturnExtraText()
        {
            // Given
            var storeNo = 834792;
            var subTeamNo = this.GetFirstFromTable<SubTeam>().SubTeam_No;
            var vendorId = this.GetFirstFromTable<Vendor>().Vendor_ID;
            var expectedRetailUnit = this.GetFirstFromTable<ItemUnit>();
            var expectedOrigin = this.GetFirstFromTable<ItemOrigin>();
            var expectedLabelType = this.GetFirstFromTable<LabelType>();
            var expectedLinkedItemKey = this.GetFirstFromTable<Item>().Item_Key;
            var expectedLinkedIdentifier = this.GetLinkedIdentifierByItemKey(expectedLinkedItemKey);
            var expectedScaleExtraTextId = this.GetFirstFromTable<ItemScale>().Scale_ExtraText_ID;
            var expectedRegion = region;
            var expectedIdentifier = "22222242";
            var expectedBusinessUnitId = 83472;
            var expectedAgeRestrictionId = 2;
            var expectedAgeRestriction = 21;
            var expectedAuthorized = true;
            var expectedCaseDiscount = true;
            var expectedDiscontinued = false;
            var expectedLocalItem = true;
            var expectedLocality = "Test Locality";
            var expectedProductCode = "TestProductCode";
            var expectedRestrictedHours = true;
            var expectedSignRomanceLong = "Test Sign Romance Long";
            var expectedSignRomanceShort = "Test Sign Romance Short";
            var expectedSignDescription = "Test Sign Description";
            var expectedTmDiscount = true;
            var expectedMsrp = 2000m;
            var expectedChicagoBaby = "Test Baby";
            var expectedColorAdd = true;
            var expectedElectronicShelfTag = true;
            var expectedExclusive = DateTime.Today;
            var expectedNumberOfDigitsSentToScale = 123;
            var expectedScaleExtraText = "Test Item Nutrition Extra Text";
            var expectedTagUom = 23;
            var expectedEventTypeid = IrmaEventTypes.ItemLocaleAddOrUpdate;
            var expectedDefaultScanCode = false;

            // Insert New Item
            var itemKey = InsertNewItem(subTeamNo, expectedSignDescription, expectedProductCode,
                expectedOrigin.Origin_ID, expectedLabelType.LabelType_ID, expectedRetailUnit.Unit_ID);

            // Insert New Item Identifier
            InsertNewItemIdentifier(itemKey, expectedIdentifier, expectedNumberOfDigitsSentToScale, expectedDefaultScanCode);

            // Insert New Store
            InsertNewStore(storeNo, expectedBusinessUnitId, 1, true);

            // Insert New Store Region Mapping
            InsertNewStoreRegionMapping(storeNo, expectedRegion);

            // Insert New Price
            InsertNewPrice(expectedCaseDiscount, expectedElectronicShelfTag, expectedLocalItem, expectedRestrictedHours, expectedTmDiscount,
                expectedMsrp, storeNo, itemKey, expectedAgeRestrictionId, expectedLinkedItemKey);

            // Insert New VSC, Item-Vendor, Store-Item-Vendor, Store-Item
            InsertNewValidatedScanCode(expectedIdentifier);
            InsertItemVendor(itemKey, vendorId);
            InsertStoreItemVendor(vendorId, storeNo, itemKey, expectedDiscontinued, true, DateTime.Now);
            InsertStoreItem(storeNo, itemKey, expectedAuthorized);

            // Insert Sign Attributes
            InsertItemSignAttributes(itemKey, expectedChicagoBaby, expectedColorAdd, expectedLocality, expectedSignRomanceLong,
                expectedSignRomanceShort, expectedTagUom, expectedExclusive);

            // Insert Item Extra Text
            var itemExtraTextId = this.dbProvider.Insert(new IrmaQueryParams<Item_ExtraText, int>(
                IrmaTestObjectFactory.Build<Item_ExtraText>()
                    .With(x => x.ExtraText, expectedScaleExtraText)
                    .ToObject(),
                x => x.Item_ExtraText_ID));

            // Insert Item Nutrition
            this.dbProvider.Insert(new IrmaQueryParams<ItemNutrition, int>(
                IrmaTestObjectFactory.Build<ItemNutrition>()
                    .With(x => x.ItemKey, itemKey)
                    .With(x => x.Item_ExtraText_ID, itemExtraTextId)
                    .ToObject(),
                x => x.ItemNutritionId));

            // Insert Scale Extra Text
            //var scaleExtraTextId = this.dbProvider.Insert(new IrmaQueryParams<Scale_ExtraText, int>(
            //    IrmaTestObjectFactory.Build<Scale_ExtraText>()
            //        .With(x => x.ExtraText, expectedScaleExtraText)
            //        .With(x => x.Description, "Dummy")
            //        .ToObject(),
            //    x => x.Scale_ExtraText_ID));

            // Insert Item Scale
            //this.dbProvider.Insert(new IrmaQueryParams<ItemScale, int>(
            //    IrmaTestObjectFactory.Build<ItemScale>()
            //        .With(x => x.Item_Key, itemKey)
            //        .ToObject(),
            //    x => x.ItemScale_ID));

            var expectedQueueId = InsertToItemLocaleChangeQueue(itemKey, storeNo, expectedIdentifier, IrmaEventTypes.ItemLocaleAddOrUpdate);

            //When
            var actual = query.Search(parameters).First();

            //Then
            Assert.AreEqual(expectedQueueId, actual.QueueId, "The expected QueueID did not match the actual.");
            Assert.AreEqual(expectedEventTypeid, actual.EventTypeId, "The expected EventTypeId did not match the actual.");
            Assert.AreEqual(expectedAgeRestriction, actual.AgeRestriction, "The expected AgeCode did not match the actual.");
            Assert.AreEqual(expectedAuthorized, actual.Authorized, "The expected AuthorizedForSale did not match the actual.");
            Assert.AreEqual(expectedBusinessUnitId, actual.BusinessUnitId, "The expected BusinessUnit did not match the actual.");
            Assert.AreEqual(expectedCaseDiscount, actual.CaseDiscount, "The expected CaseDiscountEligible did not match the actual.");
            Assert.AreEqual(expectedRestrictedHours, actual.RestrictedHours, "The expected RestrictedHours did not match the actual.");
            Assert.AreEqual(expectedIdentifier, actual.ScanCode, "The expected ScanCode did not match the actual.");
            Assert.AreEqual(expectedChicagoBaby, actual.ChicagoBaby, "The expected ChicagoBaby did not match the actual.");
            Assert.AreEqual(expectedColorAdd, actual.ColorAdded, "The expected ColorAdded did not match the actual.");
            Assert.AreEqual(expectedOrigin.Origin_Name, actual.CountryOfProcessing, "The expected CountryOfProcessing did not match the actual.");
            Assert.AreEqual(expectedDiscontinued, actual.Discontinued, "The expected Discontinued did not match the actual.");
            Assert.AreEqual(expectedElectronicShelfTag, actual.ElectronicShelfTag, "The expected ElectronicShelfTag did not match the actual.");
            Assert.AreEqual(expectedExclusive, actual.Exclusive, "The expected Exclusive did not match the actual.");
            Assert.AreEqual(expectedLabelType.LabelTypeDesc, actual.LabelTypeDescription, "The expected LabelTypeDescription did not match the actual.");
            Assert.AreEqual(expectedLinkedIdentifier, actual.LinkedItem, "The expected LinkedItem did not match the actual.");
            Assert.AreEqual(expectedLocalItem, actual.LocalItem, "The expected LocalItem did not match the actual.");
            Assert.AreEqual(expectedLocality, actual.Locality, "The expected Locality did not match the actual.");
            Assert.AreEqual(expectedNumberOfDigitsSentToScale, actual.NumberOfDigitsSentToScale, "The expected NumberOfDigitsSentToScale did not match the actual.");
            Assert.AreEqual(expectedOrigin.Origin_Name, actual.Origin, "The expected Origin did not match the actual.");
            Assert.AreEqual(expectedProductCode, actual.ProductCode, "The expected ProductCode did not match the actual.");
            Assert.AreEqual(expectedRegion, actual.Region, "The expected Region did not match the actual.");
            Assert.AreEqual(expectedRetailUnit.Unit_Name, actual.RetailUnit, "The expected RetailUnit did not match the actual.");
            Assert.AreEqual(expectedScaleExtraText, actual.ScaleExtraText, "The expected ScaleExtraText did not match the actual.");
            Assert.AreEqual(expectedSignDescription, actual.SignDescription, "The expected SignDescription did not match the actual.");
            Assert.AreEqual(expectedSignRomanceLong, actual.SignRomanceLong, "The expected SignRomanceLong did not match the actual.");
            Assert.AreEqual(expectedSignRomanceShort, actual.SignRomanceShort, "The expected SignRomanceShort did not match the actual.");
            Assert.AreEqual(expectedTagUom.ToString(), actual.TagUom, "The expected TagUom did not match the actual.");
            Assert.AreEqual(Convert.ToDouble(expectedMsrp), actual.Msrp, "The expected Msrp did not match the actual.");
            Assert.AreEqual(expectedDefaultScanCode, actual.DefaultScanCode, "The expected DefaultScanCode did not match the actual.");
            Assert.IsTrue(string.IsNullOrEmpty(actual.ErrorMessage));
        }

        [TestMethod]
        public void GetItemLocaleEvents_AddUpdateEventsExistsWithStoreNumbersForNonTsRegion_ShouldReturnEventsAndAllData()
        {
            // Given
            var storeNo = 834792;
            var subTeamNo = this.GetFirstFromTable<SubTeam>().SubTeam_No;
            var vendor = this.GetFirstFromTable<Vendor>();
            var vendorId = vendor.Vendor_ID;
            var costUnitId = this.GetFirstFromTable<ItemUnit>().Unit_ID;
            var freightUnitId = costUnitId;
            var expectedRetailUnit = this.GetFirstFromTable<ItemUnit>();
            var expectedOrigin = this.GetFirstFromTable<ItemOrigin>();
            var expectedLabelType = this.GetFirstFromTable<LabelType>();
            var expectedLinkedItemKey = this.GetFirstFromTable<Item>().Item_Key;
            var expectedLinkedIdentifier = this.GetLinkedIdentifierByItemKey(expectedLinkedItemKey);
            var expectedScaleExtraTextId = this.GetFirstFromTable<ItemScale>().Scale_ExtraText_ID;
            var expectedRegion = region;
            var expectedIdentifier = "22222242";
            var expectedBusinessUnitId = 83472;
            var expectedAgeRestrictionId = 2;
            var expectedAgeRestriction = 21;
            var expectedAuthorized = true;
            var expectedCaseDiscount = true;
            var expectedDiscontinued = false;
            var expectedLocalItem = true;
            var expectedLocality = "Test Locality";
            var expectedProductCode = "TestProductCode";
            var expectedRestrictedHours = true;
            var expectedSignRomanceLong = "Test Sign Romance Long";
            var expectedSignRomanceShort = "Test Sign Romance Short";
            var expectedSignDescription = "Test Sign Description";
            var expectedTmDiscount = true;
            var expectedMsrp = 2000m;
            var expectedChicagoBaby = "Test Baby";
            var expectedColorAdd = true;
            var expectedElectronicShelfTag = true;
            var expectedExclusive = DateTime.Today;
            var expectedNumberOfDigitsSentToScale = 123;
            var expectedScaleExtraText = "Test Scale Extra Text";
            var expectedTagUom = 23;
            var expectedEventTypeid = IrmaEventTypes.ItemLocaleAddOrUpdate;
            var expectedVendorItemId = "123456789789456";
            var expectedVendorCaseSize = 12;
            var expectedVendorKey = vendor.Vendor_Key;
            var expectedVendorCompanyName = vendor.CompanyName;
            var expectedOrderedByInfor = true;
            var expectedDefaultScanCode = true;
            decimal? expectedAltRetailSize = null;
            string expectedAltRetailUOM = null;
            bool? expectedForceTare = true;
            bool? expectedSendtoCFS = null;
            string expectedWrappedTareWeight = "9999";
            string expectedUnwrappedTareWeight = "9998";
            bool? expectedScaleItem = null;
            string expectedUseBy = "Test";
            int? expectedShelfLife = null;

            // Insert New Item
            var itemKey = InsertNewItem(subTeamNo, expectedSignDescription, expectedProductCode,
                expectedOrigin.Origin_ID, expectedLabelType.LabelType_ID, expectedRetailUnit.Unit_ID);

            // Insert New Item Identifier
            InsertNewItemIdentifier(itemKey, expectedIdentifier, expectedNumberOfDigitsSentToScale, expectedDefaultScanCode);

            // Insert New Store
            InsertNewStore(storeNo, expectedBusinessUnitId, 1, true);

            // Insert New Store Region Mapping
            InsertNewStoreRegionMapping(storeNo, expectedRegion);

            // Insert New Price
            InsertNewPrice(expectedCaseDiscount, expectedElectronicShelfTag, expectedLocalItem, expectedRestrictedHours, expectedTmDiscount,
                expectedMsrp, storeNo, itemKey, expectedAgeRestrictionId, expectedLinkedItemKey);

            // Insert New Validated Scan Code, Item-Vendor, Store-Item-Vendor
            InsertNewValidatedScanCode(expectedIdentifier);
            InsertItemVendor(itemKey, vendorId, expectedVendorItemId);
            var storeItemVendorId = InsertStoreItemVendor(vendorId, storeNo, itemKey, expectedDiscontinued, true, DateTime.Now);

            // Insert VendorCostHistory
            this.dbProvider.Insert(new IrmaQueryParams<VendorCostHistory, int>(
                IrmaTestObjectFactory.BuildVendorCostHistory()
                    .With(x => x.CostUnit_ID, costUnitId)
                    .With(x => x.FreightUnit_ID, freightUnitId)
                    .With(x => x.Package_Desc1, expectedVendorCaseSize)
                    .With(x => x.StoreItemVendorID, storeItemVendorId)
                    .ToObject(),
                x => x.VendorCostHistoryID));

            // Insert StoreItem
            InsertStoreItem(storeNo, itemKey, expectedAuthorized);

            // Insert Sign Attributes
            InsertItemSignAttributes(itemKey, expectedChicagoBaby, expectedColorAdd, expectedLocality, expectedSignRomanceLong,
                expectedSignRomanceShort, expectedTagUom, expectedExclusive);

            // Insert WrappedTareWeight
            var wrappedTareWeight = this.dbProvider.Insert(new IrmaQueryParams<Scale_Tare, int>(
                IrmaTestObjectFactory.Build<Scale_Tare>()
                    .With(x => x.Description, expectedWrappedTareWeight)
                    .ToObject(),
                x => x.Scale_Tare_ID));

            // Insert UnwrappedTareWeight
            var unwrappedTareWeight = this.dbProvider.Insert(new IrmaQueryParams<Scale_Tare, int>(
                IrmaTestObjectFactory.Build<Scale_Tare>()
                    .With(x => x.Description, expectedUnwrappedTareWeight)
                    .ToObject(),
                x => x.Scale_Tare_ID));

            var scale_EatBy_ID = this.dbProvider.Insert(new IrmaQueryParams<Scale_EatBy, int>(
               IrmaTestObjectFactory.Build<Scale_EatBy>()
                   .With(x => x.Description, expectedUseBy)
                   .ToObject(),
               x => x.Scale_EatBy_ID));

            // Insert Scale Extra Text
            var scaleExtraTextId = this.dbProvider.Insert(new IrmaQueryParams<Scale_ExtraText, int>(
                IrmaTestObjectFactory.Build<Scale_ExtraText>()
                    .With(x => x.ExtraText, expectedScaleExtraText)
                    .With(x => x.Description, "Dummy")
                    .ToObject(),
                x => x.Scale_ExtraText_ID));

            // Insert Item Scale
            this.dbProvider.Insert(new IrmaQueryParams<ItemScale, int>(
                IrmaTestObjectFactory.Build<ItemScale>()
                    .With(x => x.Item_Key, itemKey)
                    .With(x => x.Scale_ExtraText_ID, scaleExtraTextId)
                    .With(x => x.ForceTare, expectedForceTare)
                    .With(x => x.ShelfLife_Length, expectedShelfLife)
                    .With(x => x.Scale_Tare_ID, wrappedTareWeight)
                    .With(x => x.Scale_Alternate_Tare_ID, unwrappedTareWeight)
                    .With(x => x.Scale_EatBy_ID, scale_EatBy_ID)
                    .ToObject(),
                x => x.ItemScale_ID));

            // Insert StoreItemExtended
            this.dbProvider.Insert(new IrmaQueryParams<StoreItemExtended, int>(
                IrmaTestObjectFactory.Build<StoreItemExtended>()
                    .With(x => x.Item_Key, itemKey)
                    .With(x => x.Store_No, storeNo)
                    .With(x => x.OrderedByInfor, expectedOrderedByInfor)
                    .ToObject(),
                x => x.StoreItemExtendedID));

            var expectedQueueId = InsertToItemLocaleChangeQueue(itemKey, storeNo, expectedIdentifier, IrmaEventTypes.ItemLocaleAddOrUpdate);

            //When
            var actual = query.Search(parameters).First();

            //Then
            Assert.AreEqual(expectedQueueId, actual.QueueId, "The expected QueueID did not match the actual.");
            Assert.AreEqual(expectedEventTypeid, actual.EventTypeId, "The expected EventTypeId did not match the actual.");
            Assert.AreEqual(expectedAgeRestriction, actual.AgeRestriction, "The expected AgeCode did not match the actual.");
            Assert.AreEqual(expectedAuthorized, actual.Authorized, "The expected AuthorizedForSale did not match the actual.");
            Assert.AreEqual(expectedBusinessUnitId, actual.BusinessUnitId, "The expected BusinessUnit did not match the actual.");
            Assert.AreEqual(expectedCaseDiscount, actual.CaseDiscount, "The expected CaseDiscountEligible did not match the actual.");
            Assert.AreEqual(expectedRestrictedHours, actual.RestrictedHours, "The expected RestrictedHours did not match the actual.");
            Assert.AreEqual(expectedIdentifier, actual.ScanCode, "The expected ScanCode did not match the actual.");
            Assert.AreEqual(expectedChicagoBaby, actual.ChicagoBaby, "The expected ChicagoBaby did not match the actual.");
            Assert.AreEqual(expectedColorAdd, actual.ColorAdded, "The expected ColorAdded did not match the actual.");
            Assert.AreEqual(expectedOrigin.Origin_Name, actual.CountryOfProcessing, "The expected CountryOfProcessing did not match the actual.");
            Assert.AreEqual(expectedDiscontinued, actual.Discontinued, "The expected Discontinued did not match the actual.");
            Assert.AreEqual(expectedElectronicShelfTag, actual.ElectronicShelfTag, "The expected ElectronicShelfTag did not match the actual.");
            Assert.AreEqual(expectedExclusive, actual.Exclusive, "The expected Exclusive did not match the actual.");
            Assert.AreEqual(expectedLabelType.LabelTypeDesc, actual.LabelTypeDescription, "The expected LabelTypeDescription did not match the actual.");
            Assert.AreEqual(expectedLinkedIdentifier, actual.LinkedItem, "The expected LinkedItem did not match the actual.");
            Assert.AreEqual(expectedLocalItem, actual.LocalItem, "The expected LocalItem did not match the actual.");
            Assert.AreEqual(expectedLocality, actual.Locality, "The expected Locality did not match the actual.");
            Assert.AreEqual(expectedNumberOfDigitsSentToScale, actual.NumberOfDigitsSentToScale, "The expected NumberOfDigitsSentToScale did not match the actual.");
            Assert.AreEqual(expectedOrigin.Origin_Name, actual.Origin, "The expected Origin did not match the actual.");
            Assert.AreEqual(expectedProductCode, actual.ProductCode, "The expected ProductCode did not match the actual.");
            Assert.AreEqual(expectedRegion, actual.Region, "The expected Region did not match the actual.");
            Assert.AreEqual(expectedRetailUnit.Unit_Name, actual.RetailUnit, "The expected RetailUnit did not match the actual.");
            Assert.AreEqual(expectedScaleExtraText, actual.ScaleExtraText, "The expected ScaleExtraText did not match the actual.");
            Assert.AreEqual(expectedSignDescription, actual.SignDescription, "The expected SignDescription did not match the actual.");
            Assert.AreEqual(expectedSignRomanceLong, actual.SignRomanceLong, "The expected SignRomanceLong did not match the actual.");
            Assert.AreEqual(expectedSignRomanceShort, actual.SignRomanceShort, "The expected SignRomanceShort did not match the actual.");
            Assert.AreEqual(expectedTagUom.ToString(), actual.TagUom, "The expected TagUom did not match the actual.");
            Assert.AreEqual(Convert.ToDouble(expectedMsrp), actual.Msrp, "The expected Msrp did not match the actual.");
            Assert.AreEqual(expectedVendorItemId, actual.VendorItemId, "The expected VendorItemId did not match the actual.");
            Assert.AreEqual(expectedVendorCaseSize, actual.VendorCaseSize, "The expected VendorCaseSize did not match the actual.");
            Assert.AreEqual(expectedVendorKey, actual.VendorKey, "The expected VendorKey did not match the actual.");
            Assert.AreEqual(expectedVendorCompanyName, actual.VendorCompanyName, "The expected VendorCompanyName did not match the actual.");
            Assert.AreEqual(expectedOrderedByInfor, actual.OrderedByInfor, "The expected OrderedByInfor did not match the actual.");
            Assert.AreEqual(expectedDefaultScanCode, actual.DefaultScanCode, "The expected DefaultScanCode did not match the actual.");
            Assert.AreEqual(expectedAltRetailSize, actual.AltRetailSize, "The expected AltRetailSize did not match the actual.");
            Assert.AreEqual(expectedAltRetailUOM, actual.AltRetailUOM, "The expected AltRetailUOM did not match the actual.");
            Assert.IsTrue(string.IsNullOrEmpty(actual.ErrorMessage));
            Assert.AreEqual(expectedForceTare, actual.ForceTare, "The expected ForceTare did not match the actual.");
            Assert.AreEqual(expectedWrappedTareWeight, actual.WrappedTareWeight, "The expected WrappedTareWeight did not match the actual.");
            Assert.AreEqual(expectedUnwrappedTareWeight, actual.UnwrappedTareWeight, "The expected UnWrappedTareWeight did not match the actual.");
            Assert.AreEqual(expectedScaleItem, actual.ScaleItem, "The expected ScaleItem did not match the actual.");
            Assert.AreEqual(expectedUseBy, actual.UseBy, "The expected UseBy did not match the actual.");
            Assert.AreEqual(expectedShelfLife, actual.ShelfLife, "The expected ShelfLife did not match the actual.");
            Assert.AreEqual(expectedSendtoCFS, actual.SendtoCFS, "The expected SendtoCFS did not match the actual.");
        }

        [TestMethod]
        public void GetItemLocaleEvents_AddUpdateEventsExistsWithItemOverride_ShouldReturnEventsWithAltSizeAndAltUOM()
        {
            // Given
            var storeNo = 834792;
            var subTeamNo = this.GetFirstFromTable<SubTeam>().SubTeam_No;
            var vendor = this.GetFirstFromTable<Vendor>();
            var vendorId = vendor.Vendor_ID;
            var costUnitId = this.GetFirstFromTable<ItemUnit>().Unit_ID;
            var freightUnitId = costUnitId;
            var expectedRetailUnit = this.GetFirstFromTable<ItemUnit>();
            var expectedOrigin = this.GetFirstFromTable<ItemOrigin>();
            var expectedLabelType = this.GetFirstFromTable<LabelType>();
            var expectedLinkedItemKey = this.GetFirstFromTable<Item>().Item_Key;
            var expectedLinkedIdentifier = this.GetLinkedIdentifierByItemKey(expectedLinkedItemKey);
            var expectedScaleExtraTextId = this.GetFirstFromTable<ItemScale>().Scale_ExtraText_ID;
            var expectedRegion = region;
            var expectedIdentifier = "22222242";
            var expectedBusinessUnitId = 83472;
            var expectedAgeRestrictionId = 2;
            var expectedAgeRestriction = 21;
            var expectedAuthorized = true;
            var expectedCaseDiscount = true;
            var expectedDiscontinued = false;
            var expectedLocalItem = true;
            var expectedLocality = "Test Locality";
            var expectedProductCode = "TestProductCode";
            var expectedRestrictedHours = true;
            var expectedSignRomanceLong = "Test Sign Romance Long";
            var expectedSignRomanceShort = "Test Sign Romance Short";
            var expectedSignDescription = "Test Override Sign_Description";
            var expectedTmDiscount = true;
            var expectedMsrp = 2000m;
            var expectedChicagoBaby = "Test Baby";
            var expectedColorAdd = true;
            var expectedElectronicShelfTag = true;
            var expectedExclusive = DateTime.Today;
            var expectedNumberOfDigitsSentToScale = 123;
            var expectedScaleExtraText = "Test Scale Extra Text";
            var expectedTagUom = 23;
            var expectedEventTypeid = IrmaEventTypes.ItemLocaleAddOrUpdate;
            var expectedVendorItemId = "123456789789456";
            var expectedVendorCaseSize = 12;
            var expectedVendorKey = vendor.Vendor_Key;
            var expectedVendorCompanyName = vendor.CompanyName;
            var expectedOrderedByInfor = true;
            var expectedDefaultScanCode = false;
            var expectedAltRetailSize = 9.8m;
            var expectedAltRetailUOM = this.GetFirstFromTable<ItemUnit>();
            var testItemUnitId = this.GetFirstFromTable<ItemUnit>().Unit_ID;

            // Insert new Store Jurisdiction
            var storeJurisdictionID = this.dbProvider.Insert(
                new IrmaQueryParams<StoreJurisdiction, int>(
                IrmaTestObjectFactory.Build<StoreJurisdiction>()
                    .With(x => x.CurrencyID, 1)
                    .With(x => x.StoreJurisdictionDesc, "Test")
                    .ToObject(),
                x => x.StoreJurisdictionID));

            // Insert New Item
            var itemKey = InsertNewItem(subTeamNo, expectedSignDescription, expectedProductCode,
                expectedOrigin.Origin_ID, expectedLabelType.LabelType_ID, expectedRetailUnit.Unit_ID);

            // Insert New Item Identifier
            InsertNewItemIdentifier(itemKey, expectedIdentifier, expectedNumberOfDigitsSentToScale, expectedDefaultScanCode);

            // Insert New Store
            InsertNewStore(storeNo, expectedBusinessUnitId, storeJurisdictionID, true);

            // Insert New Store Region Mapping
            InsertNewStoreRegionMapping(storeNo, expectedRegion);

            // Insert New Price
            InsertNewPrice(expectedCaseDiscount, expectedElectronicShelfTag, expectedLocalItem, expectedRestrictedHours, expectedTmDiscount,
                expectedMsrp, storeNo, itemKey, expectedAgeRestrictionId, expectedLinkedItemKey);

            // Insert New Validated Scan Code, Item-Vendor, Store-Item-Vendor
            InsertNewValidatedScanCode(expectedIdentifier);
            InsertItemVendor(itemKey, vendorId, expectedVendorItemId);
            var storeItemVendorId = InsertStoreItemVendor(vendorId, storeNo, itemKey, expectedDiscontinued, true, DateTime.Now);

            // Insert VendorCostHistory
            this.dbProvider.Insert(new IrmaQueryParams<VendorCostHistory, int>(
                IrmaTestObjectFactory.BuildVendorCostHistory()
                    .With(x => x.CostUnit_ID, costUnitId)
                    .With(x => x.FreightUnit_ID, freightUnitId)
                    .With(x => x.Package_Desc1, expectedVendorCaseSize)
                    .With(x => x.StoreItemVendorID, storeItemVendorId)
                    .ToObject(),
                x => x.VendorCostHistoryID));

            // Insert StoreItem
            InsertStoreItem(storeNo, itemKey, expectedAuthorized);

            // Insert Sign Attributes
            InsertItemSignAttributes(itemKey, expectedChicagoBaby, expectedColorAdd, expectedLocality,
                expectedSignRomanceLong, expectedSignRomanceShort, expectedTagUom, expectedExclusive);

            // Insert Scale Extra Text
            var scaleExtraTextId = this.dbProvider.Insert(new IrmaQueryParams<Scale_ExtraText, int>(
                IrmaTestObjectFactory.Build<Scale_ExtraText>()
                    .With(x => x.ExtraText, expectedScaleExtraText)
                    .With(x => x.Description, "Dummy")
                    .ToObject(),
                x => x.Scale_ExtraText_ID));

            // Insert Item Scale
            this.dbProvider.Insert(new IrmaQueryParams<ItemScale, int>(
                IrmaTestObjectFactory.Build<ItemScale>()
                    .With(x => x.Item_Key, itemKey)
                    .With(x => x.Scale_ExtraText_ID, scaleExtraTextId)
                    .ToObject(),
                x => x.ItemScale_ID));

            // Insert StoreItemExtended
            this.dbProvider.Insert(new IrmaQueryParams<StoreItemExtended, int>(
                IrmaTestObjectFactory.Build<StoreItemExtended>()
                    .With(x => x.Item_Key, itemKey)
                    .With(x => x.Store_No, storeNo)
                    .With(x => x.OrderedByInfor, expectedOrderedByInfor)
                    .ToObject(),
                x => x.StoreItemExtendedID));

            // Insert ItemOverride
            this.dbProvider.Insert(
                IrmaTestObjectFactory.BuildItemOverride()
                    .With(x => x.Item_Key, itemKey)
                    .With(x => x.Package_Desc2, expectedAltRetailSize)
                    .With(x => x.Package_Unit_ID, expectedAltRetailUOM.Unit_ID)
                    .With(x => x.StoreJurisdictionID, storeJurisdictionID)
                    .With(x => x.Distribution_Unit_ID, testItemUnitId)
                    .With(x => x.Retail_Unit_ID, testItemUnitId)
                    .With(x => x.Vendor_Unit_ID, testItemUnitId)
                    .ToObject());

            var expectedQueueId = InsertToItemLocaleChangeQueue(itemKey, storeNo, expectedIdentifier, IrmaEventTypes.ItemLocaleAddOrUpdate);

            //When
            var actual = query.Search(parameters).First();

            //Then
            Assert.AreEqual(expectedQueueId, actual.QueueId, "The expected QueueID did not match the actual.");
            Assert.AreEqual(expectedEventTypeid, actual.EventTypeId, "The expected EventTypeId did not match the actual.");
            Assert.AreEqual(expectedAgeRestriction, actual.AgeRestriction, "The expected AgeCode did not match the actual.");
            Assert.AreEqual(expectedAuthorized, actual.Authorized, "The expected AuthorizedForSale did not match the actual.");
            Assert.AreEqual(expectedBusinessUnitId, actual.BusinessUnitId, "The expected BusinessUnit did not match the actual.");
            Assert.AreEqual(expectedCaseDiscount, actual.CaseDiscount, "The expected CaseDiscountEligible did not match the actual.");
            Assert.AreEqual(expectedRestrictedHours, actual.RestrictedHours, "The expected RestrictedHours did not match the actual.");
            Assert.AreEqual(expectedIdentifier, actual.ScanCode, "The expected ScanCode did not match the actual.");
            Assert.AreEqual(expectedChicagoBaby, actual.ChicagoBaby, "The expected ChicagoBaby did not match the actual.");
            Assert.AreEqual(expectedColorAdd, actual.ColorAdded, "The expected ColorAdded did not match the actual.");
            Assert.AreEqual(expectedOrigin.Origin_Name, actual.CountryOfProcessing, "The expected CountryOfProcessing did not match the actual.");
            Assert.AreEqual(expectedDiscontinued, actual.Discontinued, "The expected Discontinued did not match the actual.");
            Assert.AreEqual(expectedElectronicShelfTag, actual.ElectronicShelfTag, "The expected ElectronicShelfTag did not match the actual.");
            Assert.AreEqual(expectedExclusive, actual.Exclusive, "The expected Exclusive did not match the actual.");
            Assert.AreEqual(expectedLabelType.LabelTypeDesc, actual.LabelTypeDescription, "The expected LabelTypeDescription did not match the actual.");
            Assert.AreEqual(expectedLinkedIdentifier, actual.LinkedItem, "The expected LinkedItem did not match the actual.");
            Assert.AreEqual(expectedLocalItem, actual.LocalItem, "The expected LocalItem did not match the actual.");
            Assert.AreEqual(expectedLocality, actual.Locality, "The expected Locality did not match the actual.");
            Assert.AreEqual(expectedNumberOfDigitsSentToScale, actual.NumberOfDigitsSentToScale, "The expected NumberOfDigitsSentToScale did not match the actual.");
            Assert.AreEqual(expectedOrigin.Origin_Name, actual.Origin, "The expected Origin did not match the actual.");
            Assert.AreEqual(expectedProductCode, actual.ProductCode, "The expected ProductCode did not match the actual.");
            Assert.AreEqual(expectedRegion, actual.Region, "The expected Region did not match the actual.");
            Assert.AreEqual(expectedRetailUnit.Unit_Name, actual.RetailUnit, "The expected RetailUnit did not match the actual.");
            Assert.AreEqual(expectedScaleExtraText, actual.ScaleExtraText, "The expected ScaleExtraText did not match the actual.");
            Assert.AreEqual(expectedSignDescription, actual.SignDescription, "The expected SignDescription did not match the actual.");
            Assert.AreEqual(expectedSignRomanceLong, actual.SignRomanceLong, "The expected SignRomanceLong did not match the actual.");
            Assert.AreEqual(expectedSignRomanceShort, actual.SignRomanceShort, "The expected SignRomanceShort did not match the actual.");
            Assert.AreEqual(expectedTagUom.ToString(), actual.TagUom, "The expected TagUom did not match the actual.");
            Assert.AreEqual(Convert.ToDouble(expectedMsrp), actual.Msrp, "The expected Msrp did not match the actual.");
            Assert.AreEqual(expectedVendorItemId, actual.VendorItemId, "The expected VendorItemId did not match the actual.");
            Assert.AreEqual(expectedVendorCaseSize, actual.VendorCaseSize, "The expected VendorCaseSize did not match the actual.");
            Assert.AreEqual(expectedVendorKey, actual.VendorKey, "The expected VendorKey did not match the actual.");
            Assert.AreEqual(expectedVendorCompanyName, actual.VendorCompanyName, "The expected VendorCompanyName did not match the actual.");
            Assert.AreEqual(expectedOrderedByInfor, actual.OrderedByInfor, "The expected OrderedByInfor did not match the actual.");
            Assert.AreEqual(expectedDefaultScanCode, actual.DefaultScanCode, "The expected DefaultScanCode did not match the actual.");
            Assert.AreEqual(expectedAltRetailSize, actual.AltRetailSize, "The expected AltRetailSize did not match the actual.");
            Assert.AreEqual(expectedAltRetailUOM.Unit_Abbreviation, actual.AltRetailUOM, "The expected AltRetailUOM did not match the actual.");
            Assert.IsTrue(string.IsNullOrEmpty(actual.ErrorMessage));
        }

        [TestMethod]
        public void GetItemLocaleEvents_EventsDontExistWithInProcessByEqualToInstance_ShouldReturnEmptyList()
        {
            //When
            var results = query.Search(parameters);

            //Then
            Assert.IsFalse(results.Any());
        }

        [TestMethod]
        public void GetItemLocaleEvents_DeauthorizeEventsExistWithNullStoreNumbers_ShouldReturnOneRowForEachStoreAssociatedAndAuthorizedValueIsFalse()
        {
            // Given
            var storeNo = 834792;
            var expectedInternalStore = true;
            var expectedWfmStore = true;
            var subTeamNo = this.GetFirstFromTable<SubTeam>().SubTeam_No;
            var vendorId = this.GetFirstFromTable<Vendor>().Vendor_ID;
            var expectedRetailUnit = this.GetFirstFromTable<ItemUnit>();
            var expectedOrigin = this.GetFirstFromTable<ItemOrigin>();
            var expectedLabelType = this.GetFirstFromTable<LabelType>();
            var expectedLinkedItemKey = this.GetFirstFromTable<Item>().Item_Key;
            var expectedLinkedIdentifier = this.GetLinkedIdentifierByItemKey(expectedLinkedItemKey);
            var expectedScaleExtraTextId = this.GetFirstFromTable<ItemScale>().Scale_ExtraText_ID;
            var expectedRegion = region;
            var expectedIdentifier = "1234567";
            var expectedBusinessUnitId = 83472;
            var expectedAgeRestrictionId = 2;
            var expectedAgeRestriction = 21;
            var expectedAuthorized = true;
            var expectedCaseDiscount = true;
            var expectedDiscontinued = false;
            var expectedLocalItem = true;
            var expectedLocality = "Test Locality";
            var expectedProductCode = "TestProductCode";
            var expectedRestrictedHours = true;
            var expectedSignRomanceLong = "Test Sign Romance Long";
            var expectedSignRomanceShort = "Test Sign Romance Short";
            var expectedSignDescription = "Test Sign Description";
            var expectedTmDiscount = true;
            var expectedMsrp = 2000m;
            var expectedChicagoBaby = "Test Baby";
            var expectedColorAdd = true;
            var expectedElectronicShelfTag = true;
            var expectedExclusive = DateTime.Today;
            var expectedNumberOfDigitsSentToScale = 123;
            var expectedScaleExtraText = "Test Scale Extra Text";
            var expectedTagUom = 23;
            var expectedEventTypeid = IrmaEventTypes.ItemDelete;
            var expectedStoreNumbers = this.GetValidStoreNumbers();
            var storeRegionMapper = this.GetStoreRegionMapping();
            var expectedRows = expectedStoreNumbers.Count();

            // Insert New Item
            var itemKey = InsertNewItem(subTeamNo, expectedSignDescription, expectedProductCode,
                expectedOrigin.Origin_ID, expectedLabelType.LabelType_ID, expectedRetailUnit.Unit_ID);

            // Insert New Item Identifier
            InsertNewItemIdentifier(itemKey, expectedIdentifier, expectedNumberOfDigitsSentToScale);

            // Insert New Store
            InsertNewStore(storeNo, expectedBusinessUnitId, 1, expectedWfmStore, expectedInternalStore);

            // Insert New Store Region Mapping
            InsertNewStoreRegionMapping(storeNo, expectedRegion);

            expectedStoreNumbers.ForEach(s =>
            {
                // Insert New Price
                InsertNewPrice(expectedCaseDiscount, expectedElectronicShelfTag, expectedLocalItem, expectedRestrictedHours, expectedTmDiscount,
                    expectedMsrp, s.Store_No, itemKey, expectedAgeRestrictionId, expectedLinkedItemKey);
            });

            // Insert New Validated Scan Code, Item-Vendor
            InsertNewValidatedScanCode(expectedIdentifier);
            InsertItemVendor(itemKey, vendorId);

            expectedStoreNumbers.ForEach(s =>
            {
                // Insert StoreItemVendor
                InsertStoreItemVendor(vendorId, s.Store_No, itemKey, expectedDiscontinued, true, DateTime.Now);
            });

            // Insert StoreItem            
            expectedStoreNumbers.ForEach(s =>
            {
                InsertStoreItem(s.Store_No, itemKey, expectedAuthorized);
            });

            // Insert Sign Attributes
            InsertItemSignAttributes(itemKey, expectedChicagoBaby, expectedColorAdd, expectedLocality, expectedSignRomanceLong,
                expectedSignRomanceShort, expectedTagUom, expectedExclusive);

            // Insert Scale Extra Text
            var scaleExtraTextId = this.dbProvider.Insert(new IrmaQueryParams<Scale_ExtraText, int>(
                IrmaTestObjectFactory.Build<Scale_ExtraText>()
                    .With(x => x.ExtraText, expectedScaleExtraText)
                    .With(x => x.Description, "Dummy")
                    .ToObject(),
                x => x.Scale_ExtraText_ID));

            // Insert Item Scale
            this.dbProvider.Insert(new IrmaQueryParams<ItemScale, int>(
                IrmaTestObjectFactory.Build<ItemScale>()
                    .With(x => x.Item_Key, itemKey)
                    .With(x => x.Scale_ExtraText_ID, scaleExtraTextId)
                    .ToObject(),
                x => x.ItemScale_ID));
;
            var expectedQueueId = InsertToItemLocaleChangeQueue(itemKey, null, expectedIdentifier, expectedEventTypeid);

            // When
            var actualRowSet = query.Search(parameters).OrderBy(r => r.BusinessUnitId).ToList();

            // Then
            Assert.AreEqual(expectedRows, actualRowSet.Count, "The number of actual rows did not match expected number of rows.");

            expectedStoreNumbers.OrderBy(s => s.BusinessUnit_ID).Select((s, i) => new { Index = i, Store = s }).ToList().ForEach(s =>
            {
                Assert.AreEqual(expectedQueueId, actualRowSet[s.Index].QueueId, "The expected QueueID did not match the actual.");
                Assert.AreEqual(expectedEventTypeid, actualRowSet[s.Index].EventTypeId, "The expected EventTypeId did not match the actual.");
                Assert.AreEqual(expectedAgeRestriction, actualRowSet[s.Index].AgeRestriction, "The expected AgeCode did not match the actual.");
                Assert.AreEqual(false, actualRowSet[s.Index].Authorized, "The expected AuthorizedForSale did not match the actual.");
                Assert.AreEqual(s.Store.BusinessUnit_ID, actualRowSet[s.Index].BusinessUnitId, "The expected BusinessUnit did not match the actual.");
                Assert.AreEqual(expectedCaseDiscount, actualRowSet[s.Index].CaseDiscount, "The expected CaseDiscountEligible did not match the actual.");
                Assert.AreEqual(expectedRestrictedHours, actualRowSet[s.Index].RestrictedHours, "The expected RestrictedHours did not match the actual.");
                Assert.AreEqual(expectedIdentifier, actualRowSet[s.Index].ScanCode, "The expected ScanCode did not match the actual.");
                Assert.AreEqual(expectedChicagoBaby, actualRowSet[s.Index].ChicagoBaby, "The expected ChicagoBaby did not match the actual.");
                Assert.AreEqual(expectedColorAdd, actualRowSet[s.Index].ColorAdded, "The expected ColorAdded did not match the actual.");
                Assert.AreEqual(expectedOrigin.Origin_Name, actualRowSet[s.Index].CountryOfProcessing, "The expected CountryOfProcessing did not match the actual.");
                Assert.AreEqual(expectedDiscontinued, actualRowSet[s.Index].Discontinued, "The expected Discontinued did not match the actual.");
                Assert.AreEqual(expectedElectronicShelfTag, actualRowSet[s.Index].ElectronicShelfTag, "The expected ElectronicShelfTag did not match the actual.");
                Assert.AreEqual(expectedExclusive, actualRowSet[s.Index].Exclusive, "The expected Exclusive did not match the actual.");
                Assert.AreEqual(expectedLabelType.LabelTypeDesc, actualRowSet[s.Index].LabelTypeDescription, "The expected LabelTypeDescription did not match the actual.");
                Assert.AreEqual(expectedLinkedIdentifier, actualRowSet[s.Index].LinkedItem, "The expected LinkedItem did not match the actual.");
                Assert.AreEqual(expectedLocalItem, actualRowSet[s.Index].LocalItem, "The expected LocalItem did not match the actual.");
                Assert.AreEqual(expectedLocality, actualRowSet[s.Index].Locality, "The expected Locality did not match the actual.");
                Assert.AreEqual(expectedNumberOfDigitsSentToScale, actualRowSet[s.Index].NumberOfDigitsSentToScale, "The expected NumberOfDigitsSentToScale did not match the actual.");
                Assert.AreEqual(expectedOrigin.Origin_Name, actualRowSet[s.Index].Origin, "The expected Origin did not match the actual.");
                Assert.AreEqual(expectedProductCode, actualRowSet[s.Index].ProductCode, "The expected ProductCode did not match the actual.");
                Assert.AreEqual(storeRegionMapper.First(m => m.Store_No == s.Store.Store_No).Region_Code, actualRowSet[s.Index].Region, "The expected Region did not match the actual.");
                Assert.AreEqual(expectedRetailUnit.Unit_Name, actualRowSet[s.Index].RetailUnit, "The expected RetailUnit did not match the actual.");
                Assert.AreEqual(expectedScaleExtraText, actualRowSet[s.Index].ScaleExtraText, "The expected ScaleExtraText did not match the actual.");
                Assert.AreEqual(expectedSignDescription, actualRowSet[s.Index].SignDescription, "The expected SignDescription did not match the actual.");
                Assert.AreEqual(expectedSignRomanceLong, actualRowSet[s.Index].SignRomanceLong, "The expected SignRomanceLong did not match the actual.");
                Assert.AreEqual(expectedSignRomanceShort, actualRowSet[s.Index].SignRomanceShort, "The expected SignRomanceShort did not match the actual.");
                Assert.AreEqual(expectedTagUom.ToString(), actualRowSet[s.Index].TagUom, "The expected TagUom did not match the actual.");
                Assert.AreEqual(Convert.ToDouble(expectedMsrp), actualRowSet[s.Index].Msrp, "The expected Msrp did not match the actual.");
                Assert.IsTrue(string.IsNullOrEmpty(actualRowSet[s.Index].ErrorMessage));
            });
        }

        [TestMethod]
        public void GetItemLocaleEvents_AddUpdateEventsExistWithNullStoreNumber_ReturnsOneRowForEachStoreAssociatedToTheItem()
        {
            // Given
            var storeNo = 834792;
            var expectedInternalStore = true;
            var expectedWfmStore = true;
            var subTeamNo = this.GetFirstFromTable<SubTeam>().SubTeam_No;
            var vendorId = this.GetFirstFromTable<Vendor>().Vendor_ID;
            var expectedRetailUnit = this.GetFirstFromTable<ItemUnit>();
            var expectedOrigin = this.GetFirstFromTable<ItemOrigin>();
            var expectedLabelType = this.GetFirstFromTable<LabelType>();
            var expectedLinkedItemKey = this.GetFirstFromTable<Item>().Item_Key;
            var expectedLinkedIdentifier = this.GetLinkedIdentifierByItemKey(expectedLinkedItemKey);
            var expectedScaleExtraTextId = this.GetFirstFromTable<ItemScale>().Scale_ExtraText_ID;
            var expectedRegion = region;
            var expectedIdentifier = "1234567";
            var expectedBusinessUnitId = 83472;
            var expectedAgeRestrictionId = 2;
            var expectedAgeRestriction = 21;
            var expectedAuthorized = true;
            var expectedCaseDiscount = true;
            var expectedDiscontinued = false;
            var expectedLocalItem = true;
            var expectedLocality = "Test Locality";
            var expectedProductCode = "TestProductCode";
            var expectedRestrictedHours = true;
            var expectedSignRomanceLong = "Test Sign Romance Long";
            var expectedSignRomanceShort = "Test Sign Romance Short";
            var expectedSignDescription = "Test Sign Description";
            var expectedTmDiscount = true;
            var expectedMsrp = 2000m;
            var expectedChicagoBaby = "Test Baby";
            var expectedColorAdd = true;
            var expectedElectronicShelfTag = true;
            var expectedExclusive = DateTime.Today;
            var expectedNumberOfDigitsSentToScale = 123;
            var expectedScaleExtraText = "Test Scale Extra Text";
            var expectedTagUom = 23;
            var expectedEventTypeid = IrmaEventTypes.ItemLocaleAddOrUpdate;
            var expectedStoreNumbers = this.GetValidStoreNumbers();
            var storeRegionMapper = this.GetStoreRegionMapping();
            var expectedRows = expectedStoreNumbers.Count();

            // Insert New Item
            var itemKey = InsertNewItem(subTeamNo, expectedSignDescription, expectedProductCode,
                expectedOrigin.Origin_ID, expectedLabelType.LabelType_ID, expectedRetailUnit.Unit_ID);

            // Insert New Item Identifier
            InsertNewItemIdentifier(itemKey, expectedIdentifier, expectedNumberOfDigitsSentToScale);

            // Insert New Store
            InsertNewStore(storeNo, expectedBusinessUnitId, 1, expectedWfmStore, expectedInternalStore);

            // Insert New Store Region Mapping
            InsertNewStoreRegionMapping(storeNo, expectedRegion);

            expectedStoreNumbers.ForEach(s =>
            {
                // Insert New Price
                InsertNewPrice(expectedCaseDiscount, expectedElectronicShelfTag, expectedLocalItem, expectedRestrictedHours, expectedTmDiscount,
                    expectedMsrp, s.Store_No, itemKey, expectedAgeRestrictionId, expectedLinkedItemKey);
            });

            // Insert New Validated Scan Code, Item-Vendor
            InsertNewValidatedScanCode(expectedIdentifier);
            InsertItemVendor(itemKey, vendorId);

            var z = expectedStoreNumbers.Select(s => s.Store_No).ToList();

            expectedStoreNumbers.ForEach(s =>
            {
                // Insert StoreItemVendor 
                InsertStoreItemVendor(vendorId, s.Store_No, itemKey, expectedDiscontinued, true, DateTime.Now);
            });

            // Insert StoreItem
            expectedStoreNumbers.ForEach(s =>
            {
                InsertStoreItem(s.Store_No, itemKey, expectedAuthorized);
            });

            // Insert Sign Attributes
            InsertItemSignAttributes(itemKey, expectedChicagoBaby, expectedColorAdd, expectedLocality, expectedSignRomanceLong,
                expectedSignRomanceShort, expectedTagUom, expectedExclusive);

            // Insert Scale Extra Text
            var scaleExtraTextId = this.dbProvider.Insert(new IrmaQueryParams<Scale_ExtraText, int>(
                IrmaTestObjectFactory.Build<Scale_ExtraText>()
                    .With(x => x.ExtraText, expectedScaleExtraText)
                    .With(x => x.Description, "Dummy")
                    .ToObject(),
                x => x.Scale_ExtraText_ID));

            // Insert Item Scale
            this.dbProvider.Insert(new IrmaQueryParams<ItemScale, int>(
                IrmaTestObjectFactory.Build<ItemScale>()
                    .With(x => x.Item_Key, itemKey)
                    .With(x => x.Scale_ExtraText_ID, scaleExtraTextId)
                    .ToObject(),
                x => x.ItemScale_ID));

            var expectedQueueId = InsertToItemLocaleChangeQueue(itemKey, null, expectedIdentifier, expectedEventTypeid);

            // When
            var actualRowSet = query.Search(parameters).OrderBy(r => r.BusinessUnitId).ToList();

            // Then
            Assert.AreEqual(expectedRows, actualRowSet.Count, "The number of actual rows did not match expected number of rows.");

            expectedStoreNumbers.OrderBy(s => s.BusinessUnit_ID).Select((s, i) => new { Index = i, Store = s }).ToList().ForEach(s =>
            {
                Assert.AreEqual(expectedQueueId, actualRowSet[s.Index].QueueId, "The expected QueueID did not match the actual.");
                Assert.AreEqual(expectedEventTypeid, actualRowSet[s.Index].EventTypeId, "The expected EventTypeId did not match the actual.");
                Assert.AreEqual(expectedAgeRestriction, actualRowSet[s.Index].AgeRestriction, "The expected AgeCode did not match the actual.");
                Assert.AreEqual(expectedAuthorized, actualRowSet[s.Index].Authorized, "The expected AuthorizedForSale did not match the actual.");
                Assert.AreEqual(s.Store.BusinessUnit_ID, actualRowSet[s.Index].BusinessUnitId, "The expected BusinessUnit did not match the actual.");
                Assert.AreEqual(expectedCaseDiscount, actualRowSet[s.Index].CaseDiscount, "The expected CaseDiscountEligible did not match the actual.");
                Assert.AreEqual(expectedRestrictedHours, actualRowSet[s.Index].RestrictedHours, "The expected RestrictedHours did not match the actual.");
                Assert.AreEqual(expectedIdentifier, actualRowSet[s.Index].ScanCode, "The expected ScanCode did not match the actual.");
                Assert.AreEqual(expectedChicagoBaby, actualRowSet[s.Index].ChicagoBaby, "The expected ChicagoBaby did not match the actual.");
                Assert.AreEqual(expectedColorAdd, actualRowSet[s.Index].ColorAdded, "The expected ColorAdded did not match the actual.");
                Assert.AreEqual(expectedOrigin.Origin_Name, actualRowSet[s.Index].CountryOfProcessing, "The expected CountryOfProcessing did not match the actual.");
                Assert.AreEqual(expectedDiscontinued, actualRowSet[s.Index].Discontinued, "The expected Discontinued did not match the actual.");
                Assert.AreEqual(expectedElectronicShelfTag, actualRowSet[s.Index].ElectronicShelfTag, "The expected ElectronicShelfTag did not match the actual.");
                Assert.AreEqual(expectedExclusive, actualRowSet[s.Index].Exclusive, "The expected Exclusive did not match the actual.");
                Assert.AreEqual(expectedLabelType.LabelTypeDesc, actualRowSet[s.Index].LabelTypeDescription, "The expected LabelTypeDescription did not match the actual.");
                Assert.AreEqual(expectedLinkedIdentifier, actualRowSet[s.Index].LinkedItem, "The expected LinkedItem did not match the actual.");
                Assert.AreEqual(expectedLocalItem, actualRowSet[s.Index].LocalItem, "The expected LocalItem did not match the actual.");
                Assert.AreEqual(expectedLocality, actualRowSet[s.Index].Locality, "The expected Locality did not match the actual.");
                Assert.AreEqual(expectedNumberOfDigitsSentToScale, actualRowSet[s.Index].NumberOfDigitsSentToScale, "The expected NumberOfDigitsSentToScale did not match the actual.");
                Assert.AreEqual(expectedOrigin.Origin_Name, actualRowSet[s.Index].Origin, "The expected Origin did not match the actual.");
                Assert.AreEqual(expectedProductCode, actualRowSet[s.Index].ProductCode, "The expected ProductCode did not match the actual.");
                Assert.AreEqual(storeRegionMapper.First(m => m.Store_No == s.Store.Store_No).Region_Code, actualRowSet[s.Index].Region, "The expected Region did not match the actual.");
                Assert.AreEqual(expectedRetailUnit.Unit_Name, actualRowSet[s.Index].RetailUnit, "The expected RetailUnit did not match the actual.");
                Assert.AreEqual(expectedScaleExtraText, actualRowSet[s.Index].ScaleExtraText, "The expected ScaleExtraText did not match the actual.");
                Assert.AreEqual(expectedSignDescription, actualRowSet[s.Index].SignDescription, "The expected SignDescription did not match the actual.");
                Assert.AreEqual(expectedSignRomanceLong, actualRowSet[s.Index].SignRomanceLong, "The expected SignRomanceLong did not match the actual.");
                Assert.AreEqual(expectedSignRomanceShort, actualRowSet[s.Index].SignRomanceShort, "The expected SignRomanceShort did not match the actual.");
                Assert.AreEqual(expectedTagUom.ToString(), actualRowSet[s.Index].TagUom, "The expected TagUom did not match the actual.");
                Assert.AreEqual(Convert.ToDouble(expectedMsrp), actualRowSet[s.Index].Msrp, "The expected Msrp did not match the actual.");
                Assert.IsTrue(string.IsNullOrEmpty(actualRowSet[s.Index].ErrorMessage));
            });
        }

        [TestMethod]
        public void GetItemLocaleEvents_AddUpdateEventsExistWithValidStoreNoAndNullStoreNo_ReturnsOneRowForEachStoreAssociatedToTheItem()
        {
            // Given
            var storeNo = 834792;
            var expectedInternalStore = true;
            var expectedWfmStore = true;
            var subTeamNo = this.GetFirstFromTable<SubTeam>().SubTeam_No;
            var vendorId = this.GetFirstFromTable<Vendor>().Vendor_ID;
            var expectedRetailUnit = this.GetFirstFromTable<ItemUnit>();
            var expectedOrigin = this.GetFirstFromTable<ItemOrigin>();
            var expectedLabelType = this.GetFirstFromTable<LabelType>();
            var expectedLinkedItemKey = this.GetFirstFromTable<Item>().Item_Key;
            var expectedLinkedIdentifier = this.GetLinkedIdentifierByItemKey(expectedLinkedItemKey);
            var expectedScaleExtraTextId = this.GetFirstFromTable<ItemScale>().Scale_ExtraText_ID;
            var expectedRegion = region;
            var expectedIdentifier = "1234567";
            var expectedBusinessUnitId = 83472;
            var expectedAgeRestrictionId = 2;
            var expectedAgeRestriction = 21;
            var expectedAuthorized = true;
            var expectedCaseDiscount = true;
            var expectedDiscontinued = false;
            var expectedLocalItem = true;
            var expectedLocality = "Test Locality";
            var expectedProductCode = "TestProdCode";
            var expectedRestrictedHours = true;
            var expectedSignRomanceLong = "Test Sign Romance Long";
            var expectedSignRomanceShort = "Test Sign Romance Short";
            var expectedSignDescription = "Test Sign Description";
            var expectedTmDiscount = true;
            var expectedMsrp = 2000m;
            var expectedChicagoBaby = "Test Baby";
            var expectedColorAdd = true;
            var expectedElectronicShelfTag = true;
            var expectedExclusive = DateTime.Today;
            var expectedNumberOfDigitsSentToScale = 123;
            var expectedScaleExtraText = "Test Scale Extra Text";
            var expectedTagUom = 23;
            var expectedEventTypeid = IrmaEventTypes.ItemLocaleAddOrUpdate;
            var expectedStoreNumbers = this.GetValidStoreNumbers();
            var storeRegionMapper = this.GetStoreRegionMapping();
            var expectedRows = expectedStoreNumbers.Count() + 1;

            // Insert New Item
            var itemKey = InsertNewItem(subTeamNo, expectedSignDescription, expectedProductCode,
                expectedOrigin.Origin_ID, expectedLabelType.LabelType_ID, expectedRetailUnit.Unit_ID);
            var itemKey2 = InsertNewItem(subTeamNo, expectedSignDescription + "2", expectedProductCode + "2",
                expectedOrigin.Origin_ID, expectedLabelType.LabelType_ID, expectedRetailUnit.Unit_ID);

            // Insert New Item Identifiers
            InsertNewItemIdentifier(itemKey, expectedIdentifier, expectedNumberOfDigitsSentToScale);
            InsertNewItemIdentifier(itemKey2, expectedIdentifier + "2", expectedNumberOfDigitsSentToScale + 2);

            // Insert New Store
            InsertNewStore(storeNo, expectedBusinessUnitId, 1, expectedWfmStore, expectedInternalStore);

            // Insert New Store Region Mapping
            InsertNewStoreRegionMapping(storeNo, expectedRegion);

            expectedStoreNumbers.ForEach(s =>
            {
                // Insert New Price
                InsertNewPrice(expectedCaseDiscount, expectedElectronicShelfTag, expectedLocalItem, expectedRestrictedHours, expectedTmDiscount,
                    expectedMsrp, s.Store_No, itemKey, expectedAgeRestrictionId, expectedLinkedItemKey);
            });

            // Insert another Price for ItemKey2
            InsertNewPrice(expectedCaseDiscount, expectedElectronicShelfTag, expectedLocalItem, expectedRestrictedHours, expectedTmDiscount,
                expectedMsrp + 2, storeNo, itemKey2, expectedAgeRestrictionId, expectedLinkedItemKey);
            
            // Insert New Validated Scan Codes
            InsertNewValidatedScanCode(expectedIdentifier);
            InsertNewValidatedScanCode(expectedIdentifier + "2");

            // Insert Item Vendor
            InsertItemVendor(itemKey, vendorId);

            expectedStoreNumbers.ForEach(s =>
            {
                // Insert StoreItemVendor
                InsertStoreItemVendor(vendorId, s.Store_No, itemKey, expectedDiscontinued, true, DateTime.Now);
            });

            // Insert StoreItem
            expectedStoreNumbers.ForEach(s =>
            {
                InsertStoreItem(s.Store_No, itemKey, expectedAuthorized);
            });


            this.dbProvider.Insert(new IrmaQueryParams<StoreItem, int>(
            IrmaTestObjectFactory.Build<StoreItem>()
                .With(x => x.Store_No, storeNo)
                .With(x => x.Item_Key, itemKey2)
                .With(x => x.Authorized, expectedAuthorized)
                .ToObject(),
            x => x.StoreItemAuthorizationID));

            // Insert Sign Attributes
            InsertItemSignAttributes(itemKey, expectedChicagoBaby, expectedColorAdd, expectedLocality, expectedSignRomanceLong,
                expectedSignRomanceShort, expectedTagUom, expectedExclusive);
            InsertItemSignAttributes(itemKey2, expectedChicagoBaby + "2", expectedColorAdd, expectedLocality + "2", expectedSignRomanceLong + "2",
                expectedSignRomanceShort + "2", expectedTagUom + 2, expectedExclusive);

            // Insert Scale Extra Text
            var scaleExtraTextId = this.dbProvider.Insert(new IrmaQueryParams<Scale_ExtraText, int>(
                IrmaTestObjectFactory.Build<Scale_ExtraText>()
                    .With(x => x.ExtraText, expectedScaleExtraText)
                    .With(x => x.Description, "Dummy")
                    .ToObject(),
                x => x.Scale_ExtraText_ID));

            var scaleExtraTextId2 = this.dbProvider.Insert(new IrmaQueryParams<Scale_ExtraText, int>(
                IrmaTestObjectFactory.Build<Scale_ExtraText>()
                    .With(x => x.ExtraText, expectedScaleExtraText + "2")
                    .With(x => x.Description, "Dummy 2")
                    .ToObject(),
                x => x.Scale_ExtraText_ID));

            // Insert Item Scale
            this.dbProvider.Insert(new IrmaQueryParams<ItemScale, int>(
                IrmaTestObjectFactory.Build<ItemScale>()
                    .With(x => x.Item_Key, itemKey)
                    .With(x => x.Scale_ExtraText_ID, scaleExtraTextId)
                    .ToObject(),
                x => x.ItemScale_ID));

            this.dbProvider.Insert(new IrmaQueryParams<ItemScale, int>(
                IrmaTestObjectFactory.Build<ItemScale>()
                    .With(x => x.Item_Key, itemKey2)
                    .With(x => x.Scale_ExtraText_ID, scaleExtraTextId2)
                    .ToObject(),
                x => x.ItemScale_ID));

            var expectedQueueId = InsertToItemLocaleChangeQueue(itemKey, null, expectedIdentifier, expectedEventTypeid);
            var expectedQueueId2 = InsertToItemLocaleChangeQueue(itemKey2, storeNo, expectedIdentifier + "2", expectedEventTypeid);

            // When
            var actualRowSet = query.Search(parameters).OrderBy(q => q.ScanCode).ThenBy(r => r.BusinessUnitId).ToList();

            // Then
            Assert.AreEqual(expectedRows, actualRowSet.Count, "The number of actual rows did not match expected number of rows.");

            expectedStoreNumbers.OrderBy(s => s.BusinessUnit_ID).Select((s, i) => new { Index = i, Store = s }).ToList().ForEach(s =>
            {
                Assert.AreEqual(expectedQueueId, actualRowSet[s.Index].QueueId, "The expected QueueID did not match the actual.");
                Assert.AreEqual(expectedEventTypeid, actualRowSet[s.Index].EventTypeId, "The expected EventTypeId did not match the actual.");
                Assert.AreEqual(expectedAgeRestriction, actualRowSet[s.Index].AgeRestriction, "The expected AgeCode did not match the actual.");
                Assert.AreEqual(expectedAuthorized, actualRowSet[s.Index].Authorized, "The expected AuthorizedForSale did not match the actual.");
                Assert.AreEqual(s.Store.BusinessUnit_ID, actualRowSet[s.Index].BusinessUnitId, "The expected BusinessUnit did not match the actual.");
                Assert.AreEqual(expectedCaseDiscount, actualRowSet[s.Index].CaseDiscount, "The expected CaseDiscountEligible did not match the actual.");
                Assert.AreEqual(expectedRestrictedHours, actualRowSet[s.Index].RestrictedHours, "The expected RestrictedHours did not match the actual.");
                Assert.AreEqual(expectedIdentifier, actualRowSet[s.Index].ScanCode, "The expected ScanCode did not match the actual.");
                Assert.AreEqual(expectedChicagoBaby, actualRowSet[s.Index].ChicagoBaby, "The expected ChicagoBaby did not match the actual.");
                Assert.AreEqual(expectedColorAdd, actualRowSet[s.Index].ColorAdded, "The expected ColorAdded did not match the actual.");
                Assert.AreEqual(expectedOrigin.Origin_Name, actualRowSet[s.Index].CountryOfProcessing, "The expected CountryOfProcessing did not match the actual.");
                Assert.AreEqual(expectedDiscontinued, actualRowSet[s.Index].Discontinued, "The expected Discontinued did not match the actual.");
                Assert.AreEqual(expectedElectronicShelfTag, actualRowSet[s.Index].ElectronicShelfTag, "The expected ElectronicShelfTag did not match the actual.");
                Assert.AreEqual(expectedExclusive, actualRowSet[s.Index].Exclusive, "The expected Exclusive did not match the actual.");
                Assert.AreEqual(expectedLabelType.LabelTypeDesc, actualRowSet[s.Index].LabelTypeDescription, "The expected LabelTypeDescription did not match the actual.");
                Assert.AreEqual(expectedLinkedIdentifier, actualRowSet[s.Index].LinkedItem, "The expected LinkedItem did not match the actual.");
                Assert.AreEqual(expectedLocalItem, actualRowSet[s.Index].LocalItem, "The expected LocalItem did not match the actual.");
                Assert.AreEqual(expectedLocality, actualRowSet[s.Index].Locality, "The expected Locality did not match the actual.");
                Assert.AreEqual(expectedNumberOfDigitsSentToScale, actualRowSet[s.Index].NumberOfDigitsSentToScale, "The expected NumberOfDigitsSentToScale did not match the actual.");
                Assert.AreEqual(expectedOrigin.Origin_Name, actualRowSet[s.Index].Origin, "The expected Origin did not match the actual.");
                Assert.AreEqual(expectedProductCode, actualRowSet[s.Index].ProductCode, "The expected ProductCode did not match the actual.");
                Assert.AreEqual(storeRegionMapper.First(m => m.Store_No == s.Store.Store_No).Region_Code, actualRowSet[s.Index].Region, "The expected Region did not match the actual.");
                Assert.AreEqual(expectedRetailUnit.Unit_Name, actualRowSet[s.Index].RetailUnit, "The expected RetailUnit did not match the actual.");
                Assert.AreEqual(expectedScaleExtraText, actualRowSet[s.Index].ScaleExtraText, "The expected ScaleExtraText did not match the actual.");
                Assert.AreEqual(expectedSignDescription, actualRowSet[s.Index].SignDescription, "The expected SignDescription did not match the actual.");
                Assert.AreEqual(expectedSignRomanceLong, actualRowSet[s.Index].SignRomanceLong, "The expected SignRomanceLong did not match the actual.");
                Assert.AreEqual(expectedSignRomanceShort, actualRowSet[s.Index].SignRomanceShort, "The expected SignRomanceShort did not match the actual.");
                Assert.AreEqual(expectedTagUom.ToString(), actualRowSet[s.Index].TagUom, "The expected TagUom did not match the actual.");
                Assert.AreEqual(Convert.ToDouble(expectedMsrp), actualRowSet[s.Index].Msrp, "The expected Msrp did not match the actual.");
                Assert.IsTrue(string.IsNullOrEmpty(actualRowSet[s.Index].ErrorMessage));
            });

            // Assert that last row is for ItemKey2
            int y = expectedStoreNumbers.Count;
            Assert.AreEqual(expectedQueueId2, actualRowSet[y].QueueId, "The expected QueueID did not match the actual.");
            Assert.AreEqual(expectedEventTypeid, actualRowSet[y].EventTypeId, "The expected EventTypeId did not match the actual.");
            Assert.AreEqual(expectedAgeRestriction, actualRowSet[y].AgeRestriction, "The expected AgeCode did not match the actual.");
            Assert.AreEqual(expectedAuthorized, actualRowSet[y].Authorized, "The expected AuthorizedForSale did not match the actual.");
            Assert.AreEqual(expectedBusinessUnitId, actualRowSet[y].BusinessUnitId, "The expected BusinessUnit did not match the actual.");
            Assert.AreEqual(expectedCaseDiscount, actualRowSet[y].CaseDiscount, "The expected CaseDiscountEligible did not match the actual.");
            Assert.AreEqual(expectedRestrictedHours, actualRowSet[y].RestrictedHours, "The expected RestrictedHours did not match the actual.");
            Assert.AreEqual(expectedIdentifier + "2", actualRowSet[y].ScanCode, "The expected ScanCode did not match the actual.");
            Assert.AreEqual(expectedChicagoBaby + "2", actualRowSet[y].ChicagoBaby, "The expected ChicagoBaby did not match the actual.");
            Assert.AreEqual(expectedColorAdd, actualRowSet[y].ColorAdded, "The expected ColorAdded did not match the actual.");
            Assert.AreEqual(expectedOrigin.Origin_Name, actualRowSet[y].CountryOfProcessing, "The expected CountryOfProcessing did not match the actual.");
            Assert.AreEqual(expectedDiscontinued, actualRowSet[y].Discontinued, "The expected Discontinued did not match the actual.");
            Assert.AreEqual(expectedElectronicShelfTag, actualRowSet[y].ElectronicShelfTag, "The expected ElectronicShelfTag did not match the actual.");
            Assert.AreEqual(expectedExclusive, actualRowSet[y].Exclusive, "The expected Exclusive did not match the actual.");
            Assert.AreEqual(expectedLabelType.LabelTypeDesc, actualRowSet[y].LabelTypeDescription, "The expected LabelTypeDescription did not match the actual.");
            Assert.AreEqual(expectedLinkedIdentifier, actualRowSet[y].LinkedItem, "The expected LinkedItem did not match the actual.");
            Assert.AreEqual(expectedLocalItem, actualRowSet[y].LocalItem, "The expected LocalItem did not match the actual.");
            Assert.AreEqual(expectedLocality + "2", actualRowSet[y].Locality, "The expected Locality did not match the actual.");
            Assert.AreEqual(expectedNumberOfDigitsSentToScale + 2, actualRowSet[y].NumberOfDigitsSentToScale, "The expected NumberOfDigitsSentToScale did not match the actual.");
            Assert.AreEqual(expectedOrigin.Origin_Name, actualRowSet[y].Origin, "The expected Origin did not match the actual.");
            Assert.AreEqual(expectedProductCode + "2", actualRowSet[y].ProductCode, "The expected ProductCode did not match the actual.");
            Assert.AreEqual(expectedRegion, actualRowSet[y].Region, "The expected Region did not match the actual.");
            Assert.AreEqual(expectedRetailUnit.Unit_Name, actualRowSet[y].RetailUnit, "The expected RetailUnit did not match the actual.");
            Assert.AreEqual(expectedScaleExtraText + "2", actualRowSet[y].ScaleExtraText, "The expected ScaleExtraText did not match the actual.");
            Assert.AreEqual(expectedSignDescription + "2", actualRowSet[y].SignDescription, "The expected SignDescription did not match the actual.");
            Assert.AreEqual(expectedSignRomanceLong + "2", actualRowSet[y].SignRomanceLong, "The expected SignRomanceLong did not match the actual.");
            Assert.AreEqual(expectedSignRomanceShort + "2", actualRowSet[y].SignRomanceShort, "The expected SignRomanceShort did not match the actual.");
            Assert.AreEqual((expectedTagUom + 2).ToString(), actualRowSet[y].TagUom, "The expected TagUom did not match the actual.");
            Assert.AreEqual(Convert.ToDouble(expectedMsrp + 2), actualRowSet[y].Msrp, "The expected Msrp did not match the actual.");
            Assert.IsTrue(string.IsNullOrEmpty(actualRowSet[y].ErrorMessage));
        }

        [TestMethod]
        public void GetItemLocaleEvents_AddUpdateEventsExistsWithNullStoreNumber_ShouldReturnZeroRowsForNonInternalStore()
        {
            // Given
            var storeNo = 834792;
            var expectedStoreInternal = false;
            var subTeamNo = this.GetFirstFromTable<SubTeam>().SubTeam_No;
            var vendorId = this.GetFirstFromTable<Vendor>().Vendor_ID;
            var expectedRetailUnit = this.GetFirstFromTable<ItemUnit>();
            var expectedOrigin = this.GetFirstFromTable<ItemOrigin>();
            var expectedLabelType = this.GetFirstFromTable<LabelType>();
            var expectedLinkedItemKey = this.GetFirstFromTable<Item>().Item_Key;
            var expectedLinkedIdentifier = this.GetLinkedIdentifierByItemKey(expectedLinkedItemKey);
            var expectedScaleExtraTextId = this.GetFirstFromTable<ItemScale>().Scale_ExtraText_ID;
            var expectedRegion = "TS";
            var expectedIdentifier = "1234567";
            var expectedBusinessUnitId = 83472;
            var expectedAgeRestrictionId = 2;
            var expectedAuthorized = true;
            var expectedCaseDiscount = true;
            var expectedDiscontinued = false;
            var expectedLocalItem = true;
            var expectedLocality = "Test Locality";
            var expectedProductCode = "TestProductCode";
            var expectedRestrictedHours = true;
            var expectedSignRomanceLong = "Test Sign Romance Long";
            var expectedSignRomanceShort = "Test Sign Romance Short";
            var expectedSignDescription = "Test Sign Description";
            var expectedTmDiscount = true;
            var expectedMsrp = 2000m;
            var expectedChicagoBaby = "Test Baby";
            var expectedColorAdd = true;
            var expectedElectronicShelfTag = true;
            var expectedExclusive = DateTime.Today;
            var expectedNumberOfDigitsSentToScale = 123;
            var expectedScaleExtraText = "Test Scale Extra Text";
            var expectedTagUom = 23;
            var expectedEventTypeid = IrmaEventTypes.ItemLocaleAddOrUpdate;
            var expectedStoreNumbers = this.GetValidStoreNumbers();
            var storeRegionMapper = this.GetStoreRegionMapping();
            var expectedRows = expectedStoreNumbers.Count();

            // Insert New Item
            var itemKey = InsertNewItem(subTeamNo, expectedSignDescription, expectedProductCode,
                expectedOrigin.Origin_ID, expectedLabelType.LabelType_ID, expectedRetailUnit.Unit_ID);

            // Insert New Item Identifier
            InsertNewItemIdentifier(itemKey, expectedIdentifier, expectedNumberOfDigitsSentToScale);

            // Insert New Store
            InsertNewStore(storeNo, expectedBusinessUnitId, 1, null, expectedStoreInternal);

            // Insert New Store Region Mapping
            InsertNewStoreRegionMapping(storeNo, expectedRegion);

            expectedStoreNumbers.ForEach(s =>
            {
                // Insert New Price
                InsertNewPrice(expectedCaseDiscount, expectedElectronicShelfTag, expectedLocalItem, expectedRestrictedHours, expectedTmDiscount,
                    expectedMsrp, s.Store_No, itemKey, expectedAgeRestrictionId, expectedLinkedItemKey);
            });

            // Insert New Validated Scan Code, Item-Vendor
            InsertNewValidatedScanCode(expectedIdentifier);
            InsertItemVendor(itemKey, vendorId);
            
            expectedStoreNumbers.ForEach(s =>
            {
                // Insert StoreItemVendor
                InsertStoreItemVendor(vendorId, s.Store_No, itemKey, expectedDiscontinued, true, DateTime.Now);
            });

            // Insert StoreItem
            expectedStoreNumbers.ForEach(s =>
            {
                InsertStoreItem(s.Store_No, itemKey, expectedAuthorized);
            });

            // Insert Sign Attributes
            InsertItemSignAttributes(itemKey, expectedChicagoBaby, expectedColorAdd, expectedLocality, expectedSignRomanceLong,
                expectedSignRomanceShort, expectedTagUom, expectedExclusive);

            // Insert Scale Extra Text
            var scaleExtraTextId = this.dbProvider.Insert(new IrmaQueryParams<Scale_ExtraText, int>(
                IrmaTestObjectFactory.Build<Scale_ExtraText>()
                    .With(x => x.ExtraText, expectedScaleExtraText)
                    .With(x => x.Description, "Dummy")
                    .ToObject(),
                x => x.Scale_ExtraText_ID));

            // Insert Item Scale
            this.dbProvider.Insert(new IrmaQueryParams<ItemScale, int>(
                IrmaTestObjectFactory.Build<ItemScale>()
                    .With(x => x.Item_Key, itemKey)
                    .With(x => x.Scale_ExtraText_ID, scaleExtraTextId)
                    .ToObject(),
                x => x.ItemScale_ID));

            var expectedQueueId = InsertToItemLocaleChangeQueue(itemKey, null, expectedIdentifier, expectedEventTypeid);

            // When
            var actualRowSet = query.Search(parameters);

            // Then
            Assert.AreEqual(0, actualRowSet.Where(r => r.BusinessUnitId == expectedBusinessUnitId).Count());
        }

        [TestMethod]
        public void GetItemLocaleEvents_AddUpdateEventsExistsWithNullStoreNumber_ShouldReturnZeroRowsForStoreNotWfmOrMegaStore()
        {
            // Given
            var storeNo = 834792;
            var expectedStoreInternal = true;
            var expectedWfmStore = false;
            var expectedMegaStore = false;
            var subTeamNo = this.GetFirstFromTable<SubTeam>().SubTeam_No;
            var vendorId = this.GetFirstFromTable<Vendor>().Vendor_ID;
            var expectedRetailUnit = this.GetFirstFromTable<ItemUnit>();
            var expectedOrigin = this.GetFirstFromTable<ItemOrigin>();
            var expectedLabelType = this.GetFirstFromTable<LabelType>();
            var expectedLinkedItemKey = this.GetFirstFromTable<Item>().Item_Key;
            var expectedLinkedIdentifier = this.GetLinkedIdentifierByItemKey(expectedLinkedItemKey);
            var expectedScaleExtraTextId = this.GetFirstFromTable<ItemScale>().Scale_ExtraText_ID;
            var expectedRegion = "TS";
            var expectedIdentifier = "1234567";
            var expectedBusinessUnitId = 83472;
            var expectedAgeRestrictionId = 2;
            var expectedAuthorized = true;
            var expectedCaseDiscount = true;
            var expectedDiscontinued = false;
            var expectedLocalItem = true;
            var expectedLocality = "Test Locality";
            var expectedProductCode = "TestProductCode";
            var expectedRestrictedHours = true;
            var expectedSignRomanceLong = "Test Sign Romance Long";
            var expectedSignRomanceShort = "Test Sign Romance Short";
            var expectedSignDescription = "Test Sign Description";
            var expectedTmDiscount = true;
            var expectedMsrp = 2000m;
            var expectedChicagoBaby = "Test Baby";
            var expectedColorAdd = true;
            var expectedElectronicShelfTag = true;
            var expectedExclusive = DateTime.Today;
            var expectedNumberOfDigitsSentToScale = 123;
            var expectedScaleExtraText = "Test Scale Extra Text";
            var expectedTagUom = 23;
            var expectedEventTypeid = IrmaEventTypes.ItemLocaleAddOrUpdate;
            var expectedStoreNumbers = this.GetValidStoreNumbers();
            var storeRegionMapper = this.GetStoreRegionMapping();
            var expectedRows = expectedStoreNumbers.Count();

            // Insert New Item
            var itemKey = InsertNewItem(subTeamNo, expectedSignDescription, expectedProductCode,
                expectedOrigin.Origin_ID, expectedLabelType.LabelType_ID, expectedRetailUnit.Unit_ID);

            // Insert New Item Identifier
            InsertNewItemIdentifier(itemKey, expectedIdentifier, expectedNumberOfDigitsSentToScale);

            // Insert New Store
            InsertNewStore(storeNo, expectedBusinessUnitId, 1, expectedWfmStore, expectedStoreInternal, expectedMegaStore);

            // Insert New Store Region Mapping
            InsertNewStoreRegionMapping(storeNo, expectedRegion);

            expectedStoreNumbers.ForEach(s =>
            {
                // Insert New Price
                InsertNewPrice(expectedCaseDiscount, expectedElectronicShelfTag, expectedLocalItem, expectedRestrictedHours, expectedTmDiscount,
                    expectedMsrp, s.Store_No, itemKey, expectedAgeRestrictionId, expectedLinkedItemKey);
            });

            // Insert New Validated Scan Code, Item-Vendor
            InsertNewValidatedScanCode(expectedIdentifier);
            InsertItemVendor(itemKey, vendorId);

            expectedStoreNumbers.ForEach(s =>
            {
                // Insert StoreItemVendor
                InsertStoreItemVendor(vendorId, s.Store_No, itemKey, expectedDiscontinued, true, DateTime.Now);
            });

            // Insert StoreItem
            expectedStoreNumbers.ForEach(s =>
            {
                InsertStoreItem(s.Store_No, itemKey, expectedAuthorized);
            });

            // Insert Sign Attributes
            InsertItemSignAttributes(itemKey, expectedChicagoBaby, expectedColorAdd, expectedLocality, expectedSignRomanceLong,
                expectedSignRomanceShort, expectedTagUom, expectedExclusive);

            // Insert Scale Extra Text
            var scaleExtraTextId = this.dbProvider.Insert(new IrmaQueryParams<Scale_ExtraText, int>(
                IrmaTestObjectFactory.Build<Scale_ExtraText>()
                    .With(x => x.ExtraText, expectedScaleExtraText)
                    .With(x => x.Description, "Dummy")
                    .ToObject(),
                x => x.Scale_ExtraText_ID));

            // Insert Item Scale
            this.dbProvider.Insert(new IrmaQueryParams<ItemScale, int>(
                IrmaTestObjectFactory.Build<ItemScale>()
                    .With(x => x.Item_Key, itemKey)
                    .With(x => x.Scale_ExtraText_ID, scaleExtraTextId)
                    .ToObject(),
                x => x.ItemScale_ID));

            var expectedQueueId = InsertToItemLocaleChangeQueue(itemKey, null, expectedIdentifier, expectedEventTypeid);

            // When
            var actualRowSet = query.Search(parameters);

            // Then
            Assert.AreEqual(0, actualRowSet.Where(r => r.BusinessUnitId == expectedBusinessUnitId).Count());
        }

        [TestMethod]
       public void GetItemLocaleEvents_AddUpdateAndItemDeleteEventsExistWithValidStoreNoAndNullStoreNo_ReturnsOneRowForEachStoreAssociatedToTheItem()
        {
            // Given
            var storeNo = 834792;
            var expectedInternalStore = true;
            var expectedWfmStore = true;
            var subTeamNo = this.GetFirstFromTable<SubTeam>().SubTeam_No;
            var vendorId = this.GetFirstFromTable<Vendor>().Vendor_ID;
            var expectedRetailUnit = this.GetFirstFromTable<ItemUnit>();
            var expectedOrigin = this.GetFirstFromTable<ItemOrigin>();
            var expectedLabelType = this.GetFirstFromTable<LabelType>();
            var expectedLinkedItemKey = this.GetFirstFromTable<Item>().Item_Key;
            var expectedLinkedIdentifier = this.GetLinkedIdentifierByItemKey(expectedLinkedItemKey);
            var expectedScaleExtraTextId = this.GetFirstFromTable<ItemScale>().Scale_ExtraText_ID;
            var expectedRegion = region;
            var expectedIdentifier = "1234567";
            var expectedBusinessUnitId = 83472;
            var expectedAgeRestrictionId = 2;
            var expectedAgeRestriction = 21;
            var expectedAuthorized = true;
            var expectedCaseDiscount = true;
            var expectedDiscontinued = false;
            var expectedLocalItem = true;
            var expectedLocality = "Test Locality";
            var expectedProductCode = "TestProdCode";
            var expectedRestrictedHours = true;
            var expectedSignRomanceLong = "Test Sign Romance Long";
            var expectedSignRomanceShort = "Test Sign Romance Short";
            var expectedSignDescription = "Test Sign Description";
            var expectedTmDiscount = true;
            var expectedMsrp = 2000m;
            var expectedChicagoBaby = "Test Baby";
            var expectedColorAdd = true;
            var expectedElectronicShelfTag = true;
            var expectedExclusive = DateTime.Today;
            var expectedNumberOfDigitsSentToScale = 123;
            var expectedScaleExtraText = "Test Scale Extra Text";
            var expectedTagUom = 23;
            var expectedEventTypeid = IrmaEventTypes.ItemLocaleAddOrUpdate;
            var expectedStoreNumbers = this.GetValidStoreNumbers();
            var storeRegionMapper = this.GetStoreRegionMapping();
            var expectedRows = expectedStoreNumbers.Count() + 2; // one more for the row with valid storeNo and another row for the ItemDelete events

            // Insert New Item
            var itemKey = InsertNewItem(subTeamNo, expectedSignDescription, expectedProductCode,
                expectedOrigin.Origin_ID, expectedLabelType.LabelType_ID, expectedRetailUnit.Unit_ID);
            var itemKey2 = InsertNewItem(subTeamNo, expectedSignDescription + "2", expectedProductCode + "2",
                expectedOrigin.Origin_ID, expectedLabelType.LabelType_ID, expectedRetailUnit.Unit_ID);

            // Insert New Item Identifiers
            InsertNewItemIdentifier(itemKey, expectedIdentifier, expectedNumberOfDigitsSentToScale);
            InsertNewItemIdentifier(itemKey2, expectedIdentifier + "2", expectedNumberOfDigitsSentToScale + 2);

            // Insert New Store
            InsertNewStore(storeNo, expectedBusinessUnitId, 1, expectedWfmStore, expectedInternalStore);

            // Insert New Store Region Mapping
            InsertNewStoreRegionMapping(storeNo, expectedRegion);

            expectedStoreNumbers.ForEach(s =>
            {
                // Insert New Price
                InsertNewPrice(expectedCaseDiscount, expectedElectronicShelfTag, expectedLocalItem, expectedRestrictedHours, expectedTmDiscount,
                    expectedMsrp, s.Store_No, itemKey, expectedAgeRestrictionId, expectedLinkedItemKey);
            });

            // Insert another Price for ItemKey2
            InsertNewPrice(expectedCaseDiscount, expectedElectronicShelfTag, expectedLocalItem, expectedRestrictedHours, expectedTmDiscount,
                expectedMsrp + 2, storeNo, itemKey2, expectedAgeRestrictionId, expectedLinkedItemKey);

            // Insert New Validated Scan Codes
            InsertNewValidatedScanCode(expectedIdentifier, 1);
            InsertNewValidatedScanCode(expectedIdentifier + "2", 2);

            // Insert Item Vendor
            InsertItemVendor(itemKey, vendorId);

            expectedStoreNumbers.ForEach(s =>
            {
                // Insert StoreItemVendor
                InsertStoreItemVendor(vendorId, s.Store_No, itemKey, expectedDiscontinued, true, DateTime.Now);
            });

            // Insert StoreItem
            expectedStoreNumbers.ForEach(s =>
            {
                InsertStoreItem(s.Store_No, itemKey, expectedAuthorized);
            });

            this.dbProvider.Insert(new IrmaQueryParams<StoreItem, int>(
            IrmaTestObjectFactory.Build<StoreItem>()
                .With(x => x.Store_No, storeNo)
                .With(x => x.Item_Key, itemKey2)
                .With(x => x.Authorized, expectedAuthorized)
                .ToObject(),
            x => x.StoreItemAuthorizationID));

            // Insert Sign Attributes
            InsertItemSignAttributes(itemKey, expectedChicagoBaby, expectedColorAdd, expectedLocality, expectedSignRomanceLong,
                expectedSignRomanceShort, expectedTagUom, expectedExclusive);
            InsertItemSignAttributes(itemKey2, expectedChicagoBaby + "2", expectedColorAdd, expectedLocality + "2", expectedSignRomanceLong + "2",
                expectedSignRomanceShort + "2", expectedTagUom + 2, expectedExclusive);

            // Insert Scale Extra Text
            var scaleExtraTextId = this.dbProvider.Insert(new IrmaQueryParams<Scale_ExtraText, int>(
                IrmaTestObjectFactory.Build<Scale_ExtraText>()
                    .With(x => x.ExtraText, expectedScaleExtraText)
                    .With(x => x.Description, "Dummy")
                    .ToObject(),
                x => x.Scale_ExtraText_ID));

            var scaleExtraTextId2 = this.dbProvider.Insert(new IrmaQueryParams<Scale_ExtraText, int>(
                IrmaTestObjectFactory.Build<Scale_ExtraText>()
                    .With(x => x.ExtraText, expectedScaleExtraText + "2")
                    .With(x => x.Description, "Dummy 2")
                    .ToObject(),
                x => x.Scale_ExtraText_ID));

            // Insert Item Scale
            this.dbProvider.Insert(new IrmaQueryParams<ItemScale, int>(
                IrmaTestObjectFactory.Build<ItemScale>()
                    .With(x => x.Item_Key, itemKey)
                    .With(x => x.Scale_ExtraText_ID, scaleExtraTextId)
                    .ToObject(),
                x => x.ItemScale_ID));

            this.dbProvider.Insert(new IrmaQueryParams<ItemScale, int>(
                IrmaTestObjectFactory.Build<ItemScale>()
                    .With(x => x.Item_Key, itemKey2)
                    .With(x => x.Scale_ExtraText_ID, scaleExtraTextId2)
                    .ToObject(),
                x => x.ItemScale_ID));

            var expectedQueueId = InsertToItemLocaleChangeQueue(itemKey, null, expectedIdentifier, expectedEventTypeid);
            var expectedQueueId2 = InsertToItemLocaleChangeQueue(itemKey2, storeNo, expectedIdentifier + "2", expectedEventTypeid);
            var expectedDeleteQueueId = InsertToItemLocaleChangeQueue(itemKey2, storeNo, expectedIdentifier + "2", IrmaEventTypes.ItemDelete);  

            // When
            var actualRowSet = query.Search(parameters).OrderBy(q => q.ScanCode).ThenBy(r => r.EventTypeId).ThenBy(r => r.BusinessUnitId).ToList();

            // Then
            Assert.AreEqual(expectedRows, actualRowSet.Count, "The number of actual rows did not match expected number of rows.");

            expectedStoreNumbers.OrderBy(s => s.BusinessUnit_ID).Select((s, i) => new { Index = i, Store = s }).ToList().ForEach(s =>
            {
                Assert.AreEqual(expectedQueueId, actualRowSet[s.Index].QueueId, "The expected QueueID did not match the actual.");
                Assert.AreEqual(expectedEventTypeid, actualRowSet[s.Index].EventTypeId, "The expected EventTypeId did not match the actual.");
                Assert.AreEqual(expectedAgeRestriction, actualRowSet[s.Index].AgeRestriction, "The expected AgeCode did not match the actual.");
                Assert.AreEqual(expectedAuthorized, actualRowSet[s.Index].Authorized, "The expected AuthorizedForSale did not match the actual.");
                Assert.AreEqual(s.Store.BusinessUnit_ID, actualRowSet[s.Index].BusinessUnitId, "The expected BusinessUnit did not match the actual.");
                Assert.AreEqual(expectedCaseDiscount, actualRowSet[s.Index].CaseDiscount, "The expected CaseDiscountEligible did not match the actual.");
                Assert.AreEqual(expectedRestrictedHours, actualRowSet[s.Index].RestrictedHours, "The expected RestrictedHours did not match the actual.");
                Assert.AreEqual(expectedIdentifier, actualRowSet[s.Index].ScanCode, "The expected ScanCode did not match the actual.");
                Assert.AreEqual(expectedChicagoBaby, actualRowSet[s.Index].ChicagoBaby, "The expected ChicagoBaby did not match the actual.");
                Assert.AreEqual(expectedColorAdd, actualRowSet[s.Index].ColorAdded, "The expected ColorAdded did not match the actual.");
                Assert.AreEqual(expectedOrigin.Origin_Name, actualRowSet[s.Index].CountryOfProcessing, "The expected CountryOfProcessing did not match the actual.");
                Assert.AreEqual(expectedDiscontinued, actualRowSet[s.Index].Discontinued, "The expected Discontinued did not match the actual.");
                Assert.AreEqual(expectedElectronicShelfTag, actualRowSet[s.Index].ElectronicShelfTag, "The expected ElectronicShelfTag did not match the actual.");
                Assert.AreEqual(expectedExclusive, actualRowSet[s.Index].Exclusive, "The expected Exclusive did not match the actual.");
                Assert.AreEqual(expectedLabelType.LabelTypeDesc, actualRowSet[s.Index].LabelTypeDescription, "The expected LabelTypeDescription did not match the actual.");
                Assert.AreEqual(expectedLinkedIdentifier, actualRowSet[s.Index].LinkedItem, "The expected LinkedItem did not match the actual.");
                Assert.AreEqual(expectedLocalItem, actualRowSet[s.Index].LocalItem, "The expected LocalItem did not match the actual.");
                Assert.AreEqual(expectedLocality, actualRowSet[s.Index].Locality, "The expected Locality did not match the actual.");
                Assert.AreEqual(expectedNumberOfDigitsSentToScale, actualRowSet[s.Index].NumberOfDigitsSentToScale, "The expected NumberOfDigitsSentToScale did not match the actual.");
                Assert.AreEqual(expectedOrigin.Origin_Name, actualRowSet[s.Index].Origin, "The expected Origin did not match the actual.");
                Assert.AreEqual(expectedProductCode, actualRowSet[s.Index].ProductCode, "The expected ProductCode did not match the actual.");
                Assert.AreEqual(storeRegionMapper.First(m => m.Store_No == s.Store.Store_No).Region_Code, actualRowSet[s.Index].Region, "The expected Region did not match the actual.");
                Assert.AreEqual(expectedRetailUnit.Unit_Name, actualRowSet[s.Index].RetailUnit, "The expected RetailUnit did not match the actual.");
                Assert.AreEqual(expectedScaleExtraText, actualRowSet[s.Index].ScaleExtraText, "The expected ScaleExtraText did not match the actual.");
                Assert.AreEqual(expectedSignDescription, actualRowSet[s.Index].SignDescription, "The expected SignDescription did not match the actual.");
                Assert.AreEqual(expectedSignRomanceLong, actualRowSet[s.Index].SignRomanceLong, "The expected SignRomanceLong did not match the actual.");
                Assert.AreEqual(expectedSignRomanceShort, actualRowSet[s.Index].SignRomanceShort, "The expected SignRomanceShort did not match the actual.");
                Assert.AreEqual(expectedTagUom.ToString(), actualRowSet[s.Index].TagUom, "The expected TagUom did not match the actual.");
                Assert.AreEqual(Convert.ToDouble(expectedMsrp), actualRowSet[s.Index].Msrp, "The expected Msrp did not match the actual.");
                Assert.IsTrue(string.IsNullOrEmpty(actualRowSet[s.Index].ErrorMessage));
            });

            // Assert that last row is for ItemKey2
            int y = expectedRows - 2;
            Assert.AreEqual(expectedQueueId2, actualRowSet[y].QueueId, "The expected QueueID did not match the actual.");
            Assert.AreEqual(expectedEventTypeid, actualRowSet[y].EventTypeId, "The expected EventTypeId did not match the actual.");
            Assert.AreEqual(expectedAgeRestriction, actualRowSet[y].AgeRestriction, "The expected AgeCode did not match the actual.");
            Assert.AreEqual(expectedAuthorized, actualRowSet[y].Authorized, "The expected AuthorizedForSale did not match the actual.");
            Assert.AreEqual(expectedBusinessUnitId, actualRowSet[y].BusinessUnitId, "The expected BusinessUnit did not match the actual.");
            Assert.AreEqual(expectedCaseDiscount, actualRowSet[y].CaseDiscount, "The expected CaseDiscountEligible did not match the actual.");
            Assert.AreEqual(expectedRestrictedHours, actualRowSet[y].RestrictedHours, "The expected RestrictedHours did not match the actual.");
            Assert.AreEqual(expectedIdentifier + "2", actualRowSet[y].ScanCode, "The expected ScanCode did not match the actual.");
            Assert.AreEqual(expectedChicagoBaby + "2", actualRowSet[y].ChicagoBaby, "The expected ChicagoBaby did not match the actual.");
            Assert.AreEqual(expectedColorAdd, actualRowSet[y].ColorAdded, "The expected ColorAdded did not match the actual.");
            Assert.AreEqual(expectedOrigin.Origin_Name, actualRowSet[y].CountryOfProcessing, "The expected CountryOfProcessing did not match the actual.");
            Assert.AreEqual(expectedDiscontinued, actualRowSet[y].Discontinued, "The expected Discontinued did not match the actual.");
            Assert.AreEqual(expectedElectronicShelfTag, actualRowSet[y].ElectronicShelfTag, "The expected ElectronicShelfTag did not match the actual.");
            Assert.AreEqual(expectedExclusive, actualRowSet[y].Exclusive, "The expected Exclusive did not match the actual.");
            Assert.AreEqual(expectedLabelType.LabelTypeDesc, actualRowSet[y].LabelTypeDescription, "The expected LabelTypeDescription did not match the actual.");
            Assert.AreEqual(expectedLinkedIdentifier, actualRowSet[y].LinkedItem, "The expected LinkedItem did not match the actual.");
            Assert.AreEqual(expectedLocalItem, actualRowSet[y].LocalItem, "The expected LocalItem did not match the actual.");
            Assert.AreEqual(expectedLocality + "2", actualRowSet[y].Locality, "The expected Locality did not match the actual.");
            Assert.AreEqual(expectedNumberOfDigitsSentToScale + 2, actualRowSet[y].NumberOfDigitsSentToScale, "The expected NumberOfDigitsSentToScale did not match the actual.");
            Assert.AreEqual(expectedOrigin.Origin_Name, actualRowSet[y].Origin, "The expected Origin did not match the actual.");
            Assert.AreEqual(expectedProductCode + "2", actualRowSet[y].ProductCode, "The expected ProductCode did not match the actual.");
            Assert.AreEqual(expectedRegion, actualRowSet[y].Region, "The expected Region did not match the actual.");
            Assert.AreEqual(expectedRetailUnit.Unit_Name, actualRowSet[y].RetailUnit, "The expected RetailUnit did not match the actual.");
            Assert.AreEqual(expectedScaleExtraText + "2", actualRowSet[y].ScaleExtraText, "The expected ScaleExtraText did not match the actual.");
            Assert.AreEqual(expectedSignDescription + "2", actualRowSet[y].SignDescription, "The expected SignDescription did not match the actual.");
            Assert.AreEqual(expectedSignRomanceLong + "2", actualRowSet[y].SignRomanceLong, "The expected SignRomanceLong did not match the actual.");
            Assert.AreEqual(expectedSignRomanceShort + "2", actualRowSet[y].SignRomanceShort, "The expected SignRomanceShort did not match the actual.");
            Assert.AreEqual((expectedTagUom + 2).ToString(), actualRowSet[y].TagUom, "The expected TagUom did not match the actual.");
            Assert.AreEqual(Convert.ToDouble(expectedMsrp + 2), actualRowSet[y].Msrp, "The expected Msrp did not match the actual.");
            Assert.IsTrue(string.IsNullOrEmpty(actualRowSet[y].ErrorMessage));

            // Assert that last row is for ItemKey2
            int z = expectedRows - 1;
            Assert.AreEqual(expectedDeleteQueueId, actualRowSet[z].QueueId, "The expected QueueID did not match the actual.");
            Assert.AreEqual(IrmaEventTypes.ItemDelete, actualRowSet[z].EventTypeId, "The expected EventTypeId did not match the actual.");
            Assert.AreEqual(expectedAgeRestriction, actualRowSet[z].AgeRestriction, "The expected AgeCode did not match the actual.");
            Assert.AreEqual(false, actualRowSet[z].Authorized, "The expected AuthorizedForSale did not match the actual.");
            Assert.AreEqual(expectedBusinessUnitId, actualRowSet[z].BusinessUnitId, "The expected BusinessUnit did not match the actual.");
            Assert.AreEqual(expectedCaseDiscount, actualRowSet[z].CaseDiscount, "The expected CaseDiscountEligible did not match the actual.");
            Assert.AreEqual(expectedRestrictedHours, actualRowSet[z].RestrictedHours, "The expected RestrictedHours did not match the actual.");
            Assert.AreEqual(expectedIdentifier + "2", actualRowSet[z].ScanCode, "The expected ScanCode did not match the actual.");
            Assert.AreEqual(expectedChicagoBaby + "2", actualRowSet[z].ChicagoBaby, "The expected ChicagoBaby did not match the actual.");
            Assert.AreEqual(expectedColorAdd, actualRowSet[z].ColorAdded, "The expected ColorAdded did not match the actual.");
            Assert.AreEqual(expectedOrigin.Origin_Name, actualRowSet[z].CountryOfProcessing, "The expected CountryOfProcessing did not match the actual.");
            Assert.AreEqual(expectedDiscontinued, actualRowSet[z].Discontinued, "The expected Discontinued did not match the actual.");
            Assert.AreEqual(expectedElectronicShelfTag, actualRowSet[z].ElectronicShelfTag, "The expected ElectronicShelfTag did not match the actual.");
            Assert.AreEqual(expectedExclusive, actualRowSet[z].Exclusive, "The expected Exclusive did not match the actual.");
            Assert.AreEqual(expectedLabelType.LabelTypeDesc, actualRowSet[z].LabelTypeDescription, "The expected LabelTypeDescription did not match the actual.");
            Assert.AreEqual(expectedLinkedIdentifier, actualRowSet[z].LinkedItem, "The expected LinkedItem did not match the actual.");
            Assert.AreEqual(expectedLocalItem, actualRowSet[z].LocalItem, "The expected LocalItem did not match the actual.");
            Assert.AreEqual(expectedLocality + "2", actualRowSet[z].Locality, "The expected Locality did not match the actual.");
            Assert.AreEqual(expectedNumberOfDigitsSentToScale + 2, actualRowSet[z].NumberOfDigitsSentToScale, "The expected NumberOfDigitsSentToScale did not match the actual.");
            Assert.AreEqual(expectedOrigin.Origin_Name, actualRowSet[z].Origin, "The expected Origin did not match the actual.");
            Assert.AreEqual(expectedProductCode + "2", actualRowSet[z].ProductCode, "The expected ProductCode did not match the actual.");
            Assert.AreEqual(expectedRegion, actualRowSet[z].Region, "The expected Region did not match the actual.");
            Assert.AreEqual(expectedRetailUnit.Unit_Name, actualRowSet[z].RetailUnit, "The expected RetailUnit did not match the actual.");
            Assert.AreEqual(expectedScaleExtraText + "2", actualRowSet[z].ScaleExtraText, "The expected ScaleExtraText did not match the actual.");
            Assert.AreEqual(expectedSignDescription + "2", actualRowSet[z].SignDescription, "The expected SignDescription did not match the actual.");
            Assert.AreEqual(expectedSignRomanceLong + "2", actualRowSet[z].SignRomanceLong, "The expected SignRomanceLong did not match the actual.");
            Assert.AreEqual(expectedSignRomanceShort + "2", actualRowSet[z].SignRomanceShort, "The expected SignRomanceShort did not match the actual.");
            Assert.AreEqual((expectedTagUom + 2).ToString(), actualRowSet[z].TagUom, "The expected TagUom did not match the actual.");
            Assert.AreEqual(Convert.ToDouble(expectedMsrp + 2), actualRowSet[z].Msrp, "The expected Msrp did not match the actual.");
            Assert.IsTrue(string.IsNullOrEmpty(actualRowSet[z].ErrorMessage));
        }

        [TestMethod]
        public void GetItemLocaleEvents_AddUpdateEventsExistWithNullStoreNumber_RowsReturnDoNotHaveBusinessUnitForClosedOrLabStores()
        {
            // Given
            var storeNo = 834792;
            var expectedInternalStore = true;
            var expectedWfmStore = true;
            var subTeamNo = this.GetFirstFromTable<SubTeam>().SubTeam_No;
            var vendorId = this.GetFirstFromTable<Vendor>().Vendor_ID;
            var expectedRetailUnit = this.GetFirstFromTable<ItemUnit>();
            var expectedOrigin = this.GetFirstFromTable<ItemOrigin>();
            var expectedLabelType = this.GetFirstFromTable<LabelType>();
            var expectedLinkedItemKey = this.GetFirstFromTable<Item>().Item_Key;
            var expectedLinkedIdentifier = this.GetLinkedIdentifierByItemKey(expectedLinkedItemKey);
            var expectedScaleExtraTextId = this.GetFirstFromTable<ItemScale>().Scale_ExtraText_ID;
            var expectedRegion = region;
            var expectedIdentifier = "1234567";
            var expectedBusinessUnitId = 83472;
            var expectedAgeRestrictionId = 2;
            var expectedAuthorized = true;
            var expectedCaseDiscount = true;
            var expectedDiscontinued = false;
            var expectedLocalItem = true;
            var expectedLocality = "Test Locality";
            var expectedProductCode = "TestProductCode";
            var expectedRestrictedHours = true;
            var expectedSignRomanceLong = "Test Sign Romance Long";
            var expectedSignRomanceShort = "Test Sign Romance Short";
            var expectedSignDescription = "Test Sign Description";
            var expectedTmDiscount = true;
            var expectedMsrp = 2000m;
            var expectedChicagoBaby = "Test Baby";
            var expectedColorAdd = true;
            var expectedElectronicShelfTag = true;
            var expectedExclusive = DateTime.Today;
            var expectedNumberOfDigitsSentToScale = 123;
            var expectedScaleExtraText = "Test Scale Extra Text";
            var expectedTagUom = 23;
            var expectedEventTypeid = IrmaEventTypes.ItemLocaleAddOrUpdate;
            var validStores = this.GetValidStoreNumbers();
            var storeRegionMapper = this.GetStoreRegionMapping();
            var expectedRows = validStores.Count();
            var allStores = this.GetAllStores();
            UpdateLabAndClosedStoreValues(new List<int> { storeNo });

            // Insert New Item
            var itemKey = InsertNewItem(subTeamNo, expectedSignDescription, expectedProductCode,
                expectedOrigin.Origin_ID, expectedLabelType.LabelType_ID, expectedRetailUnit.Unit_ID);

            // Insert New Item Identifier
            InsertNewItemIdentifier(itemKey, expectedIdentifier, expectedNumberOfDigitsSentToScale);

            // Insert New Store
            InsertNewStore(storeNo, expectedBusinessUnitId, 1, expectedWfmStore, expectedInternalStore);

            // Insert New Store Region Mapping
            InsertNewStoreRegionMapping(storeNo, expectedRegion);

            allStores.ForEach(s =>
            {
                // Insert New Price
                InsertNewPrice(expectedCaseDiscount, expectedElectronicShelfTag, expectedLocalItem, expectedRestrictedHours, expectedTmDiscount,
                    expectedMsrp, s.Store_No, itemKey, expectedAgeRestrictionId, expectedLinkedItemKey);
            });

            // Insert New Validated Scan Code
            InsertNewValidatedScanCode(expectedIdentifier);

            // Insert Item Vendor
            InsertItemVendor(itemKey, vendorId);

            allStores.ForEach(s =>
            {
                // Insert StoreItemVendor
                InsertStoreItemVendor(vendorId, s.Store_No, itemKey, expectedDiscontinued, true, DateTime.Now);
            });

            // Insert StoreItem
            allStores.ForEach(s =>
            {
                InsertStoreItem(s.Store_No, itemKey, expectedAuthorized);
            });

            // Insert Sign Attributes
            InsertItemSignAttributes(itemKey, expectedChicagoBaby, expectedColorAdd, expectedLocality, expectedSignRomanceLong,
                expectedSignRomanceShort, expectedTagUom, expectedExclusive);

            // Insert Scale Extra Text
            var scaleExtraTextId = this.dbProvider.Insert(new IrmaQueryParams<Scale_ExtraText, int>(
                IrmaTestObjectFactory.Build<Scale_ExtraText>()
                    .With(x => x.ExtraText, expectedScaleExtraText)
                    .With(x => x.Description, "Dummy")
                    .ToObject(),
                x => x.Scale_ExtraText_ID));

            // Insert Item Scale
            this.dbProvider.Insert(new IrmaQueryParams<ItemScale, int>(
                IrmaTestObjectFactory.Build<ItemScale>()
                    .With(x => x.Item_Key, itemKey)
                    .With(x => x.Scale_ExtraText_ID, scaleExtraTextId)
                    .ToObject(),
                x => x.ItemScale_ID));

            var expectedQueueId = InsertToItemLocaleChangeQueue(itemKey, null, expectedIdentifier, expectedEventTypeid);

            // When
            var actualRowSet = query.Search(parameters).OrderBy(r => r.BusinessUnitId).ToList();

            //Then
            List<int> closeOrLabBusinessUnits = this.dbProvider.Connection
                .Query<int>(@"  DECLARE @ExcludedStoreNo varchar(250);
                                SET @ExcludedStoreNo = (SELECT dbo.fn_GetAppConfigValue('LabAndClosedStoreNo','IRMA Client'));
                                SELECT BusinessUnit_ID FROM Store WHERE Store_No IN (SELECT Key_Value as BusinessUnitId FROM dbo.fn_Parse_List(@ExcludedStoreNo, '|'))",
                                transaction: this.dbProvider.Transaction)
                .ToList();

            // Make sure none of the BusinessUnits listed in the results contain the lab or closed store business units.
            Assert.IsFalse(actualRowSet.Select(a => a.BusinessUnitId).Any(b => closeOrLabBusinessUnits.Contains(b)));
        }

        [TestMethod]
        public void GetItemLocaleEvents_AddUpdateEventsExistsWithNullStoreNumberAndTwoRegions_ShouldReturnNumberOfRowsBasedOnNumberOfValidStore()
        {
            // Given
            var storeNo = 834792;
            var subTeamNo = this.GetFirstFromTable<SubTeam>().SubTeam_No;
            var vendorId = this.GetFirstFromTable<Vendor>().Vendor_ID;
            var expectedRetailUnit = this.GetFirstFromTable<ItemUnit>();
            var expectedOrigin = this.GetFirstFromTable<ItemOrigin>();
            var expectedLabelType = this.GetFirstFromTable<LabelType>();
            var expectedLinkedItemKey = this.GetFirstFromTable<Item>().Item_Key;
            var expectedLinkedIdentifier = this.GetLinkedIdentifierByItemKey(expectedLinkedItemKey);
            var expectedScaleExtraTextId = this.GetFirstFromTable<ItemScale>().Scale_ExtraText_ID;
            var expectedRegion = "TS";
            var expectedIdentifier = "1234567";
            var expectedBusinessUnitId = 83472;
            var expectedAgeRestrictionId = 2;
            var expectedAgeRestriction = 21;
            var expectedAuthorized = true;
            var expectedCaseDiscount = true;
            var expectedDiscontinued = false;
            var expectedLocalItem = true;
            var expectedLocality = "Test Locality";
            var expectedProductCode = "TestProductCode";
            var expectedRestrictedHours = true;
            var expectedSignRomanceLong = "Test Sign Romance Long";
            var expectedSignRomanceShort = "Test Sign Romance Short";
            var expectedSignDescription = "Test Sign Description";
            var expectedTmDiscount = true;
            var expectedMsrp = 2000m;
            var expectedChicagoBaby = "Test Baby";
            var expectedColorAdd = true;
            var expectedElectronicShelfTag = true;
            var expectedExclusive = DateTime.Today;
            var expectedNumberOfDigitsSentToScale = 123;
            var expectedScaleExtraText = "Test Scale Extra Text";
            var expectedTagUom = 23;
            var expectedEventTypeid = IrmaEventTypes.ItemLocaleAddOrUpdate;
            var expectedStoreNumbers = this.GetValidStoreNumbers();
            var storeRegionMapper = this.GetStoreRegionMapping();
            var expectedRows = expectedStoreNumbers.Count();

            // Insert New Item
            var itemKey = InsertNewItem(subTeamNo, expectedSignDescription, expectedProductCode,
                expectedOrigin.Origin_ID, expectedLabelType.LabelType_ID, expectedRetailUnit.Unit_ID);

            // Insert New Item Identifier
            InsertNewItemIdentifier(itemKey, expectedIdentifier, expectedNumberOfDigitsSentToScale);

            // Insert New Store
            InsertNewStore(storeNo, expectedBusinessUnitId, 1, null, true, true);

            // Insert New Store Region Mapping
            InsertNewStoreRegionMapping(storeNo, expectedRegion);

            expectedStoreNumbers.ForEach(s =>
            {
                // Insert New Price
                InsertNewPrice(expectedCaseDiscount, expectedElectronicShelfTag, expectedLocalItem, expectedRestrictedHours, expectedTmDiscount,
                    expectedMsrp, s.Store_No, itemKey, expectedAgeRestrictionId, expectedLinkedItemKey);
            });

            // Insert New Validated Scan Code
            InsertNewValidatedScanCode(expectedIdentifier);

            // Insert Item Vendor
            InsertItemVendor(itemKey, vendorId);

            expectedStoreNumbers.ForEach(s =>
            {
                // Insert StoreItemVendor
                InsertStoreItemVendor(vendorId, s.Store_No, itemKey, expectedDiscontinued, true, DateTime.Now);
            });

            // Insert StoreItem
            expectedStoreNumbers.ForEach(s =>
            {
                InsertStoreItem(s.Store_No, itemKey, expectedAuthorized);
            });

            // Insert Sign Attributes
            InsertItemSignAttributes(itemKey, expectedChicagoBaby, expectedColorAdd, expectedLocality, expectedSignRomanceLong,
                expectedSignRomanceShort, expectedTagUom, expectedExclusive);

            // Insert Scale Extra Text
            var scaleExtraTextId = this.dbProvider.Insert(new IrmaQueryParams<Scale_ExtraText, int>(
                IrmaTestObjectFactory.Build<Scale_ExtraText>()
                    .With(x => x.ExtraText, expectedScaleExtraText)
                    .With(x => x.Description, "Dummy")
                    .ToObject(),
                x => x.Scale_ExtraText_ID));

            // Insert Item Scale
            this.dbProvider.Insert(new IrmaQueryParams<ItemScale, int>(
                IrmaTestObjectFactory.Build<ItemScale>()
                    .With(x => x.Item_Key, itemKey)
                    .With(x => x.Scale_ExtraText_ID, scaleExtraTextId)
                    .ToObject(),
                x => x.ItemScale_ID));

            var expectedQueueId = InsertToItemLocaleChangeQueue(itemKey, null, expectedIdentifier, expectedEventTypeid);

            // When
            var actualRowSet = query.Search(parameters).OrderBy(r => r.BusinessUnitId).ToList();

            // Then
            Assert.AreEqual(expectedRows, actualRowSet.Count, "The number of actual rows did not match expected number of rows.");

            expectedStoreNumbers.OrderBy(s => s.BusinessUnit_ID).Select((s, i) => new { Index = i, Store = s }).ToList().ForEach(s =>
            {
                Assert.AreEqual(expectedQueueId, actualRowSet[s.Index].QueueId, "The expected QueueID did not match the actual.");
                Assert.AreEqual(expectedEventTypeid, actualRowSet[s.Index].EventTypeId, "The expected EventTypeId did not match the actual.");
                Assert.AreEqual(expectedAgeRestriction, actualRowSet[s.Index].AgeRestriction, "The expected AgeCode did not match the actual.");
                Assert.AreEqual(expectedAuthorized, actualRowSet[s.Index].Authorized, "The expected AuthorizedForSale did not match the actual.");
                Assert.AreEqual(s.Store.BusinessUnit_ID, actualRowSet[s.Index].BusinessUnitId, "The expected BusinessUnit did not match the actual.");
                Assert.AreEqual(expectedCaseDiscount, actualRowSet[s.Index].CaseDiscount, "The expected CaseDiscountEligible did not match the actual.");
                Assert.AreEqual(expectedRestrictedHours, actualRowSet[s.Index].RestrictedHours, "The expected RestrictedHours did not match the actual.");
                Assert.AreEqual(expectedIdentifier, actualRowSet[s.Index].ScanCode, "The expected ScanCode did not match the actual.");
                Assert.AreEqual(expectedChicagoBaby, actualRowSet[s.Index].ChicagoBaby, "The expected ChicagoBaby did not match the actual.");
                Assert.AreEqual(expectedColorAdd, actualRowSet[s.Index].ColorAdded, "The expected ColorAdded did not match the actual.");
                Assert.AreEqual(expectedOrigin.Origin_Name, actualRowSet[s.Index].CountryOfProcessing, "The expected CountryOfProcessing did not match the actual.");
                Assert.AreEqual(expectedDiscontinued, actualRowSet[s.Index].Discontinued, "The expected Discontinued did not match the actual.");
                Assert.AreEqual(expectedElectronicShelfTag, actualRowSet[s.Index].ElectronicShelfTag, "The expected ElectronicShelfTag did not match the actual.");
                Assert.AreEqual(expectedExclusive, actualRowSet[s.Index].Exclusive, "The expected Exclusive did not match the actual.");
                Assert.AreEqual(expectedLabelType.LabelTypeDesc, actualRowSet[s.Index].LabelTypeDescription, "The expected LabelTypeDescription did not match the actual.");
                Assert.AreEqual(expectedLinkedIdentifier, actualRowSet[s.Index].LinkedItem, "The expected LinkedItem did not match the actual.");
                Assert.AreEqual(expectedLocalItem, actualRowSet[s.Index].LocalItem, "The expected LocalItem did not match the actual.");
                Assert.AreEqual(expectedLocality, actualRowSet[s.Index].Locality, "The expected Locality did not match the actual.");
                Assert.AreEqual(expectedNumberOfDigitsSentToScale, actualRowSet[s.Index].NumberOfDigitsSentToScale, "The expected NumberOfDigitsSentToScale did not match the actual.");
                Assert.AreEqual(expectedOrigin.Origin_Name, actualRowSet[s.Index].Origin, "The expected Origin did not match the actual.");
                Assert.AreEqual(expectedProductCode, actualRowSet[s.Index].ProductCode, "The expected ProductCode did not match the actual.");
                Assert.AreEqual(storeRegionMapper.First(m => m.Store_No == s.Store.Store_No).Region_Code, actualRowSet[s.Index].Region, "The expected Region did not match the actual.");
                Assert.AreEqual(expectedRetailUnit.Unit_Name, actualRowSet[s.Index].RetailUnit, "The expected RetailUnit did not match the actual.");
                Assert.AreEqual(expectedScaleExtraText, actualRowSet[s.Index].ScaleExtraText, "The expected ScaleExtraText did not match the actual.");
                Assert.AreEqual(expectedSignDescription, actualRowSet[s.Index].SignDescription, "The expected SignDescription did not match the actual.");
                Assert.AreEqual(expectedSignRomanceLong, actualRowSet[s.Index].SignRomanceLong, "The expected SignRomanceLong did not match the actual.");
                Assert.AreEqual(expectedSignRomanceShort, actualRowSet[s.Index].SignRomanceShort, "The expected SignRomanceShort did not match the actual.");
                Assert.AreEqual(expectedTagUom.ToString(), actualRowSet[s.Index].TagUom, "The expected TagUom did not match the actual.");
                Assert.AreEqual(Convert.ToDouble(expectedMsrp), actualRowSet[s.Index].Msrp, "The expected Msrp did not match the actual.");
                Assert.IsTrue(string.IsNullOrEmpty(actualRowSet[s.Index].ErrorMessage));
            });
        }

        [TestMethod]
        public void GetItemLocaleEvents_AddUpdateEventsForTsStore_ShouldReturnThoseWithTsRegion()
        {
            // Given
            var storeNo = 834792;
            var subTeamNo = this.GetFirstFromTable<SubTeam>().SubTeam_No;
            var vendorId = this.GetFirstFromTable<Vendor>().Vendor_ID;
            var expectedRetailUnit = this.GetFirstFromTable<ItemUnit>();
            var expectedOrigin = this.GetFirstFromTable<ItemOrigin>();
            var expectedLabelType = this.GetFirstFromTable<LabelType>();
            var expectedLinkedItemKey = this.GetFirstFromTable<Item>().Item_Key;
            var expectedLinkedIdentifier = this.GetLinkedIdentifierByItemKey(expectedLinkedItemKey);
            var expectedScaleExtraTextId = this.GetFirstFromTable<ItemScale>().Scale_ExtraText_ID;
            var expectedRegion = "TS";
            var expectedIdentifier = "1234567";
            var expectedBusinessUnitId = 83472;
            var expectedAgeRestrictionId = 2;
            var expectedAgeRestriction = 21;
            var expectedAuthorized = true;
            var expectedCaseDiscount = true;
            var expectedDiscontinued = false;
            var expectedLocalItem = true;
            var expectedLocality = "Test Locality";
            var expectedProductCode = "TestProductCode";
            var expectedRestrictedHours = true;
            var expectedSignRomanceLong = "Test Sign Romance Long";
            var expectedSignRomanceShort = "Test Sign Romance Short";
            var expectedSignDescription = "Test Sign Description";
            var expectedTmDiscount = true;
            var expectedMsrp = 2000m;
            var expectedChicagoBaby = "Test Baby";
            var expectedColorAdd = true;
            var expectedElectronicShelfTag = true;
            var expectedExclusive = DateTime.Today;
            var expectedNumberOfDigitsSentToScale = 123;
            var expectedScaleExtraText = "Test Scale Extra Text";
            var expectedTagUom = 23;
            var expectedEventTypeid = IrmaEventTypes.ItemLocaleAddOrUpdate;

            // Insert New Item
            var itemKey = InsertNewItem(subTeamNo, expectedSignDescription, expectedProductCode,
                expectedOrigin.Origin_ID, expectedLabelType.LabelType_ID, expectedRetailUnit.Unit_ID);

            // Insert New Item Identifier
            InsertNewItemIdentifier(itemKey, expectedIdentifier, expectedNumberOfDigitsSentToScale);

            // Insert New Store
            InsertNewStore(storeNo, expectedBusinessUnitId, 1);

            // Insert New Store Region Mapping
            InsertNewStoreRegionMapping(storeNo, expectedRegion);

            // Insert New Price
            InsertNewPrice(expectedCaseDiscount, expectedElectronicShelfTag, expectedLocalItem, expectedRestrictedHours, expectedTmDiscount,
                expectedMsrp, storeNo, itemKey, expectedAgeRestrictionId, expectedLinkedItemKey);

            // Insert New Validated Scan Code
            InsertNewValidatedScanCode(expectedIdentifier);

            // Insert Item Vendor
            InsertItemVendor(itemKey, vendorId);

            // Insert StoreItemVendor
            InsertStoreItemVendor(vendorId, storeNo, itemKey, expectedDiscontinued, true, DateTime.Now);

            // Insert StoreItem
            InsertStoreItem(storeNo, itemKey, expectedAuthorized);

            // Insert Sign Attributes
            InsertItemSignAttributes(itemKey, expectedChicagoBaby, expectedColorAdd, expectedLocality, expectedSignRomanceLong,
                expectedSignRomanceShort, expectedTagUom, expectedExclusive);

            // Insert Scale Extra Text
            var scaleExtraTextId = this.dbProvider.Insert(new IrmaQueryParams<Scale_ExtraText, int>(
                IrmaTestObjectFactory.Build<Scale_ExtraText>()
                    .With(x => x.ExtraText, expectedScaleExtraText)
                    .With(x => x.Description, "Dummy")
                    .ToObject(),
                x => x.Scale_ExtraText_ID));

            // Insert Item Scale
            this.dbProvider.Insert(new IrmaQueryParams<ItemScale, int>(
                IrmaTestObjectFactory.Build<ItemScale>()
                    .With(x => x.Item_Key, itemKey)
                    .With(x => x.Scale_ExtraText_ID, scaleExtraTextId)
                    .ToObject(),
                x => x.ItemScale_ID));

            var expectedQueueId = InsertToItemLocaleChangeQueue(itemKey, storeNo, expectedIdentifier, expectedEventTypeid);

            //When
            var actual = query.Search(parameters).First();

            //Then
            Assert.AreEqual(expectedQueueId, actual.QueueId, "The expected QueueID did not match the actual.");
            Assert.AreEqual(expectedEventTypeid, actual.EventTypeId, "The expected EventTypeId did not match the actual.");
            Assert.AreEqual(expectedAgeRestriction, actual.AgeRestriction, "The expected AgeCode did not match the actual.");
            Assert.AreEqual(expectedAuthorized, actual.Authorized, "The expected AuthorizedForSale did not match the actual.");
            Assert.AreEqual(expectedBusinessUnitId, actual.BusinessUnitId, "The expected BusinessUnit did not match the actual.");
            Assert.AreEqual(expectedCaseDiscount, actual.CaseDiscount, "The expected CaseDiscountEligible did not match the actual.");
            Assert.AreEqual(expectedRestrictedHours, actual.RestrictedHours, "The expected RestrictedHours did not match the actual.");
            Assert.AreEqual(expectedIdentifier, actual.ScanCode, "The expected ScanCode did not match the actual.");
            Assert.AreEqual(expectedChicagoBaby, actual.ChicagoBaby, "The expected ChicagoBaby did not match the actual.");
            Assert.AreEqual(expectedColorAdd, actual.ColorAdded, "The expected ColorAdded did not match the actual.");
            Assert.AreEqual(expectedOrigin.Origin_Name, actual.CountryOfProcessing, "The expected CountryOfProcessing did not match the actual.");
            Assert.AreEqual(expectedDiscontinued, actual.Discontinued, "The expected Discontinued did not match the actual.");
            Assert.AreEqual(expectedElectronicShelfTag, actual.ElectronicShelfTag, "The expected ElectronicShelfTag did not match the actual.");
            Assert.AreEqual(expectedExclusive, actual.Exclusive, "The expected Exclusive did not match the actual.");
            Assert.AreEqual(expectedLabelType.LabelTypeDesc, actual.LabelTypeDescription, "The expected LabelTypeDescription did not match the actual.");
            Assert.AreEqual(expectedLinkedIdentifier, actual.LinkedItem, "The expected LinkedItem did not match the actual.");
            Assert.AreEqual(expectedLocalItem, actual.LocalItem, "The expected LocalItem did not match the actual.");
            Assert.AreEqual(expectedLocality, actual.Locality, "The expected Locality did not match the actual.");
            Assert.AreEqual(expectedNumberOfDigitsSentToScale, actual.NumberOfDigitsSentToScale, "The expected NumberOfDigitsSentToScale did not match the actual.");
            Assert.AreEqual(expectedOrigin.Origin_Name, actual.Origin, "The expected Origin did not match the actual.");
            Assert.AreEqual(expectedProductCode, actual.ProductCode, "The expected ProductCode did not match the actual.");
            Assert.AreEqual(expectedRegion, actual.Region, "The expected Region did not match the actual.");
            Assert.AreEqual(expectedRetailUnit.Unit_Name, actual.RetailUnit, "The expected RetailUnit did not match the actual.");
            Assert.AreEqual(expectedScaleExtraText, actual.ScaleExtraText, "The expected ScaleExtraText did not match the actual.");
            Assert.AreEqual(expectedSignDescription, actual.SignDescription, "The expected SignDescription did not match the actual.");
            Assert.AreEqual(expectedSignRomanceLong, actual.SignRomanceLong, "The expected SignRomanceLong did not match the actual.");
            Assert.AreEqual(expectedSignRomanceShort, actual.SignRomanceShort, "The expected SignRomanceShort did not match the actual.");
            Assert.AreEqual(expectedTagUom.ToString(), actual.TagUom, "The expected TagUom did not match the actual.");
            Assert.AreEqual(Convert.ToDouble(expectedMsrp), actual.Msrp, "The expected Msrp did not match the actual.");
            Assert.IsTrue(string.IsNullOrEmpty(actual.ErrorMessage));
        }

        [TestMethod]
        public void GetItemLocaleEvents_AddUpdateEventsForTsStoreAndRmRegion_ShouldReturnRowsForEachRegion()
        {
            // Given
            var storeNoTs = 834792;
            var storeNoRm = 8585656;
            var subTeamNo = this.GetFirstFromTable<SubTeam>().SubTeam_No;
            var vendorId = this.GetFirstFromTable<Vendor>().Vendor_ID;
            var expectedRetailUnit = this.GetFirstFromTable<ItemUnit>();
            var expectedOrigin = this.GetFirstFromTable<ItemOrigin>();
            var expectedLabelType = this.GetFirstFromTable<LabelType>();
            var expectedLinkedItemKey = this.GetFirstFromTable<Item>().Item_Key;
            var expectedLinkedIdentifier = this.GetLinkedIdentifierByItemKey(expectedLinkedItemKey);
            var expectedScaleExtraTextId = this.GetFirstFromTable<ItemScale>().Scale_ExtraText_ID;
            var expectedRegionTs = "TS";
            var expectedRegionRm = "FL";
            var expectedIdentifier = "1234567";
            var expectedBusinessUnitIdTs = 83472;
            var expectedBusinessUnitIdRm = 7774445;
            var expectedAgeRestrictionId = 2;
            var expectedAgeRestriction = 21;
            var expectedAuthorized = true;
            var expectedCaseDiscount = true;
            var expectedDiscontinued = false;
            var expectedLocalItem = true;
            var expectedLocality = "Test Locality";
            var expectedProductCode = "TestProductCode";
            var expectedRestrictedHours = true;
            var expectedSignRomanceLong = "Test Sign Romance Long";
            var expectedSignRomanceShort = "Test Sign Romance Short";
            var expectedSignDescription = "Test Sign Description";
            var expectedTmDiscount = true;
            var expectedMsrp = 2000m;
            var expectedChicagoBaby = "Test Baby";
            var expectedColorAdd = true;
            var expectedElectronicShelfTag = true;
            var expectedExclusive = DateTime.Today;
            var expectedNumberOfDigitsSentToScale = 123;
            var expectedScaleExtraText = "Test Scale Extra Text";
            var expectedTagUom = 23;
            var expectedEventTypeid = IrmaEventTypes.ItemLocaleAddOrUpdate;

            // Insert New Item
            var itemKey = InsertNewItem(subTeamNo, expectedSignDescription, expectedProductCode,
                expectedOrigin.Origin_ID, expectedLabelType.LabelType_ID, expectedRetailUnit.Unit_ID);

            // Insert New Item Identifier
            InsertNewItemIdentifier(itemKey, expectedIdentifier, expectedNumberOfDigitsSentToScale);

            // Insert New Stores
            InsertNewStore(storeNoTs, expectedBusinessUnitIdTs, 1, true, null, null, "Test TS Store");
            InsertNewStore(storeNoRm, expectedBusinessUnitIdRm, 1, true, null, null, "Test RM Store");

            // Insert New Store Region Mapping
            // Ts
            InsertNewStoreRegionMapping(storeNoTs, expectedRegionTs);
            // Rm
            InsertNewStoreRegionMapping(storeNoRm, expectedRegionRm);

            // Insert New Price
            InsertNewPrice(expectedCaseDiscount, expectedElectronicShelfTag, expectedLocalItem, expectedRestrictedHours, expectedTmDiscount,
                expectedMsrp, storeNoTs, itemKey, expectedAgeRestrictionId, expectedLinkedItemKey);
            InsertNewPrice(expectedCaseDiscount, expectedElectronicShelfTag, expectedLocalItem, expectedRestrictedHours, expectedTmDiscount,
                expectedMsrp, storeNoRm, itemKey, expectedAgeRestrictionId, expectedLinkedItemKey);

            // Insert New Validated Scan Code
            InsertNewValidatedScanCode(expectedIdentifier);

            // Insert Item Vendor
            InsertItemVendor(itemKey, vendorId);

            // Insert StoreItemVendor
            InsertStoreItemVendor(vendorId, storeNoTs, itemKey, expectedDiscontinued, true, DateTime.Now);
            InsertStoreItemVendor(vendorId, storeNoRm, itemKey, expectedDiscontinued, true, DateTime.Now);

            // Insert StoreItem
            InsertStoreItem(storeNoTs, itemKey, expectedAuthorized);
            InsertStoreItem(storeNoRm, itemKey, expectedAuthorized);

            // Insert Sign Attributes
            InsertItemSignAttributes(itemKey, expectedChicagoBaby, expectedColorAdd, expectedLocality, expectedSignRomanceLong,
                expectedSignRomanceShort, expectedTagUom, expectedExclusive);

            // Insert Scale Extra Text
            var scaleExtraTextId = this.dbProvider.Insert(new IrmaQueryParams<Scale_ExtraText, int>(
                IrmaTestObjectFactory.Build<Scale_ExtraText>()
                    .With(x => x.ExtraText, expectedScaleExtraText)
                    .With(x => x.Description, "Dummy")
                    .ToObject(),
                x => x.Scale_ExtraText_ID));

            // Insert Item Scale
            this.dbProvider.Insert(new IrmaQueryParams<ItemScale, int>(
                IrmaTestObjectFactory.Build<ItemScale>()
                    .With(x => x.Item_Key, itemKey)
                    .With(x => x.Scale_ExtraText_ID, scaleExtraTextId)
                    .ToObject(),
                x => x.ItemScale_ID));

            var expectedQueueIdTs = this.dbProvider.Insert(
                new IrmaQueryParams<TestQueueModel, int>(
                    new TestQueueModel
                    {
                        ItemKey = itemKey,
                        StoreNo = storeNoTs,
                        Identifier = expectedIdentifier,
                        EventTypeId = expectedEventTypeid,
                        InsertDate = DateTime.Now,
                        InProcessBy = this.parameters.Instance
                    },
                    null,
                    new Dictionary<string, string>
                    {
                        { "ItemKey", "Item_Key" },
                        { "StoreNo", "Store_No" },
                    },
                    "mammoth.ItemLocaleChangeQueue",
                    true));

            var expectedQueueIdRm = InsertToItemLocaleChangeQueue(itemKey, storeNoRm, expectedIdentifier, expectedEventTypeid);

            //When
            var actual = query.Search(parameters).OrderBy(a => a.Region).ToList();

            //Then
            Assert.AreEqual(2, actual.Count);

            Assert.AreEqual(expectedQueueIdRm, actual[0].QueueId, "The expected QueueID did not match the actual.");
            Assert.AreEqual(expectedEventTypeid, actual[0].EventTypeId, "The expected EventTypeId did not match the actual.");
            Assert.AreEqual(expectedAgeRestriction, actual[0].AgeRestriction, "The expected AgeCode did not match the actual.");
            Assert.AreEqual(expectedAuthorized, actual[0].Authorized, "The expected AuthorizedForSale did not match the actual.");
            Assert.AreEqual(expectedBusinessUnitIdRm, actual[0].BusinessUnitId, "The expected BusinessUnit did not match the actual.");
            Assert.AreEqual(expectedCaseDiscount, actual[0].CaseDiscount, "The expected CaseDiscountEligible did not match the actual.");
            Assert.AreEqual(expectedRestrictedHours, actual[0].RestrictedHours, "The expected RestrictedHours did not match the actual.");
            Assert.AreEqual(expectedIdentifier, actual[0].ScanCode, "The expected ScanCode did not match the actual.");
            Assert.AreEqual(expectedChicagoBaby, actual[0].ChicagoBaby, "The expected ChicagoBaby did not match the actual.");
            Assert.AreEqual(expectedColorAdd, actual[0].ColorAdded, "The expected ColorAdded did not match the actual.");
            Assert.AreEqual(expectedOrigin.Origin_Name, actual[0].CountryOfProcessing, "The expected CountryOfProcessing did not match the actual.");
            Assert.AreEqual(expectedDiscontinued, actual[0].Discontinued, "The expected Discontinued did not match the actual.");
            Assert.AreEqual(expectedElectronicShelfTag, actual[0].ElectronicShelfTag, "The expected ElectronicShelfTag did not match the actual.");
            Assert.AreEqual(expectedExclusive, actual[0].Exclusive, "The expected Exclusive did not match the actual.");
            Assert.AreEqual(expectedLabelType.LabelTypeDesc, actual[0].LabelTypeDescription, "The expected LabelTypeDescription did not match the actual.");
            Assert.AreEqual(expectedLinkedIdentifier, actual[0].LinkedItem, "The expected LinkedItem did not match the actual.");
            Assert.AreEqual(expectedLocalItem, actual[0].LocalItem, "The expected LocalItem did not match the actual.");
            Assert.AreEqual(expectedLocality, actual[0].Locality, "The expected Locality did not match the actual.");
            Assert.AreEqual(expectedNumberOfDigitsSentToScale, actual[0].NumberOfDigitsSentToScale, "The expected NumberOfDigitsSentToScale did not match the actual.");
            Assert.AreEqual(expectedOrigin.Origin_Name, actual[0].Origin, "The expected Origin did not match the actual.");
            Assert.AreEqual(expectedProductCode, actual[0].ProductCode, "The expected ProductCode did not match the actual.");
            Assert.AreEqual(expectedRegionRm, actual[0].Region, "The expected Region did not match the actual.");
            Assert.AreEqual(expectedRetailUnit.Unit_Name, actual[0].RetailUnit, "The expected RetailUnit did not match the actual.");
            Assert.AreEqual(expectedScaleExtraText, actual[0].ScaleExtraText, "The expected ScaleExtraText did not match the actual.");
            Assert.AreEqual(expectedSignDescription, actual[0].SignDescription, "The expected SignDescription did not match the actual.");
            Assert.AreEqual(expectedSignRomanceLong, actual[0].SignRomanceLong, "The expected SignRomanceLong did not match the actual.");
            Assert.AreEqual(expectedSignRomanceShort, actual[0].SignRomanceShort, "The expected SignRomanceShort did not match the actual.");
            Assert.AreEqual(expectedTagUom.ToString(), actual[0].TagUom, "The expected TagUom did not match the actual.");
            Assert.AreEqual(Convert.ToDouble(expectedMsrp), actual[0].Msrp, "The expected Msrp did not match the actual.");
            Assert.IsTrue(string.IsNullOrEmpty(actual[0].ErrorMessage));

            Assert.AreEqual(expectedQueueIdTs, actual[1].QueueId, "The expected QueueID did not match the actual.");
            Assert.AreEqual(expectedEventTypeid, actual[1].EventTypeId, "The expected EventTypeId did not match the actual.");
            Assert.AreEqual(expectedAgeRestriction, actual[1].AgeRestriction, "The expected AgeCode did not match the actual.");
            Assert.AreEqual(expectedAuthorized, actual[1].Authorized, "The expected AuthorizedForSale did not match the actual.");
            Assert.AreEqual(expectedBusinessUnitIdTs, actual[1].BusinessUnitId, "The expected BusinessUnit did not match the actual.");
            Assert.AreEqual(expectedCaseDiscount, actual[1].CaseDiscount, "The expected CaseDiscountEligible did not match the actual.");
            Assert.AreEqual(expectedRestrictedHours, actual[1].RestrictedHours, "The expected RestrictedHours did not match the actual.");
            Assert.AreEqual(expectedIdentifier, actual[1].ScanCode, "The expected ScanCode did not match the actual.");
            Assert.AreEqual(expectedChicagoBaby, actual[1].ChicagoBaby, "The expected ChicagoBaby did not match the actual.");
            Assert.AreEqual(expectedColorAdd, actual[1].ColorAdded, "The expected ColorAdded did not match the actual.");
            Assert.AreEqual(expectedOrigin.Origin_Name, actual[1].CountryOfProcessing, "The expected CountryOfProcessing did not match the actual.");
            Assert.AreEqual(expectedDiscontinued, actual[1].Discontinued, "The expected Discontinued did not match the actual.");
            Assert.AreEqual(expectedElectronicShelfTag, actual[1].ElectronicShelfTag, "The expected ElectronicShelfTag did not match the actual.");
            Assert.AreEqual(expectedExclusive, actual[1].Exclusive, "The expected Exclusive did not match the actual.");
            Assert.AreEqual(expectedLabelType.LabelTypeDesc, actual[1].LabelTypeDescription, "The expected LabelTypeDescription did not match the actual.");
            Assert.AreEqual(expectedLinkedIdentifier, actual[1].LinkedItem, "The expected LinkedItem did not match the actual.");
            Assert.AreEqual(expectedLocalItem, actual[1].LocalItem, "The expected LocalItem did not match the actual.");
            Assert.AreEqual(expectedLocality, actual[1].Locality, "The expected Locality did not match the actual.");
            Assert.AreEqual(expectedNumberOfDigitsSentToScale, actual[1].NumberOfDigitsSentToScale, "The expected NumberOfDigitsSentToScale did not match the actual.");
            Assert.AreEqual(expectedOrigin.Origin_Name, actual[1].Origin, "The expected Origin did not match the actual.");
            Assert.AreEqual(expectedProductCode, actual[1].ProductCode, "The expected ProductCode did not match the actual.");
            Assert.AreEqual(expectedRegionTs, actual[1].Region, "The expected Region did not match the actual.");
            Assert.AreEqual(expectedRetailUnit.Unit_Name, actual[1].RetailUnit, "The expected RetailUnit did not match the actual.");
            Assert.AreEqual(expectedScaleExtraText, actual[1].ScaleExtraText, "The expected ScaleExtraText did not match the actual.");
            Assert.AreEqual(expectedSignDescription, actual[1].SignDescription, "The expected SignDescription did not match the actual.");
            Assert.AreEqual(expectedSignRomanceLong, actual[1].SignRomanceLong, "The expected SignRomanceLong did not match the actual.");
            Assert.AreEqual(expectedSignRomanceShort, actual[1].SignRomanceShort, "The expected SignRomanceShort did not match the actual.");
            Assert.AreEqual(expectedTagUom.ToString(), actual[1].TagUom, "The expected TagUom did not match the actual.");
            Assert.AreEqual(Convert.ToDouble(expectedMsrp), actual[1].Msrp, "The expected Msrp did not match the actual.");
            Assert.IsTrue(string.IsNullOrEmpty(actual[1].ErrorMessage));
        }

        [TestMethod]
        public void GetItemLocaleEvents_HasIrmaItemKeyAndDefaultScanCode_ShouldReturnExpectedValues()
        {
            // Given
            var storeNo = 834792;
            var subTeamNo = this.GetFirstFromTable<SubTeam>().SubTeam_No;
            var vendorId = this.GetFirstFromTable<Vendor>().Vendor_ID;
            var expectedRetailUnit = this.GetFirstFromTable<ItemUnit>();
            var expectedOrigin = this.GetFirstFromTable<ItemOrigin>();
            var expectedLabelType = this.GetFirstFromTable<LabelType>();
            var expectedLinkedItemKey = this.GetFirstFromTable<Item>().Item_Key;
            var expectedLinkedIdentifier = this.GetLinkedIdentifierByItemKey(expectedLinkedItemKey);
            var expectedScaleExtraTextId = this.GetFirstFromTable<ItemScale>().Scale_ExtraText_ID;
            var expectedRegion = region;
            var expectedIdentifier = "22222242";
            var expectedBusinessUnitId = 83472;
            var expectedAgeRestrictionId = 2;
            var expectedAuthorized = true;
            var expectedCaseDiscount = true;
            var expectedDiscontinued = false;
            var expectedLocalItem = true;
            var expectedLocality = "Test Locality";
            var expectedProductCode = "TestProductCode";
            var expectedRestrictedHours = true;
            var expectedSignRomanceLong = "Test Sign Romance Long";
            var expectedSignRomanceShort = "Test Sign Romance Short";
            var expectedSignDescription = "Test Sign Description";
            var expectedTmDiscount = true;
            var expectedMsrp = 2000m;
            var expectedChicagoBaby = "Test Baby";
            var expectedColorAdd = true;
            var expectedElectronicShelfTag = true;
            var expectedExclusive = DateTime.Today;
            var expectedNumberOfDigitsSentToScale = 123;
            var expectedTagUom = 23;
            var expectedEventTypeid = IrmaEventTypes.ItemLocaleAddOrUpdate;
            var expectedDefaultScanCode = true;
                        
            // Insert New Item 
            var itemKey = InsertNewItem(subTeamNo, expectedSignDescription, expectedProductCode,
                expectedOrigin.Origin_ID, expectedLabelType.LabelType_ID, expectedRetailUnit.Unit_ID);

            var expectedIrmaItemKey = itemKey;

            // Insert New Item Identifier
            InsertNewItemIdentifier(itemKey, expectedIdentifier, expectedNumberOfDigitsSentToScale, expectedDefaultScanCode);

            // Insert New Store
            InsertNewStore(storeNo, expectedBusinessUnitId, 1, true);

            // Insert New Store Region Mapping
            InsertNewStoreRegionMapping(storeNo, expectedRegion);

            // Insert New Price
            InsertNewPrice(expectedCaseDiscount, expectedElectronicShelfTag, expectedLocalItem, expectedRestrictedHours, expectedTmDiscount,
                expectedMsrp, storeNo, itemKey, expectedAgeRestrictionId, expectedLinkedItemKey);

            // Insert New Validated Scan Code
            InsertNewValidatedScanCode(expectedIdentifier);

            // Insert Item Vendor
            InsertItemVendor(itemKey, vendorId);

            // Insert StoreItemVendor
            InsertStoreItemVendor(vendorId, storeNo, itemKey, expectedDiscontinued, true, DateTime.Now);

            // Insert StoreItem
            InsertStoreItem(storeNo, itemKey, expectedAuthorized);

            // Insert Sign Attributes
            InsertItemSignAttributes(itemKey, expectedChicagoBaby, expectedColorAdd, expectedLocality, expectedSignRomanceLong,
                expectedSignRomanceShort, expectedTagUom, expectedExclusive);

            var expectedQueueId = InsertToItemLocaleChangeQueue(itemKey, storeNo, expectedIdentifier);          

            //When
            var actual = query.Search(parameters).First();

            //Then
            Assert.AreEqual(expectedQueueId, actual.QueueId, "The expected QueueID did not match the actual.");
            Assert.AreEqual(expectedEventTypeid, actual.EventTypeId, "The expected EventTypeId did not match the actual.");
            Assert.AreEqual(expectedDefaultScanCode, actual.DefaultScanCode, "The expected DefaultScanCode did not match the actual.");
            Assert.AreEqual(expectedIrmaItemKey, actual.IrmaItemKey, "The expected IrmaItemKey did not match the actual.");
            Assert.IsTrue(string.IsNullOrEmpty(actual.ErrorMessage));
        }

        [TestMethod]
        public void GetItemLocaleEvents_HasIrmaItemKeyAsNull_ShouldReturnNullIrmaItemKey()
        {
            var storeNo = 834792;
            var subTeamNo = this.GetFirstFromTable<SubTeam>().SubTeam_No;
            var vendorId = this.GetFirstFromTable<Vendor>().Vendor_ID;
            var expectedRetailUnit = this.GetFirstFromTable<ItemUnit>();
            var expectedOrigin = this.GetFirstFromTable<ItemOrigin>();
            var expectedLabelType = this.GetFirstFromTable<LabelType>();
            var expectedLinkedItemKey = this.GetFirstFromTable<Item>().Item_Key;
            var expectedLinkedIdentifier = this.GetLinkedIdentifierByItemKey(expectedLinkedItemKey);
            var expectedScaleExtraTextId = this.GetFirstFromTable<ItemScale>().Scale_ExtraText_ID;
            var expectedRegion = region;
            var expectedIdentifier = "22222242";
            var expectedBusinessUnitId = 83472;
            var expectedAgeRestrictionId = 2;
            var expectedAuthorized = true;
            var expectedCaseDiscount = true;
            var expectedDiscontinued = false;
            var expectedLocalItem = true;
            var expectedLocality = "Test Locality";
            var expectedProductCode = "TestProductCode";
            var expectedRestrictedHours = true;
            var expectedSignRomanceLong = "Test Sign Romance Long";
            var expectedSignRomanceShort = "Test Sign Romance Short";
            var expectedSignDescription = "Test Sign Description";
            var expectedTmDiscount = true;
            var expectedMsrp = 2000m;
            var expectedChicagoBaby = "Test Baby";
            var expectedColorAdd = true;
            var expectedElectronicShelfTag = true;
            var expectedExclusive = DateTime.Today;
            var expectedNumberOfDigitsSentToScale = 123;
            var expectedTagUom = 23;
            var expectedEventTypeid = IrmaEventTypes.ItemLocaleAddOrUpdate;
            var expectedDefaultScanCode = true;

            var itemKey = InsertNewItem(subTeamNo, expectedSignDescription, expectedProductCode,
                expectedOrigin.Origin_ID, expectedLabelType.LabelType_ID, expectedRetailUnit.Unit_ID);
            var expectedIrmaItemKey = itemKey;

            InsertNewItemIdentifier(itemKey, expectedIdentifier, expectedNumberOfDigitsSentToScale, expectedDefaultScanCode, true);
            InsertNewStore(storeNo, expectedBusinessUnitId);
            InsertNewStoreRegionMapping(storeNo, expectedRegion);
            InsertNewPrice(expectedCaseDiscount, expectedElectronicShelfTag, expectedLocalItem, expectedRestrictedHours, expectedTmDiscount,
                expectedMsrp, storeNo, itemKey, expectedAgeRestrictionId, expectedLinkedItemKey);
            InsertNewValidatedScanCode(expectedIdentifier);
            InsertItemVendor(itemKey, vendorId);
            InsertStoreItemVendor(vendorId, storeNo, itemKey, expectedDiscontinued);
            InsertStoreItem(storeNo, itemKey, expectedAuthorized);
            InsertItemSignAttributes(itemKey, expectedChicagoBaby, expectedColorAdd, expectedLocality, expectedSignRomanceLong,
                expectedSignRomanceShort, expectedTagUom, expectedExclusive);

            var expectedQueueId = InsertToItemLocaleChangeQueue(itemKey, storeNo, expectedIdentifier);

            //When
            var actual = query.Search(parameters).First();

            //Then
            Assert.AreEqual(expectedEventTypeid, actual.EventTypeId, "The expected EventTypeId did not match the actual.");
            Assert.IsNull(actual.IrmaItemKey, nameof(ItemLocaleEventModel.IrmaItemKey));
            Assert.AreEqual(expectedDefaultScanCode, actual.DefaultScanCode, "The expected DefaultScanCode did not match the actual.");
        }

        [TestMethod]
        public void GetItemLocaleEvents_IrmaItemIdentifierIsRemoved_ShouldReturnNullIrmaItemKey()
        {
            var storeNo = 834792;
            var subTeamNo = this.GetFirstFromTable<SubTeam>().SubTeam_No;
            var vendorId = this.GetFirstFromTable<Vendor>().Vendor_ID;
            var expectedRetailUnit = this.GetFirstFromTable<ItemUnit>();
            var expectedOrigin = this.GetFirstFromTable<ItemOrigin>();
            var expectedLabelType = this.GetFirstFromTable<LabelType>();
            var expectedLinkedItemKey = this.GetFirstFromTable<Item>().Item_Key;
            var expectedLinkedIdentifier = this.GetLinkedIdentifierByItemKey(expectedLinkedItemKey);
            var expectedScaleExtraTextId = this.GetFirstFromTable<ItemScale>().Scale_ExtraText_ID;
            var expectedRegion = region;
            var expectedIdentifier = "22222242";
            var expectedBusinessUnitId = 83472;
            var expectedAgeRestrictionId = 2;
            var expectedAuthorized = true;
            var expectedCaseDiscount = true;
            var expectedDiscontinued = false;
            var expectedLocalItem = true;
            var expectedLocality = "Test Locality";
            var expectedProductCode = "TestProductCode";
            var expectedRestrictedHours = true;
            var expectedSignRomanceLong = "Test Sign Romance Long";
            var expectedSignRomanceShort = "Test Sign Romance Short";
            var expectedSignDescription = "Test Sign Description";
            var expectedTmDiscount = true;
            var expectedMsrp = 2000m;
            var expectedChicagoBaby = "Test Baby";
            var expectedColorAdd = true;
            var expectedElectronicShelfTag = true;
            var expectedExclusive = DateTime.Today;
            var expectedNumberOfDigitsSentToScale = 123;
            var expectedTagUom = 23;
            var expectedDefaultScanCode = true;

            var itemKey = InsertNewItem(subTeamNo, expectedSignDescription, expectedProductCode,
                expectedOrigin.Origin_ID, expectedLabelType.LabelType_ID, expectedRetailUnit.Unit_ID);
            var expectedIrmaItemKey = itemKey;

            InsertNewItemIdentifier(
                itemKey: itemKey,
                Identifier: expectedIdentifier,
                numberOfDigitsSentToScale: expectedNumberOfDigitsSentToScale,
                defaultScanCode: expectedDefaultScanCode,
                removeIdentifier: true,
                deletedIdentifier: false);
            InsertNewStore(storeNo, expectedBusinessUnitId);
            InsertNewStoreRegionMapping(storeNo, expectedRegion);
            InsertNewPrice(expectedCaseDiscount, expectedElectronicShelfTag, expectedLocalItem, expectedRestrictedHours, expectedTmDiscount,
                expectedMsrp, storeNo, itemKey, expectedAgeRestrictionId, expectedLinkedItemKey);
            InsertNewValidatedScanCode(expectedIdentifier);
            InsertItemVendor(itemKey, vendorId);
            InsertStoreItemVendor(vendorId, storeNo, itemKey, expectedDiscontinued);
            InsertStoreItem(storeNo, itemKey, expectedAuthorized);
            InsertItemSignAttributes(itemKey, expectedChicagoBaby, expectedColorAdd, expectedLocality, expectedSignRomanceLong,
                expectedSignRomanceShort, expectedTagUom, expectedExclusive);

            var expectedQueueId = InsertToItemLocaleChangeQueue(itemKey, storeNo, expectedIdentifier);

            //When
            var actual = query.Search(parameters).First();

            //Then
            Assert.IsNull(actual.IrmaItemKey, nameof(ItemLocaleEventModel.IrmaItemKey));
        }

        [TestMethod]
        public void GetItemLocaleEvents_IrmaItemIdentifierIsDeleted_ShouldReturnNullIrmaItemKey()
        {
            var storeNo = 834792;
            var subTeamNo = this.GetFirstFromTable<SubTeam>().SubTeam_No;
            var vendorId = this.GetFirstFromTable<Vendor>().Vendor_ID;
            var expectedRetailUnit = this.GetFirstFromTable<ItemUnit>();
            var expectedOrigin = this.GetFirstFromTable<ItemOrigin>();
            var expectedLabelType = this.GetFirstFromTable<LabelType>();
            var expectedLinkedItemKey = this.GetFirstFromTable<Item>().Item_Key;
            var expectedLinkedIdentifier = this.GetLinkedIdentifierByItemKey(expectedLinkedItemKey);
            var expectedScaleExtraTextId = this.GetFirstFromTable<ItemScale>().Scale_ExtraText_ID;
            var expectedRegion = region;
            var expectedIdentifier = "22222242";
            var expectedBusinessUnitId = 83472;
            var expectedAgeRestrictionId = 2;
            var expectedAuthorized = true;
            var expectedCaseDiscount = true;
            var expectedDiscontinued = false;
            var expectedLocalItem = true;
            var expectedLocality = "Test Locality";
            var expectedProductCode = "TestProductCode";
            var expectedRestrictedHours = true;
            var expectedSignRomanceLong = "Test Sign Romance Long";
            var expectedSignRomanceShort = "Test Sign Romance Short";
            var expectedSignDescription = "Test Sign Description";
            var expectedTmDiscount = true;
            var expectedMsrp = 2000m;
            var expectedChicagoBaby = "Test Baby";
            var expectedColorAdd = true;
            var expectedElectronicShelfTag = true;
            var expectedExclusive = DateTime.Today;
            var expectedNumberOfDigitsSentToScale = 123;
            var expectedTagUom = 23;
            var expectedDefaultScanCode = true;

            var itemKey = InsertNewItem(subTeamNo, expectedSignDescription, expectedProductCode,
                expectedOrigin.Origin_ID, expectedLabelType.LabelType_ID, expectedRetailUnit.Unit_ID);
            var expectedIrmaItemKey = itemKey;

            InsertNewItemIdentifier(
                itemKey: itemKey,
                Identifier: expectedIdentifier,
                numberOfDigitsSentToScale: expectedNumberOfDigitsSentToScale,
                defaultScanCode: expectedDefaultScanCode,
                removeIdentifier: false,
                deletedIdentifier: true);
            InsertNewStore(storeNo, expectedBusinessUnitId);
            InsertNewStoreRegionMapping(storeNo, expectedRegion);
            InsertNewPrice(expectedCaseDiscount, expectedElectronicShelfTag, expectedLocalItem, expectedRestrictedHours, expectedTmDiscount,
                expectedMsrp, storeNo, itemKey, expectedAgeRestrictionId, expectedLinkedItemKey);
            InsertNewValidatedScanCode(expectedIdentifier);
            InsertItemVendor(itemKey, vendorId);
            InsertStoreItemVendor(vendorId, storeNo, itemKey, expectedDiscontinued);
            InsertStoreItem(storeNo, itemKey, expectedAuthorized);
            InsertItemSignAttributes(itemKey, expectedChicagoBaby, expectedColorAdd, expectedLocality, expectedSignRomanceLong,
                expectedSignRomanceShort, expectedTagUom, expectedExclusive);

            var expectedQueueId = InsertToItemLocaleChangeQueue(itemKey, storeNo, expectedIdentifier);

            var test = dbProvider.Connection.Query<ItemIdentifier>(
                "SELECT * FROM dbo.ItemIdentifier WHERE Identifier = @identifier",
                new { identifier = expectedIdentifier },
                dbProvider.Transaction)
                .ToList();

            //When
            var actual = query.Search(parameters).First();

            var test2 = dbProvider.Connection.Query<ItemIdentifier>(
                "SELECT * FROM dbo.ItemIdentifier WHERE Identifier = @identifier",
                new { identifier = expectedIdentifier },
                dbProvider.Transaction)
                .ToList();

            var test3 = dbProvider.Connection.Query<ItemIdentifier>(
                "SELECT * FROM mammoth.ItemLocaleChangeQueue",
                null,
                dbProvider.Transaction)
                .ToList();

            //Then
            Assert.IsNull(actual.IrmaItemKey, nameof(ItemLocaleEventModel.IrmaItemKey));
        }
        [TestMethod]

        public void GetItemLocaleEvents_IrmaItemIsRemoved_ShouldReturnNullIrmaItemKey()
        {
            var storeNo = 834792;
            var subTeamNo = this.GetFirstFromTable<SubTeam>().SubTeam_No;
            var vendorId = this.GetFirstFromTable<Vendor>().Vendor_ID;
            var expectedRetailUnit = this.GetFirstFromTable<ItemUnit>();
            var expectedOrigin = this.GetFirstFromTable<ItemOrigin>();
            var expectedLabelType = this.GetFirstFromTable<LabelType>();
            var expectedLinkedItemKey = this.GetFirstFromTable<Item>().Item_Key;
            var expectedLinkedIdentifier = this.GetLinkedIdentifierByItemKey(expectedLinkedItemKey);
            var expectedScaleExtraTextId = this.GetFirstFromTable<ItemScale>().Scale_ExtraText_ID;
            var expectedRegion = region;
            var expectedIdentifier = "22222242";
            var expectedBusinessUnitId = 83472;
            var expectedAgeRestrictionId = 2;
            var expectedAuthorized = true;
            var expectedCaseDiscount = true;
            var expectedDiscontinued = false;
            var expectedLocalItem = true;
            var expectedLocality = "Test Locality";
            var expectedProductCode = "TestProductCode";
            var expectedRestrictedHours = true;
            var expectedSignRomanceLong = "Test Sign Romance Long";
            var expectedSignRomanceShort = "Test Sign Romance Short";
            var expectedSignDescription = "Test Sign Description";
            var expectedTmDiscount = true;
            var expectedMsrp = 2000m;
            var expectedChicagoBaby = "Test Baby";
            var expectedColorAdd = true;
            var expectedElectronicShelfTag = true;
            var expectedExclusive = DateTime.Today;
            var expectedNumberOfDigitsSentToScale = 123;
            var expectedTagUom = 23;
            var expectedDefaultScanCode = true;

            var itemKey = InsertNewItem(subTeamNo, expectedSignDescription, expectedProductCode,
                expectedOrigin.Origin_ID, expectedLabelType.LabelType_ID, expectedRetailUnit.Unit_ID, true);
            var expectedIrmaItemKey = itemKey;

            InsertNewItemIdentifier(itemKey, expectedIdentifier, expectedNumberOfDigitsSentToScale, expectedDefaultScanCode, true);
            InsertNewStore(storeNo, expectedBusinessUnitId);
            InsertNewStoreRegionMapping(storeNo, expectedRegion);
            InsertNewPrice(expectedCaseDiscount, expectedElectronicShelfTag, expectedLocalItem, expectedRestrictedHours, expectedTmDiscount,
                expectedMsrp, storeNo, itemKey, expectedAgeRestrictionId, expectedLinkedItemKey);
            InsertNewValidatedScanCode(expectedIdentifier);
            InsertItemVendor(itemKey, vendorId);
            InsertStoreItemVendor(vendorId, storeNo, itemKey, expectedDiscontinued);
            InsertStoreItem(storeNo, itemKey, expectedAuthorized);
            InsertItemSignAttributes(itemKey, expectedChicagoBaby, expectedColorAdd, expectedLocality, expectedSignRomanceLong,
                expectedSignRomanceShort, expectedTagUom, expectedExclusive);

            var expectedQueueId = InsertToItemLocaleChangeQueue(itemKey, storeNo, expectedIdentifier);

            //When
            var actual = query.Search(parameters).First();

            //Then
            Assert.IsNull(actual.IrmaItemKey, nameof(ItemLocaleEventModel.IrmaItemKey));
        }

        [TestMethod]
        public void GetItemLocaleEvents_IrmaItemIsDeleted_ShouldReturnNullIrmaItemKey()
        {
            var storeNo = 834792;
            var subTeamNo = this.GetFirstFromTable<SubTeam>().SubTeam_No;
            var vendorId = this.GetFirstFromTable<Vendor>().Vendor_ID;
            var expectedRetailUnit = this.GetFirstFromTable<ItemUnit>();
            var expectedOrigin = this.GetFirstFromTable<ItemOrigin>();
            var expectedLabelType = this.GetFirstFromTable<LabelType>();
            var expectedLinkedItemKey = this.GetFirstFromTable<Item>().Item_Key;
            var expectedLinkedIdentifier = this.GetLinkedIdentifierByItemKey(expectedLinkedItemKey);
            var expectedScaleExtraTextId = this.GetFirstFromTable<ItemScale>().Scale_ExtraText_ID;
            var expectedRegion = region;
            var expectedIdentifier = "22222242";
            var expectedBusinessUnitId = 83472;
            var expectedAgeRestrictionId = 2;
            var expectedAuthorized = true;
            var expectedCaseDiscount = true;
            var expectedDiscontinued = false;
            var expectedLocalItem = true;
            var expectedLocality = "Test Locality";
            var expectedProductCode = "TestProductCode";
            var expectedRestrictedHours = true;
            var expectedSignRomanceLong = "Test Sign Romance Long";
            var expectedSignRomanceShort = "Test Sign Romance Short";
            var expectedSignDescription = "Test Sign Description";
            var expectedTmDiscount = true;
            var expectedMsrp = 2000m;
            var expectedChicagoBaby = "Test Baby";
            var expectedColorAdd = true;
            var expectedElectronicShelfTag = true;
            var expectedExclusive = DateTime.Today;
            var expectedNumberOfDigitsSentToScale = 123;
            var expectedTagUom = 23;
            var expectedDefaultScanCode = true;

            var itemKey = InsertNewItem(subTeamNo, expectedSignDescription, expectedProductCode,
                expectedOrigin.Origin_ID, expectedLabelType.LabelType_ID, expectedRetailUnit.Unit_ID, false, true);
            var expectedIrmaItemKey = itemKey;

            InsertNewItemIdentifier(itemKey, expectedIdentifier, expectedNumberOfDigitsSentToScale, expectedDefaultScanCode, true);
            InsertNewStore(storeNo, expectedBusinessUnitId);
            InsertNewStoreRegionMapping(storeNo, expectedRegion);
            InsertNewPrice(expectedCaseDiscount, expectedElectronicShelfTag, expectedLocalItem, expectedRestrictedHours, expectedTmDiscount,
                expectedMsrp, storeNo, itemKey, expectedAgeRestrictionId, expectedLinkedItemKey);
            InsertNewValidatedScanCode(expectedIdentifier);
            InsertItemVendor(itemKey, vendorId);
            InsertStoreItemVendor(vendorId, storeNo, itemKey, expectedDiscontinued);
            InsertStoreItem(storeNo, itemKey, expectedAuthorized);
            InsertItemSignAttributes(itemKey, expectedChicagoBaby, expectedColorAdd, expectedLocality, expectedSignRomanceLong,
                expectedSignRomanceShort, expectedTagUom, expectedExclusive);

            var expectedQueueId = InsertToItemLocaleChangeQueue(itemKey, storeNo, expectedIdentifier);

            //When
            var actual = query.Search(parameters).First();

            //Then
            Assert.IsNull(actual.IrmaItemKey, nameof(ItemLocaleEventModel.IrmaItemKey));
        }

        protected int InsertToItemLocaleChangeQueue(int itemKey, int? storeNo, string identfiier, int eventType = IrmaEventTypes.ItemLocaleAddOrUpdate)
        {
            var expectedQueueId = this.dbProvider.Insert(
                new IrmaQueryParams<TestQueueModel, int>(
                    new TestQueueModel
                    {
                        ItemKey = itemKey,
                        StoreNo = storeNo,
                        Identifier = identfiier,
                        EventTypeId = eventType,
                        InsertDate = DateTime.Now,
                        InProcessBy = this.parameters.Instance
                    },
                    null,
                    new Dictionary<string, string>
                    {
                        { "ItemKey", "Item_Key" },
                        { "StoreNo", "Store_No" },
                    },
                    "mammoth.ItemLocaleChangeQueue",
                    true));
            return expectedQueueId;
        }


        private void InsertIntoChangeQueue(int rows, int jobInstance, int eventTypeId, int? storeNumber)
        {
            string sql = String.Format(@"SELECT TOP {0}
                                            ii.Identifier as Identifier,
                                            ii.Item_Key as ItemKey
                                        FROM
                                            ItemIdentifier ii
                                            JOIN Item i on ii.Item_Key = i.Item_Key
                                            JOIN ValidatedScanCode vsc on ii.Identifier = vsc.ScanCode
                                        WHERE
		                                    i.Deleted_Item = 0
		                                    AND ii.Deleted_Identifier = 0
		                                    AND i.Remove_Item = 0
		                                    AND ii.Remove_Identifier = 0
                                            AND NOT EXISTS (SELECT 1 FROM mammoth.ItemLocaleChangeQueue q WHERE q.Item_Key = i.Item_Key)", rows);

            IEnumerable<dynamic> itemIdentifier = this.dbProvider.Connection.Query(sql, transaction: dbProvider.Transaction);

            List<TestQueueModel> queueList = new List<TestQueueModel>();
            foreach (var row in itemIdentifier)
            {
                queueList.Add(new TestQueueModel
                {
                    ItemKey = row.ItemKey,
                    StoreNo = storeNumber,
                    Identifier = row.Identifier,
                    EventTypeId = eventTypeId,
                    EventReferenceId = null,
                    InsertDate = DateTime.Now,
                    ProcessFailedDate = null,
                    InProcessBy = jobInstance
                });
            }

            sql =
            @"INSERT [mammoth].[ItemLocaleChangeQueue]
            (
                Item_Key, 
                Store_No,
                Identifier,
                EventTypeID,
                EventReferenceID,
                InsertDate,
                ProcessFailedDate,
                InProcessBy
            )
            VALUES
            (
                @ItemKey, 
                @StoreNo,
                @Identifier,
                @EventTypeId,
                @EventReferenceId,
                @InsertDate,
                @ProcessFailedDate,
                @InProcessBy
            )";

            this.dbProvider.Connection.Execute(@sql, queueList, dbProvider.Transaction);
        }

        private string GetSqlForExpectedData()
        {
            string sql = @" DECLARE @ExcludedStoreNo varchar(250);
                            SET @ExcludedStoreNo = (SELECT dbo.fn_GetAppConfigValue('LabAndClosedStoreNo','IRMA Client'));

                            SELECT
	                            q.QueueID					as QueueId,
                                q.EventTypeID				as EventTypeId,
                                @Region                     as Region,
	                            q.Identifier				as ScanCode,
	                            s.BusinessUnit_ID			as BusinessUnitId,
                                CASE
		                            WHEN p.AgeCode = 1 THEN 18
		                            WHEN p.AgeCode = 2 THEN 21
		                            ELSE NULL
	                            END							as AgeRestriction,
                                CASE 
                                    WHEN t.EventTypeName = 'ItemDelete' THEN 0
		                            ELSE si.Authorized
	                            END							as Authorized,
	                            p.IBM_Discount				as CaseDiscount,
	                            sa.UomRegulationChicagoBaby as ChicagoBaby,
	                            sa.ColorAdded				as ColorAdded,
	                            COALESCE(ivc.Origin_Name, co.Origin_Name)	as CountryOfProcessing,
	                            siv.DiscontinueItem			as Discontinued,
	                            p.ElectronicShelfTag		as ElectronicShelfTag,
	                            sa.Exclusive				as Exclusive,
	                            lt.LabelTypeDesc			as LabelTypeDescription,
	                            p.LinkedItem				as LinkedItem,
	                            p.LocalItem					as LocalItem,
	                            sa.Locality					as Locality,
                                ii.NumPluDigitsSentToScale  as NumberOfDigitsSentToScale,
	                            COALESCE(ovo.Origin_Name, oo.Origin_Name)	as Origin,
	                            i.Product_Code				as ProductCode,
	                            p.Restricted_Hours			as RestrictedHours,
	                            COALESCE(ovu.Unit_Name, iu.Unit_Name)		as RetailUnit,
                                COALESCE(soe.ExtraText,sce.ExtraText)		as ScaleExtraText,
	                            sa.SignRomanceTextLong		as SignRomanceLong,
	                            sa.SignRomanceTextShort		as SignRomanceShort,
	                            COALESCE(iov.Sign_Description, i.Sign_Description) as SignDescription,
	                            sa.UomRegulationTagUom		as TagUom,
	                            p.Discountable				as TmDiscount,
                                p.MSRPPrice                 as Msrp
                            FROM
	                            [mammoth].[ItemLocaleChangeQueue]	q
	                            INNER JOIN mammoth.ItemChangeEventType t on q.EventTypeID = t.EventTypeID
	                            INNER JOIN Item						i	on	q.Item_Key	= i.Item_Key
                                LEFT JOIN ItemIdentifier           ii  on  i.Item_Key  = ii.Item_Key
                                                                        AND q.Identifier = ii.Identifier
                                                                        AND ii.Deleted_Identifier = 0
                                LEFT JOIN Store						s	on	((q.Store_No = s.Store_No)
												                            OR (q.Store_No IS NULL))
	                            LEFT JOIN StoreItemVendor			siv	on	i.Item_Key	= siv.Item_Key
											                            AND s.Store_No = siv.Store_No
											                            AND siv.PrimaryVendor = 1
											                            AND siv.DeleteDate IS NULL
	                            LEFT JOIN Price                     p   on  i.Item_Key = p.Item_Key
                                                                        AND s.Store_No = p.Store_No
	                            LEFT JOIN StoreItem				    si	on	s.Store_No	= si.Store_No
											                            AND i.Item_Key	= si.Item_Key
	                            LEFT JOIN ItemSignAttribute			sa	on	i.Item_Key	= sa.Item_Key
	                            LEFT JOIN ItemOrigin				co	on	i.CountryProc_ID = co.Origin_ID                     -- country of processing
	                            LEFT JOIN ItemOrigin				oo	on	i.Origin_ID = oo.Origin_ID		                    -- origin
	                            LEFT JOIN LabelType					lt	on	i.LabelType_ID = lt.LabelType_ID
	                            LEFT JOIN ItemScale					sc	on	i.Item_Key	= sc.Item_Key
	                            LEFT JOIN Scale_ExtraText			sce	on	sc.Scale_ExtraText_ID = sce.Scale_ExtraText_ID      -- scale extra text
	                            LEFT JOIN ItemOverride				iov	on	i.Item_Key	= iov.Item_Key
											                            AND iov.StoreJurisdictionID = s.StoreJurisdictionID
	                            LEFT JOIN ItemOrigin				ivc	on	iov.CountryProc_ID = ivc.Origin_ID	                -- alternate country of processing
	                            LEFT JOIN ItemOrigin				ovo	on	iov.Origin_ID = ovo.Origin_ID		                -- alternate origin
	                            LEFT JOIN ItemScaleOverride			iso	on	i.Item_Key	= iso.Item_Key
											                            AND s.StoreJurisdictionID = iso.StoreJurisdictionID
	                            LEFT JOIN Scale_ExtraText			soe	on	iso.Scale_ExtraText_ID = soe.Scale_ExtraText_ID     -- alternate scale extra text
	                            LEFT JOIN ItemUnit					iu	on	i.Retail_Unit_ID	= iu.Unit_ID                    -- retail unit
	                            LEFT JOIN ItemUnit					ovu	on	iov.Retail_Unit_ID	= ovu.Unit_ID                   -- alternate retail unit
                            WHERE
	                            q.InProcessBy = @JobInstance
                                AND (s.WFM_Store = 1 OR s.Mega_Store = 1 )
                                AND (Internal = 1 AND BusinessUnit_ID IS NOT NULL)
                                AND s.Store_No NOT IN (SELECT Key_Value FROM dbo.fn_Parse_List(@ExcludedStoreNo, '|'))
                            ORDER BY
                                q.QueueID;";

            return sql;
        }

        private T GetFirstFromTable<T>(string alternateTableName = null)
        {
            var tableName = alternateTableName ?? typeof(T).Name;

            var result = dbProvider.Connection.Query<T>(
                "SELECT TOP 1 * FROM " + tableName,
                null,
                dbProvider.Transaction).First();

            return result;
        }

        private List<Store> GetValidStoreNumbers()
        {
            List<Store> validStores = this.dbProvider.Connection
                .Query<Store>(@" DECLARE @ExcludedStoreNo varchar(250);
                            SET @ExcludedStoreNo = (SELECT dbo.fn_GetAppConfigValue('LabAndClosedStoreNo','IRMA Client'));
                            SELECT * FROM Store 
                            WHERE 
                                Store_No NOT IN (SELECT Key_Value as BusinessUnitId FROM dbo.fn_Parse_List(@ExcludedStoreNo, '|'))
                                AND (WFM_Store = 1 OR Mega_Store = 1 )
                                AND (Internal = 1 AND BusinessUnit_ID IS NOT NULL);",
                            transaction: this.dbProvider.Transaction)
                .ToList();

            return validStores;
        }

        private List<Store> GetAllStores()
        {
            List<Store> validStores = this.dbProvider.Connection
                .Query<Store>(@"SELECT * FROM Store;", transaction: this.dbProvider.Transaction)
                .ToList();

            return validStores;
        }

        private List<StoreRegionMapping> GetStoreRegionMapping()
        {
            List<StoreRegionMapping> storeRegionMapping = this.dbProvider.Connection
                .Query<StoreRegionMapping>(@"SELECT * FROM StoreRegionMapping", null, this.dbProvider.Transaction)
                .ToList();
            return storeRegionMapping;
        }

        private string GetLinkedIdentifierByItemKey(int itemKey)
        {
            string sql = @"SELECT Identifier FROM ItemIdentifier WHERE Item_Key = @ItemKey AND Default_Identifier = 1";
            string identifier = this.dbProvider.Connection.Query<string>(sql, new { ItemKey = itemKey }, this.dbProvider.Transaction).First();
            return identifier;
        }

        private void UpdateLabAndClosedStoreValues(List<int> storeNumbers)
        {
            string key = "LabAndClosedStoreNo";
            string value = String.Join("|", storeNumbers.Select(n => n.ToString()).ToArray());

            int keyId = this.dbProvider.Connection.Query<int>("SELECT KeyID from AppConfigKey where Name = @KeyName", new { KeyName = key }, this.dbProvider.Transaction).First();
            bool updateExistingValue = true;
            int userId = this.dbProvider.Connection.Query<int>("SELECT user_id FROM Users WHERE UserName = 'System'", null, this.dbProvider.Transaction).First();
            string applicationName = "IRMA Client";
            Guid environmentId = this.dbProvider.Connection.Query<Guid>("SELECT EnvironmentID FROM AppConfigEnv WHERE Name = 'TEST'", null, this.dbProvider.Transaction).First();
            Guid applicationId = this.dbProvider.Connection
                .Query<Guid>(
                    "SELECT ApplicationID FROM AppConfigApp WHERE Name = @ApplicationName AND EnvironmentID = @EnvironmentID",
                    new { ApplicationName = applicationName, EnvironmentID = environmentId },
                    this.dbProvider.Transaction)
                .First();

            int affectedRows = this.dbProvider.Connection
                .Execute("AppConfig_AddKeyValue",
                    new
                    {
                        ApplicationID = applicationId.ToString(),
                        EnvironmentID = environmentId.ToString(),
                        UpdateExistingKeyValue = updateExistingValue,
                        KeyID = keyId,
                        Value = value,
                        User_ID = userId
                    },
                    this.dbProvider.Transaction,
                    null,
                    CommandType.StoredProcedure);
        }
        
        private int InsertNewItem(int subTeamNo, string signDescription, string productCode,
            int originID, int labelTypeID, int retailUnitID, bool removeItem = false, bool deletedItem = false)
        {
            // Insert New Item
            var itemKey = this.dbProvider.Insert(
                new IrmaQueryParams<Item, int>(
                    IrmaTestObjectFactory.BuildItem()
                        .With(x => x.SubTeam_No, subTeamNo)
                        .With(x => x.Sign_Description, signDescription)
                        .With(x => x.Product_Code, productCode)
                        .With(x => x.Origin_ID, originID)
                        .With(x => x.CountryProc_ID, originID)
                        .With(x => x.LabelType_ID, labelTypeID)
                        .With(x => x.Retail_Unit_ID, retailUnitID)
                        .With(x => x.Remove_Item, removeItem ? (byte)1 : (byte)0)
                        .With(x => x.Deleted_Item, deletedItem)
                        .ToObject(),
                x => x.Item_Key));
            return itemKey;
        }

        private void InsertItemSignAttributes(int itemKey, string chicagoBaby, bool? ColorAdd,
            string locality, string signRomanceLong, string signRomanceShort, int? tagUom, DateTime? exclusive)
        {
            // Insert Sign Attributes
            this.dbProvider.Insert(new IrmaQueryParams<ItemSignAttribute, int>(
                IrmaTestObjectFactory.Build<ItemSignAttribute>()
                    .With(x => x.Item_Key, itemKey)
                    .With(x => x.UomRegulationChicagoBaby, chicagoBaby)
                    .With(x => x.ColorAdded, ColorAdd)
                    .With(x => x.Locality, locality)
                    .With(x => x.SignRomanceTextLong, signRomanceLong)
                    .With(x => x.SignRomanceTextShort, signRomanceShort)
                    .With(x => x.UomRegulationTagUom, tagUom)
                    .With(x => x.Exclusive, exclusive)
                    .ToObject(),
                x => x.ItemSignAttributeID));
        }

        private void InsertStoreItem(int storeNo, int itemKey, bool authorized)
        {
            // Insert StoreItem
            this.dbProvider.Insert(new IrmaQueryParams<StoreItem, int>(
                IrmaTestObjectFactory.Build<StoreItem>()
                    .With(x => x.Store_No, storeNo)
                    .With(x => x.Item_Key, itemKey)
                    .With(x => x.Authorized, authorized)
                    .ToObject(),
                x => x.StoreItemAuthorizationID));
        }

        private int InsertStoreItemVendor(int vendorId, int storeNo, int itemKey,
            bool discontinued, bool primaryVendor = true, DateTime? lastCostRefreshedDate = null)
        {
            if (lastCostRefreshedDate == null) lastCostRefreshedDate = DateTime.Now;
            // Insert StoreItemVendor
            int storeItemVendorId = this.dbProvider.Insert(new IrmaQueryParams<StoreItemVendor, int>(
                IrmaTestObjectFactory.Build<StoreItemVendor>()
                    .With(x => x.Vendor_ID, vendorId)
                    .With(x => x.Store_No, storeNo)
                    .With(x => x.Item_Key, itemKey)
                    .With(x => x.DiscontinueItem, discontinued)
                    .With(x => x.PrimaryVendor, primaryVendor)
                    .With(x => x.LastCostRefreshedDate, lastCostRefreshedDate)
                    .ToObject(),
                x => x.StoreItemVendorID));

            return storeItemVendorId;
        }
        
        private void InsertNewItemIdentifier(int itemKey, string Identifier, int? numberOfDigitsSentToScale,
            bool defaultScanCode = true, bool removeIdentifier = false, bool deletedIdentifier = false)
        {
            // Insert New Item Identifier
            this.dbProvider.Insert(
                new IrmaQueryParams<ItemIdentifier, int>(
                    IrmaTestObjectFactory.BuildItemIdentifier()
                        .With(x => x.Item_Key, itemKey)
                        .With(x => x.Identifier, Identifier)
                        .With(x => x.NumPluDigitsSentToScale, numberOfDigitsSentToScale)
                        .With(x => x.Default_Identifier, defaultScanCode ? (byte)1 : (byte)0)
                        .With(x => x.Remove_Identifier, removeIdentifier ? (byte)1 : (byte)0)
                        .With(x => x.Deleted_Identifier, deletedIdentifier ? (byte)1 : (byte)0)
                        .ToObject(),
                    x => x.Identifier_ID));
        }

        private void InsertNewStore(int storeNo, int? businessUnitId, int? jurisdictionID = 1,
            bool? wfm_store = true, bool? internal_store = false, bool? mega_store = false, string storeName = "Test Store")
        {
            // Insert New Store
            this.dbProvider.Insert(IrmaTestObjectFactory.BuildStore()
                .With(x => x.Store_No, storeNo)
                .With(x => x.BusinessUnit_ID, businessUnitId)
                .With(x => x.StoreJurisdictionID, jurisdictionID)
                .With(x => x.WFM_Store, wfm_store)
                .With(x => x.Store_Name, storeName)
                .With(x => x.Mega_Store, mega_store)
                .With(x => x.Internal, internal_store)
                .ToObject());
        }

        private void InsertNewStoreRegionMapping(int storeNo, string region)
        {
            // Insert New Store Region Mapping
            this.dbProvider.Insert(IrmaTestObjectFactory.Build<StoreRegionMapping>()
            .With(x => x.Store_No, storeNo)
            .With(x => x.Region_Code, region)
            .ToObject());
        }

        private void InsertNewPrice(bool caseDiscount, bool? rlectronicShelfTag, bool localItem,
            bool restrictedHours, bool tmDiscount, decimal msrp, int storeNo, int itemKey,
            int? ageRestrictionId, int? linkedItemKey)
        {
            // Insert New Price
            this.dbProvider.Insert(new IrmaQueryParams<Price, int>(
                IrmaTestObjectFactory.Build<Price>()
                    .With(x => x.IBM_Discount, caseDiscount)
                    .With(x => x.ElectronicShelfTag, rlectronicShelfTag)
                    .With(x => x.LocalItem, localItem)
                    .With(x => x.Restricted_Hours, restrictedHours)
                    .With(x => x.Discountable, tmDiscount)
                    .With(x => x.MSRPPrice, msrp)
                    .With(x => x.Store_No, storeNo)
                    .With(x => x.Item_Key, itemKey)
                    .With(x => x.AgeCode, ageRestrictionId)
                    .With(x => x.LinkedItem, linkedItemKey)
                .ToObject(),
            null,
            new Dictionary<string, string> { { "Price1", "Price" } }));
        }

        private void InsertNewValidatedScanCode(string identifier, int inforItemId = 0)
        {
            // Insert New Validated Scan Code
            this.dbProvider.Insert(new IrmaQueryParams<ValidatedScanCode, int>(
                IrmaTestObjectFactory.BuildValidatedScanCode()
                    .With(x => x.ScanCode, identifier)
                    .With(x => x.InforItemId, inforItemId)
                    .ToObject(),
                x => x.Id));
        }

        private void InsertItemVendor(int itemKey, int vendorId, string vendorItemId = null)
        {
            // Insert Item Vendor
            this.dbProvider.Insert(
                IrmaTestObjectFactory.Build<ItemVendor>()
                    .With(x => x.Item_Key, itemKey)
                    .With(x => x.Vendor_ID, vendorId)
                    .With(x => x.Item_ID, vendorItemId)
                    .ToObject());
        }
    }
}