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
    public class GetItemLocaleEventsQueryTests_New
    {
        private GetItemLocaleDataQuery query;
        private GetItemLocaleDataParameters parameters;
        private SqlDbProvider dbProvider;
        private DbDataManagerForForItemLocaleEventTesting testDataManager;

        protected static string region
        {
            get
            {
                var regionForTesting = AppSettingsAccessor.GetStringSetting("RegionForTesting", false);
                return regionForTesting ?? "FL";
            }
        }
        protected static string irmaDatabaseForRegion
        {
            get
            {
                return $"ItemCatalog_{region}";
            }
        }

        protected static string irmaConnectionString
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

            testDataManager = new DbDataManagerForForItemLocaleEventTesting(dbProvider, region);
        }

        [TestCleanup]
        public void Cleanup()
        {
            dbProvider.Transaction.Rollback();
            dbProvider.Transaction.Dispose();
            dbProvider.Connection.Dispose();
        }

        public ItemLocaleEventQueryTestParameters BuildTestParameters(
            DbDataCollectionForItemLocaleEventTesting existingDbData,
            string identifier,
            bool isScaleItem,
            bool useAltJurisdiction = false,
            ItemLocaleEventQueryTestParameters_Price priceParamters = null,
            ItemLocaleEventQueryTestParameters_Scale scaleParameters = null,
            ItemLocaleEventQueryTestParameters_ItemOverride itemOverrideParams = null,
            ItemLocaleEventQueryTestParameters_Vendor vendorParameters = null)
        {
            var store = useAltJurisdiction
                ? existingDbData.StoreInAltJurisdictionIfAny
                : existingDbData.StoreInDefaultJurisdiction;

            ItemLocaleEventQueryTestParameters_Store expectedStoreData = null;

            if (store == null)
            {
                // no existing store in db, set parameters to create one later
                expectedStoreData = BuildStoreParameters(region, existingDbData.AltJurisdictionId);
            }
            else
            {
                // create expected store params from existing store
                expectedStoreData = new ItemLocaleEventQueryTestParameters_Store(region, store);
              }

            return BuildTestParametersSpecifyStore(
                expectedStoreData,
                identifier,
                isScaleItem,
                useAltJurisdiction,
                priceParamters,
                scaleParameters,
                itemOverrideParams,
                vendorParameters);
        }

        public ItemLocaleEventQueryTestParameters BuildTestParametersSpecifyStore(
            ItemLocaleEventQueryTestParameters_Store storeParams,
            string identifier,
            bool isScaleItem,
            bool useAltJurisdiction = false,
            ItemLocaleEventQueryTestParameters_Price priceParamters = null,
            ItemLocaleEventQueryTestParameters_Scale scaleParameters = null,
            ItemLocaleEventQueryTestParameters_ItemOverride itemOverrideParams = null,
            ItemLocaleEventQueryTestParameters_Vendor vendorParameters = null)
        {
            // default item values
            bool isDefaultScanCode = true;
            bool isCfsItem = false;
            int? numPluDigitsSentToScale = null;
            bool isRemovedItem = false;
            bool isDeletedItem = false;
            bool isRemovedIdentifier = false;
            bool isDeletedIdentifier = false;
            string signDescription = "Test Sign Description";
            string productCode = "TestProdCode";
            DateTime? exclusiveDate = null;
            string locality = "Test Locality";
            string signRomanceLong = "Test Sign Romance Long";
            string signRomanceShort = "Test Sign Romance Short";
            string chicagoBaby = "Test Baby";
            bool colorAdded = true;
            //// default vendor values
            //int vendorCaseSize = 1;
            //bool vendorIsPrimary = true;
            //bool isDiscontinued = false;
            //DateTime? vendorCostLastRefreshed = null;
            // default price values
            //decimal msrp = DefaultTestValuesForItemLocaleEvent.Msrp;
            decimal msrp = 5.99m;

            var expectedVendorStoreItemData = vendorParameters ??
                BuildVendorParameters();

            var expectedPriceData = priceParamters ??
                BuildPriceParameters(msrp);

            //var expectedError = expectedErrorData ??
            //    BuildExpectedErrors();

            var testParameters = new ItemLocaleEventQueryTestParameters(identifier)
            {
                IsValidated = true,
                Identifier = identifier,
                SignDescription = signDescription,
                ProductCode = productCode,
                IsDefaultScanCode = isDefaultScanCode,
                IsScaleIdentifier = isScaleItem,
                NumPluDigitsSentToScale = numPluDigitsSentToScale,
                IsCfsItem = isCfsItem,
                IsRemovedItem = isRemovedItem,
                IsDeletedItem = isDeletedItem,
                IsRemovedIdentifier = isRemovedIdentifier,
                IsDeletedIdentifier = isDeletedIdentifier,
                ExclusiveDate = exclusiveDate ?? DateTime.Now,
                Locality = locality,
                SignRomanceLong = signRomanceLong,
                SignRomanceShort = signRomanceShort,
                ChicagoBaby = chicagoBaby,
                ColorAdded = colorAdded,
                StoreParams = storeParams,
                VendorStoreItemParams = expectedVendorStoreItemData,
                PriceParams = expectedPriceData,
            };

            if (isScaleItem)
            {
                testParameters.ScaleParams = scaleParameters ?? BuildScaleParameters();
            }

            if (useAltJurisdiction && itemOverrideParams !=  null)
            {
                testParameters.ItemOverrideParams = itemOverrideParams;
            }

            return testParameters;
        }

        public ItemLocaleEventQueryTestParameters_Store BuildStoreParameters(
            string region,
            int? jurisdictionId,
            int storeNo = 834792,
            int businessUnit = 83472,
            string storeName = "Test Store",
            bool isMega = false,
            bool isWfm = true,
            bool isInternal = true)
        {
            return new ItemLocaleEventQueryTestParameters_Store(
                region,
                jurisdictionId,
                storeNo,
                businessUnit,
                storeName,
                isMega,
                isWfm,
                isInternal);
        }

        public ItemLocaleEventQueryTestParameters_Vendor BuildVendorParameters(
            int caseSize = 1,
            bool isPrimaryVendor = true,
            bool isDiscontinued = false,
            DateTime? lastCostRefreshed = null)
        {
            return new ItemLocaleEventQueryTestParameters_Vendor(
                caseSize,
                isPrimaryVendor,
                isDiscontinued,
                lastCostRefreshed ?? DateTime.Now);
        }

        public ItemLocaleEventQueryTestParameters_Price BuildPriceParameters(
            decimal msrp,
            bool hasCaseDiscount = false,
            bool hasElectronicShelfTag = false,
            bool isLocalItem = false,
            bool hasRestrictedHours = false,
            int? ageRestrictionValue = null,
            short priceMultiple = 1,
            bool teamDiscountEligible = true,
            bool isRetail = true)
        {
            return new ItemLocaleEventQueryTestParameters_Price(
                msrp,
                hasCaseDiscount,
                hasElectronicShelfTag,
                isLocalItem,
                hasRestrictedHours,
                ageRestrictionValue,
                priceMultiple,
                teamDiscountEligible,
                isRetail);
        }

        private ItemLocaleEventQueryTestParameters_Scale BuildScaleParameters(
            short? shelfLife = 3,
            string wrappedTareWeight = "9999",
            string unwrappedTareWeight = "9998",
            string eatByText = "7",
            bool forceTare = false)
        {
            return new ItemLocaleEventQueryTestParameters_Scale
            {
                WrappedTareWeight = wrappedTareWeight,
                UnwrappedTareWeight = unwrappedTareWeight,
                EatByText = eatByText,
                ForceTare = forceTare,
                ShelfLifeLength = shelfLife,
            };
        }

        private ItemLocaleEventQueryTestParameters_ItemOverride BuildItemOverrideParameters(
            int? jurisdictionId,
            string altRetailUomAbbrev = "KG",
            decimal altRetailSize = 1.1m,
            string altRetailUnit = "KILOGRAM",
            string altSignDescription = "Test Override Sign Description",
            string altOrigin = null,
            string altCountryOfProcessing = null)
        {
            return new ItemLocaleEventQueryTestParameters_ItemOverride
            {
                JurisdictionId = jurisdictionId,
                //AltSignRomanceLong = altSignRomanceLong,
                //AltSignRomanceShort = altSignRomanceShort,
                AltRetailSize = altRetailSize,
                AltRetailUomAbbrev = altRetailUomAbbrev,
                AltRetailUnitJurisdiction = altRetailUnit,
                AltSignDescription = altSignDescription,
                AltOriginName = altOrigin,
                AltCountryOfProcessingName = altCountryOfProcessing,
            };
        }

        [TestMethod]
        public void GetItemLocaleEvents_AddUpdate_NonScaleItem_WithPlainVanillaData_ShouldReturnAllExpectedData()
        {
            // Given 
            var expectedEventType = IrmaEventTypes.ItemLocaleAddOrUpdate;
            var expectedIdentifier = "22222242";
            // load some data from the db to assist with testing
            var testSupportData = testDataManager.LoadExistingDataForTest();
            // gather some typical "vanilla" item-locale values to use as test parameters
            var testParameters = BuildTestParameters(
                existingDbData: testSupportData,
                identifier: expectedIdentifier,
                isScaleItem: false);

            // insert item-locale data to match the test parameters
            var insertedItemKey = testDataManager.InsertTestData(testSupportData, testParameters.StoreParams.Store_No, testParameters);
            // put a message on the queue 
            var insertedQueueId = testDataManager.InsertToItemLocaleChangeQueue(
                insertedItemKey,
                testParameters.StoreParams.Store_No,
                expectedIdentifier,
                this.parameters.Instance,
                expectedEventType);
            // update expected data with inserted ids
            testParameters.SetExpectedDbValues_ForItemKeyAndQueueIdAndEventType(insertedItemKey, insertedQueueId, expectedEventType);

            //When
            var actual = query.Search(parameters).First();

            //Then
            AssertQueryResultsMatchExpected_All(testParameters, actual);
        }

        [TestMethod]
        public void GetItemLocaleEvents_AddUpdate_NonScaleItem_WithItemExtraText_ShouldReturnExtraText()
        {
            // Given 
            var expectedEventType = IrmaEventTypes.ItemLocaleAddOrUpdate;
            var expectedIdentifier = "22222242";
            var expectedScaleExtraText = "Test Item Nutrition Extra Text";
            // load some data from the db to assist with testing
            var testSupportData = testDataManager.LoadExistingDataForTest();
            // gather some typical "vanilla" item-locale values to use as test parameters
            var testParameters = BuildTestParameters(
                existingDbData: testSupportData,
                identifier: expectedIdentifier,
                isScaleItem: false);
            // add the "scale" extra text we are interested in for this test
            testParameters.ScaleExtraText = expectedScaleExtraText;

            // insert item-locale data to match the test parameters
            var insertedItemKey = testDataManager.InsertTestData(testSupportData, testParameters.StoreParams.Store_No, testParameters);
            // put a message on the queue 
            var insertedQueueId = testDataManager.InsertToItemLocaleChangeQueue(
                insertedItemKey,
                testParameters.StoreParams.Store_No,
                expectedIdentifier,
                this.parameters.Instance,
                expectedEventType);
            // update expected data with inserted ids
            testParameters.SetExpectedDbValues_ForItemKeyAndQueueIdAndEventType(insertedItemKey, insertedQueueId, expectedEventType);

            //When
            var actual = query.Search(parameters).First();

            //Then
            Assert.AreEqual(expectedScaleExtraText, actual.ScaleExtraText, UnequalMsg(nameof(actual.ScaleExtraText)));
            AssertQueryResultsMatchExpected_All(testParameters, actual);
        }

        [TestMethod]
        public void GetItemLocaleEvents_AddUpdate_NonScaleItem_WithItemExtraTextOverride_ShouldReturnExtraText()
        {
            // Given 
            var expectedEventType = IrmaEventTypes.ItemLocaleAddOrUpdate;
            var expectedIdentifier = "22222242";
            var expectedExtraText = "Test Item Nutrition Extra Text";
            var expectedExtraTextOverride = "OVERRIDE Test Item Nutrition Extra Text";
            // load some data from the db to assist with testing
            var testSupportData = testDataManager.LoadExistingDataForTest();
            // gather some typical "vanilla" item-locale values to use as test parameters
            var testParameters = BuildTestParameters(
                existingDbData: testSupportData,
                identifier: expectedIdentifier,
                isScaleItem: false,
                useAltJurisdiction: true);
            // add the "scale" extra text override we are interested in for this test
            testParameters.ScaleExtraText = expectedExtraText;
            testParameters.ScaleExtraTextOverride = expectedExtraTextOverride;

            // insert item-locale data to match the test parameters
            var insertedItemKey = testDataManager.InsertTestData(testSupportData, testParameters.StoreParams.Store_No, testParameters);
            // put a message on the queue 
            var insertedQueueId = testDataManager.InsertToItemLocaleChangeQueue(
                insertedItemKey,
                testParameters.StoreParams.Store_No,
                expectedIdentifier,
                this.parameters.Instance,
                expectedEventType);
            // update expected data with inserted ids
            testParameters.SetExpectedDbValues_ForItemKeyAndQueueIdAndEventType(insertedItemKey, insertedQueueId, expectedEventType);

            //When
            var actual = query.Search(parameters).First();

            //Then
            Assert.AreEqual(testParameters.RetailUnitName, actual.RetailUnit, UnequalMsg(nameof(actual.RetailUnit)));
            Assert.AreEqual(expectedExtraTextOverride, actual.ScaleExtraText, UnequalMsg(nameof(actual.ScaleExtraText)));
            AssertQueryResultsMatchExpected_All(testParameters, actual);
        }

        [TestMethod]
        public void GetItemLocaleEvents_AddUpdate_ScaleItem_WithScaleExtraText_ShouldReturnExtraText()
        {
            // Given 
            var expectedEventType = IrmaEventTypes.ItemLocaleAddOrUpdate;
            var expectedIdentifier = "22222242";
            var expectedScaleExtraText = "Test Scale Extra Text";
            // load some data from the db to assist with testing
            var testSupportData = testDataManager.LoadExistingDataForTest();
            // gather some typical "vanilla" item-locale values to use as test parameters
            var testParameters = BuildTestParameters(
                existingDbData: testSupportData,
                identifier: expectedIdentifier,
                isScaleItem: false,
                useAltJurisdiction: false);
            // add the scale extra text we are interested in for this test
            testParameters.ScaleExtraText = expectedScaleExtraText;

            // insert item-locale data to match the test parameters
            var insertedItemKey = testDataManager.InsertTestData(testSupportData, testParameters.StoreParams.Store_No, testParameters);
            // put a message on the queue 
            var insertedQueueId = testDataManager.InsertToItemLocaleChangeQueue(
                insertedItemKey,
                testParameters.StoreParams.Store_No,
                expectedIdentifier,
                this.parameters.Instance,
                expectedEventType);
            // update expected data with inserted ids
            testParameters.SetExpectedDbValues_ForItemKeyAndQueueIdAndEventType(insertedItemKey, insertedQueueId, expectedEventType);

            //When
            var actual = query.Search(parameters).First();

            //Then
            Assert.AreEqual(expectedScaleExtraText, actual.ScaleExtraText, UnequalMsg(nameof(actual.ScaleExtraText)));
            AssertQueryResultsMatchExpected_All(testParameters, actual);
        }

        [TestMethod]
        public void GetItemLocaleEvents_AddUpdate_ScaleItem_WithScaleExtraTextOverride_ShouldReturnExtraText()
        {
            // Given 
            var expectedEventType = IrmaEventTypes.ItemLocaleAddOrUpdate;
            var expectedScaleExtraText = "Test Scale Extra Text";
            var expectedScaleExtraTextOverride = "OVERRIDE Scale Extra Text";
            // load some data from the db to assist with testing
            var testSupportData = testDataManager.LoadExistingDataForTest();
            // set parameters
            var expectedIdentifier = "22222242";
            // gather some item-locale values including the specified overrides to use as test parameters
            var testParameters = BuildTestParameters(
                existingDbData: testSupportData,
                identifier: expectedIdentifier,
                isScaleItem: true,
                useAltJurisdiction: true);
            // add the scale extra text we are interested in for this test
            testParameters.ScaleExtraText = expectedScaleExtraText;
            testParameters.ScaleExtraTextOverride = expectedScaleExtraTextOverride;

            // insert item-locale data to match the test parameters
            var insertedItemKey = testDataManager.InsertTestData(testSupportData, testParameters.StoreParams.Store_No, testParameters);
            // put a message on the queue 
            var insertedQueueId = testDataManager.InsertToItemLocaleChangeQueue(
                insertedItemKey,
                testParameters.StoreParams.Store_No,
                expectedIdentifier,
                this.parameters.Instance,
                expectedEventType);
            // update expected data with inserted ids
            testParameters.SetExpectedDbValues_ForItemKeyAndQueueIdAndEventType(insertedItemKey, insertedQueueId, expectedEventType);
            
            //When
            var actual = query.Search(parameters).First();

            //Then
            Assert.AreEqual(expectedScaleExtraTextOverride, actual.ScaleExtraText, UnequalMsg(nameof(actual.ScaleExtraText)));
        }

        [TestMethod]
        public void GetItemLocaleEvents_AddUpdate_NonScaleItem_WithSpecifiedPrice_ShouldReturnExpectedPrice()
        {
            // Given 
            var expectedEventType = IrmaEventTypes.ItemLocaleAddOrUpdate;
            var expectedIdentifier = "22222242";
            var expectedMsrp = 5.99m;
            var expectedCaseDiscount = true;
            var expectedElectronicShelfTag = true;
            var expectedLocalItem = true;
            var expectedRestrictedHours = true;
            var expectedAgeRestriction = 21;
            var expectedPriceMultiple = (short)4;
            var expectedTmDiscount = true;
            var priceParams = BuildPriceParameters(
                expectedMsrp,
                expectedCaseDiscount,
                expectedElectronicShelfTag,
                expectedLocalItem,
                expectedRestrictedHours,
                expectedAgeRestriction,
                expectedPriceMultiple,
                expectedTmDiscount);
            // load some data from the db to assist with testing
            var testSupportData = testDataManager.LoadExistingDataForTest();
            // gather some typical "vanilla" item-locale values including the specified price data to use as test parameters
            var testParameters = BuildTestParameters(
                existingDbData: testSupportData,
                identifier: expectedIdentifier,
                isScaleItem: false,
                useAltJurisdiction: false,
                priceParamters: priceParams);

            // insert item-locale data to match the test parameters
            var insertedItemKey = testDataManager.InsertTestData(testSupportData, testParameters.StoreParams.Store_No, testParameters);
            // put a message on the queue 
            var insertedQueueId = testDataManager.InsertToItemLocaleChangeQueue(
                insertedItemKey,
                testParameters.StoreParams.Store_No,
                expectedIdentifier,
                this.parameters.Instance,
                expectedEventType);
            // update expected data with inserted ids
            testParameters.SetExpectedDbValues_ForItemKeyAndQueueIdAndEventType(insertedItemKey, insertedQueueId, expectedEventType);

            //When
            var actual = query.Search(parameters).First();

            //Then
            AssertQueryResultsMatchExpected_ForPrice(priceParams, actual);
        }

        [TestMethod]
        public void GetItemLocaleEvents_AddUpdate_NonScaleItem_WithSpecifiedPrice_ShouldReturnAllExpectedData()
        {
            // Given
            var expectedEventType = IrmaEventTypes.ItemLocaleAddOrUpdate;
            var expectedIdentifier = "22222242";
            var expectedMsrp = 5.99m;
            var expectedCaseDiscount = true;
            var expectedElectronicShelfTag = true;
            var expectedLocalItem = true;
            var expectedRestrictedHours = true;
            var expectedAgeRestriction = 21;
            var expectedPriceMultiple = (short)4;
            var expectedTmDiscount = true;
            var priceParams = BuildPriceParameters(
                expectedMsrp,
                expectedCaseDiscount,
                expectedElectronicShelfTag,
                expectedLocalItem,
                expectedRestrictedHours,
                expectedAgeRestriction,
                expectedPriceMultiple,
                expectedTmDiscount);
            // load some data from the db to assist with testing
            var testSupportData = testDataManager.LoadExistingDataForTest();
            // gather some typical "vanilla" item-locale values including the specified price data to use as test parameters
            var testParameters = BuildTestParameters(
                existingDbData: testSupportData,
                identifier: expectedIdentifier,
                isScaleItem: false,
                useAltJurisdiction: false,
                priceParamters: priceParams);

            // insert item-locale data to match the test parameters
            var insertedItemKey = testDataManager.InsertTestData(testSupportData, testParameters.StoreParams.Store_No, testParameters);
            // put a message on the queue 
            var insertedQueueId = testDataManager.InsertToItemLocaleChangeQueue(
                insertedItemKey,
                testParameters.StoreParams.Store_No,
                expectedIdentifier,
                this.parameters.Instance,
                expectedEventType);
            // update expected data with inserted ids
            testParameters.SetExpectedDbValues_ForItemKeyAndQueueIdAndEventType(insertedItemKey, insertedQueueId, expectedEventType);

            //When
            var actual = query.Search(parameters).First();

            //Then
            AssertQueryResultsMatchExpected_All(testParameters, actual);
        }

        [TestMethod]
        public void GetItemLocaleEvents_AddUpdate_ScaleItem_WithVanillaScaleData_ShouldReturnExpectedScaleData()
        {
            // Given 
            var expectedEventType = IrmaEventTypes.ItemLocaleAddOrUpdate;
            var expectedIdentifier = "22222242";
            // load some data from the db to assist with testing
            var testSupportData = testDataManager.LoadExistingDataForTest();
            // gather some typical "vanilla" item-locale values including scale attributes use as test parameters
            var testParameters = BuildTestParameters(
                existingDbData: testSupportData,
                identifier: expectedIdentifier,
                isScaleItem: true,
                useAltJurisdiction: false);

            // insert item-locale data to match the test parameters
            var insertedItemKey = testDataManager.InsertTestData(testSupportData, testParameters.StoreParams.Store_No, testParameters);
            // put a message on the queue 
            var insertedQueueId = testDataManager.InsertToItemLocaleChangeQueue(
                insertedItemKey,
                testParameters.StoreParams.Store_No,
                expectedIdentifier,
                this.parameters.Instance,
                expectedEventType);
            // update expected data with inserted ids
            testParameters.SetExpectedDbValues_ForItemKeyAndQueueIdAndEventType(insertedItemKey, insertedQueueId, expectedEventType);

            //When
            var actual = query.Search(parameters).First();

            //Then
            AssertQueryResultsMatchExpected_ForScaleData(testParameters.IsScaleItem, testParameters.ScaleParams, actual);
        }

        [TestMethod]
        public void GetItemLocaleEvents_AddUpdate_ScaleItem_WithVanillaScaleData_ShouldReturnAllExpectedData()
        {
            // Given 
            var expectedEventType = IrmaEventTypes.ItemLocaleAddOrUpdate;
            var expectedIdentifier = "22222242";
            // load some data from the db to assist with testing
            var testSupportData = testDataManager.LoadExistingDataForTest();
            // gather some typical "vanilla" item-locale values including scale attributes use as test parameters
            var testParameters = BuildTestParameters(
                existingDbData: testSupportData,
                identifier: expectedIdentifier,
                isScaleItem: true,
                useAltJurisdiction: false);

            // insert item-locale data to match the test parameters
            var insertedItemKey = testDataManager.InsertTestData(testSupportData, testParameters.StoreParams.Store_No, testParameters);
            // put a message on the queue 
            var insertedQueueId = testDataManager.InsertToItemLocaleChangeQueue(
                insertedItemKey,
                testParameters.StoreParams.Store_No,
                expectedIdentifier,
                this.parameters.Instance,
                expectedEventType);
            // update expected data with inserted ids
            testParameters.SetExpectedDbValues_ForItemKeyAndQueueIdAndEventType(insertedItemKey, insertedQueueId, expectedEventType);

            //When
            var actual = query.Search(parameters).First();

            //Then
            AssertQueryResultsMatchExpected_All(testParameters, actual);
        }

        [TestMethod]
        public void GetItemLocaleEvents_AddUpdate_ScaleItem_WithSpecifiedScaleData_ShouldReturnExpectedScaleData()
        {
            // Given
            var expectedEventType = IrmaEventTypes.ItemLocaleAddOrUpdate;
            var expectedIdentifier = "22222242";
            short expectedShelfLife = 13;
            bool expectedForceTare = true;
            string expectedWrappedTareWeight = "1.234";
            string expectedUnwrappedTareWeight = "0.987";
            string expectedUseBy = "42";
            var scaleParams = BuildScaleParameters(
                expectedShelfLife,
                expectedWrappedTareWeight,
                expectedUnwrappedTareWeight,
                expectedUseBy,
                expectedForceTare);
            // load some data from the db to assist with testing
            var testSupportData = testDataManager.LoadExistingDataForTest();
            // gather some values including specified scale attributes use as test parameters
            var testParameters = BuildTestParameters(
                existingDbData: testSupportData,
                identifier: expectedIdentifier,
                isScaleItem: true,
                useAltJurisdiction: false,
                priceParamters: null, // use default test price info
                scaleParameters: scaleParams);

            // insert item-locale data to match the test parameters
            var insertedItemKey = testDataManager.InsertTestData(testSupportData, testParameters.StoreParams.Store_No, testParameters);
            // put a message on the queue 
            var insertedQueueId = testDataManager.InsertToItemLocaleChangeQueue(
                insertedItemKey,
                testParameters.StoreParams.Store_No,
                expectedIdentifier,
                this.parameters.Instance,
                expectedEventType);
            // update expected data with inserted ids
            testParameters.SetExpectedDbValues_ForItemKeyAndQueueIdAndEventType(insertedItemKey, insertedQueueId, expectedEventType);

            //When
            var actual = query.Search(parameters).First();

            //Then
            AssertQueryResultsMatchExpected_ForScaleData(testParameters.IsScaleItem, scaleParams, actual);
        }

        [TestMethod]
        public void GetItemLocaleEvents_AddUpdate_ScaleItem_WithSpecifiedScaleData_ShouldReturnAllExpectedData()
        {
            // Given
            var expectedEventType = IrmaEventTypes.ItemLocaleAddOrUpdate;
            var expectedIdentifier = "22222242";
            short expectedShelfLife = 13;
            bool expectedForceTare = true;
            string expectedWrappedTareWeight = "1.234";
            string expectedUnwrappedTareWeight = "0.987";
            string expectedUseBy = "42";
            var scaleParams = BuildScaleParameters(
                expectedShelfLife,
                expectedWrappedTareWeight,
                expectedUnwrappedTareWeight,
                expectedUseBy,
                expectedForceTare);
            // load some data from the db to assist with testing
            var testSupportData = testDataManager.LoadExistingDataForTest();
            // gather some values including specified scale attributes use as test parameters
            var testParameters = BuildTestParameters(
                existingDbData: testSupportData,
                identifier: expectedIdentifier,
                isScaleItem: true,
                useAltJurisdiction: false,
                priceParamters: null, // use default test price info
                scaleParameters: scaleParams);

            // insert item-locale data to match the test parameters
            var insertedItemKey = testDataManager.InsertTestData(testSupportData, testParameters.StoreParams.Store_No, testParameters);
            // put a message on the queue 
            var insertedQueueId = testDataManager.InsertToItemLocaleChangeQueue(
                insertedItemKey,
                testParameters.StoreParams.Store_No,
                expectedIdentifier,
                this.parameters.Instance,
                expectedEventType);
            // update expected data with inserted ids
            testParameters.SetExpectedDbValues_ForItemKeyAndQueueIdAndEventType(insertedItemKey, insertedQueueId, expectedEventType);

            //When
            var actual = query.Search(parameters).First();
            AssertQueryResultsMatchExpected_All(testParameters, actual);
        }

        [TestMethod]
        public void GetItemLocaleEvents_AddUpdate_ScaleItem_WithSpecifiedScaleAndPriceData_ShouldReturnAllExpectedData()
        {
            // Given
            var expectedEventType = IrmaEventTypes.ItemLocaleAddOrUpdate;
            var expectedIdentifier = "22222242";
            short expectedShelfLife = 13;
            bool expectedForceTare = true;
            string expectedWrappedTareWeight = "1.234";
            string expectedUnwrappedTareWeight = "0.987";
            string expectedUseBy = "42";
            var expectedMsrp = 5.99m;
            var expectedCaseDiscount = true;
            var expectedElectronicShelfTag = true;
            var expectedLocalItem = true;
            var expectedRestrictedHours = true;
            var expectedAgeRestriction = 21;
            var expectedPriceMultiple = (short)4;
            var expectedTmDiscount = true;
            var scaleParams = BuildScaleParameters(
                expectedShelfLife,
                expectedWrappedTareWeight,
                expectedUnwrappedTareWeight,
                expectedUseBy,
                expectedForceTare);
            var priceParams = BuildPriceParameters(
                expectedMsrp,
                expectedCaseDiscount,
                expectedElectronicShelfTag,
                expectedLocalItem,
                expectedRestrictedHours,
                expectedAgeRestriction,
                expectedPriceMultiple,
                expectedTmDiscount);
            // load some data from the db to assist with testing
            var testSupportData = testDataManager.LoadExistingDataForTest();
            // gather some values including specified scale & price data use as test parameters
            var testParameters = BuildTestParameters(
                existingDbData: testSupportData,
                identifier: expectedIdentifier,
                isScaleItem: true,
                useAltJurisdiction: false,
                priceParamters: priceParams,
                scaleParameters: scaleParams);

            // insert item-locale data to match the test parameters
            var insertedItemKey = testDataManager.InsertTestData(testSupportData, testParameters.StoreParams.Store_No, testParameters);
            // put a message on the queue 
            var insertedQueueId = testDataManager.InsertToItemLocaleChangeQueue(
                insertedItemKey,
                testParameters.StoreParams.Store_No,
                expectedIdentifier,
                this.parameters.Instance,
                expectedEventType);
            // update expected data with inserted ids
            testParameters.SetExpectedDbValues_ForItemKeyAndQueueIdAndEventType(insertedItemKey, insertedQueueId, expectedEventType);

            //When
            var actual = query.Search(parameters).First();

            //Then
            AssertQueryResultsMatchExpected_All(testParameters, actual);
        }

        [TestMethod]
        public void GetItemLocaleEvents_AddUpdate_ScaleItem_WithExtendedProperty_ShouldReturnExpectedProperty()
        {
            // Given 
            var expectedEventType = IrmaEventTypes.ItemLocaleAddOrUpdate;
            var expectedIdentifier = "22222242";
            var expectedOrderedByInfor = true;
            // load some data from the db to assist with testing
            var testSupportData = testDataManager.LoadExistingDataForTest();
            // gather some typical "vanilla" item-locale values including scale attributes use as test parameters
            var testParameters = BuildTestParameters(
                existingDbData: testSupportData,
                identifier: expectedIdentifier,
                isScaleItem: true);
            // add store-item-extended property
            testParameters.IsOrderedByInfor = expectedOrderedByInfor;

            // insert item-locale data to match the test parameters
            var insertedItemKey = testDataManager.InsertTestData(testSupportData, testParameters.StoreParams.Store_No, testParameters);
            // put a message on the queue 
            var insertedQueueId = testDataManager.InsertToItemLocaleChangeQueue(
                insertedItemKey,
                testParameters.StoreParams.Store_No,
                expectedIdentifier,
                this.parameters.Instance,
                expectedEventType);
            // update expected data with inserted ids
            testParameters.SetExpectedDbValues_ForItemKeyAndQueueIdAndEventType(insertedItemKey, insertedQueueId, expectedEventType);

            //When
            var actual = query.Search(parameters).First();

            //Then 
            Assert.AreEqual(expectedOrderedByInfor, actual.OrderedByInfor, UnequalMsg(nameof(actual.OrderedByInfor)));
        }

        [TestMethod]
        public void GetItemLocaleEvents_AddUpdate_NonScaleItem_WithExtendedProperty_ShouldReturnAllExpectedData()
        {
            // Given
            var expectedEventType = IrmaEventTypes.ItemLocaleAddOrUpdate;
            var expectedIdentifier = "22222242";
            var expectedOrderedByInfor = true;
            // load some data from the db to assist with testing
            var testSupportData = testDataManager.LoadExistingDataForTest();
            // gather some typical "vanilla" item-locale values including scale attributes use as test parameters
            var testParameters = BuildTestParameters(
                existingDbData: testSupportData,
                identifier: expectedIdentifier,
                isScaleItem: false);
            // add store-item-extended property
            testParameters.IsOrderedByInfor = expectedOrderedByInfor;

            // insert item-locale data to match the test parameters
            var insertedItemKey = testDataManager.InsertTestData(testSupportData, testParameters.StoreParams.Store_No, testParameters);
            // put a message on the queue 
            var insertedQueueId = testDataManager.InsertToItemLocaleChangeQueue(
                insertedItemKey,
                testParameters.StoreParams.Store_No,
                expectedIdentifier,
                this.parameters.Instance,
                expectedEventType);
            // update expected data with inserted ids
            testParameters.SetExpectedDbValues_ForItemKeyAndQueueIdAndEventType(insertedItemKey, insertedQueueId, expectedEventType);

            //When
            var actual = query.Search(parameters).First();

            //Then
            AssertQueryResultsMatchExpected_All(testParameters, actual);
        }

        [TestMethod]
        public void GetItemLocaleEvents_AddUpdate_NonScaleItem_WithItemOverrides_ShouldReturnExpectedOverrides()
        {
            // Given
            var expectedEventType = IrmaEventTypes.ItemLocaleAddOrUpdate;
            // load some data from the db to assist with testing
            var testSupportData = testDataManager.LoadExistingDataForTest();
            // set parameters
            var expectedIdentifier = "22222242";
            var expectedAltRetailSize = 9.8m;
            var expectedAltRetailUOM = testSupportData.Unit_Kg_Abbrev;
            var itemOverrideParams = BuildItemOverrideParameters(
                testSupportData.AltJurisdictionId,
                expectedAltRetailUOM,
                expectedAltRetailSize);
            // gather some item-locale values including the specified overrides to use as test parameters
            var testParameters = BuildTestParameters(
                existingDbData: testSupportData,
                identifier: expectedIdentifier,
                isScaleItem: false,
                useAltJurisdiction: true,
                priceParamters: null, // use default test price values
                scaleParameters: null,
                itemOverrideParams: itemOverrideParams);

            // insert item-locale data to match the test parameters
            var insertedItemKey = testDataManager.InsertTestData(testSupportData, testParameters.StoreParams.Store_No, testParameters);
            // put a message on the queue 
            var insertedQueueId = testDataManager.InsertToItemLocaleChangeQueue(
                insertedItemKey,
                testParameters.StoreParams.Store_No,
                expectedIdentifier,
                this.parameters.Instance,
                expectedEventType);
            // update expected data with inserted ids
            testParameters.SetExpectedDbValues_ForItemKeyAndQueueIdAndEventType(insertedItemKey, insertedQueueId, expectedEventType);

            //When
            var actual = query.Search(parameters).First();

            //Then
            AssertQueryResultsMatchExpected_ForItemOverrides(testParameters, actual);
        }

        [TestMethod]
        public void GetItemLocaleEvents_AddUpdate_NonScaleItem_WithRetailUnitStoreOverride_ShouldReturnExpectedRetailUnit()
        {
            // Given
            var expectedEventType = IrmaEventTypes.ItemLocaleAddOrUpdate;
            // load some data from the db to assist with testing
            var testSupportData = testDataManager.LoadExistingDataForTest();
            // set parameters
            var expectedIdentifier = "22222242";
            var expectedStoreRetailUnitOverride = testSupportData.Unit_Oz_Name;
            // gather some item-locale values including the specified overrides to use as test parameters
            var testParameters = BuildTestParameters(
                existingDbData: testSupportData,
                identifier: expectedIdentifier,
                isScaleItem: false,
                useAltJurisdiction: true);
            testParameters.RetailUnitStoreOverride = expectedStoreRetailUnitOverride;

            // insert item-locale data to match the test parameters
            var insertedItemKey = testDataManager.InsertTestData(testSupportData, testParameters.StoreParams.Store_No, testParameters);
            // put a message on the queue 
            var insertedQueueId = testDataManager.InsertToItemLocaleChangeQueue(
                insertedItemKey,
                testParameters.StoreParams.Store_No,
                expectedIdentifier,
                this.parameters.Instance,
                expectedEventType);
            // update expected data with inserted ids
            testParameters.SetExpectedDbValues_ForItemKeyAndQueueIdAndEventType(insertedItemKey, insertedQueueId, expectedEventType);

            //When
            var actual = query.Search(parameters).First();

            //Then
            Assert.AreEqual(expectedStoreRetailUnitOverride, actual.RetailUnit, UnequalMsg(nameof(actual.RetailUnit)));
        }

        [TestMethod]
        public void GetItemLocaleEvents_AddUpdate_NonScaleItem_WithAllOverrides_ShouldReturnExpectedOverrides()
        {
            // Given 
            var expectedEventType = IrmaEventTypes.ItemLocaleAddOrUpdate;
            // load some data from the db to assist with testing
            var testSupportData = testDataManager.LoadExistingDataForTest();
            // set parameters
            var expectedIdentifier = "22222242";
            var expectedAltRetailUOM = testSupportData.Unit_Kg_Abbrev;
            var expectedAltSignDescription = "OVERRIDE Sign Description Test";
            var expectedAltOrigin = testSupportData.Origin_CAN_Name;
            var expectedAltCountryOfProcessing = testSupportData.Origin_FRA_Name;
            var expectedAltRetailUnit = testSupportData.Unit_Kg_Name;
            var expectedAltRetailSize = 9.8m;
            var expectedAltScaleExtraText = "OVERRIDE EXTRA TEXT";
            var expectedStoreRetailUnitOverride = testSupportData.Unit_Oz_Name;
            var itemOverrideParams = BuildItemOverrideParameters(
                testSupportData.AltJurisdictionId,
                expectedAltRetailUOM,
                expectedAltRetailSize,
                expectedAltRetailUnit,
                expectedAltSignDescription,
                expectedAltOrigin,
                expectedAltCountryOfProcessing);
            // gather some item-locale values including the specified overrides to use as test parameters
            var testParameters = BuildTestParameters(
                existingDbData: testSupportData,
                identifier: expectedIdentifier,
                isScaleItem: false,
                useAltJurisdiction: true,
                priceParamters: null, // use default test price values
                scaleParameters: null,
                itemOverrideParams: itemOverrideParams);
            testParameters.ScaleExtraTextOverride = expectedAltScaleExtraText;
            testParameters.RetailUnitStoreOverride = expectedStoreRetailUnitOverride;

            // insert item-locale data to match the test parameters
            var insertedItemKey = testDataManager.InsertTestData(testSupportData, testParameters.StoreParams.Store_No, testParameters);
            // put a message on the queue 
            var insertedQueueId = testDataManager.InsertToItemLocaleChangeQueue(
                insertedItemKey,
                testParameters.StoreParams.Store_No,
                expectedIdentifier,
                this.parameters.Instance,
                expectedEventType);
            // update expected data with inserted ids
            testParameters.SetExpectedDbValues_ForItemKeyAndQueueIdAndEventType(insertedItemKey, insertedQueueId, expectedEventType);

            //When
            var actual = query.Search(parameters).First();

            //Then
            AssertQueryResultsMatchExpected_ForItemOverrides(testParameters, actual);
            Assert.AreEqual(expectedAltScaleExtraText, actual.ScaleExtraText, UnequalMsg(nameof(actual.ScaleExtraText)));
            Assert.AreEqual(expectedStoreRetailUnitOverride, actual.RetailUnit, UnequalMsg(nameof(actual.RetailUnit)));
        }

        [TestMethod]
        public void GetItemLocaleEvents_AddUpdate_NonScaleItem_WithAllOverrides_ShouldReturnAllExpectedData()
        {
            // Given 
            var expectedEventType = IrmaEventTypes.ItemLocaleAddOrUpdate;
            // load some data from the db to assist with testing
            var testSupportData = testDataManager.LoadExistingDataForTest();
            // set parameters
            var expectedIdentifier = "22222242";
            var expectedAltRetailUOM = testSupportData.Unit_Kg_Abbrev;
            var expectedAltSignDescription = "OVERRIDE Sign Description Test";
            var expectedAltOrigin = testSupportData.Origin_CAN_Name;
            var expectedAltCountryOfProcessing = testSupportData.Origin_FRA_Name;
            var expectedAltRetailUnit = testSupportData.Unit_Kg_Name;
            var expectedAltRetailSize = 9.8m;
            var expectedAltScaleExtraText = "OVERRIDE EXTRA TEXT";
            var expectedStoreRetailUnitOverride = testSupportData.Unit_Oz_Name;
            var itemOverrideParams = BuildItemOverrideParameters(
                testSupportData.AltJurisdictionId,
                expectedAltRetailUOM,
                expectedAltRetailSize,
                expectedAltRetailUnit,
                expectedAltSignDescription,
                expectedAltOrigin,
                expectedAltCountryOfProcessing);
            // gather some item-locale values including the specified overrides to use as test parameters
            var testParameters = BuildTestParameters(
                existingDbData: testSupportData,
                identifier: expectedIdentifier,
                isScaleItem: false,
                useAltJurisdiction: true,
                priceParamters: null, // use default test price values
                scaleParameters: null,
                itemOverrideParams: itemOverrideParams);
            testParameters.ScaleExtraTextOverride = expectedAltScaleExtraText;
            testParameters.RetailUnitStoreOverride = expectedStoreRetailUnitOverride;

            // insert item-locale data to match the test parameters
            var insertedItemKey = testDataManager.InsertTestData(testSupportData, testParameters.StoreParams.Store_No, testParameters);
            // put a message on the queue 
            var insertedQueueId = testDataManager.InsertToItemLocaleChangeQueue(
                insertedItemKey,
                testParameters.StoreParams.Store_No,
                expectedIdentifier,
                this.parameters.Instance,
                expectedEventType);
            // update expected data with inserted ids
            testParameters.SetExpectedDbValues_ForItemKeyAndQueueIdAndEventType(insertedItemKey, insertedQueueId, expectedEventType);

            //When
            var actual = query.Search(parameters).First();

            //Then
            AssertQueryResultsMatchExpected_All(testParameters, actual);
        }

        [TestMethod]
        public void GetItemLocaleEvents_AddUpdate_ScaleItem_WithRetailUnitStoreOverride_ShouldReturnExpectedRetailUnit()
        {
            // Given 
            var expectedEventType = IrmaEventTypes.ItemLocaleAddOrUpdate;
            // load some data from the db to assist with testing
            var testSupportData = testDataManager.LoadExistingDataForTest();
            // set parameters
            var expectedIdentifier = "22222242";
            var expectedStoreRetailUnitOverride = testSupportData.Unit_Oz_Name;
            // gather some item-locale values including the specified overrides to use as test parameters
            var testParameters = BuildTestParameters(
                existingDbData: testSupportData,
                identifier: expectedIdentifier,
                isScaleItem: true,
                useAltJurisdiction: true);
            testParameters.RetailUnitStoreOverride = expectedStoreRetailUnitOverride;

            // insert item-locale data to match the test parameters
            var insertedItemKey = testDataManager.InsertTestData(testSupportData, testParameters.StoreParams.Store_No, testParameters);
            // put a message on the queue 
            var insertedQueueId = testDataManager.InsertToItemLocaleChangeQueue(
                insertedItemKey,
                testParameters.StoreParams.Store_No,
                expectedIdentifier,
                this.parameters.Instance,
                expectedEventType);
            // update expected data with inserted ids
            testParameters.SetExpectedDbValues_ForItemKeyAndQueueIdAndEventType(insertedItemKey, insertedQueueId, expectedEventType);

            //When
            var actual = query.Search(parameters).First();

            //Then
            Assert.AreEqual(expectedStoreRetailUnitOverride, actual.RetailUnit, UnequalMsg(nameof(actual.RetailUnit)));
        }

        [TestMethod]
        public void GetItemLocaleEvents_AddUpdate_ScaleItem_WithItemOverrides_ShouldReturnAllExpectedOverrides()
        {
            // Given 
            var expectedEventType = IrmaEventTypes.ItemLocaleAddOrUpdate;
            // load some data from the db to assist with testing
            var testSupportData = testDataManager.LoadExistingDataForTest();
            // set parameters
            var expectedIdentifier = "22222242";
            var expectedAltRetailSize = 9.8m;
            var expectedAltRetailUOM = testSupportData.Unit_Kg_Abbrev;
            var itemOverrideParams = BuildItemOverrideParameters(
                testSupportData.AltJurisdictionId,
                expectedAltRetailUOM,
                expectedAltRetailSize);
            // gather some item-locale values including the specified overrides to use as test parameters
            var testParameters = BuildTestParameters(
                existingDbData: testSupportData,
                identifier: expectedIdentifier,
                isScaleItem: true,
                useAltJurisdiction: true,
                priceParamters: null, // use default test price values
                scaleParameters: null,
                itemOverrideParams: itemOverrideParams);

            // insert item-locale data to match the test parameters
            var insertedItemKey = testDataManager.InsertTestData(testSupportData, testParameters.StoreParams.Store_No, testParameters);
            // put a message on the queue 
            var insertedQueueId = testDataManager.InsertToItemLocaleChangeQueue(
                insertedItemKey,
                testParameters.StoreParams.Store_No,
                expectedIdentifier,
                this.parameters.Instance,
                expectedEventType);
            // update expected data with inserted ids
            testParameters.SetExpectedDbValues_ForItemKeyAndQueueIdAndEventType(insertedItemKey, insertedQueueId, expectedEventType);

            //When
            var actual = query.Search(parameters).First();

            //Then
            AssertQueryResultsMatchExpected_ForItemOverrides(testParameters, actual);
        }

        [TestMethod]
        public void GetItemLocaleEvents_AddUpdate_ScaleItem_WithAllOverrides_ShouldReturnAllExpectedOverrides()
        {
            // Given 
            var expectedEventType = IrmaEventTypes.ItemLocaleAddOrUpdate;
            // load some data from the db to assist with testing
            var testSupportData = testDataManager.LoadExistingDataForTest();
            // set parameters
            var expectedIdentifier = "22222242";
            var expectedAltRetailUOM = testSupportData.Unit_Kg_Abbrev;
            var expectedAltSignDescription = "OVERRIDE Sign Description Test";
            var expectedAltOrigin = testSupportData.Origin_CAN_Name;
            var expectedAltCountryOfProcessing = testSupportData.Origin_FRA_Name;
            var expectedAltRetailUnit = testSupportData.Unit_Kg_Name;
            var expectedAltRetailSize = 9.8m;
            var expectedAltScaleExtraText = "OVERRIDE SCALE EXTRA TEXT";
            var expectedStoreRetailUnitOverride = testSupportData.Unit_Oz_Name;
            var itemOverrideParams = BuildItemOverrideParameters(
                testSupportData.AltJurisdictionId,
                expectedAltRetailUOM,
                expectedAltRetailSize,
                expectedAltRetailUnit,
                expectedAltSignDescription,
                expectedAltOrigin,
                expectedAltCountryOfProcessing);
            // gather some item-locale values including the specified overrides to use as test parameters
            var testParameters = BuildTestParameters(
                existingDbData: testSupportData,
                identifier: expectedIdentifier,
                isScaleItem: true,
                useAltJurisdiction: true,
                priceParamters: null, // use default test price values
                scaleParameters: null,
                itemOverrideParams: itemOverrideParams);
            testParameters.ScaleExtraTextOverride = expectedAltScaleExtraText;
            testParameters.RetailUnitStoreOverride = expectedStoreRetailUnitOverride;
            // insert item-locale data to match the test parameters
            var insertedItemKey = testDataManager.InsertTestData(testSupportData, testParameters.StoreParams.Store_No, testParameters);
            // put a message on the queue 
            var insertedQueueId = testDataManager.InsertToItemLocaleChangeQueue(
                insertedItemKey,
                testParameters.StoreParams.Store_No,
                expectedIdentifier,
                this.parameters.Instance,
                expectedEventType);
            // update expected data with inserted ids
            testParameters.SetExpectedDbValues_ForItemKeyAndQueueIdAndEventType(insertedItemKey, insertedQueueId, expectedEventType);

            //When
            var actual = query.Search(parameters).First();

            //Then
            AssertQueryResultsMatchExpected_ForItemOverrides(testParameters, actual);
        }

        [TestMethod]
        public void GetItemLocaleEvents_AddUpdate_ScaleItem_WithAllOverrides_ShouldReturnAllExpectedData()
        {
            // Given 
            var expectedEventType = IrmaEventTypes.ItemLocaleAddOrUpdate;
            // load some data from the db to assist with testing
            var testSupportData = testDataManager.LoadExistingDataForTest();
            // set parameters
            var expectedIdentifier = "22222242";
            var expectedAltRetailUOM = testSupportData.Unit_Kg_Abbrev;
            var expectedAltSignDescription = "OVERRIDE Sign Description Test";
            var expectedAltOrigin = testSupportData.Origin_CAN_Name;
            var expectedAltCountryOfProcessing = testSupportData.Origin_FRA_Name;
            var expectedAltRetailUnit = testSupportData.Unit_Kg_Name;
            var expectedAltRetailSize = 9.8m;
            var expectedAltScaleExtraText = "OVERRIDE SCALE EXTRA TEXT";
            var expectedStoreRetailUnitOverride = testSupportData.Unit_Oz_Name;
            var itemOverrideParams = BuildItemOverrideParameters(
                testSupportData.AltJurisdictionId,
                expectedAltRetailUOM,
                expectedAltRetailSize,
                expectedAltRetailUnit,
                expectedAltSignDescription,
                expectedAltOrigin,
                expectedAltCountryOfProcessing);
            // gather some item-locale values including the specified overrides to use as test parameters
            var testParameters = BuildTestParameters(
                existingDbData: testSupportData,
                identifier: expectedIdentifier,
                isScaleItem: true,
                useAltJurisdiction: true,
                priceParamters: null, // use default test price values
                scaleParameters: null,
                itemOverrideParams: itemOverrideParams);
            testParameters.ScaleExtraTextOverride = expectedAltScaleExtraText;
            testParameters.RetailUnitStoreOverride = expectedStoreRetailUnitOverride;
            // insert item-locale data to match the test parameters
            var insertedItemKey = testDataManager.InsertTestData(testSupportData, testParameters.StoreParams.Store_No, testParameters);
            // put a message on the queue 
            var insertedQueueId = testDataManager.InsertToItemLocaleChangeQueue(
                insertedItemKey,
                testParameters.StoreParams.Store_No,
                expectedIdentifier,
                this.parameters.Instance,
                expectedEventType);
            // update expected data with inserted ids
            testParameters.SetExpectedDbValues_ForItemKeyAndQueueIdAndEventType(insertedItemKey, insertedQueueId, expectedEventType);

            //When
            var actual = query.Search(parameters).First();

            //Then
            AssertQueryResultsMatchExpected_All(testParameters, actual);
        }

        [TestMethod]
        public void GetItemLocaleEvents_WhenNoEventsForProcessInstance_ShouldReturnEmptyList()
        {
            //When
            var results = query.Search(parameters);

            //Then
            Assert.IsFalse(results.Any());
        }

        [TestMethod]
        public void GetItemLocaleEvents_Deauthorize_SingleStore_ShouldReturnOneRowAndUnAuthorized()
        {
            // Given
            var expectedEventType = IrmaEventTypes.ItemDeauthorization;
            var expectedIdentifier = "1234567";
            var expectedScaleExtraText = "Test Scale Extra Text";
            // load some data from the db to assist with testing
            var testSupportData = testDataManager.LoadExistingDataForTest();
            var expectedRows = 1;
            var expectedStore = new ItemLocaleEventQueryTestParameters_Store(region, testSupportData.StoreInDefaultJurisdiction);
            // gather some typical "vanilla" item-locale values including scale attributes use as test parameters
            var testParameters = BuildTestParametersSpecifyStore(
                storeParams: expectedStore,
                identifier: expectedIdentifier,
                isScaleItem: true,
                useAltJurisdiction: false);
            testParameters.ScaleExtraText = expectedScaleExtraText;
            testParameters.ExpectAuthorizedItem = false;

            // insert item-locale data to match the test parameters
            var insertedItemKey = testDataManager.InsertTestData(testSupportData, expectedStore.Store_No, testParameters);
            // put a message on the queue 
            var insertedQueueId = testDataManager.InsertToItemLocaleChangeQueue(
                insertedItemKey,
                null,
                expectedIdentifier,
                this.parameters.Instance,
                expectedEventType);
            // update expected data with inserted ids
            testParameters.SetExpectedDbValues_ForItemKeyAndQueueIdAndEventType(insertedItemKey, insertedQueueId, expectedEventType);

            // When
            var actualRowSet = query.Search(parameters).OrderBy(r => r.BusinessUnitId).ToList();
            // Then
            Assert.AreEqual(expectedRows, actualRowSet.Count, "The number of actual rows did not match expected number of rows.");

            Assert.AreEqual(false, actualRowSet[0].Authorized, "The expected AuthorizedForSale did not match the actual.");

            AssertQueryResultsMatchExpected_ForStore(
                testParameters.StoreParams.Region,
                testParameters.StoreParams.Store_No,
                testParameters.StoreParams.BusinessUnit,
                actualRowSet[0]);

            AssertQueryResultsMatchExpected_AllExceptStore(testParameters, actualRowSet[0]);
        }

        [TestMethod]
        public void GetItemLocaleEvents_Deauthorize_WithNullStoreNumber_ShouldReturnOneRowForEachStoreAndUnAuthorized()
        {
            // Given
            var expectedEventType = IrmaEventTypes.ItemDeauthorization;
            var expectedIdentifier = "12345678";
            var expectedScaleExtraText = "Test Scale Extra Text";
            // load some data from the db to assist with testing
            var testSupportData = testDataManager.LoadExistingDataForTest();
            var expectedStores = testSupportData.ValidStores;
            var expectedStoreNumbers = testSupportData.ValidStoreNumbers;
            var expectedRowCount = expectedStoreNumbers.Count;
            // gather some typical "vanilla" item-locale values including scale attributes use as test parameters
            var testParameters = BuildTestParameters(
                existingDbData: testSupportData,
                identifier: expectedIdentifier,
                isScaleItem: true,
                useAltJurisdiction: false);
            testParameters.ScaleExtraText = expectedScaleExtraText;
            testParameters.ExpectAuthorizedItem = false;

            // insert item-locale data to match the test parameters
            var insertedItemKey = testDataManager.InsertTestDataAndSetExpectedDbValues(testSupportData, expectedStoreNumbers, testParameters);
            // put a message on the queue 
            var insertedQueueId = testDataManager.InsertToItemLocaleChangeQueue(
                insertedItemKey,
                null,
                expectedIdentifier,
                this.parameters.Instance,
                expectedEventType);
            // update expected data with inserted ids
            testParameters.SetExpectedDbValues_ForItemKeyAndQueueIdAndEventType(insertedItemKey, insertedQueueId, expectedEventType);

            // When
            var actualRowSet = query.Search(parameters).OrderBy(r => r.BusinessUnitId).ToList();

            // Then
            Assert.AreEqual(expectedRowCount, actualRowSet.Count, "The number of actual rows did not match expected number of rows.");

            expectedStores.OrderBy(s => s.BusinessUnit_ID).Select((s, i) => new { Index = i, Store = s }).ToList().ForEach(s =>
            {
                Assert.AreEqual(false, actualRowSet[s.Index].Authorized, "The expected AuthorizedForSale did not match the actual.");

                AssertQueryResultsMatchExpected_ForStore(
                    region,
                    s.Store.Store_No,
                    s.Store.BusinessUnit_ID.Value,
                    actualRowSet[s.Index]);

                AssertQueryResultsMatchExpected_AllExceptStore(testParameters, actualRowSet[s.Index]);
            });
        }

        [TestMethod]
        public void GetItemLocaleEvents_AddUpdate_WithNullStoreNumber_ReturnsOneRowForEachStoreAssociatedToTheItem()
        {
            // Given
            var expectedEventType = IrmaEventTypes.ItemLocaleAddOrUpdate;
            var expectedIdentifier = "12345678";
            var expectedScaleExtraText = "Test Scale Extra Text";
            // load some data from the db to assist with testing
            var testSupportData = testDataManager.LoadExistingDataForTest();
            var expectedStores = testSupportData.ValidStores;
            var expectedStoreNumbers = testSupportData.ValidStoreNumbers;
            var expectedRowCount = expectedStoreNumbers.Count;
            // gather some typical "vanilla" item-locale values including scale attributes use as test parameters
            var testParameters = BuildTestParameters(
                existingDbData: testSupportData,
                identifier: expectedIdentifier,
                isScaleItem: true,
                useAltJurisdiction: false);
            testParameters.ScaleExtraText = expectedScaleExtraText;

            // insert item-locale data to match the test parameters
            var insertedItemKey = testDataManager.InsertTestDataAndSetExpectedDbValues(testSupportData, expectedStoreNumbers, testParameters);
            // put a message on the queue 
            var insertedQueueId = testDataManager.InsertToItemLocaleChangeQueue(
                insertedItemKey,
                null,
                expectedIdentifier,
                this.parameters.Instance,
                expectedEventType);
            // update expected data with inserted ids
            testParameters.SetExpectedDbValues_ForItemKeyAndQueueIdAndEventType(insertedItemKey, insertedQueueId, expectedEventType);
            // When
            var actualRowSet = query.Search(parameters).OrderBy(r => r.BusinessUnitId).ToList();

            // Then
            Assert.AreEqual(expectedRowCount, actualRowSet.Count, "The number of actual rows did not match expected number of rows.");

            expectedStores.OrderBy(s => s.BusinessUnit_ID).Select((s, i) => new { Index = i, Store = s }).ToList().ForEach(s =>
            {
                AssertQueryResultsMatchExpected_ForStore(
                    region,
                    s.Store.Store_No,
                    s.Store.BusinessUnit_ID.Value,
                    actualRowSet[s.Index]);

                AssertQueryResultsMatchExpected_AllExceptStore(testParameters, actualRowSet[s.Index]);
            });
        }

        [TestMethod]
        public void GetItemLocaleEvents_AddUpdate_WhenTwoEvents_OneValidStoreAndOneNullStore_ReturnsRowForEachStoreAssociatedToEachItem()
        {
            // Given 
            var expectedEventType = IrmaEventTypes.ItemLocaleAddOrUpdate;
            var expectedIdentifier1 = "12345678";
            var expectedIdentifier2 = "123456789";
            // load some data from the db to assist with testing
            var testSupportData = testDataManager.LoadExistingDataForTest();
            var expectedStores = testSupportData.ValidStores;
            var expectedStoreNumbers = testSupportData.ValidStoreNumbers;
            var expectedRowCount = expectedStoreNumbers.Count + 1;
            // gather some typical "vanilla" item-locale values including scale attributes use as test parameters
            var testParameters1 = BuildTestParameters(
                existingDbData: testSupportData,
                identifier: expectedIdentifier1,
                isScaleItem: true,
                useAltJurisdiction: false);
            var storeForSecondItem = expectedStores.OrderByDescending(s => s.BusinessUnit_ID).First();
            var storeParamsForSecondItem = new ItemLocaleEventQueryTestParameters_Store(region, storeForSecondItem);
            var testParameters2 = BuildTestParametersSpecifyStore(
                storeParams: storeParamsForSecondItem,
                identifier: expectedIdentifier2,
                isScaleItem: true,
                useAltJurisdiction: false);
            testParameters2.SignDescription = testParameters1.SignDescription + "2";
            testParameters2.ProductCode = testParameters1.ProductCode + "2";

            // insert item-locale data to match the test parameters
            var insertedItemKey1 = testDataManager.InsertTestDataAndSetExpectedDbValues(testSupportData, expectedStoreNumbers, testParameters1);
            var insertedItemKey2 = testDataManager.InsertTestData(testSupportData, testParameters2.StoreParams.Store_No, testParameters2);
            // put a message on the queue for the first item for all stores 
            var insertedQueueId1 = testDataManager.InsertToItemLocaleChangeQueue(
                insertedItemKey1,
                null,
                expectedIdentifier1,
                this.parameters.Instance,
                expectedEventType);
            // update expected data with inserted ids
            testParameters1.SetExpectedDbValues_ForItemKeyAndQueueIdAndEventType(insertedItemKey1, insertedQueueId1, expectedEventType);
            // put another message on the queue for the seconds item for a single store
            var insertedQueueId2 = testDataManager.InsertToItemLocaleChangeQueue(
                insertedItemKey2,
                testParameters2.StoreParams.Store_No,
                expectedIdentifier2,
                this.parameters.Instance,
                expectedEventType);
            // update expected data with inserted ids
            testParameters2.SetExpectedDbValues_ForItemKeyAndQueueIdAndEventType(insertedItemKey2, insertedQueueId2, expectedEventType);

            //When
            var actualRowSet = query.Search(parameters)
                .OrderBy(q => q.ScanCode)
                .ThenBy(r => r.BusinessUnitId)
                .ToList();

            //Then
            Assert.AreEqual(expectedRowCount, actualRowSet.Count, "The number of actual rows did not match expected number of rows.");
            // check each row for the null (all stores) event
            expectedStores.OrderBy(s => s.BusinessUnit_ID).Select((s, i) => new { Index = i, Store = s }).ToList().ForEach(s =>
            {
                AssertQueryResultsMatchExpected_ForStore(
                    region,
                    s.Store.Store_No,
                    s.Store.BusinessUnit_ID.Value,
                    actualRowSet[s.Index]);

                AssertQueryResultsMatchExpected_AllExceptStore(testParameters1, actualRowSet[s.Index]);
            });
            // check the result for the second (single store) event
            AssertQueryResultsMatchExpected_All(testParameters2, actualRowSet[expectedStores.Count]);
        }

        private void AssertQueryResultsMatchExpected_ForPrice(ItemLocaleEventQueryTestParameters_Price expected, ItemLocaleEventModel actual)
        {
            Assert.AreEqual(Convert.ToDouble(expected.Msrp), actual.Msrp, UnequalMsg(nameof(actual.Msrp)));
            Assert.AreEqual(expected.HasElectronicShelfTag, actual.ElectronicShelfTag, UnequalMsg(nameof(actual.ElectronicShelfTag)));
            Assert.AreEqual(expected.AgeRestrictionValue, actual.AgeRestriction, UnequalMsg(nameof(actual.AgeRestriction)));
            Assert.AreEqual(expected.HasCaseDiscount, actual.CaseDiscount, UnequalMsg(nameof(actual.CaseDiscount)));
            Assert.AreEqual(expected.IsLocalItem, actual.LocalItem, UnequalMsg(nameof(actual.LocalItem)));
            Assert.AreEqual(expected.HasRestrictedHours, actual.RestrictedHours, UnequalMsg(nameof(actual.RestrictedHours)));
            Assert.AreEqual(expected.TeamDiscountEligible, actual.TmDiscount, UnequalMsg(nameof(actual.TmDiscount)));
        }

        private void AssertQueryResultsMatchExpected_ForScaleData(bool expectedToBeScaleItem, ItemLocaleEventQueryTestParameters_Scale expected, ItemLocaleEventModel actual)
        {
            if (expectedToBeScaleItem)
            {
                Assert.AreEqual(expected.ForceTare, actual.ForceTare, UnequalMsg(nameof(actual.ForceTare)));
                Assert.AreEqual(expected.WrappedTareWeight, actual.WrappedTareWeight, UnequalMsg(nameof(actual.WrappedTareWeight)));
                Assert.AreEqual(expected.UnwrappedTareWeight, actual.UnwrappedTareWeight, UnequalMsg(nameof(actual.UnwrappedTareWeight)));
                Assert.AreEqual(expected.EatByText, actual.UseBy, UnequalMsg(nameof(actual.UseBy)));
                Assert.AreEqual(expected.ShelfLifeLength, actual.ShelfLife, UnequalMsg(nameof(actual.ShelfLife)));
            }
            else
            {
                Assert.IsNull(actual.ForceTare, ExpectedNullMsg(nameof(actual.ForceTare), actual.ForceTare));
                Assert.IsNull(actual.WrappedTareWeight, ExpectedNullMsg(nameof(actual.WrappedTareWeight), actual.WrappedTareWeight));
                Assert.IsNull(actual.UnwrappedTareWeight, ExpectedNullMsg(nameof(actual.UnwrappedTareWeight), actual.UnwrappedTareWeight));
                Assert.IsNull(actual.UseBy, ExpectedNullMsg(nameof(actual.UseBy), actual.UseBy));
                Assert.IsNull(actual.ShelfLife, ExpectedNullMsg(nameof(actual.ShelfLife), actual.ShelfLife));
            }
        }

        private void AssertQueryResultsMatchExpected_ForItemOverrides(ItemLocaleEventQueryTestParameters expected, ItemLocaleEventModel actual)
        {
            if (!string.IsNullOrWhiteSpace(expected.ItemOverrideParams.AltRetailUomAbbrev))
            {
                Assert.AreEqual(expected.ItemOverrideParams.AltRetailUomAbbrev, actual.AltRetailUOM, UnequalMsg(nameof(actual.AltRetailUOM)));
            }
            else
            {
                Assert.IsNull(actual.AltRetailUOM, ExpectedNullMsg(nameof(actual.AltRetailUOM), actual.AltRetailUOM));
            }

            if (expected.ItemOverrideParams.AltRetailSize.HasValue)
            {
                Assert.AreEqual(expected.ItemOverrideParams.AltRetailSize, actual.AltRetailSize, UnequalMsg(nameof(actual.AltRetailSize)));
            }
            else
            {
                Assert.IsNull(actual.AltRetailUOM, ExpectedNullMsg(nameof(actual.AltRetailSize), actual.AltRetailSize));
            }

            if (!string.IsNullOrWhiteSpace(expected.ItemOverrideParams.AltSignDescription))
            {

                Assert.AreEqual(expected.ItemOverrideParams.AltSignDescription, actual.SignDescription, UnequalMsg(nameof(actual.SignDescription)));
            }
            else
            {
                Assert.AreEqual(expected.SignDescription, actual.SignDescription, UnequalMsg(nameof(actual.SignDescription)));
            }


            if (!string.IsNullOrWhiteSpace(expected.ItemOverrideParams.AltOriginName))
            {
                Assert.AreEqual(expected.ItemOverrideParams.AltOriginName, actual.Origin, UnequalMsg(nameof(actual.Origin)));
            }
            else
            {
                Assert.AreEqual(expected.OriginName, actual.Origin, UnequalMsg(nameof(actual.Origin)));
            }

            if (!string.IsNullOrWhiteSpace(expected.ItemOverrideParams.AltCountryOfProcessingName))
            {
                Assert.AreEqual(expected.ItemOverrideParams.AltCountryOfProcessingName, actual.CountryOfProcessing, UnequalMsg(nameof(actual.CountryOfProcessing)));
            }
            else
            {
                Assert.AreEqual(expected.CountryOfProcessingName, actual.CountryOfProcessing, UnequalMsg(nameof(actual.CountryOfProcessing)));
            }

            if (!string.IsNullOrWhiteSpace(expected.ItemOverrideParams.AltRetailUnitJurisdiction)
                && string.IsNullOrWhiteSpace(expected.RetailUnitStoreOverride))
            {
                Assert.AreEqual(expected.ItemOverrideParams.AltRetailUnitJurisdiction, actual.RetailUnit, UnequalMsg(nameof(actual.RetailUnit)));
            }
        }


        private void AssertQueryResultsMatchExpected_AllExceptStore(ItemLocaleEventQueryTestParameters expected, ItemLocaleEventModel actual)
        {
            Assert.AreEqual(expected.InsertedEventTypeId, actual.EventTypeId, UnequalMsg(nameof(actual.EventTypeId)));
            Assert.AreEqual(expected.InsertedQueueId, actual.QueueId, UnequalMsg(nameof(actual.QueueId)));

            Assert.AreEqual(expected.Identifier, actual.ScanCode, UnequalMsg(nameof(actual.ScanCode)));
            Assert.AreEqual(expected.LabelTypeDesc, actual.LabelTypeDescription, UnequalMsg(nameof(actual.LabelTypeDescription)));
            Assert.AreEqual(expected.ExpectAuthorizedItem, actual.Authorized, UnequalMsg(nameof(actual.Authorized)));
            Assert.AreEqual(expected.TagUom.ToString(), actual.TagUom, UnequalMsg(nameof(actual.TagUom)));
            Assert.AreEqual(expected.IsScaleIdentifier, actual.ScaleItem, UnequalMsg(nameof(actual.ScaleItem)));
            if (expected.IsCfsItem)
            {
                Assert.AreEqual(expected, actual.SendtoCFS, UnequalMsg(nameof(actual.SendtoCFS)));
            }
            else
            {
                Assert.IsNull(actual.SendtoCFS, ExpectedNullMsg(nameof(actual.SendtoCFS), actual.SendtoCFS));
            }
            Assert.AreEqual(expected.Locality, actual.Locality, UnequalMsg(nameof(actual.Locality)));
            Assert.AreEqual(expected.IsDefaultScanCode, actual.DefaultScanCode, UnequalMsg(nameof(actual.DefaultScanCode)));
            Assert.AreEqual(expected.ChicagoBaby, actual.ChicagoBaby, UnequalMsg(nameof(actual.ChicagoBaby)));
            Assert.AreEqual(expected.ColorAdded, actual.ColorAdded, UnequalMsg(nameof(actual.ColorAdded)));
            Assert.AreEqual(expected.ProductCode, actual.ProductCode, UnequalMsg(nameof(actual.ProductCode)));
            Assert.AreEqual(expected.SignRomanceLong, actual.SignRomanceLong, UnequalMsg(nameof(actual.SignRomanceLong)));
            Assert.AreEqual(expected.SignRomanceShort, actual.SignRomanceShort, UnequalMsg(nameof(actual.SignRomanceShort)));
            if (expected.ExclusiveDate.HasValue)
            {
                AssertTimesCloseEnough(expected.ExclusiveDate.Value, actual.Exclusive.Value, nameof(actual.Exclusive));
            }
            else
            {
                Assert.IsNull(actual.Exclusive, ExpectedNullMsg(nameof(actual.Exclusive), actual.Exclusive));
            }
            Assert.AreEqual(expected.LinkedItemIdentifier, actual.LinkedItem, UnequalMsg(nameof(actual.LinkedItem)));
            Assert.AreEqual(expected.ExpectedNumberOfScaleDigits, actual.NumberOfDigitsSentToScale, UnequalMsg(nameof(actual.NumberOfDigitsSentToScale)));
            Assert.AreEqual(expected.IsOrderedByInfor, actual.OrderedByInfor, UnequalMsg(nameof(actual.OrderedByInfor)));
            Assert.AreEqual(expected.IrmaItemKey, actual.IrmaItemKey, UnequalMsg(nameof(actual.IrmaItemKey)));

            Assert.AreEqual(expected.VendorStoreItemParams.VendorItemId, actual.VendorItemId, UnequalMsg(nameof(actual.VendorItemId)));
            Assert.AreEqual(expected.VendorStoreItemParams.VendorCaseSize, actual.VendorCaseSize, UnequalMsg(nameof(actual.VendorCaseSize)));
            Assert.AreEqual(expected.VendorStoreItemParams.VendorKey, actual.VendorKey, UnequalMsg(nameof(actual.VendorKey)));
            Assert.AreEqual(expected.VendorStoreItemParams.VendorCompanyName, actual.VendorCompanyName, UnequalMsg(nameof(actual.VendorCompanyName)));
            Assert.AreEqual(expected.VendorStoreItemParams.IsDiscontinued, actual.Discontinued, UnequalMsg(nameof(actual.Discontinued)));

            AssertQueryResultsMatchExpected_ForPrice(expected.PriceParams, actual);

            AssertQueryResultsMatchExpected_ForScaleData(expected.IsScaleItem, expected.ScaleParams, actual);

            if (expected.HasItemOverrides)
            {
                AssertQueryResultsMatchExpected_ForItemOverrides(expected, actual);
            }
            else
            {
                Assert.IsNull(actual.AltRetailUOM, ExpectedNullMsg(nameof(actual.AltRetailUOM), actual.AltRetailUOM));
                Assert.IsNull(actual.AltRetailSize, ExpectedNullMsg(nameof(actual.AltRetailSize), actual.AltRetailSize));
                Assert.AreEqual(expected.SignDescription, actual.SignDescription, UnequalMsg(nameof(actual.SignDescription)));
                Assert.AreEqual(expected.OriginName, actual.Origin, UnequalMsg(nameof(actual.Origin)));
                Assert.AreEqual(expected.CountryOfProcessingName, actual.CountryOfProcessing, UnequalMsg(nameof(actual.CountryOfProcessing)));
            }

            if (string.IsNullOrWhiteSpace(expected.RetailUnitStoreOverride) && !expected.HasItemOverrides)
            {
                Assert.AreEqual(expected.RetailUnitName, actual.RetailUnit, UnequalMsg(nameof(actual.RetailUnit)));
            }
            else
            {
                Assert.AreEqual(expected.RetailUnitStoreOverride, actual.RetailUnit, UnequalMsg(nameof(actual.RetailUnit)));
            }

            if (string.IsNullOrWhiteSpace(expected.ScaleExtraTextOverride))
            {
                Assert.AreEqual(expected.ScaleExtraText, actual.ScaleExtraText, UnequalMsg(nameof(actual.ScaleExtraText)));
            }
            else
            {
                Assert.AreEqual(expected.ScaleExtraTextOverride, actual.ScaleExtraText, UnequalMsg(nameof(actual.ScaleExtraText)));
            }

            if (expected.ExpectError)
            {
                Assert.IsNotNull(actual.ErrorMessage);
                Assert.AreEqual(expected.ExpectedErrors.ErrorMessage, actual.ErrorMessage);
                Assert.AreEqual(expected.ExpectedErrors.ErrorDetails, actual.ErrorDetails);
                Assert.AreEqual(expected.ExpectedErrors.ErrorSource, actual.ErrorSource);
            }
            else
            {
                Assert.IsNull(actual.ErrorMessage);
                Assert.IsNull(actual.ErrorDetails);
                Assert.IsNull(actual.ErrorSource);
            }
        }

        private void AssertQueryResultsMatchExpected_ForStore(string expectedRegion,
            int expectedStoreNo, int expectedBusinessUnit, ItemLocaleEventModel actual)
        {
            Assert.AreEqual(expectedRegion, actual.Region, UnequalMsg(nameof(actual.Region)));
            Assert.AreEqual(expectedBusinessUnit, actual.BusinessUnitId, UnequalMsg(nameof(actual.BusinessUnitId)));
            Assert.AreEqual(expectedStoreNo, actual.StoreNo, UnequalMsg(nameof(actual.StoreNo)));
        }

        private void AssertQueryResultsMatchExpected_All(ItemLocaleEventQueryTestParameters expected, ItemLocaleEventModel actual)
        {
            AssertQueryResultsMatchExpected_ForStore(
                expected.StoreParams.Region,
                expected.StoreParams.Store_No,
                expected.StoreParams.BusinessUnit,
                actual);
            AssertQueryResultsMatchExpected_AllExceptStore(expected, actual);
        } 

        public string UnequalMsg(string propertyName)
        {
            return $"The expected \"{propertyName}\" value did not match the actual .";
        }

        public string ExpectedNullMsg(string propertyName, object value)
        {
            return $"Expected \"{propertyName}\" to be null but was {value}";
        }

        public static void AssertTimesCloseEnough(DateTime expected, DateTime actual,
            string propertyName,
            bool ignoreMinutes = false,
            bool ignoreSeconds = true)
        {
            var msg = $"The expected {propertyName} DateTime value did not match the expected";
            if (expected == null && actual == null)
            {
                return;
            }
            Assert.AreEqual(expected.Year, actual.Year, msg += " Year value.");
            Assert.AreEqual(expected.Month, actual.Month, msg += " Month value.");
            Assert.AreEqual(expected.Day, actual.Day, msg += " Day value.");
            Assert.AreEqual(expected.Date, actual.Date, msg += " Date value.");
            Assert.AreEqual(expected.Hour, actual.Hour, msg += " Hour value.");
            if (!ignoreMinutes)
            {
                Assert.AreEqual(expected.Minute, actual.Minute, msg += " Minute value.");
                if (!ignoreSeconds)
                {
                    Assert.AreEqual(expected.Second, actual.Second, msg += " Second value.");
                    // ignore milliseconds
                }
            }
        }
    }
}

