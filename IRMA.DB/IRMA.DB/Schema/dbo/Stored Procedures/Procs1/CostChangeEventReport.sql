CREATE PROCEDURE [dbo].[CostChangeEventReport]
@Vendor_ID int,
@Team_No int,
@SubTeam_No int
AS 

BEGIN

SET NOCOUNT ON

SELECT Item.Item_Key,
	S.Store_Name,
	V.CompanyName,	
    Item.Item_Description,
	ISNULL(dbo.fn_GetCurrentCost(Item.Item_Key, SIV.Store_No), 0) AS CurrCost,
	VCH.UnitCost,
    (ISNULL(dbo.fn_GetCurrentCost(Item.Item_Key, SIV.Store_No),0) - VCH.UnitCost) As Diff,
	PCT = CASE
		WHEN (ISNULL(dbo.fn_GetCurrentCost(Item.Item_Key, SIV.Store_No), 0)) <> 0 AND (ISNULL(VCH.UnitCost, 0)) <> 0 THEN ((dbo.fn_GetCurrentCost(Item.Item_Key, SIV.Store_No) / VCH.UnitCost) * 100)
		WHEN (ISNULL(dbo.fn_GetCurrentCost(Item.Item_Key, SIV.Store_No), 0)) = 0 AND (ISNULL(VCH.UnitCost, 0)) > 0 THEN -100
		WHEN (ISNULL(dbo.fn_GetCurrentCost(Item.Item_Key, SIV.Store_No), 0)) > 0 AND (ISNULL(VCH.UnitCost, 0)) = 0 THEN 100
		ELSE 0
		END,
	ItemUnit.Unit_Abbreviation As UOM,
	Item.Package_Desc1 AS CasePack, 
    SubTeam.SubTeam_Name,
    VCH.StartDate, 
	VCH.EndDate,
	ISNULL(dbo.fn_GetCurrentNetCost(Item.Item_Key, SIV.Store_No), 0) AS NetCost
    FROM VendorCostHistory VCH
    INNER JOIN
        StoreItemVendor SIV
        on SIV.StoreItemVendorID = VCH.StoreItemVendorID   
    INNER JOIN
        Item (nolock)
        ON Item.Item_Key = SIV.Item_Key
    INNER JOIN
        ItemIdentifier
        ON ItemIdentifier.Item_Key = Item.Item_Key AND ItemIdentifier.Default_Identifier = 1
    INNER JOIN
        SubTeam
        ON SubTeam.SubTeam_No = Item.SubTeam_No
    INNER JOIN
        ItemUnit
        ON ItemUnit.Unit_ID = Item.Package_Unit_ID
	INNER JOIN 
		Vendor V
		ON V.Vendor_ID = @Vendor_ID
	INNER JOIN
		Store S
		ON S.Store_No = SIV.Store_No
    WHERE Item.Deleted_Item = 0 AND
		SubTeam.Team_No = ISNULL(@Team_No, SubTeam.Team_No) AND 
        SubTeam.SubTeam_No = ISNULL(@SubTeam_No, SubTeam.SubTeam_No)
	ORDER BY Item.Item_Key, VCH.StartDate
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[CostChangeEventReport] TO [IRMAReportsRole]
    AS [dbo];

