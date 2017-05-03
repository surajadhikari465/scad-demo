IF EXISTS (SELECT name 
	   FROM   sysobjects 
	   WHERE  name = N'ItemHistoryDel' 
	   AND 	  type = 'TR')
    DROP TRIGGER ItemHistoryDel
GO

CREATE TRIGGER [dbo].[ItemHistoryDel]
ON [dbo].[ItemHistory]
FOR DELETE 
AS 
BEGIN
    DECLARE @Error_No int
    SELECT @Error_No = 0

    UPDATE OnHand
    SET Quantity = OnHand.Quantity - (ISNULL(Deleted.Quantity, 0) * ItemAdjustment.Adjustment_Type),
        Weight = OnHand.Weight - (ISNULL(Deleted.Weight, 0) * ItemAdjustment.Adjustment_Type)
    FROM OnHand
    INNER JOIN
        Deleted
        ON Deleted.Item_Key = OnHand.Item_Key AND
           Deleted.Store_No = OnHand.Store_No AND
           Deleted.SubTeam_No = OnHand.SubTeam_No
    INNER JOIN
        ItemAdjustment
        ON ItemAdjustment.Adjustment_ID = Deleted.Adjustment_ID
    INNER JOIN
        Item (nolock)
        ON Deleted.Item_Key = Item.Item_Key
    WHERE Deleted.DateStamp >= ISNULL(OnHand.LastReset, Deleted.DateStamp)
        AND Adjustment_Type <> 0

    SELECT @Error_No = @@ERROR


-- ************************* Insert into ItemHistoryDeletedQueue for Average Cost Update **** exclude Ingredient items here ***** Alex Z *****
	IF @Error_No = 0
    BEGIN
        
			INSERT INTO ItemHistoryDeletedQueue 
				(
					Store_No, 
					Item_Key, 
					DateStamp, 
					SubTeam_No, 
					ItemHistoryID, 
					Adjustment_ID
				)
			SELECT	DELETED.Store_No, 
					DELETED.Item_Key,
					DELETED.DateStamp,
					DELETED.SubTeam_No,
					DELETED.ItemHistoryID,
					DELETED.Adjustment_ID
			FROM	DELETED
					INNER JOIN Item (nolock) ON Item.Item_Key = DELETED.Item_Key
					INNER JOIN Store (nolock) ON Store.Store_No = DELETED.Store_No
			WHERE		(Ingredient = 0 AND ISNULL(UseLastReceivedCost, 0) = 0)
					AND Item.Subteam_No = DELETED.Subteam_No
					AND DELETED.Adjustment_ID = 5
					AND Store.UseAvgCostHistory = 1
        
			SELECT @Error_No = @@ERROR
    END

    IF @Error_No <> 0
    BEGIN
        ROLLBACK TRAN
        DECLARE @Severity smallint
        SELECT @Severity = ISNULL((SELECT severity FROM master.dbo.sysmessages WHERE error = @Error_No), 16)
        RAISERROR ('ItemHistoryDel trigger failed with @@ERROR: %d', @Severity, 1, @Error_No)
    END
END
GO

