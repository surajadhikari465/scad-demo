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
GRANT EXECUTE
    ON OBJECT::[dbo].[GetAdjustmentInfo] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetAdjustmentInfo] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetAdjustmentInfo] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetAdjustmentInfo] TO [IRMAReportsRole]
    AS [dbo];

