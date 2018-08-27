using Dapper;
using Mammoth.Common.DataAccess.CommandQuery;
using Mammoth.Common.DataAccess.DbProviders;
using Mammoth.ItemLocale.Controller.DataAccess.Models;
using System;
using System.Collections.Generic;

namespace Mammoth.ItemLocale.Controller.DataAccess.Queries
{
    public class GetItemLocaleDataQuery : IQueryHandler<GetItemLocaleDataParameters, List<ItemLocaleEventModel>>
    {
        private IDbProvider db;

        public GetItemLocaleDataQuery(IDbProvider db)
        {
            this.db = db;
        }

        public List<ItemLocaleEventModel> Search(GetItemLocaleDataParameters parameters)
        {
            // This query will get all events regardless of whether the Store_No IS NULL or not.
            List<ItemLocaleEventModel> itemLocaleData = new List<ItemLocaleEventModel>();
            DateTime today = DateTime.Today;
            string sql = $@" SET TRANSACTION ISOLATION LEVEL SNAPSHOT;

DECLARE @ExcludedStoreNo VARCHAR(250);

SET @ExcludedStoreNo = (
		SELECT dbo.fn_GetAppConfigValue('LabAndClosedStoreNo', 'IRMA Client')
		);

-- For all the rows where ItemLocaleChangeQueue.Store_No IS NOT NULL
SELECT q.QueueID AS QueueId
	,q.EventTypeID AS EventTypeId
	,srm.Region_Code AS Region
	,q.Identifier AS ScanCode
	,s.BusinessUnit_ID AS BusinessUnitId
	,s.Store_No AS StoreNo
	,CASE 
		WHEN p.AgeCode = 1
			THEN 18
		WHEN p.AgeCode = 2
			THEN 21
		ELSE NULL
		END AS AgeRestriction
	,CASE 
		WHEN t.EventTypeName = 'ItemDelete'
			THEN 0
		ELSE si.Authorized
		END AS Authorized
	,p.IBM_Discount AS CaseDiscount
	,sa.UomRegulationChicagoBaby AS ChicagoBaby
	,sa.ColorAdded AS ColorAdded
	,COALESCE(ivc.Origin_Name, co.Origin_Name) AS CountryOfProcessing
	,siv.DiscontinueItem AS Discontinued
	,p.ElectronicShelfTag AS ElectronicShelfTag
	,sa.Exclusive AS Exclusive
	,lt.LabelTypeDesc AS LabelTypeDescription
	,lsc.Identifier AS LinkedItem
	,p.LocalItem AS LocalItem
	,sa.Locality AS Locality
	,ii.NumPluDigitsSentToScale AS NumberOfDigitsSentToScale
	,COALESCE(ovo.Origin_Name, oo.Origin_Name) AS Origin
	,i.Product_Code AS ProductCode
	,p.Restricted_Hours AS RestrictedHours
	,COALESCE(uiu.Unit_Name, ovu.Unit_Name, iu.Unit_Name) AS RetailUnit
	,COALESCE(soe.ExtraText, sce.ExtraText, iet.ExtraText) AS ScaleExtraText
	,COALESCE(iov.SignRomanceTextLong, sa.SignRomanceTextLong) AS SignRomanceLong
	,COALESCE(iov.SignRomanceTextShort, sa.SignRomanceTextShort) AS SignRomanceShort
	,COALESCE(iov.Sign_Description, i.Sign_Description) AS SignDescription
	,sa.UomRegulationTagUom AS TagUom
	,p.Discountable AS TmDiscount
	,p.MSRPPrice AS Msrp
	,sie.OrderedByInfor AS OrderedByInfor
    ,iov.Package_Desc2 AS AltRetailSize
    ,ovu2.Unit_Abbreviation AS AltRetailUOM
    ,CAST(ii.Default_Identifier AS BIT) AS DefaultScanCode
	,iv.Item_ID AS VendorItemId
	,vch.Package_Desc1 AS VendorCaseSize
	,v.Vendor_Key AS VendorKey
	,v.CompanyName AS VendorCompanyName
    ,sc.ForceTare as ForceTare
    ,icf.SendToScale as SendtoCFS
    ,sct.Description as WrappedTareWeight
    ,utw.Description as UnWrappedTareWeight
    ,ii.Scale_Identifier as ScaleItem
    ,seb.Description as UseBy
    ,sc.ShelfLife_Length as ShelfLife
    ,CASE 
        WHEN ii.Remove_Identifier = 1 OR ii.Deleted_Identifier = 1 THEN NULL 
        ELSE i.Item_Key 
    END as IrmaItemKey
FROM [mammoth].[ItemLocaleChangeQueue] q
INNER JOIN mammoth.ItemChangeEventType t ON q.EventTypeID = t.EventTypeID
INNER JOIN Item i ON q.Item_Key = i.Item_Key
LEFT JOIN ItemIdentifier ii ON i.Item_Key = ii.Item_Key
	AND q.Identifier = ii.Identifier
	AND ii.Deleted_Identifier = 0
LEFT JOIN Store s ON q.Store_No = s.Store_No
LEFT JOIN StoreRegionMapping srm ON s.Store_No = srm.Store_No
LEFT JOIN StoreItemVendor siv ON i.Item_Key = siv.Item_Key
	AND s.Store_No = siv.Store_No
	AND siv.PrimaryVendor = 1
	AND siv.DeleteDate IS NULL
LEFT JOIN fn_VendorCostAll(@Today) vch on vch.Store_No = siv.Store_No
	and vch.Item_Key = siv.Item_Key
	and vch.Vendor_ID = siv.Vendor_ID
LEFT JOIN ItemVendor iv ON siv.Item_Key = iv.Item_Key
	AND siv.Vendor_ID = iv.Vendor_ID
LEFT JOIN Vendor v ON siv.Vendor_ID = v.Vendor_ID
LEFT JOIN Price p ON i.Item_Key = p.Item_Key
	AND s.Store_No = p.Store_No
LEFT JOIN StoreItem si ON s.Store_No = si.Store_No
	AND i.Item_Key = si.Item_Key
LEFT JOIN ItemSignAttribute sa ON i.Item_Key = sa.Item_Key
LEFT JOIN ItemOrigin co ON i.CountryProc_ID = co.Origin_ID -- country of processing
LEFT JOIN ItemOrigin oo ON i.Origin_ID = oo.Origin_ID -- origin
LEFT JOIN LabelType lt ON i.LabelType_ID = lt.LabelType_ID
LEFT JOIN ItemScale sc ON i.Item_Key = sc.Item_Key
LEFT JOIN dbo.Scale_Tare sct ON sct.Scale_Tare_ID = sc.Scale_Tare_ID
LEFT JOIN dbo.Scale_Tare utw ON utw.Scale_Tare_ID = sc.Scale_Alternate_Tare_ID
LEFT JOIN dbo.Scale_EatBy seb ON seb.Scale_EatBy_ID = sc.Scale_EatBy_ID
LEFT JOIN Scale_ExtraText sce ON sc.Scale_ExtraText_ID = sce.Scale_ExtraText_ID -- scale extra text
LEFT JOIN ItemOverride iov ON i.Item_Key = iov.Item_Key
	AND iov.StoreJurisdictionID = s.StoreJurisdictionID
LEFT JOIN ItemOrigin ivc ON iov.CountryProc_ID = ivc.Origin_ID -- alternate country of processing
LEFT JOIN ItemOrigin ovo ON iov.Origin_ID = ovo.Origin_ID -- alternate origin
LEFT JOIN ItemScaleOverride iso ON i.Item_Key = iso.Item_Key
	AND s.StoreJurisdictionID = iso.StoreJurisdictionID
LEFT JOIN Scale_ExtraText soe ON iso.Scale_ExtraText_ID = soe.Scale_ExtraText_ID -- alternate scale extra text
LEFT JOIN ItemUnit iu ON i.Retail_Unit_ID = iu.Unit_ID -- retail unit
LEFT JOIN ItemUnit ovu ON iov.Retail_Unit_ID = ovu.Unit_ID -- alternate retail unit
LEFT JOIN ItemUnit ovu2 ON iov.Package_Unit_ID = ovu2.Unit_ID
LEFT JOIN ItemIdentifier lsc ON p.LinkedItem = lsc.Item_Key
	AND lsc.Default_Identifier = 1
LEFT JOIN ItemNutrition nut ON nut.ItemKey = ii.Item_Key
LEFT JOIN Item_ExtraText iet ON iet.Item_ExtraText_ID = nut.Item_ExtraText_ID
LEFT JOIN ItemUomOverride iuo ON iuo.Item_Key = i.Item_Key
	AND iuo.Store_No = s.Store_No
LEFT JOIN ItemUnit uiu ON iuo.Retail_Unit_ID = uiu.Unit_ID
LEFT JOIN StoreItemExtended sie ON i.Item_Key = sie.Item_Key
	AND s.Store_No = sie.Store_No
LEFT JOIN dbo.itemCustomerFacingScale icf ON i.Item_Key = icf.Item_Key
WHERE q.InProcessBy = @JobInstance
	AND q.Store_No IS NOT NULL

UNION ALL

-- For all the rows where ItemLocaleChangeQueue.Store_No IS NULL
-- Order of JOINs is different than above
SELECT q.QueueID AS QueueId
	,q.EventTypeID AS EventTypeId
	,srm.Region_Code AS Region
	,q.Identifier AS ScanCode
	,s.BusinessUnit_ID AS BusinessUnitId
	,s.Store_No AS StoreNo
	,CASE 
		WHEN p.AgeCode = 1
			THEN 18
		WHEN p.AgeCode = 2
			THEN 21
		ELSE NULL
		END AS AgeRestriction
	,CASE 
		WHEN t.EventTypeName = 'ItemDelete'
			THEN 0
		ELSE si.Authorized
		END AS Authorized
	,p.IBM_Discount AS CaseDiscount
	,sa.UomRegulationChicagoBaby AS ChicagoBaby
	,sa.ColorAdded AS ColorAdded
	,COALESCE(ivc.Origin_Name, co.Origin_Name) AS CountryOfProcessing
	,siv.DiscontinueItem AS Discontinued
	,p.ElectronicShelfTag AS ElectronicShelfTag
	,sa.Exclusive AS Exclusive
	,lt.LabelTypeDesc AS LabelTypeDescription
	,lsc.Identifier AS LinkedItem
	,p.LocalItem AS LocalItem
	,sa.Locality AS Locality
	,ii.NumPluDigitsSentToScale AS NumberOfDigitsSentToScale
	,COALESCE(ovo.Origin_Name, oo.Origin_Name) AS Origin
	,i.Product_Code AS ProductCode
	,p.Restricted_Hours AS RestrictedHours
	,COALESCE(uiu.Unit_Name, ovu.Unit_Name, iu.Unit_Name) AS RetailUnit
	,COALESCE(soe.ExtraText, sce.ExtraText, iet.ExtraText) AS ScaleExtraText
	,COALESCE(iov.SignRomanceTextLong, sa.SignRomanceTextLong) AS SignRomanceLong
	,COALESCE(iov.SignRomanceTextShort, sa.SignRomanceTextShort) AS SignRomanceShort
	,COALESCE(iov.Sign_Description, i.Sign_Description) AS SignDescription
	,sa.UomRegulationTagUom AS TagUom
	,p.Discountable AS TmDiscount
	,p.MSRPPrice AS Msrp
	,sie.OrderedByInfor AS OrderedByInfor
    ,iov.Package_Desc2 AS AltRetailSize
    ,ovu2.Unit_Abbreviation AS AltRetailUOM
    ,CAST(ii.Default_Identifier AS BIT) AS DefaultScanCode
	,iv.Item_ID AS VendorItemId
	,vch.Package_Desc1 AS VendorCaseSize
	,v.Vendor_Key AS VendorKey
	,v.CompanyName AS VendorCompanyName
    ,sc.ForceTare as ForceTare
    ,icf.SendToScale as SendtoCFS
    ,sct.Description as WrappedTareWeight
    ,utw.Description as UnWrappedTareWeight
    ,ii.Scale_Identifier as ScaleItem
    ,seb.Description as UseBy
    ,sc.ShelfLife_Length as ShelfLife
    ,CASE 
        WHEN ii.Remove_Identifier = 1 OR ii.Deleted_Identifier = 1 THEN NULL 
        ELSE i.Item_Key 
    END as IrmaItemKey

FROM [mammoth].[ItemLocaleChangeQueue] q
INNER JOIN mammoth.ItemChangeEventType t ON q.EventTypeID = t.EventTypeID
INNER JOIN Item i ON q.Item_Key = i.Item_Key
LEFT JOIN ItemIdentifier ii ON i.Item_Key = ii.Item_Key
	AND q.Identifier = ii.Identifier
	AND ii.Deleted_Identifier = 0
LEFT JOIN Price p ON i.Item_Key = p.Item_Key
LEFT JOIN StoreItem si ON i.Item_Key = si.Item_Key
	AND p.Store_No = si.Store_No
LEFT JOIN Store s ON p.Store_No = s.Store_No
LEFT JOIN StoreRegionMapping srm ON s.Store_No = srm.Store_No
LEFT JOIN StoreItemVendor siv ON i.Item_Key = siv.Item_Key
	AND s.Store_No = siv.Store_No
	AND siv.PrimaryVendor = 1
	AND siv.DeleteDate IS NULL
LEFT JOIN fn_VendorCostAll(@Today) vch on vch.Store_No = siv.Store_No
	and vch.Item_Key = siv.Item_Key
	and vch.Vendor_ID = siv.Vendor_ID
LEFT JOIN ItemVendor iv ON siv.Item_Key = iv.Item_Key
	AND siv.Vendor_ID = iv.Vendor_ID
LEFT JOIN Vendor v ON siv.Vendor_ID = v.Vendor_ID
LEFT JOIN ItemSignAttribute sa ON i.Item_Key = sa.Item_Key
LEFT JOIN ItemOrigin co ON i.CountryProc_ID = co.Origin_ID -- country of processing
LEFT JOIN ItemOrigin oo ON i.Origin_ID = oo.Origin_ID -- origin
LEFT JOIN LabelType lt ON i.LabelType_ID = lt.LabelType_ID
LEFT JOIN ItemScale sc ON i.Item_Key = sc.Item_Key
LEFT JOIN dbo.Scale_Tare sct ON sct.Scale_Tare_ID = sc.Scale_Tare_ID
LEFT JOIN dbo.Scale_Tare utw ON utw.Scale_Tare_ID = sc.Scale_Alternate_Tare_ID
LEFT JOIN dbo.Scale_EatBy seb ON seb.Scale_EatBy_ID = sc.Scale_EatBy_ID
LEFT JOIN Scale_ExtraText sce ON sc.Scale_ExtraText_ID = sce.Scale_ExtraText_ID -- scale extra text
LEFT JOIN ItemOverride iov ON i.Item_Key = iov.Item_Key
	AND iov.StoreJurisdictionID = s.StoreJurisdictionID
LEFT JOIN ItemOrigin ivc ON iov.CountryProc_ID = ivc.Origin_ID -- alternate country of processing
LEFT JOIN ItemOrigin ovo ON iov.Origin_ID = ovo.Origin_ID -- alternate origin
LEFT JOIN ItemScaleOverride iso ON i.Item_Key = iso.Item_Key
	AND s.StoreJurisdictionID = iso.StoreJurisdictionID
LEFT JOIN Scale_ExtraText soe ON iso.Scale_ExtraText_ID = soe.Scale_ExtraText_ID -- alternate scale extra text
LEFT JOIN ItemUnit iu ON i.Retail_Unit_ID = iu.Unit_ID -- retail unit
LEFT JOIN ItemUnit ovu ON iov.Retail_Unit_ID = ovu.Unit_ID -- alternate retail unit
LEFT JOIN ItemUnit ovu2 ON iov.Package_Unit_ID = ovu2.Unit_ID
LEFT JOIN ItemIdentifier lsc ON p.LinkedItem = lsc.Item_Key
	AND lsc.Default_Identifier = 1
LEFT JOIN ItemNutrition nut ON nut.ItemKey = ii.Item_Key
LEFT JOIN Item_ExtraText iet ON iet.Item_ExtraText_ID = nut.Item_ExtraText_ID
LEFT JOIN ItemUomOverride iuo ON iuo.Item_Key = i.Item_Key
	AND iuo.Store_No = s.Store_No
LEFT JOIN ItemUnit uiu ON iuo.Retail_Unit_ID = uiu.Unit_ID
LEFT JOIN StoreItemExtended sie ON i.Item_Key = sie.Item_Key
	AND s.Store_No = sie.Store_No
LEFT JOIN dbo.itemCustomerFacingScale icf ON i.Item_Key = icf.Item_Key
WHERE q.InProcessBy = @JobInstance
	AND (
		s.WFM_Store = 1
		OR s.Mega_Store = 1
		)
	AND (
		Internal = 1
		AND BusinessUnit_ID IS NOT NULL
		)
	AND s.Store_No NOT IN (
		SELECT Key_Value
		FROM dbo.fn_Parse_List(@ExcludedStoreNo, '|')
		)
	AND q.Store_No IS NULL;";

            itemLocaleData = this.db.Connection
                .Query<ItemLocaleEventModel>(sql,
                    new { JobInstance = parameters.Instance, Today = today },
                    transaction: db.Transaction)
                .AsList();

            return itemLocaleData;
        }
    }
}