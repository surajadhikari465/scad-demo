CREATE PROCEDURE dbo.msqSeafoodInventory
    @Store_No INT, 
    @SubTeam_No INT 
AS
/**********************************************************************************************************************************************************************************************************************************
CHANGE LOG
DEV					DATE					TASK						Description
-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
DN					2013.01.11				TFS 8755					Using the function dbo.fn_GetDiscontinueStatus instead of the Discontinue_Item field in the Item table.
**********************************************************************************************************************************************************************************************************************************/

BEGIN
    SET NOCOUNT ON

	SELECT 
		Category_Name AS 'Category', 
		Identifier, 
		Item_Description, 
		Item.Package_Desc1 AS 'Pack', 
		Item.Package_Desc2 AS 'Size', 
		Unit_Name AS 'Unit',
        ISNULL(dbo.fn_AvgCostHistory(Item.Item_Key, Price.Store_No, Item.SubTeam_No, GETDATE()), 0) AS 'AvgCost'
    FROM ItemUnit RetailUnit (NOLOCK) RIGHT JOIN (
           ItemCategory (NOLOCK) RIGHT JOIN (
             ItemOrigin (NOLOCK) RIGHT JOIN (
               StoreSubTeam SST (nolock) INNER JOIN (
                 ItemIdentifier (NOLOCK) INNER JOIN (
                     Price (NOLOCK) INNER JOIN 
                        Item (NOLOCK) ON (Item.Item_Key = Price.Item_Key AND Price.Store_No = @Store_No)
                 ) ON (ItemIdentifier.Item_Key = Item.Item_Key AND ItemIdentifier.Default_Identifier = 1)
               ) ON (SST.Store_No = Price.Store_No AND SST.SubTeam_No = Item.SubTeam_No)
             ) ON (ItemOrigin.Origin_ID = Item.Origin_ID)
           ) ON (ItemCategory.Category_ID = Item.Category_ID)	
         ) ON (RetailUnit.Unit_ID = Item.Retail_Unit_ID)
    WHERE 
		(Item.SubTeam_No = @SubTeam_No) 
		AND (Item.WFM_Item >= 0) 
		AND dbo.fn_GetDiscontinueStatus(Item.Item_Key,NULL,NULL) = 0
    ORDER BY 
		Category_Name, 
		Item_Description

    SET NOCOUNT OFF
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[msqSeafoodInventory] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[msqSeafoodInventory] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[msqSeafoodInventory] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[msqSeafoodInventory] TO [IRMAReportsRole]
    AS [dbo];

