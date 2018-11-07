CREATE PROCEDURE [dbo].[InventoryBalance]
    @Warehouse_ID int,
    @SubTeam_No int
AS

   -- Modification History:
   -- Date			Init		Comment
   -- 10/03/2012	AlexB		Removed all references to ItemCaseHistory
BEGIN
    SET NOCOUNT ON
    
    DECLARE @MarkUpDollars smallmoney
    SELECT @MarkUpDollars = CaseDistHandlingCharge FROM Vendor (nolock) where Vendor.Store_No = @Warehouse_ID


    -- Get the Item Unit ID's so we can call CostConverion
    DECLARE @Case int, @Pound int, @Unit int               
    SELECT @Case = Unit_ID FROM ItemUnit WHERE EDISysCode = 'CA'
    SELECT @Unit = Unit_ID FROM ItemUnit WHERE EDISysCode = 'UN'
    SELECT @Pound = Unit_ID FROM ItemUnit WHERE EDISysCode = 'LB'


    --Temporary table to hold all the items and the Average Cost that we are working with.
    CREATE TABLE #Details
        (
	    ItemKey int,
        SubTeamNo int,
        CasesOnHand decimal(18, 4),
        UnitsOnHand decimal(18, 4),
        AvgCaseCost money--,
        --AvgCost money
	    )	   
    CREATE INDEX idxDetails_ItemKey_SubTeamNo ON #Details (ItemKey, SubTeamNo)


    --Fill the Temporary table 
    INSERT INTO #Details
    SELECT ItemHistory.Item_Key, ItemHistory.SubTeam_No,
           --Calculate Cases On Hand       
           SUM(
                      dbo.fn_CostConversion(ISNULL(ItemHistory.Quantity, 0) + ISNULL(ItemHistory.Weight, 0),
                                            @Case,
                                            CASE WHEN Item.CostedByWeight = 1 THEN @pound ELSE @unit END,
                                            Item.Package_Desc1,
                                            Item.Package_Desc2 ,
                                            Item.Package_Unit_ID
                                           )
                      * ItemAdjustment.Adjustment_Type
              )  AS CasesOnHand,
           SUM((ISNULL(ItemHistory.Quantity, 0) + ISNULL(ItemHistory.Weight, 0)
                    ) * ItemAdjustment.Adjustment_Type
              ) AS UnitsOnHand,
           --Calculate AvgCaseCost
           ISNULL(dbo.fn_CostConversion(dbo.fn_AvgCostHistory(ItemHistory.Item_Key, @Warehouse_ID, ISNULL(@SubTeam_No, Item.SubTeam_No), GETDATE()),
                                        CASE WHEN Item.CostedByWeight = 1 THEN @pound ELSE @unit END,
                                        @Case,
                                        Item.Package_Desc1,
                                         Item.Package_Desc2 ,
                                        Item.Package_Unit_ID
                                       ),
                  0
                 )--,
          --Calculate AvgCost (We would like to not have two calls to fn_AvgCostHistory, but this 
          --is the path of least resistance as we don't have time right now to come up with a better solution)
          --ISNULL(dbo.fn_AvgCostHistory(ItemHistory.Item_Key, @Warehouse_ID, @SubTeam_No, GETDATE()), 0) AvgCost
    FROM OnHand 
      INNER JOIN
         ItemHistory (nolock)
         ON OnHand.Item_Key = ItemHistory.Item_Key AND 
            OnHand.Store_No = ItemHistory.Store_No AND 
            OnHand.SubTeam_No = ItemHistory.SubTeam_No
      INNER JOIN
         ItemAdjustment (nolock) 
         ON (ItemHistory.Adjustment_ID = ItemAdjustment.Adjustment_ID)
      INNER JOIN
         Item (nolock) 
         ON (Item.Item_Key = ItemHistory.Item_Key)
      -- Limit to items they actually distribute
      INNER JOIN
		 ItemVendor IV (nolock)
		 ON IV.Item_Key = OnHand.Item_Key 
			AND IV.DeleteDate IS NULL
			AND IV.Vendor_ID = (SELECT Vendor_ID FROM Vendor (nolock) WHERE Vendor.Store_No = @Warehouse_ID)     
    WHERE ItemHistory.Store_No = @Warehouse_ID AND ItemHistory.SubTeam_No = ISNULL(@SubTeam_No, Item.SubTeam_No)
          AND ItemHistory.DateStamp >= OnHand.LastReset
    GROUP BY ItemHistory.Item_Key, ItemHistory.SubTeam_No, Item.SubTeam_No, Item.Package_Desc1, Item.Package_Desc2, Item.Package_Unit_ID,
             Item.CostedByWeight
    HAVING SUM((ISNULL(ItemHistory.Quantity, 0) + ISNULL(ItemHistory.Weight, 0)
                         ) * ItemAdjustment.Adjustment_Type
              ) <> 0


    SELECT Details.ItemKey Item_Key, Item.Item_Description, II.Identifier, 
           ISNULL(Details.AvgCaseCost, 0) AS ExtCost,                      
           CASE WHEN Details.AvgCaseCost <> 0 
                  THEN Details.AvgCaseCost +  @MarkUpDollars
                  ELSE 0 
                END AS MarkupExtCost,
           IC.Category_Name,
           ISNULL(Details.CasesOnHand, 0) as Balance,
           ISNULL(Details.AvgCaseCost * Details.CasesOnHand, 0) AS LineTotal --potentialy needs to include markup
    FROM Item (nolock)
    INNER JOIN
        ItemIdentifier II (nolock)
        ON II.Item_Key = Item.Item_Key AND II.Default_Identifier = 1
	-- Limit to items they actually distribute
	INNER JOIN
		ItemVendor IV (nolock)
		ON IV.Item_Key = Item.Item_Key 
			AND IV.DeleteDate IS NULL
			AND IV.Vendor_ID = (SELECT Vendor_ID FROM Vendor (nolock) WHERE Vendor.Store_No = @Warehouse_ID)
    LEFT JOIN
        ItemCategory IC (nolock)
        ON Item.Category_ID = IC.Category_ID
    LEFT JOIN 
        #Details Details
        ON Item.Item_Key = Details.ItemKey 
    WHERE Item.Deleted_Item = 0 AND dbo.fn_GetDiscontinueStatus(Item.Item_Key, NULL, NULL) = 0
          AND Item.SubTeam_No = CASE WHEN Details.SubTeamNo IS NULL THEN ISNULL(@SubTeam_No, Item.SubTeam_No) ELSE Item.SubTeam_No END

    
    SET NOCOUNT OFF
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[InventoryBalance] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[InventoryBalance] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[InventoryBalance] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[InventoryBalance] TO [IRMAReportsRole]
    AS [dbo];

