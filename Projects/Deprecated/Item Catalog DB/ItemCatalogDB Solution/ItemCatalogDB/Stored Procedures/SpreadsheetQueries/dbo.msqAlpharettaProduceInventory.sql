SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'dbo.msqAlpharettaProduceInventory') and OBJECTPROPERTY(id, N'IsProcedure') = 1) drop procedure dbo.msqAlpharettaProduceInventory
GO


CREATE PROCEDURE dbo.msqAlpharettaProduceInventory

AS
/**********************************************************************************************************************************************************************************************************************************
CHANGE LOG
DEV					DATE					TASK						Description
-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
DN					2013.01.11				TFS 8755					Reference the field DiscontinueItem in the StoreItemVendor table instead of the Discontinue_Item field in the Item table.
**********************************************************************************************************************************************************************************************************************************/

BEGIN
    SET NOCOUNT ON

DECLARE @CurrDate datetime

SELECT @CurrDate = CONVERT(datetime, CONVERT(varchar(255), GETDATE(), 101))


SELECT 
	Item_Description AS 'Description', 
	Item.Package_Desc1 AS 'Pack', 
	Unit_Name AS 'Unit', 
	(UnitCost + UnitFreight) * Item.Package_Desc1 AS 'Cost'
FROM Item (nolock)
	JOIN ItemIdentifier II(nolock)				ON	II.Item_Key = Item.Item_Key AND 
													Default_Identifier = 1
	JOIN StoreItemVendor SIV(nolock)			ON	SIV.Item_Key = Item.Item_Key AND 
													Vendor_ID = 4798 AND 
													Store_No = 101
	JOIN dbo.fn_VendorCostAll(@CurrDate) VCA	ON	VCA.Item_Key = SIV.Item_Key AND 
													VCA.Vendor_ID = 4798 AND 
													VCA.Store_No = 101
	LEFT JOIN ItemCategory IC(nolock)			ON	IC.category_ID = Item.Category_ID
	LEFT JOIN ItemUnit IU(nolock)				ON	IU.Unit_ID = Item.Package_Unit_ID
WHERE 
	(Item.SubTeam_No = 1700) AND
	(Item.WFM_Item >= 0) AND
	SIV.DiscontinueItem = 0 AND
	Item.Category_ID IS NOT NULL

ORDER BY 
	Category_Name, 
	Item_Description


    SET NOCOUNT OFF
END

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


