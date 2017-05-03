/****** Object:  StoredProcedure [dbo].[MarginByVendorReport_UseLastCost]    Script Date: 10/10/2006 09:45:42 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[MarginByVendorReport_UseLastCost]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].[MarginByVendorReport_UseLastCost]
GO

/****** Object:  StoredProcedure [dbo].[MarginByVendorReport_UseLastCost]    Script Date: 10/05/2006 16:27:01 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[MarginByVendorReport_UseLastCost]
    @Store_No int,
    @Vendor_ID int,
    @Minval int = 0,
    @Maxval int = 100,
    @Range bit = 1
AS

-- **************************************************************************
-- Procedure: MarginByVendorReport_UseLasCost()
--    Author: n/a
--      Date: n/a
--
-- Description:
-- This procedure is called from multiple RDL files and generates reports consumed
-- by SSRS procedures.
--
-- Modification History:
-- Date        Init	Comment
-- 01/11/2013  BAS	Update i.Discontinue_Item filter in WHERE clause to
--					account for schema change. Renamed file to .sql. Coding Standards.
-- **************************************************************************

BEGIN
	SET NOCOUNT ON
	DECLARE @CurrDate datetime
	SELECT @CurrDate = CONVERT(datetime, CONVERT(varchar(255), GETDATE(), 101))
	if @Range = 1
		begin
		select distinct * from (select *, dbo.fn_GetMargin(Price, Multiple, Cost) as Margin
			from (	SELECT
						Item.Item_Description,
						SIV.Vendor_ID,
						Vendor.CompanyName,
						Price.Store_No,
						Store.Store_Name,
						Price.Multiple,
						Price.Price,
						dbo.fn_GetCurrentNetCost(Item.Item_Key, Store.Store_No) /VCH.Package_Desc1 AS Cost,--ADDED for Margin Calculation
						ItemBrand.Brand_Name,
						ItemIdentifier.Identifier,
						Item.Item_Key,
						VCH.Package_Desc1
					FROM
						StoreItemVendor SIV (nolock)
						INNER JOIN Price (nolock) ON	Price.Item_Key = SIV.Item_Key
														AND Price.Store_No = SIV.Store_No
						INNER JOIN Item (nolock) ON Item.Item_Key = Price.Item_Key
						INNER JOIN ItemIdentifier (nolock) ON ItemIdentifier.Item_Key = Item.Item_Key
																AND ItemIdentifier.Default_Identifier = 1
						INNER JOIN Store (nolock) ON Store.Store_No = Price.Store_No
						INNER JOIN Vendor (nolock) ON Vendor.Vendor_ID = SIV.Vendor_ID
						LEFT JOIN ItemBrand (nolock) ON Item.Brand_ID = ItemBrand.Brand_ID
						INNER JOIN ItemVendor iv (nolock) ON ( iv.Item_Key = ItemIdentifier.Item_Key 
															and iv.Vendor_ID = SIV.Vendor_ID)
						-- use function to retrieve only current cost records
						INNER JOIN dbo.fn_VendorCostAll(getdate()) vch ON ( vch.Item_Key = iv.Item_Key
																		   and vch.Vendor_ID = iv.Vendor_ID 
																			and siv.Store_no = vch.Store_no)
					WHERE
						Retail_Sale = 1
						AND Deleted_Item = 0
						AND SIV.DiscontinueItem = 0
						AND Price.Price > 0
						AND Price.Store_No = ISNULL(@Store_No, Price.Store_No)
						AND (WFM_Store = 1 OR Mega_Store = 1)
						AND SIV.Vendor_ID = @Vendor_ID
						AND @CurrDate < ISNULL(SIV.DeleteDate, DATEADD(day, 1, @CurrDate))
					) as inner_result
				where cost is not null
			   ) as outer_result
			where margin >= @Minval and margin < @Maxval
			ORDER BY Store_No, Identifier
		end
	else
		begin
			select distinct * from (select *, dbo.fn_GetMargin(Price,Multiple,Cost) as Margin
				from (	SELECT
							Item.Item_Description,
							SIV.Vendor_ID,
							Vendor.CompanyName,
							Price.Store_No,
							Store.Store_Name,
							Price.Multiple,Price.Price,
							dbo.fn_GetCurrentNetCost(Item.Item_Key, Store.Store_No)/VCH.Package_Desc1 AS Cost,--ADDED for Margin Calculation
							ItemBrand.Brand_Name,
							ItemIdentifier.Identifier,
							Item.Item_Key,
							VCH.Package_Desc1
						FROM
							StoreItemVendor SIV (nolock)
							INNER JOIN Price (nolock) ON Price.Item_Key = SIV.Item_Key
														AND Price.Store_No = SIV.Store_No
							INNER JOIN Item (nolock) ON Item.Item_Key = Price.Item_Key
							INNER JOIN ItemIdentifier (nolock) ON ItemIdentifier.Item_Key = Item.Item_Key
																	AND ItemIdentifier.Default_Identifier = 1
							INNER JOIN Store (nolock) ON Store.Store_No = Price.Store_No
							INNER JOIN Vendor (nolock) ON Vendor.Vendor_ID = SIV.Vendor_ID
							LEFT JOIN ItemBrand (nolock) ON Item.Brand_ID = ItemBrand.Brand_ID
							INNER JOIN ItemVendor iv (nolock) ON ( iv.Item_Key = ItemIdentifier.Item_Key 
																and iv.Vendor_ID = SIV.Vendor_ID)
							-- use function to retrieve only current cost records
							INNER JOIN dbo.fn_VendorCostAll(getdate()) vch ON ( vch.Item_Key = iv.Item_Key
																			   and vch.Vendor_ID = iv.Vendor_ID 
																				and siv.Store_no = vch.Store_no)
						WHERE
							Retail_Sale = 1 
							AND Deleted_Item = 0 
							AND SIV.DiscontinueItem = 0
							AND Price.Price > 0 
							AND Price.Store_No = ISNULL(@Store_No, Price.Store_No)
							AND (WFM_Store = 1 OR Mega_Store = 1)
							AND SIV.Vendor_ID = @Vendor_ID
							AND @CurrDate < ISNULL(SIV.DeleteDate, DATEADD(day, 1, @CurrDate))
					  ) as inner_result
				where cost is not null
			) as outer_result
			where margin >= @Maxval or margin < @Minval 
			ORDER BY Store_No,Identifier
		end

SET NOCOUNT OFF
END
GO
