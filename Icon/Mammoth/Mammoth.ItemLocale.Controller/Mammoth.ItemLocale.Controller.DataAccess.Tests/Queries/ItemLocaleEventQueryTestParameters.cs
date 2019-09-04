using Irma.Framework;
using Mammoth.Common.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mammoth.ItemLocale.Controller.DataAccess.Tests
{
    /// <summary>
    /// Roughly equivalent to ItemLocaleEventModel, this object groups expected properties 
    /// for the results of an ItemLocaleEventQueue query. These values can be used for 
    /// setting up pre-test data dependencies and then serve as the "expected" values 
    /// for the actual query result. 
    /// </summary>
    public class ItemLocaleEventQueryTestParameters
    {
        public ItemLocaleEventQueryTestParameters(string identifier)
        {
            IsValidated = true;
            ExpectAuthorizedItem = true;
            Identifier = identifier;
            ExpectError = false;
            ExpectedErrors = new ItemLocaleEventQueryTestsParameters_ExpectedErrors();
        }

        public int InsertedEventTypeId { get; private set; }
        public int InsertedQueueId { get; private set; }
        public int InsertedItemKey { get; private set; }
        public bool ExpectAuthorizedItem { get; set; }
        public bool ExpectError { get; set; }
        public bool IsValidated { get; set; }
        public bool? IsScaleIdentifier { get; set; }
        public bool IsScaleItem => ScaleParams != null;
        public string Identifier { get; set; }
        public string SignDescription { get; set; }
        public string ProductCode { get; set; }
        public int? NumPluDigitsSentToScale { get; set; }
        public int? ExpectedNumberOfScaleDigits
        {
            get
            {
                if (StoreParams.IsMegaStore && IsCfsItem)
                {
                    return Identifier?.Length ?? 0;
                }
                return NumPluDigitsSentToScale;
            }
        }
        public bool IsDefaultScanCode { get; set; }
        public bool IsRetailItem { get; set; }
        public bool IsRemovedItem { get; set; }
        public bool IsDeletedItem { get; set; }
        public bool IsRemovedIdentifier { get; set; }
        public bool IsDeletedIdentifier { get; set; }
        public bool IsCfsItem { get; set; }
        public bool HasLinkedItem { get; set; }

        public int? IrmaItemKey { get; private set; }
        public string ChicagoBaby { get; set; }
        public bool ColorAdded { get; set; }
        public string Locality { get; set; }
        public string SignRomanceLong { get; set; }
        public string SignRomanceShort { get; set; }
        public int UomRegulationTagUom { get; set; }
        public string TagUom
        {
            get
            {
                return UomRegulationTagUom.ToString();
            }
        }
        public DateTime? ExclusiveDate { get; set; }
        public bool? IsOrderedByInfor { get; set; }
        public string ScaleExtraText { get; set; }
        public string ScaleExtraTextOverride { get; set; }
        public bool HasItemOverrides => 
            ItemOverrideParams != null 
            && (!string.IsNullOrWhiteSpace(ItemOverrideParams.AltSignDescription)
                || !string.IsNullOrWhiteSpace(ItemOverrideParams.AltRetailUomAbbrev)
                || ItemOverrideParams.AltRetailSize.HasValue
                || !string.IsNullOrWhiteSpace(ItemOverrideParams.AltOriginName)
                || !string.IsNullOrWhiteSpace(ItemOverrideParams.AltCountryOfProcessingName)
                || !string.IsNullOrWhiteSpace(ItemOverrideParams.AltRetailUnitJurisdiction));
        public bool RequiresAltJurisdiction =>
            HasItemOverrides
            || !string.IsNullOrWhiteSpace(ScaleExtraTextOverride)
            || !string.IsNullOrWhiteSpace(RetailUnitStoreOverride);
        public string RetailUnitStoreOverride { get; set; }
      
        // these fields are set based on db data, not directly set by test method
        public string LabelTypeDesc { get; private set; }
        public string RetailUnitName { get; private set; }
        public string OriginName { get; private set; }
        public string CountryOfProcessingName { get; private set; }
        public string LinkedItemIdentifier { get; private set; }

        public ItemLocaleEventQueryTestParameters_Store StoreParams { get; set; }
        public ItemLocaleEventQueryTestParameters_Vendor VendorStoreItemParams { get; set; }
        public ItemLocaleEventQueryTestParameters_Price PriceParams { get; set; }
        public ItemLocaleEventQueryTestParameters_Scale ScaleParams { get; set; }
        public ItemLocaleEventQueryTestParameters_ItemOverride ItemOverrideParams { get; set; }
        public ItemLocaleEventQueryTestsParameters_ExpectedErrors ExpectedErrors { get; set; }

        public void SetExpectedDbValues_ForLabelAndRetailUnitAndOrigin(
           string expectedLabelTypeDesc,
           string expectedRetailUnitName,
           string expectedOrigin,
           string expectedCountryOfProcessing)
        {
            LabelTypeDesc = expectedLabelTypeDesc;
            RetailUnitName = expectedRetailUnitName;
            OriginName = expectedOrigin;
            CountryOfProcessingName = expectedCountryOfProcessing;
        }

        public void SetExpectedDbValues_ForLinkedItemAndIrmaKey(
            string expectedLinkedItem,
            int? expectedIrmaItemKey)
        {
            // set the expected LinkedItem (which may be null if not a linked item)
            LinkedItemIdentifier = expectedLinkedItem;
            // set the expected IrmaItemKey for a non removed/deleted item
            if (!(IsRemovedIdentifier || IsDeletedIdentifier || IsRemovedItem || IsDeletedItem))
            {
               IrmaItemKey = expectedIrmaItemKey;
            }
        }

        public void SetExpectedDbValues_ForItemKeyAndQueueIdAndEventType(int insertedItemKey, int insertedQueueId, int eventTypeId)
        {
            InsertedItemKey = insertedItemKey;
            InsertedQueueId = insertedQueueId;
            InsertedEventTypeId = eventTypeId;
        }
    }

    public class ItemLocaleEventQueryTestParameters_Store
    {
        public ItemLocaleEventQueryTestParameters_Store(string region, Store existingStoreInDb)
            : this(region,
               existingStoreInDb.StoreJurisdictionID.Value,
               existingStoreInDb.Store_No,
               existingStoreInDb.BusinessUnit_ID.Value,
               existingStoreInDb.Store_Name,
               existingStoreInDb.Mega_Store,
               existingStoreInDb.WFM_Store,
               existingStoreInDb.Internal)
        { }

        public ItemLocaleEventQueryTestParameters_Store(string region, int? jurisdictionId,
            int storeNo, int businessUnit, string storeName,
            bool isMega, bool isWfm = true, bool isInternal = true)
        {
            Region = region;
            StoreJurisdictionID = jurisdictionId;
            Store_No = storeNo;
            BusinessUnit = businessUnit;
            StoreName = storeName;
            IsMegaStore = isMega;
            IsWfmStore = isWfm;
            IsInternalStore = isInternal;
        }

        public string Region { get; set; }
        public int Store_No { get; set; }
        public int BusinessUnit { get; set; }
        public string StoreName { get; set; }
        public int? StoreJurisdictionID { get; set; }
        public bool IsWfmStore { get; set; }
        public bool IsMegaStore { get; set; }
        public bool IsInternalStore { get; set; }
    }

    public class ItemLocaleEventQueryTestParameters_Vendor
    {
        public ItemLocaleEventQueryTestParameters_Vendor(int caseSize,
            bool isPrimaryVendor,
            bool isDiscontinued,
            DateTime lastCostRefreshed)
        {
            VendorCaseSize = caseSize;
            IsPrimaryVendor = isPrimaryVendor;
            IsDiscontinued = isDiscontinued;
            LastCostRefreshed = lastCostRefreshed;
        }
        public bool IsPrimaryVendor { get; set; }
        public bool IsDiscontinued { get; set; }
        public DateTime LastCostRefreshed { get; set; }
        public int VendorCaseSize { get; set; }

        // these fields are set based on db data, not specified by test method
        public int VendorId { get; private set; }
        public string VendorItemId { get; private set; }
        public string VendorKey { get; private set; }
        public string VendorCompanyName { get; private set; }

        public void SetExpectedDbValues_ForVendor(
            string expectedVendorItemId,
            string expectedVendorKey,
            string expectedVendorCompany)
        {
            VendorItemId = expectedVendorItemId;
            VendorKey = expectedVendorKey;
            VendorCompanyName = expectedVendorCompany;
        }
    }

    public class ItemLocaleEventQueryTestParameters_Price
    {
        public ItemLocaleEventQueryTestParameters_Price(
            decimal msrp,
            bool hasCaseDiscount,
            bool hasElectronicShelfTag,
            bool isLocalItem,
            bool hasRestrictedHours,
            int? ageRestritionValue,
            short priceMultiple,
            bool teamDiscountEligible,
            bool isRetail)
        {
            Msrp = msrp;
            HasCaseDiscount = hasCaseDiscount;
            HasElectronicShelfTag = hasElectronicShelfTag;
            IsLocalItem = isLocalItem;
            HasRestrictedHours = hasRestrictedHours;
            AgeRestrictionValue = ageRestritionValue;
            PriceMultiple = priceMultiple;
            TeamDiscountEligible = teamDiscountEligible;
            IsRetailItem = isRetail;
        }
        public bool IsRetailItem { get; set; }
        public decimal Msrp { get; set; }
        public bool HasCaseDiscount { get; set; }
        public bool HasElectronicShelfTag { get; set; }
        public bool IsLocalItem { get; set; }
        public bool HasRestrictedHours { get; set; }
        public bool TeamDiscountEligible { get; set; }
        // Price.AgeCode 0=none, 1=18, 2=21
        public int AgeRestrictionId { get; set; }
        // null, 18 or 21
        public int? AgeRestrictionValue
        {
            get
            {
                switch (AgeRestrictionId)
                {
                    case 1:
                        return 18;
                    case 2:
                        return 21;
                    case 0:
                    default:
                        return null;

                }
            }
            set
            {
                if (value.HasValue)
                {
                    switch (value)
                    {
                        case 18:
                            AgeRestrictionId = 1;
                            break;
                        case 21:
                            AgeRestrictionId = 2;
                            break;
                        default:
                        AgeRestrictionId = 0;
                            break;
                    }
                }
                else
                {
                    AgeRestrictionId = 0;
                }
            }
        }
        public short PriceMultiple { get; set; }
        //public string LinkedItemIdentifier { get; set; }
      }

    public class ItemLocaleEventQueryTestParameters_Scale
    {
        public string WrappedTareWeight { get; set; }
        public string UnwrappedTareWeight { get; set; }
        public string EatByText { get; set; }
 
        public bool ForceTare { get; set; }
        public short? ShelfLifeLength { get; set; }
    }

    public class ItemLocaleEventQueryTestParameters_ItemOverride
    {        
        // can be null if in region w/o alt jursidiction - means it will have to be created
        public int? JurisdictionId { get; set; }

        // iov ItemOverride
        //public string AltSignRomanceLong { get; set; }
        //public string AltSignRomanceShort { get; set; }
        public string AltSignDescription { get; set; }
        public decimal? AltRetailSize { get; set; }

        // ovu2 ItemUnit (package)
        public string AltRetailUomAbbrev { get; set; }
        //public int AltRetailUomId { get; set; }

        // ovo ItemOrigin (alt origin)
        public string AltOriginName { get; set; }

        // ivc ItemOrigin (alt country of processing)
        public string AltCountryOfProcessingName { get; set; }

        // ovu ItemUnit (retail)
        public string AltRetailUnitJurisdiction { get; set; }
    }

    public class ItemLocaleEventQueryTestsParameters_ExpectedErrors
    {
        public string ErrorMessage { get; set; }
        public string ErrorDetails { get; set; }
        public string ErrorSource { get; set; }
    }
}
