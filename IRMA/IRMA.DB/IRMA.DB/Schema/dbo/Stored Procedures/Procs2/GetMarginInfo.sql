CREATE PROCEDURE dbo.GetMarginInfo
	@Item_Key int    
AS
-- ************************************************************************************************************************************
-- Procedure: GetMarginInfo()
--    Author: n/a
--      Date: n/a
--
-- Description:
--
-- Modification History:
-- Date       	Init  			TFS   	Comment
-- 05/06/2011	Dave Stacey		1684	Converted current price to varchar set-up to include multiples in the display.
-- 12/17/2012   Faisal Ahmed	9251    Added exchange rate to calculate correct margin by converting cost to Store's currency
-- 
-- ************************************************************************************************************************************
BEGIN
 		SET NOCOUNT ON

		DECLARE @CurrDate datetime

		SELECT @CurrDate = CONVERT(datetime, CONVERT(varchar(255), GETDATE(), 101))

		SELECT
		--SJ.CurrencyID as StoreCurrency,
		SIVCH.Store_No, 
		S.Store_Name, 
		V.CompanyName,	
		SIVCH.Package_Desc1,
		 
	
		--GET Avg Cost				
		CAST(ROUND(
			(CASE WHEN dbo.fn_IsRetailUnitNotCostedByWeight(SIVCH.Item_key) = 1 THEN
					ISNULL(	dbo.fn_AvgCostHistory	( SIVCH.Item_Key,  SIVCH.Store_No,  Item.SubTeam_No,  GETDATE() ), 0) 
						* dbo.fn_GetAverageUnitWeightByItemKey(SIVCH.Item_Key) 	
				ELSE
					ISNULL(	dbo.fn_AvgCostHistory	( SIVCH.Item_Key,  SIVCH.Store_No,  Item.SubTeam_No,  GETDATE() ), 0) 
				END
			) * dbo.fn_GetCurrencyConversionRate(V.CurrencyID, SJ.CurrencyID), 2) AS decimal(9,2)) 
			AS AvgCost, 
			
		--GET CURRENT UNIT COST
		CAST(ROUND(
			(CASE WHEN dbo.fn_IsRetailUnitNotCostedByWeight(SIVCH.Item_key) = 1 THEN
				CAST(ROUND((dbo.fn_GetCurrentCost(SIVCH.Item_Key, SIVCH.Store_No) / ISNULL(SIVCH.Package_Desc1,1)), 4) AS decimal(10,4))
					* dbo.fn_GetAverageUnitWeightByItemKey(SIVCH.Item_Key)
				ELSE
					CAST(ROUND((dbo.fn_GetCurrentCost(SIVCH.Item_Key, SIVCH.Store_No) / ISNULL(SIVCH.Package_Desc1,1)), 4) AS decimal(10,4)) 
				END
			) * dbo.fn_GetCurrencyConversionRate(V.CurrencyID, SJ.CurrencyID), 2) AS decimal(9,2)) 
			AS CurrentRegCost,
			
		--GET CURRENT UNIT NET COST
		CAST(ROUND(
			(CASE WHEN dbo.fn_IsRetailUnitNotCostedByWeight(SIVCH.Item_key) = 1 THEN
				CAST(ROUND(((SIVCH.NetCost) / ISNULL(SIVCH.Package_Desc1,1)), 4) AS decimal(10,4)) 
					* dbo.fn_GetAverageUnitWeightByItemKey(SIVCH.Item_Key)
				ELSE 
					CAST(ROUND(((SIVCH.NetCost) / ISNULL(SIVCH.Package_Desc1,1)), 4) AS decimal(10,4)) 
				END
			) * dbo.fn_GetCurrencyConversionRate(V.CurrencyID, SJ.CurrencyID), 2) AS Decimal(10,4)) 
			AS CurrentNetCost,
		
		CASE WHEN PCT.On_Sale = 1 THEN Cast(P.Sale_Multiple as Varchar(3)) + '/' + Cast(P.Sale_Price AS varchar(10))
			 ELSE Cast(P.Multiple as Varchar(3)) + '/' + Cast(P.Price AS varchar(10))
			 END AS CurrentPrice,  
		
		CASE WHEN P.Price * P.Multiple > 0 AND SIVCH.Package_Desc1 >= 1 THEN 
				CAST(ROUND((((P.Price / P.Multiple) - 
					(
						CASE WHEN dbo.fn_IsRetailUnitNotCostedByWeight(SIVCH.Item_key) = 1 THEN
								(dbo.fn_GetCurrentNetCost(SIVCH.Item_Key, SIVCH.Store_No) / SIVCH.Package_Desc1) * dbo.fn_GetAverageUnitWeightByItemKey(SIVCH.Item_Key)
							ELSE
								dbo.fn_GetCurrentNetCost(SIVCH.Item_Key, SIVCH.Store_No) / SIVCH.Package_Desc1
							END
					) * CAST(ROUND(dbo.fn_GetCurrencyConversionRate(V.CurrencyID, SJ.CurrencyID), 4) AS Decimal(10,4)))
					 / (p.Price / P.Multiple)) * 100, 2) AS decimal(9,2))
			 ELSE NULL
			 END AS RegMarginCurrentCost, -- RegMarg(CurrCost)
			 
		CASE WHEN P.Price * P.Multiple > 0 AND SIVCH.Package_Desc1 >= 1 THEN 
				CAST(ROUND((((P.Price / P.Multiple) - 
					(	
					CASE WHEN dbo.fn_IsRetailUnitNotCostedByWeight(SIVCH.Item_key) = 1 THEN
							ISNULL(dbo.fn_AvgCostHistory( SIVCH.Item_Key,  SIVCH.Store_No,  Item.SubTeam_No,  GETDATE() ), 0) * dbo.fn_GetAverageUnitWeightByItemKey(SIVCH.Item_Key)
						ELSE
							ISNULL(dbo.fn_AvgCostHistory( SIVCH.Item_Key,  SIVCH.Store_No,  Item.SubTeam_No,  GETDATE() ), 0)
						END
					) * CAST(ROUND(dbo.fn_GetCurrencyConversionRate(V.CurrencyID, SJ.CurrencyID), 4) AS Decimal(10,4))
				) / (p.Price / P.Multiple)) * 100, 2) AS decimal(9,2))
			 ELSE NULL
			 END AS RegMarginAvgCost, -- RegMarg(AvgCost)
			 
		--IF ITEM IS ON SALE USE SALE PRICE, OTHERWISE USE REG PRICE
		CASE WHEN P.Price <= 0 THEN NULL
			 WHEN PCT.On_Sale = 1 AND (P.Sale_Price <= 0 OR P.Sale_Multiple <= 0 OR SIVCH.Package_Desc1 <= 0) THEN NULL
			 WHEN PCT.On_Sale = 1 THEN CAST(ROUND((((P.Sale_Price / P.Sale_Multiple) - 
				(
				CASE WHEN dbo.fn_IsRetailUnitNotCostedByWeight(SIVCH.Item_key) = 1 THEN
					(SIVCH.NetCost / SIVCH.Package_Desc1) * dbo.fn_GetAverageUnitWeightByItemKey(SIVCH.Item_Key)
				ELSE
					SIVCH.NetCost / SIVCH.Package_Desc1
				END
				)* CAST(ROUND(dbo.fn_GetCurrencyConversionRate(V.CurrencyID, SJ.CurrencyID), 4) AS Decimal(10,4))
				)/ (p.Sale_Price / P.Sale_Multiple)) * 100, 2) AS decimal(9,2))
			 ELSE CAST(ROUND((((P.Price / P.Multiple) - (
				(
				CASE WHEN dbo.fn_IsRetailUnitNotCostedByWeight(SIVCH.Item_key) = 1 THEN
					(SIVCH.NetCost/ SIVCH.Package_Desc1) * dbo.fn_GetAverageUnitWeightByItemKey(SIVCH.Item_Key)
				ELSE
					SIVCH.NetCost / SIVCH.Package_Desc1
				END
				) * dbo.fn_GetCurrencyConversionRate(V.CurrencyID, SJ.CurrencyID)
			 )) / (P.Price / P.Multiple)) * 100, 2) AS decimal(9,2))
			 END AS CurrentMarginCurrentCost, --USE SALE PRICE IF ITEM IS ON SALE  - CurrMarg(CurCost)
			 
		CASE WHEN P.Price <= 0 THEN NULL
			 WHEN PCT.On_Sale = 1 AND (P.Sale_Price <= 0 OR P.Sale_Multiple <= 0 OR SIVCH.Package_Desc1 <= 0) THEN NULL
			 WHEN PCT.On_Sale = 1 THEN CAST(ROUND((((P.Sale_Price / P.Sale_Multiple) - 
					(
					CASE WHEN dbo.fn_IsRetailUnitNotCostedByWeight(SIVCH.Item_key) = 1 THEN
						ISNULL(dbo.fn_AvgCostHistory( SIVCH.Item_Key,  SIVCH.Store_No, Item.SubTeam_No, GETDATE() ), 0) * dbo.fn_GetAverageUnitWeightByItemKey(SIVCH.Item_Key)
					ELSE
						ISNULL(dbo.fn_AvgCostHistory( SIVCH.Item_Key,  SIVCH.Store_No, Item.SubTeam_No, GETDATE() ), 0)
					END
					) * CAST(ROUND(dbo.fn_GetCurrencyConversionRate(V.CurrencyID, SJ.CurrencyID), 4) AS Decimal(10,4))
					) / (p.Sale_Price / P.Sale_Multiple)) * 100, 2) AS decimal(9,2))
			 ELSE CAST(ROUND((((P.Price / P.Multiple) - 
					(
					CASE WHEN dbo.fn_IsRetailUnitNotCostedByWeight(SIVCH.Item_key) = 1 THEN
						ISNULL(dbo.fn_AvgCostHistory( SIVCH.Item_Key,  SIVCH.Store_No,  Item.SubTeam_No,  GETDATE() ), 0) * dbo.fn_GetAverageUnitWeightByItemKey(SIVCH.Item_Key)
					ELSE
						ISNULL(dbo.fn_AvgCostHistory( SIVCH.Item_Key,  SIVCH.Store_No,  Item.SubTeam_No,  GETDATE() ), 0)
					END
					) * CAST(ROUND(dbo.fn_GetCurrencyConversionRate(V.CurrencyID, SJ.CurrencyID), 4) AS Decimal(10,4))
					)/ (P.Price / P.Multiple)) * 100, 2) AS decimal(9,2))
			 END AS CurrentMarginAvgCost --USE SALE PRICE IF ITEM IS ON SALE  - CurrMarg(AvgCost)

	FROM Price(nolock) AS P
	INNER JOIN
		PriceChgType PCT (nolock) ON PCT.PriceChgTypeID = P.PriceChgTypeID
	INNER JOIN
		Store S (nolock) ON P.Store_No = S.Store_No
	INNER JOIN
		StoreJurisdiction SJ (nolock) ON SJ.StoreJurisdictionID = S.StorejurisdictionID
	INNER JOIN 
		Item  (nolock) ON Item.Item_Key = @Item_Key
	INNER JOIN
		-- Get the primary vendor for each store-item
		(SELECT
				SIV.Item_Key,
				SIV.Store_No,
				SIV.Vendor_ID,
				(SELECT TOP 1
					(CASE WHEN ISNULL(IV2.IgnoreCasePack,0) = 1 
							THEN IV2.RetailCasePack 
							ELSE VCH2.Package_Desc1 
							END) AS Package_Desc1
					FROM
						VendorCostHistory VCH2 (NoLock)
						JOIN StoreItemVendor SIV2 (NoLock) ON VCH2.StoreItemVendorID = SIV2.StoreItemVendorID
						LEFT OUTER JOIN ItemVendor IV2 (NoLock) ON SIV2.Vendor_ID = IV2.Vendor_ID
																AND SIV2.Item_Key = IV2.Item_Key
					WHERE
						SIV2.Item_Key = SIV.Item_Key
						AND SIV2.Vendor_ID = SIV.Vendor_ID
						AND SIV2.Store_No = SIV.Store_No
						AND GETDATE() BETWEEN VCH2.StartDate AND VCH2.EndDate
						AND GETDATE() < ISNULL(SIV2.DeleteDate, DATEADD(day, 1, GETDATE()))
					ORDER BY
						VCH2.VendorCostHistoryID DESC) AS Package_Desc1,
				(SELECT TOP 1
					NetCost
					FROM dbo.fn_GetNetCost(@Item_Key,SIV.Store_No,SIV.Vendor_ID,@CurrDate) NC) AS NetCost
			FROM
				StoreItemVendor SIV (NoLock)
			WHERE
				SIV.PrimaryVendor = 1
				AND SIV.Item_Key = @Item_Key) AS SIVCH
				ON P.Item_Key = SIVCH.Item_Key AND S.Store_No = SIVCH.Store_No
		INNER JOIN 
			Vendor V (NoLock)
				ON V.Vendor_ID = SIVCH.Vendor_ID
	WHERE 
		P.Item_Key = @Item_Key
		AND SIVCH.Package_Desc1 IS NOT NULL
	GROUP BY
		V.CurrencyID,
		SJ.currencyID,
		SIVCH.Store_No, 
		SIVCH.Item_Key,
		SIVCH.Package_Desc1,
		SIVCH.NetCost,
		S.Store_Name, 
		V.CompanyName,	
		PCT.On_Sale,
		P.Sale_Price,
		P.Price,
		P.Multiple,
		P.Sale_Multiple,
		Item.Subteam_No
	ORDER BY S.Store_Name
    
    SET NOCOUNT OFF
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetMarginInfo] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetMarginInfo] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetMarginInfo] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetMarginInfo] TO [IRMAReportsRole]
    AS [dbo];

