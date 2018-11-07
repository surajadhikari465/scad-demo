

/****** Object:  Trigger [ItemHistoryAdd]    Script Date: 10/11/2012 09:31:19 ******/
IF  EXISTS (SELECT * FROM sys.triggers WHERE object_id = OBJECT_ID(N'[dbo].[ItemHistoryAdd]'))
DROP TRIGGER [dbo].[ItemHistoryAdd]
GO



/****** Object:  Trigger [dbo].[ItemHistoryAdd]    Script Date: 10/11/2012 09:31:22 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER OFF
GO

CREATE TRIGGER [dbo].[ItemHistoryAdd]
ON [dbo].[ItemHistory]
FOR INSERT 
AS 
BEGIN
    DECLARE @Error_No int
    SET @Error_No = 0
     UPDATE OnHand 
    SET Quantity = InsertedData.Quantity,
        Weight = InsertedData.Weight,
        LastReset = InsertedData.LastReset
    FROM 
    (
	select Item_Key, Store_No, SubTeam_No,Quantity,Weight,LastReset from 
		(
			select Item_Key, Store_No, SubTeam_No, sum(Quantity) over (partition by Item_Key, Store_No, SubTeam_No)+ onHandQuantity Quantity,
				   sum(Weight) over (partition by Item_Key, Store_No, SubTeam_No) + OnHandWeight Weight ,
				   ROW_NUMBER () over (partition by Item_Key, Store_No, SubTeam_No order by Item_Key) Row,LastReset,OnHandWeight,onHandQuantity		   
			FROM 
			(
				SELECT Inserted.Item_Key, Inserted.Store_No, Inserted.SubTeam_No, 
					   ISNULL(Inserted.Quantity, 0) * ItemAdjustment.Adjustment_Type Quantity, 
					   ISNULL(Inserted.Weight, 0) * ItemAdjustment.Adjustment_Type Weight,
					   OnHand.Weight OnHandWeight,OnHand.Quantity OnHandQuantity,
						CASE WHEN Inserted.Adjustment_ID = 2 THEN Inserted.DateStamp ELSE LastReset END LastReset
					 	FROM Inserted
				INNER JOIN
					ItemAdjustment
					ON ItemAdjustment.Adjustment_ID = Inserted.Adjustment_ID
				INNER JOIN
					OnHand
					ON Inserted.Item_Key = OnHand.Item_Key AND
					   Inserted.Store_No = OnHand.Store_No AND
					   Inserted.SubTeam_No = OnHand.SubTeam_No
				WHERE Adjustment_Type <> 0
			) a 
		) b where ROW = 1
	) InsertedData
where				InsertedData.Item_Key = OnHand.Item_Key AND
				   InsertedData.Store_No = OnHand.Store_No AND
				   InsertedData.SubTeam_No = OnHand.SubTeam_No


    SELECT @Error_No = @@ERROR

    IF @Error_No = 0
    BEGIN
        INSERT INTO OnHand (Item_Key, Store_No, SubTeam_No, Quantity, Weight, LastReset)		
		select Item_Key, Store_No, SubTeam_No,Quantity,Weight,DateStamp from 
		(
			select Item_Key, Store_No, SubTeam_No, sum(Quantity) over (partition by Item_Key, Store_No, SubTeam_No) Quantity,
				   sum(Weight) over (partition by Item_Key, Store_No, SubTeam_No) Weight,
				   ROW_NUMBER () over (partition by Item_Key, Store_No, SubTeam_No order by Item_Key) Row,DateStamp
			FROM 
			(
				SELECT Inserted.Item_Key, Inserted.Store_No, Inserted.SubTeam_No, 
					   ISNULL(Inserted.Quantity, 0) * ItemAdjustment.Adjustment_Type Quantity, 
					   ISNULL(Inserted.Weight, 0) * ItemAdjustment.Adjustment_Type Weight,
					   DateStamp
				FROM Inserted
				INNER JOIN
					ItemAdjustment
					ON ItemAdjustment.Adjustment_ID = Inserted.Adjustment_ID
				LEFT JOIN
					OnHand
					ON Inserted.Item_Key = OnHand.Item_Key AND
					   Inserted.Store_No = OnHand.Store_No AND
					   Inserted.SubTeam_No = OnHand.SubTeam_No
				WHERE OnHand.Item_Key IS NULL
					AND Adjustment_Type <> 0
			) a 
		) b where ROW = 1

        SELECT @Error_No = @@ERROR
    END

    IF @Error_No = 0 AND EXISTS (SELECT * FROM Inserted WHERE Adjustment_ID = 2)
    BEGIN
        -- If inserting cycle counts, handle the fact that they may not be inserted until
        -- after other item movement that occurred after the count has been recorded.
        -- Adjust the reset amount by the sum of what occurred after the reset datestamp
        UPDATE OnHand
        SET Quantity = OnHand.Quantity + ISNULL(IH.Quantity, 0),
            Weight = OnHand.Weight + ISNULL(IH.Weight, 0)
        FROM OnHand
        INNER JOIN
            (SELECT Inserted.Item_Key, Inserted.Store_No, Inserted.SubTeam_No,
                    SUM(ISNULL(ItemHistory.Quantity, 0) * ItemAdjustment.Adjustment_Type) As Quantity,
                    SUM(ISNULL(ItemHistory.Weight, 0) * ItemAdjustment.Adjustment_Type) As Weight
            FROM Inserted
            INNER JOIN
                ItemHistory
                ON ItemHistory.Item_Key = Inserted.Item_Key AND
                   ItemHistory.Store_No = Inserted.Store_No AND
                   ItemHistory.SubTeam_No = Inserted.SubTeam_No
            INNER JOIN
                ItemAdjustment (nolock)
                ON ItemAdjustment.Adjustment_ID = ItemHistory.Adjustment_ID
            INNER JOIN
                Item (nolock)
                ON Inserted.Item_Key = Item.Item_Key
            WHERE Inserted.Adjustment_ID = 2 AND ItemHistory.Adjustment_ID <> 2
                AND ItemHistory.DateStamp > Inserted.DateStamp
                AND Adjustment_Type <> 0
            GROUP BY Inserted.Item_Key, Inserted.Store_No, Inserted.SubTeam_No) IH
            ON IH.Item_Key = OnHand.Item_Key AND
               IH.Store_No = OnHand.Store_No AND
               IH.SubTeam_No = OnHand.SubTeam_No
               
               
           

        SELECT @Error_No = @@ERROR
    END
    
    
            -- Capture for the update avgcost/onhand process 
        INSERT INTO ItemHistoryInsertedQueue (Store_No, Item_Key, DateStamp, SubTeam_No, ItemHistoryID, Adjustment_ID)
        SELECT Inserted.Store_No, Inserted.Item_Key, Inserted.DateStamp, Inserted.SubTeam_No, Inserted.ItemHistoryID, Inserted.Adjustment_ID
        FROM Inserted
        INNER JOIN
            Item (nolock) ON Item.Item_Key = Inserted.Item_Key
        INNER JOIN
            Store (nolock) ON Store.Store_No = Inserted.Store_No
        -- exclude ingredient items unless the affected store is a Distribution Center
        WHERE (
                     ((Ingredient = 0 AND ISNULL(UseLastReceivedCost, 0) = 0) AND (Item.Subteam_No = Inserted.Subteam_No))
                     OR Store.Distribution_Center = 1
                    )    

    IF @Error_No <> 0
    BEGIN
        ROLLBACK TRAN
        DECLARE @Severity smallint
        SELECT @Severity = ISNULL((SELECT severity FROM master.dbo.sysmessages WHERE error = @Error_No), 16)
        RAISERROR ('ItemHistoryAdd trigger failed with @@ERROR: %d', @Severity, 1, @Error_No)
    END
END
GO


