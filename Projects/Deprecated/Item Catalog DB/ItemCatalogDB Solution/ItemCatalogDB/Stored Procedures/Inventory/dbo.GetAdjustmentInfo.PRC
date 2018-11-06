SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetAdjustmentInfo]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
	drop procedure [dbo].[GetAdjustmentInfo]
GO

CREATE PROCEDURE dbo.GetAdjustmentInfo
	(@Item_Key int, 
	 @Store_No int)
AS 

SELECT 
	Item.Item_Key, 
	Item_Description, 
	Identifier, 
	SubTeam_No, 
	dbo.fn_GetCurrentVendorPackage_Desc1(@Item_Key, @Store_No) AS Package_Desc1, 
	Package_Desc2, 
	Unit_Name, 
	Shipper_Item,
	CatchWeightRequired,
	CostedByWeight,
	dbo.fn_GetRetailUnitAbbreviation(@Item_Key) As RetailUOM,
	dbo.fn_GetDistributionUnitAbbreviation(@Item_Key) As DCUOM,
	dbo.fn_IsDistributionCenter(@Store_No) As IsDC,
	dbo.fn_IsCaseItemUnit(Item.Retail_Unit_ID) As IsPackageUnit,
	Store.Distribution_Center,
	Store.WFM_Store
FROM ItemUnit RIGHT JOIN (
       ItemIdentifier INNER JOIN Item ON (ItemIdentifier.Item_key = Item.Item_Key AND ItemIdentifier.Default_Identifier = 1) 
     ) ON (Item.Package_Unit_ID = ItemUnit.Unit_ID) 
CROSS JOIN Store 
WHERE Item.Item_Key = @Item_Key
AND 	 Store.Store_No = ISNULL(@Store_No, (SELECT MIN(SM.Store_No) from Store SM))
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

