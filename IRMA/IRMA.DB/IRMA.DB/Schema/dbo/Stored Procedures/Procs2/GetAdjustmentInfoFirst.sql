CREATE PROCEDURE dbo.GetAdjustmentInfoFirst
	(@Store_No int)
AS 

SELECT 
	Item.Item_Key,
	Item_Description,
	Identifier,
	SubTeam_No,
	Package_Desc1,
	Package_Desc2,
	Unit_Name,
	Shipper_Item,
	CatchWeightRequired,
	CostedByWeight,
	dbo.fn_GetRetailUnitAbbreviation(Item.Item_Key) As RetailUOM,
	dbo.fn_GetDistributionUnitAbbreviation(Item.Item_Key) As DCUOM,
	dbo.fn_IsCaseItemUnit(Item.Retail_Unit_ID) As IsPackageUnit,
	Store.Distribution_Center,
	Store.WFM_Store
FROM ItemUnit RIGHT JOIN (
       ItemIdentifier INNER JOIN Item ON (ItemIdentifier.Item_key = Item.Item_Key AND ItemIdentifier.Default_Identifier = 1) 
     ) ON (Item.Package_Unit_ID = ItemUnit.Unit_ID) 
CROSS JOIN Store 
WHERE Item.Item_Key = (SELECT MIN(Item_Key) FROM Item)
AND 	 Store.Store_No = ISNULL(@Store_No, (SELECT MIN(SM.Store_No) from Store SM))
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetAdjustmentInfoFirst] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetAdjustmentInfoFirst] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetAdjustmentInfoFirst] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetAdjustmentInfoFirst] TO [IRMAReportsRole]
    AS [dbo];

