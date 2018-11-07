CREATE PROCEDURE [dbo].[GetFARMStoreItemData] 
AS
BEGIN


		SELECT DISTINCT 
			II.Identifier,
			substring('000000000000',1,12-len(substring(II.Identifier,1,12))) + II.Identifier as Upc12,
			SIV.Store_No,
			S.StoreAbbr, 
			V.Vendor_Key,	
			IV.Item_ID as VendorItemNumber,	
			dbo.fn_GetCurrentVendorPackage_Desc1(SIV.Item_Key, SIV.Store_No) as Package_Desc1, 
			CAST(ROUND((dbo.fn_GetCurrentCost(SIV.Item_Key, SIV.Store_No) / dbo.fn_GetCurrentVendorPackage_Desc1(SIV.Item_Key, SIV.Store_No)), 4) AS decimal(10,4)) AS CurrentRegCost,
			CAST(ROUND(dbo.fn_GetCurrentCost(SIV.Item_Key, SIV.Store_No), 4) AS decimal(10,4)) AS CurrentRegCaseCost,
			CAST(ROUND((dbo.fn_GetCurrentNetCost(SIV.Item_Key, SIV.Store_No) / dbo.fn_GetCurrentVendorPackage_Desc1(SIV.Item_Key, SIV.Store_No)), 4) AS decimal(10,4)) AS CurrentNetCost,
			CAST(ROUND(dbo.fn_GetCurrentNetCost(SIV.Item_Key, SIV.Store_No), 4) AS decimal(10,4)) AS CurrentNetCaseCost,
			CASE WHEN PCT.On_Sale = 1 THEN P.Sale_Price
				 ELSE P.Price
				 END AS CurrentRetail,
			CASE WHEN PCT.On_Sale = 1 THEN P.Sale_Multiple
				 ELSE P.Multiple
				 END AS CurrentRetailMultiple,
			P.Price/P.Multiple as RegRetail,
			PCT.PriceChgTypeDesc as PricePriorityType,
			max(TF.Tax1) as Tax1,
			max(TF.Tax2) as Tax2,
			max(TF.Tax3) as Tax3,
			max(TF.Tax4) as Tax4
		FROM Price P (nolock)
		INNER JOIN
			Store S (nolock)
			ON P.Store_No = S.Store_No
		INNER JOIN
			StoreItemVendor SIV (nolock)
			ON P.Item_Key = SIV.Item_Key
				AND P.Store_No = SIV.Store_No
		INNER JOIN
			PriceChgType PCT (nolock)
			ON PCT.PriceChgTypeID = P.PriceChgTypeID
		INNER JOIN
			VendorCostHistory VCH (nolock)
			ON VCH.StoreItemVendorID = SIV.StoreItemVendorID
		INNER JOIN
			Vendor V (nolock)
			ON V.Vendor_ID = SIV.Vendor_ID
		INNER JOIN Item I (nolock)
			ON P.Item_Key = I.Item_Key
		INNER JOIN ItemIdentifier II (nolock)
			ON I.Item_Key = II.Item_key
		INNER JOIN ItemVendor IV (nolock)
			ON I.Item_Key = IV.Item_Key and V.Vendor_ID = IV.Vendor_ID
		INNER JOIN 
			(select distinct TaxClassID, TaxJurisdictionID,
				case when TaxFlagKey = 1 and TaxFlagValue = 1 then 1 else 0 end as Tax1,
				case when TaxFlagKey = 2 and TaxFlagValue = 1 then 1 else 0 end as Tax2,
				case when TaxFlagKey = 3 and TaxFlagValue = 1 then 1 else 0 end as Tax3,
				case when TaxFlagKey = 4 and TaxFlagValue = 1 then 1 else 0 end as Tax4
			from TaxFlag
			) TF 
			ON I.TaxClassID = TF.TaxClassID
				AND S.TaxJurisdictionID = TF.TaxJurisdictionID
		WHERE SIV.PrimaryVendor = 1 and I.Deleted_Item = 0
		group by II.Identifier
		, SIV.Store_No
		, S.StoreAbbr
		, V.Vendor_Key
		, IV.Item_ID
		, SIV.Item_Key
		, PCT.On_Sale
		, PCT.PriceChgTypeDesc
		, P.Sale_Price
		, P.Sale_Multiple
		, P.Price
		, P.Multiple

END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetFARMStoreItemData] TO [IRMA_Farm_Role]
    AS [dbo];

