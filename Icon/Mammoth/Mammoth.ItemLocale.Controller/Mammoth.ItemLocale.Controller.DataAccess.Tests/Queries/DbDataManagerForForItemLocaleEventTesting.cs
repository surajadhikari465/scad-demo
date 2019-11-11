using Dapper;
using Irma.Framework;
using Irma.Testing;
using Mammoth.Common.DataAccess.DbProviders;
using Mammoth.ItemLocale.Controller.DataAccess.Tests.TestInfrastructure;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Mammoth.ItemLocale.Controller.DataAccess.Tests.Queries
{
    public class DbDataManagerForForItemLocaleEventTesting
    {
        private SqlDbProvider dbProvider;

        public DbDataManagerForForItemLocaleEventTesting(SqlDbProvider sqlDbProvider, string region)
        {
            this.dbProvider = sqlDbProvider;
            this.Region = region;
        }

        public string Region { get; set; }

        /// Reads existing database data for associated item-locale sub-objects:
        /// Store, SubTeam, Vendor, LabelType, ItemOrigin, ItemUnit, LinkedItem
        /// </summary>
        public DbDataCollectionForItemLocaleEventTesting LoadExistingDataForTest()
        {
            var dbData = new DbDataCollectionForItemLocaleEventTesting();

            var jurisdictions = GetJuridictions();
            dbData.DefaultJurisdictionId = jurisdictions
                .Single(j => j.StoreJurisdictionID == 1).StoreJurisdictionID;
            dbData.AltJurisdictionId = jurisdictions
                .FirstOrDefault(j => j.StoreJurisdictionID > 1)?.StoreJurisdictionID;

            dbData.ValidStores = GetValidStores();

            dbData.StoreInDefaultJurisdiction = dbData.ValidStores
                .FirstOrDefault( s => 
                    s.StoreJurisdictionID == dbData.DefaultJurisdictionId
                    && !s.Distribution_Center);           

            if (dbData.AltJurisdictionId.HasValue)
            {
                dbData.StoreInAltJurisdictionIfAny = dbData.ValidStores
                    .FirstOrDefault(s =>
                        s.StoreJurisdictionID == dbData.AltJurisdictionId.Value
                        && !s.Distribution_Center);
            }

            dbData.LabelSmall = GetFirstFromTableWhere<LabelType>(
                "LabelTypeDesc ='SMAL' OR LabelTypeDesc='SMLL'");

            dbData.Unit_Each = GetFirstFromTableWhere<ItemUnit>(
                "Unit_Abbreviation = 'EA' ");
            dbData.Unit_Case = GetFirstFromTableWhere<ItemUnit>(
                "Unit_Abbreviation = 'CS' ");
            dbData.Unit_Lb = GetFirstFromTableWhere<ItemUnit>(
                "Unit_Abbreviation = 'LB' ");
            dbData.Unit_Oz = GetFirstFromTableWhere<ItemUnit>(
                "Unit_Abbreviation = 'OZ' ");
            dbData.Unit_Kg = GetFirstFromTableWhere<ItemUnit>(
                "Unit_Abbreviation = 'KG' ");

            dbData.Origin_USA = GetFirstFromTableWhere<ItemOrigin>(
                "Origin_Name IN ('USA', 'United States') ");
            dbData.Origin_CAN = GetFirstFromTableWhere<ItemOrigin>(
                "Origin_Name IN ('CANADA', 'CAN') ");
            dbData.Origin_FRA = GetFirstFromTableWhere<ItemOrigin>(
                "Origin_Name IN ('FRANCE', 'FRA') ");
            dbData.Origin_ITA = GetFirstFromTableWhere<ItemOrigin>(
                "Origin_Name IN ('ITALY', 'ITA') ");

            dbData.SubTeam = GetFirstFromTable<SubTeam>();

            dbData.Vendor = GetFirstFromTableWhere<Vendor>(
                "CompanyName not like 'ZZ%' ");

            var linkedItem = GetAnItemToServeAsLinkedItem();
            dbData.LinkedItemKey = linkedItem.Item1;
            dbData.LinkedItemIdentifier = linkedItem.Item2;

            return dbData;
        }

        /// <summary>
        ///  Inserts item-locale data into the database for use in testing item-locale queries.
        ///  Data to be inserted is based partially on exisiting database data (for sub-objects
        ///  like Label, Unit, Vendor, Origin, etc.) and partially on provided parameters. 
        /// </summary>
        /// <param name="testSupportData">Existing DB data</param>
        /// <param name="testParameters">Specified test parameters (the expected state)</param>
        /// <returns></returns>
        public int InsertTestData(
            DbDataCollectionForItemLocaleEventTesting testSupportData,
            int storeNo,
            ItemLocaleEventQueryTestParameters testParameters)
        {
            return InsertTestDataAndSetExpectedDbValues(testSupportData, new List<int> { storeNo }, testParameters);
        }

        /// <summary>
        ///  Inserts item-locale data into the database for use in testing item-locale queries.
        ///  Data to be inserted is based partially on exisiting database data (for sub-objects
        ///  like Label, Unit, Vendor, Origin, etc.) and partially on provided parameters. 
        ///  Also modifies the test parameters object, updating it with "expected" values
        ///  from some of the inserted data 
        /// </summary>
        /// <param name="testSupportData">Existing DB data</param>
        /// <param name="testParameters">Specified test parameters (the expected state)</param>
        /// <returns></returns>
        public int InsertTestDataAndSetExpectedDbValues(
            DbDataCollectionForItemLocaleEventTesting testSupportData,
            List<int> storeNos,
            ItemLocaleEventQueryTestParameters testParameters)
        {
            var label = testSupportData.LabelSmall;
            var subTeam = testSupportData.SubTeam;
            var vendor = testSupportData.Vendor;
            var vendorItemId = testParameters.Identifier.PadLeft(13, '0');

            var origin = testSupportData.Origin_USA;
            var processingOrigin = testSupportData.Origin_CAN;

            var retailUnit = testSupportData.Unit_Lb;
            var packageUnit = testSupportData.Unit_Oz;
            var vendorCostUnit = testSupportData.Unit_Each;
            var vendorFreightUnit = testSupportData.Unit_Case;
            var altRetailUnit = testSupportData.Unit_Kg;
            var altPackageUnit = testSupportData.Unit_Kg;
            var altVendorUnit = testSupportData.Unit_Kg;
            var altDistroUnit = testSupportData.Unit_Kg;

            string linkedItemIdentifier = null;
            int? linkedItemKey = null;
            if (!testParameters.HasLinkedItem)
            {
                // use the linked item loaded from the existing data
                linkedItemIdentifier = testSupportData.LinkedItemIdentifier;
                linkedItemKey = testSupportData.LinkedItemKey;
            }

            // set properties of the expected data set based on the existing db data
            testParameters.SetExpectedDbValues_ForLabelAndRetailUnitAndOrigin(
                label.LabelTypeDesc,
                retailUnit.Unit_Name,
                origin.Origin_Name,
                processingOrigin.Origin_Name);

            int? altJurisdictionId = testSupportData.AltJurisdictionId;
            // do we need to insert a store to use for testing?
            if (testParameters.RequiresAltJurisdiction
                && (testSupportData.StoreInAltJurisdictionIfAny == null || altJurisdictionId.GetValueOrDefault(1) < 2))
            {
                // to have overrides in region without an alt jurisdiction, we need to creat alt store-jurisdiction

                // Insert new Store Jurisdiction
                altJurisdictionId = InsertNewStoreJurisdiction();
                if (testParameters.ItemOverrideParams != null)
                {
                    // set the new expected jurisdiction id for override params too
                    testParameters.ItemOverrideParams.JurisdictionId = altJurisdictionId;
                }

                // are desired store settings (Store_No, etc.) not already set in the provided parameters?
                if (testParameters.StoreParams == null || testParameters.StoreParams.Store_No < 1)
                {
                    // make up some store values here
                    var newStoreNo = 834792;
                    var newBusinessUnit = 83472;
                    var newStoreName = "Test Alt Jurisdiction Store";
                    // set expected store parameters
                    testParameters.StoreParams = new ItemLocaleEventQueryTestParameters_Store(
                        this.Region,
                        altJurisdictionId.Value,
                        newStoreNo,
                        newBusinessUnit,
                        newStoreName,
                        false);
                }
                else
                {
                    // make sure the store params have the jurisdictionID
                    testParameters.StoreParams.StoreJurisdictionID = altJurisdictionId;
                }

                // Insert New Store & Store-Region mapping
                InsertNewStore(testParameters.StoreParams.Store_No,
                    testParameters.StoreParams.BusinessUnit,
                    testParameters.StoreParams.StoreJurisdictionID,
                    testParameters.StoreParams.IsWfmStore,
                    true,
                    testParameters.StoreParams.IsMegaStore,
                    testParameters.StoreParams.StoreName);

                // Insert New StoreRegionMapping
                InsertNewStoreRegionMapping(testParameters.StoreParams.Store_No, testParameters.StoreParams.Region);
            }

            // Insert New Item
            var insertedItemKey = InsertNewItem(
                subTeam.SubTeam_No,
                testParameters.SignDescription,
                testParameters.ProductCode,
                origin.Origin_ID,
                processingOrigin.Origin_ID,
                label.LabelType_ID,
                retailUnit.Unit_ID,
                testParameters.IsRemovedItem,
                testParameters.IsDeletedItem,
                testParameters.IsRetailItem);
            // Insert New Item Identifier
            var identifierId = InsertNewItemIdentifier(
                insertedItemKey,
                testParameters.Identifier,
                testParameters.IsScaleIdentifier,
                testParameters.IsDefaultScanCode,
                testParameters.IsRemovedIdentifier,
                testParameters.IsDeletedIdentifier,
                testParameters.NumPluDigitsSentToScale);
            // Insert New ValidatedScanCode
            if (testParameters.IsValidated)
            {
                InsertNewValidatedScanCode(testParameters.Identifier);
            }
            // Insert ItemSignAttribute values
            InsertItemSignAttributes(
               insertedItemKey,
               testParameters.ChicagoBaby,
               testParameters.ColorAdded,
               testParameters.Locality,
               testParameters.SignRomanceLong,
               testParameters.SignRomanceShort,
               testParameters.UomRegulationTagUom,
               testParameters.ExclusiveDate);

            // set the expected linked item and IrmaItemKey
            testParameters.SetExpectedDbValues_ForLinkedItemAndIrmaKey(linkedItemIdentifier, insertedItemKey);

            // Insert Item-Vendor
            InsertItemVendor(insertedItemKey, vendor.Vendor_ID, vendorItemId);

            foreach (var storeNo in storeNos)
            {
                // this will insert to price, store-item, store-item-vendor, vendor cost history 
                // and (if needed) store-item-extended, item UOM override tables
                InsertItemMappingsForStore(
                    storeNo,
                    insertedItemKey,
                    true,
                    testParameters.PriceParams,
                    vendor.Vendor_ID,
                    vendorCostUnit.Unit_ID,
                    vendorFreightUnit.Unit_ID,
                    testParameters.VendorStoreItemParams,
                    testParameters.IsOrderedByInfor,
                    linkedItemKey,
                    testParameters.RetailUnitStoreOverride);
            }

            // set expected vendor values based on the db data
            testParameters.VendorStoreItemParams.SetExpectedDbValues_ForVendor(
                vendorItemId,
                vendor.Vendor_Key,
                vendor.CompanyName);

            if (testParameters.IsScaleItem)
            {
                // Insert Item-Scale and Extra Text (if any)
                InsertScaleItemAttributes
                    (insertedItemKey,
                    testParameters.ScaleParams,
                    testParameters.ScaleExtraText,
                    altJurisdictionId,
                    testParameters.ScaleExtraTextOverride);
            }
            else  // non-scale item
            {
                // Insert Item Extra Text (if any)
                InsertItemExtraText(
                    insertedItemKey,
                    testParameters.ScaleExtraText,
                    altJurisdictionId,
                    testParameters.ScaleExtraTextOverride);
            }

            // Insert CustomerFacingScale
            if (testParameters.IsCfsItem)
            {
                throw new NotImplementedException("need to implement CFS item test data setup");
                //TODO insert to itemCustomerFacingScale table
            }

            if (testParameters.HasItemOverrides)
            {
                var overrideParams = testParameters.ItemOverrideParams;

                if (overrideParams.JurisdictionId.GetValueOrDefault(0)<1)
                {
                    throw new ArgumentException("Alt Jurisdiction must exist and be known to insert item override data");
                }
              
                // make sure item & sign description have values (non-nullable)
                var altItemDescription = "Test Override Item Description";
                var altSignDescription = overrideParams.AltSignDescription ?? "Test Override Sign Description";

                // make sure we have Package_Desc1 & 2 values (non-nullable)
                var altCaseSize = 1.0m;
                var altRetailSize = overrideParams.AltRetailSize.GetValueOrDefault(6.0m);

                // set  up unit ids (all 4 must be set - non-nullable -  so use default value if not specified
                var altPackageUnitId = altPackageUnit.Unit_ID;
                var altRetailUnitId = altRetailUnit.Unit_ID;
                var altVendorUnitId = altVendorUnit.Unit_ID;
                var altDistroUnitId = altDistroUnit.Unit_ID;
                // look up alt Package Unit (AltRetailUOM) from abbreviation if provided
                if (!string.IsNullOrWhiteSpace(overrideParams.AltRetailUomAbbrev))
                {
                    altPackageUnitId = GetUnitIdByAbbreviation(overrideParams.AltRetailUomAbbrev);
                }
                // look up alt Retail Unit for jurisdiction (not for store- handled below)
                if (!string.IsNullOrWhiteSpace(overrideParams.AltRetailUnitJurisdiction))
                {
                    altRetailUnitId = GetUnitIdByName(overrideParams.AltRetailUnitJurisdiction);
                }

                // look up origin ids if requested to set
                int? altOriginId = null;
                if (!string.IsNullOrWhiteSpace(overrideParams.AltOriginName))
                {
                    altOriginId = GetOriginIdByName(overrideParams.AltOriginName);
                }
                int? altCountryOfProcessingId = null;
                if (!string.IsNullOrWhiteSpace(overrideParams.AltCountryOfProcessingName))
                {
                    altCountryOfProcessingId = GetOriginIdByName(overrideParams.AltCountryOfProcessingName);
                }

                if(!String.IsNullOrWhiteSpace(testParameters.RetailUnitStoreOverride))
                {
                   altRetailUnitId = this.dbProvider.GetLookupId<int>("Unit_ID", "ItemUnit", "Unit_Name", testParameters.RetailUnitStoreOverride); 
                }

                InsertItemOverride(insertedItemKey,
                    overrideParams.JurisdictionId.Value,
                    altItemDescription,
                    altSignDescription,
                    altCaseSize,
                    altRetailSize,
                    altPackageUnitId,
                    altRetailUnitId,
                    altVendorUnitId,
                    altDistroUnitId,
                    altOriginId,
                    altCountryOfProcessingId,
                    testParameters.SignRomanceLong,
                    testParameters.SignRomanceShort);
            }

            return insertedItemKey;
        }

        /// <summary>
        /// Inserts data  to price, store-item, store-item-vendor, vendor cost history 
        // and(if needed) store-item-extended, item UOM override tables
        /// </summary>
        public void InsertItemMappingsForStore(
            int storeNo,
            int itemKey,
            bool isAuthorized,
            ItemLocaleEventQueryTestParameters_Price priceParams,
            int vendorId,
            int vendorCostUnitId,
            int vendorFreightUnitId,
            ItemLocaleEventQueryTestParameters_Vendor vendorParams,
            bool? isOrderedByInfor = null,
            int? linkedItemKey = null,
            string retailUnitStoreOverride = null)
        {
            // Insert Store-Item mapping
            InsertStoreItem(storeNo, itemKey, isAuthorized);
            // Insert New Price
            InsertNewPrice(
                priceParams.HasCaseDiscount,
                priceParams.HasElectronicShelfTag,
                priceParams.IsLocalItem,
                priceParams.HasRestrictedHours,
                priceParams.TeamDiscountEligible,
                priceParams.Msrp,
                storeNo,
                itemKey,
                priceParams.AgeRestrictionId,
                linkedItemKey,
                priceParams.PriceMultiple);
            // Insert StoreItemExtended
            if (isOrderedByInfor.GetValueOrDefault(false))
            {
                InsertItemStoreExtended(itemKey, storeNo, true);
            }
            // is there a store-level override for Retail UOM?
            if (!string.IsNullOrWhiteSpace(retailUnitStoreOverride))
            {
                // Insert new ItemUomOverride
                InsertItemUomOverride(
                    storeNo,
                    itemKey,
                    retailUnitStoreOverride);
            }
            // Insert Store-Item-Vendor
            var storeItemVendorId = InsertStoreItemVendor(
                vendorId,
                storeNo,
                itemKey,
                vendorParams.IsDiscontinued,
                vendorParams.IsPrimaryVendor,
                vendorParams.LastCostRefreshed);
            // Insert VendorCostHistory (CostUnitId and FreightUnitId are from the RetailUnit set above)
            var vendorCostHistoryId = InsertVendorCostHistory(
                vendorCostUnitId,
                vendorFreightUnitId,
                vendorParams.VendorCaseSize,
                storeItemVendorId);
        }

        public void InsertScaleItemAttributes(
            int itemKey,
            ItemLocaleEventQueryTestParameters_Scale scaleParams,
            string scaleExtraText = null,
            int? altJurisdictionId = null,
            string scaleExtraTextOverride = null)
        {
            int? scaleExtraTextId = null;
            if (!string.IsNullOrWhiteSpace(scaleExtraText))
            {
                // Insert Scale ExtraText
                scaleExtraTextId = InsertScaleExtraText(scaleExtraText);
            }
            if (!string.IsNullOrWhiteSpace(scaleExtraTextOverride))
            {
                // Insert Scale ExtraText Override
                var scaleExtraTextOverrideId = InsertScaleExtraText(scaleExtraTextOverride);
                // Insert ItemScale
                InsertItemScaleOverride(
                    altJurisdictionId.Value,
                    itemKey,
                    scaleExtraTextOverrideId);
            }

            // Insert Scale_Tare and Scale_EatBy
            var wrappedScaleTareId = InsertWrappedTareWeight(scaleParams.WrappedTareWeight);
            var unwrappedScaleTareId = InsertUnwrappedTareWeight(scaleParams.UnwrappedTareWeight);
            var scaleEatById = InsertScaleEatBy(scaleParams.EatByText);

            // Insert ItemScale
            var itemScaleId = InsertItemScale(
                itemKey,
                scaleExtraTextId,
                scaleParams.ForceTare,
                scaleParams.ShelfLifeLength,
                wrappedScaleTareId,
                unwrappedScaleTareId,
                scaleEatById);
        }

        public void InsertItemExtraText(
            int itemKey,
            string itemExtraText = null,
            int? altJurisdictionId = null,
            string itemExtraTextOverride = null)
        {
            if (!string.IsNullOrWhiteSpace(itemExtraText))
            {
                // Insert Item ExtraText
                var itemExtraTextId = InsertItemExtraText(itemExtraText);
                // Insert Nutrition (for item ExtraText)
                InsertItemNutrition(itemKey, itemExtraTextId);
            }
            if (!string.IsNullOrWhiteSpace(itemExtraTextOverride))
            {
                // Insert Item ExtraText Override
                var itemExtraTextId = InsertItemExtraText(itemExtraTextOverride);
                // Insert Nutrition Override (for item ExtraText)
                InsertItemNutritionOverride(
                   altJurisdictionId.Value,
                   itemKey,
                   itemExtraTextId);
            }
        }

        public int InsertNewItem(int subTeamNo, string signDescription, string productCode,
            int originID, int countryOfProcessingOriginId, int labelTypeID, int retailUnitID,
            bool removeItem = false, bool deletedItem = false, bool isRetail = true)
        {
            // Insert New Item
            var itemKey = this.dbProvider.Insert(
                new IrmaQueryParams<Item, int>(
                    IrmaTestObjectFactory.BuildItem()
                        .With(x => x.SubTeam_No, subTeamNo)
                        .With(x => x.Sign_Description, signDescription)
                        .With(x => x.Product_Code, productCode)
                        .With(x => x.Origin_ID, originID)
                        .With(x => x.CountryProc_ID, countryOfProcessingOriginId)
                        .With(x => x.LabelType_ID, labelTypeID)
                        .With(x => x.Retail_Unit_ID, retailUnitID)
                        .With(x => x.Remove_Item, removeItem ? (byte)1 : (byte)0)
                        .With(x => x.Deleted_Item, deletedItem)
                        .With(x => x.Retail_Sale, isRetail)
                        .ToObject(),
                x => x.Item_Key));
            return itemKey;
        }

        public int InsertNewItem(int subTeamNo, string signDescription, string productCode,
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

        public void InsertItemSignAttributes(int itemKey, string chicagoBaby, bool? ColorAdd,
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

        public void InsertStoreItem(int storeNo, int itemKey, bool authorized)
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

        public int InsertStoreItemVendor(int vendorId, int storeNo, int itemKey,
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

        public int InsertNewItemIdentifier(int itemKey, string Identifier, bool? isScaleIdentifier,
            bool defaultScanCode, bool removeIdentifier = false, bool deletedIdentifier = false,
            int? numberOfDigitsSentToScale = null)
        {
            var identifierId = 
                // Insert New Item Identifier
                this.dbProvider.Insert(
                    new IrmaQueryParams<ItemIdentifier, int>(
                        IrmaTestObjectFactory.BuildItemIdentifier()
                            .With(x => x.Item_Key, itemKey)
                            .With(x => x.Identifier, Identifier)
                            .With(x => x.Scale_Identifier, isScaleIdentifier)
                            .With(x => x.Default_Identifier, defaultScanCode ? (byte)1 : (byte)0)
                            .With(x => x.Remove_Identifier, removeIdentifier ? (byte)1 : (byte)0)
                            .With(x => x.Deleted_Identifier, deletedIdentifier ? (byte)1 : (byte)0)
                            .With(x => x.NumPluDigitsSentToScale, numberOfDigitsSentToScale)
                            .ToObject(),
                        x => x.Identifier_ID));
            return identifierId;
        }

        public void InsertNewItemIdentifier(int itemKey, string Identifier, int? numberOfDigitsSentToScale )
        {
            InsertNewItemIdentifier(itemKey, Identifier, null, true, false, false, numberOfDigitsSentToScale);
        }

        public void InsertNewItemIdentifier(int itemKey, string Identifier, int? numberOfDigitsSentToScale,
            bool defaultScanCode)
        {
            InsertNewItemIdentifier(itemKey, Identifier, null, defaultScanCode, false, false, numberOfDigitsSentToScale);
        }

        public void InsertNewItemIdentifier(int itemKey, string Identifier, int? numberOfDigitsSentToScale,
            bool defaultScanCode, bool removeIdentifier)
        {
            InsertNewItemIdentifier(itemKey, Identifier, null, defaultScanCode, removeIdentifier, false, numberOfDigitsSentToScale);
        }

        public void InsertNewItemIdentifier(int itemKey, string Identifier, int? numberOfDigitsSentToScale,
            bool defaultScanCode, bool removeIdentifier, bool deletedIdentifier)
        {
            InsertNewItemIdentifier(itemKey, Identifier, null, defaultScanCode, removeIdentifier, deletedIdentifier, numberOfDigitsSentToScale);
        }

        public int InsertNewStoreJurisdiction(string description = "Test", int currencyId = 1)
        {
            var storeJurisdictionId = this.dbProvider.GetLookupId<int?>("StoreJurisdictionID", "StoreJurisdiction", "StoreJurisdictionDesc", description);

            if(storeJurisdictionId == null)
            {
                var jurisdiction = IrmaTestObjectFactory.Build<StoreJurisdiction>()
                    .With(x => x.CurrencyID, currencyId)
                    .With(x => x.StoreJurisdictionDesc, description)
                    .ToObject();
            
                storeJurisdictionId = this.dbProvider.Insert(
                    new IrmaQueryParams<StoreJurisdiction, int>( jurisdiction, x => x.StoreJurisdictionID));
            }
            return storeJurisdictionId ?? -1;
        }

        public void InsertNewStore(int storeNo, int? businessUnitId, int? jurisdictionID = 1,
            bool? wfm_store = true, bool? internal_store = false, bool? mega_store = false,
            string storeName = "Test Store")
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

        public void InsertNewStoreRegionMapping(int storeNo, string region)
        {
            // Insert New Store Region Mapping
            this.dbProvider.Insert(IrmaTestObjectFactory.Build<StoreRegionMapping>()
            .With(x => x.Store_No, storeNo)
            .With(x => x.Region_Code, region)
            .ToObject());
        }

        public void InsertNewPrice(bool caseDiscount, bool? rlectronicShelfTag, bool localItem,
            bool restrictedHours, bool tmDiscount, decimal msrp, int storeNo, int itemKey,
            int? ageRestrictionId, int? linkedItemKey, short multiple = 1)
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
                    .With(x => x.Multiple, Convert.ToByte(multiple))
                    .With(x => x.LinkedItem, linkedItemKey)
                .ToObject(),
            null,
            new Dictionary<string, string> { { "Price1", "Price" } }));
        }

        public void InsertNewValidatedScanCode(string identifier, int inforItemId = 0)
        {
            // Insert New Validated Scan Code
            this.dbProvider.Insert(new IrmaQueryParams<ValidatedScanCode, int>(
                IrmaTestObjectFactory.BuildValidatedScanCode()
                    .With(x => x.ScanCode, identifier)
                    .With(x => x.InforItemId, inforItemId)
                    .ToObject(),
                x => x.Id));
        }

        public void InsertItemVendor(int itemKey, int vendorId, string vendorItemId = null)
        {
            // Insert Item Vendor
            this.dbProvider.Insert(
                IrmaTestObjectFactory.Build<ItemVendor>()
                    .With(x => x.Item_Key, itemKey)
                    .With(x => x.Vendor_ID, vendorId)
                    .With(x => x.Item_ID, vendorItemId)
                    .ToObject());
        }

        public int InsertVendorCostHistory(int costUnitId, int freightUnitId, decimal vendorCaseSize, int storeItemVendorId)
        {
            var vendorCostHistoryObject = IrmaTestObjectFactory.BuildVendorCostHistory()
               .With(x => x.CostUnit_ID, costUnitId)
               .With(x => x.FreightUnit_ID, freightUnitId)
               .With(x => x.Package_Desc1, vendorCaseSize)
               .With(x => x.StoreItemVendorID, storeItemVendorId)
               .ToObject();
            int vendorCostHistoryId = this.dbProvider.Insert(
                new IrmaQueryParams<VendorCostHistory, int>(vendorCostHistoryObject, x => x.VendorCostHistoryID));
            return vendorCostHistoryId;
        }

        public int InsertScaleExtraText(string scaleExtraText, string description = "dummy Scale_ExtraText Description")
        {
            var scaleExtraTextObject = IrmaTestObjectFactory.Build<Scale_ExtraText>()
                        .With(x => x.ExtraText, scaleExtraText)
                        .With(x => x.Description, description)
                        .ToObject();
            var scaleExtraTextId = this.dbProvider.Insert(
                new IrmaQueryParams<Scale_ExtraText, int>(scaleExtraTextObject, x => x.Scale_ExtraText_ID));
            return scaleExtraTextId;
        }

        public int InsertItemScale(
            int itemKey,
            int? scaleExtraTextId,
            bool forceTare,
            short? shelfLife,
            int? scaleTareId,
            int? scaleAltTareId,
            int? scaleEatById)
        {
            var itemScaleObject = IrmaTestObjectFactory.Build<ItemScale>()
               .With(x => x.Item_Key, itemKey)
               .With(x => x.Scale_ExtraText_ID, scaleExtraTextId)
               .With(x => x.ForceTare, forceTare)
               .With(x => x.ShelfLife_Length, shelfLife)
               .With(x => x.Scale_Tare_ID, scaleTareId)
               .With(x => x.Scale_Alternate_Tare_ID, scaleAltTareId)
               .With(x => x.Scale_EatBy_ID, scaleEatById)
               .ToObject();
            var itemScaleId = this.dbProvider.Insert(
                new IrmaQueryParams<ItemScale, int>(itemScaleObject, x => x.ItemScale_ID));
            return itemScaleId;
        }

        public int? InsertWrappedTareWeight(string wrappedTareWeightDescription)
        {
            if (string.IsNullOrWhiteSpace(wrappedTareWeightDescription)) return null;
            var scaleTareObject = IrmaTestObjectFactory.Build<Scale_Tare>()
                    .With(x => x.Description, wrappedTareWeightDescription)
                    .ToObject();
            var wrappedTareWeightId = this.dbProvider
                .Insert(new IrmaQueryParams<Scale_Tare, int>(scaleTareObject, x => x.Scale_Tare_ID));
            return wrappedTareWeightId;
        }

        public int? InsertUnwrappedTareWeight(string unwrappedTareWeightDescription)
        {
            if (string.IsNullOrWhiteSpace(unwrappedTareWeightDescription)) return null;
            var scaleTareObject = IrmaTestObjectFactory.Build<Scale_Tare>()
                    .With(x => x.Description, unwrappedTareWeightDescription)
                    .ToObject();
            var unwrappedTareWeightId = this.dbProvider
                .Insert(new IrmaQueryParams<Scale_Tare, int>(scaleTareObject, x => x.Scale_Tare_ID));
            return unwrappedTareWeightId;
        }

        public int? InsertScaleEatBy(string eatByDescription)
        {
            if (string.IsNullOrWhiteSpace(eatByDescription)) return null;
            var scaleEatByObject = IrmaTestObjectFactory.Build<Scale_EatBy>()
                   .With(x => x.Description, eatByDescription)
                   .ToObject();
            var scale_EatBy_ID = this.dbProvider.Insert(
                new IrmaQueryParams<Scale_EatBy, int>(scaleEatByObject, x => x.Scale_EatBy_ID));
            return scale_EatBy_ID;
        }

        public int InsertItemExtraText(string itemExtraText, string description = "dummy Item_ExtraText description")
        {
            var itemExtraTextObject = IrmaTestObjectFactory.Build<Item_ExtraText>()
                        .With(x => x.ExtraText, itemExtraText)
                        .With(x => x.Description, description)
                        .ToObject();
            var itemExtraTextId = this.dbProvider.Insert(
                new IrmaQueryParams<Item_ExtraText, int>(itemExtraTextObject, x => x.Item_ExtraText_ID));
            return itemExtraTextId;
        }

        public int InsertStoreJurisdiction(string jurisdictionDesc = "Test Jurisdiction", int currencyId = 1)
        {
            var jurisdiction = IrmaTestObjectFactory.Build<StoreJurisdiction>()
                    .With(x => x.CurrencyID, currencyId)
                    .With(x => x.StoreJurisdictionDesc, jurisdictionDesc)
                    .ToObject();
            var storeJurisdictionID = this.dbProvider.Insert(
                new IrmaQueryParams<StoreJurisdiction, int>(jurisdiction, x => x.StoreJurisdictionID));
            return storeJurisdictionID;
        }

        public int InsertItemNutrition(int itemKey,
            int? itemExtraTextId)
        {
            var itemNutritionObject = IrmaTestObjectFactory.Build<ItemNutrition>()
                                  .With(x => x.ItemKey, itemKey)
                                  .With(x => x.Item_ExtraText_ID, itemExtraTextId)
                              .ToObject();
            var itemNutritionId = this.dbProvider.Insert(
                new IrmaQueryParams<ItemNutrition, int>(itemNutritionObject, x => x.ItemNutritionId));
            return itemNutritionId;
        }

        public int InsertItemStoreExtended(int itemKey, int storeNo, bool? isOrderedByInfor)
        {
            var storeItemExtendedObject = IrmaTestObjectFactory.Build<StoreItemExtended>()
                    .With(x => x.Item_Key, itemKey)
                    .With(x => x.Store_No, storeNo)
                    .With(x => x.OrderedByInfor, isOrderedByInfor)
                    .ToObject();
            var storeItemExtendedId = this.dbProvider.Insert(
                new IrmaQueryParams<StoreItemExtended, int>(storeItemExtendedObject, x => x.StoreItemExtendedID));
            return storeItemExtendedId;
        }

        /// <summary>
        /// ItemOverride has many non-nullable fields, so required params are required
        /// </summary>
        public void InsertItemOverride(int itemKey,
            int jurisdictionId,
            string itemDesc,
            string signDesc,
            decimal packageDesc1,
            decimal packageDesc2,
            int packageUnitId,
            int retailUnitId,
            int vendorUnitId,
            int distribUnitId,
            int? originId = null,
            int? countryProcId = null,
            string signRomanceLong = null,
            string signRomanceShort = null)
        {
            var idOrigin = originId == null ? "null" : originId.Value.ToString();
            var idCountryProc = countryProcId == null ? "null" : countryProcId.Value.ToString();
            var sql = $@"INSERT dbo.ItemOverride(
                                Item_Key,
                                StoreJurisdictionID,
                                Item_Description,
                                Sign_Description,
                                Package_Desc1,
                                Package_Desc2,
                                Package_Unit_ID,
                                Retail_Unit_ID,
                                Vendor_Unit_ID,
                                Distribution_Unit_ID,
                                Origin_ID,
                                CountryProc_ID,
                                SignRomanceTextLong,
                                SignRomanceTextShort,
                                POS_Description,
                                Average_Unit_Weight)
                        VALUES(
                                {itemKey},
                                {jurisdictionId},
                                '{itemDesc}',
                                '{signDesc}',
                                {packageDesc1},
                                {packageDesc2},
                                {packageUnitId},
                                {retailUnitId},
                                {vendorUnitId},
                                {distribUnitId},
                                {idOrigin},
                                {idCountryProc},
                                '{signRomanceLong}',
                                '{signRomanceShort}',
                                'Test Override POS_Des',
                                1);";

            this.dbProvider.Connection.Execute(@sql, null, dbProvider.Transaction);
        }

        public void InsertItemUomOverride(int storeNo, int itemKey, string altStoreRetailUnitName)
        {
            var retailUnitId = GetUnitIdByName(altStoreRetailUnitName);
            if (retailUnitId <= 0) retailUnitId = 2;
            var sql = $@"INSERT [dbo].[ItemUomOverride] (Item_Key, Store_No, Retail_Unit_ID )
                VALUES ({itemKey}, {storeNo}, {retailUnitId});";
            this.dbProvider.Connection.Execute(@sql, null, dbProvider.Transaction);
        }

        public void InsertItemScaleOverride(int jurisdictionId, int itemKey, int scaleExtraTextId)
        {
            var sql = $@"INSERT [dbo].[ItemScaleOverride] (StoreJurisdictionID, Item_Key, Scale_ExtraText_ID)
                VALUES ({jurisdictionId}, {itemKey}, {scaleExtraTextId});";
            this.dbProvider.Connection.Execute(@sql, null, dbProvider.Transaction);
        }

        public void InsertItemNutritionOverride(int jurisdictionId, int itemKey, int itemExtraTextId)
        {
            var sql = $@"INSERT [dbo].[ItemNutritionOverride] (StoreJurisdictionID, ItemKey, Item_ExtraText_ID)
                VALUES ({jurisdictionId}, {itemKey}, {itemExtraTextId});";
            this.dbProvider.Connection.Execute(@sql, null, dbProvider.Transaction);
        }

        public T GetFirstFromTable<T>(string alternateTableName = null)
        {
            var tableName = alternateTableName ?? typeof(T).Name;

            var result = dbProvider.Connection.Query<T>(
                "SELECT TOP 1 * FROM " + tableName,
                null,
                dbProvider.Transaction).First();

            return result;
        }

        public T GetFirstFromTableWhere<T>(string whereClause, string alternateTableName = null)
        {
            var tableName = alternateTableName ?? typeof(T).Name;

            var result = dbProvider.Connection.Query<T>(
                $"SELECT TOP 1 * FROM {tableName} WHERE {whereClause}; ",
                null,
                dbProvider.Transaction).First();

            return result;
        }

        public List<StoreJurisdiction> GetJuridictions()
        {
            var jurisdictions = this.dbProvider.Connection.Query<StoreJurisdiction>(
                    "SELECT * FROM StoreJurisdiction",
                    null,
                    dbProvider.Transaction);
            return jurisdictions?.ToList();
        }

        public List<Store> GetValidStores()
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

        public List<Store> GetAllStores()
        {
            List<Store> validStores = this.dbProvider.Connection
                .Query<Store>(@"SELECT * FROM Store;", transaction: this.dbProvider.Transaction)
                .ToList();

            return validStores;
        }

        public List<StoreRegionMapping> GetStoreRegionMapping()
        {
            List<StoreRegionMapping> storeRegionMapping = this.dbProvider.Connection
                .Query<StoreRegionMapping>(@"SELECT * FROM StoreRegionMapping", null, this.dbProvider.Transaction)
                .ToList();
            return storeRegionMapping;
        }

        public string GetLinkedIdentifierByItemKey(int itemKey)
        {
            string sql = @"SELECT Identifier FROM ItemIdentifier WHERE Item_Key = @ItemKey AND Default_Identifier = 1";
            string identifier = this.dbProvider.Connection.Query<string>(sql, new { ItemKey = itemKey }, this.dbProvider.Transaction).First();
            return identifier;
        }

        public Tuple<int, string> GetAnItemToServeAsLinkedItem()
        {
            string sql = @"
                SELECT TOP 1 ii.Item_Key, ii.Identifier
                FROM ItemIdentifier ii 
                    INNER JOIN Item i ON i.Item_Key = ii.Item_key
                 WHERE ii.Default_Identifier=1 AND ii.Deleted_Identifier=0 AND ii.Remove_Identifier=0
                        AND i.Deleted_Item=0 AND i.Remove_Item=0; ";

            dynamic result = this.dbProvider.Connection
                .Query(sql, transaction: dbProvider.Transaction)
                .First();
            var itemKey = result.Item_Key;
            var identifier = result.Identifier;
            return new Tuple<int, string>(itemKey, identifier);
        }

        public int GetUnitIdByAbbreviation(string unitAbbreviation)
        {
            string sql = $"SELECT TOP 1 Unit_ID FROM ItemUnit WHERE Unit_Abbreviation = '{unitAbbreviation}'; ";
            var id = this.dbProvider.Connection.Query<int>(sql, null, this.dbProvider.Transaction).FirstOrDefault();
            return id;
        }

        public int GetUnitIdByName(string unitName)
        {
            string sql = $"SELECT TOP 1 Unit_ID FROM ItemUnit WHERE Unit_Name = '{unitName}'; ";
            var id = this.dbProvider.Connection.Query<int>(sql, null, this.dbProvider.Transaction).FirstOrDefault();
            return id;
        }

        public int GetOriginIdByName(string originName)
        {
            string sql = $"SELECT TOP 1 Origin_ID FROM ItemOrigin WHERE Origin_Name = '{originName}'; ";
            var id = this.dbProvider.Connection.Query<int>(sql, null, this.dbProvider.Transaction).FirstOrDefault();
            return id;
        }

        public void UpdateLabAndClosedStoreValues(List<int> storeNumbers)
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

        public int InsertToItemLocaleChangeQueue(
            int itemKey,
            int? storeNo,
            string identfiier,
            int inProcessById,
            int eventType)
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
                        InProcessBy = inProcessById
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

        public void InsertIntoChangeQueue(int rows, int jobInstance, int eventTypeId, int? storeNumber)
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
    }
}