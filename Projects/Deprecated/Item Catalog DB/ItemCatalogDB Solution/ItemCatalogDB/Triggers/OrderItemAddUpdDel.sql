/****** Object:  Trigger [OrderItemAddUpdDel]    Script Date: 09/24/2008 11:57:13 ******/
IF  EXISTS (SELECT * FROM sys.triggers WHERE object_id = OBJECT_ID(N'[dbo].[OrderItemAddUpdDel]'))
DROP TRIGGER [dbo].[OrderItemAddUpdDel]
GO
/****** Object:  Trigger [OrderItemAddUpdDel]    Script Date: 09/24/2008 11:57:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE TRIGGER [dbo].[OrderItemAddUpdDel]
ON [dbo].[OrderItem]
FOR DELETE, INSERT, UPDATE 
AS 
BEGIN
    BEGIN TRY
    
    
    -- StoreOps Export 
	UPDATE OrderExportQueue
	SET QueueInsertedDate = GetDate(), DeliveredToStoreOpsDate = null
	WHERE OrderHeader_ID in (
	    SELECT DISTINCT OH.OrderHeader_ID
		FROM 
			OrderHeader OH
		INNER JOIN
			(SELECT OrderHeader_ID FROM Inserted
			 UNION
			 SELECT OrderHeader_ID FROM Deleted) OI
			ON OI.OrderHeader_ID = OH.OrderHeader_ID
		WHERE (OH.SentDate IS NOT NULL)
		-- excludes closed and reconciled (reconciled in StoreOPs)warehouse orders sending updates
		and Not(OH.OrderType_Id = 2 and OH.CloseDate is not null and OH.Return_Order=0)
		 	
	)
	IF @@ROWCOUNT=0
	BEGIN
		INSERT INTO OrderExportQueue
		SELECT DISTINCT OH.OrderHeader_ID, GetDate(), null
		FROM 
			OrderHeader OH
		INNER JOIN
			(SELECT OrderHeader_ID FROM Inserted
			 UNION
			 SELECT OrderHeader_ID FROM Deleted) OI
			ON OI.OrderHeader_ID = OH.OrderHeader_ID
		WHERE (OH.SentDate IS NOT NULL)
		-- excludes closed and reconciled (reconciled in StoreOPs)warehouse orders sending updates
		and Not(OH.OrderType_Id = 2 and OH.CloseDate is not null and OH.Return_Order=0)
	END

    --Copy the Cost and CostUnit value in OrigReceivedItemCost and OrigReceivedItemUnit
    --when data is inserted into OrderItem table
    UPDATE OI
    SET OI.OrigReceivedItemCost = I.Cost, OI.OrigReceivedItemUnit = I.CostUnit
    FROM OrderItem OI
    INNER JOIN INSERTED I ON
		I.OrderItem_ID  = OI.OrderItem_ID 
    WHERE NOT EXISTS (SELECT * FROM DELETED D WHERE D.OrderItem_ID  = I.OrderItem_ID)    	 
    
    --Copy the OrderDate from OrderHeader table insert into OHOrderDate when data is inserted into OrderItem table
    UPDATE OI
    SET OI.OHOrderDate = OH.OrderDate 
    FROM OrderItem OI
    INNER JOIN INSERTED I ON
            I.OrderItem_ID  = OI.OrderItem_ID 
    INNER JOIN OrderHeader OH ON
            OI.OrderHeader_ID = OH.OrderHeader_ID
    
    
            -- Capture for the update avgcost/onhand process 
        INSERT INTO ItemHistoryInsertedQueue (Store_No, Item_Key, DateStamp, SubTeam_No, ItemHistoryID, Adjustment_ID)
        SELECT IH.Store_No, IH.Item_Key, IH.DateStamp, IH.SubTeam_No, IH.ItemHistoryID, IH.Adjustment_ID
        FROM Inserted
        INNER JOIN Deleted ON Inserted.OrderItem_ID = Deleted.OrderItem_ID
        INNER JOIN ItemHistory IH (nolock) ON IH.OrderItem_ID = Inserted.OrderItem_ID
        INNER JOIN Item (nolock) ON Item.Item_Key = Inserted.Item_Key
        INNER JOIN Store (nolock) ON Store.Store_No = IH.Store_No
        -- exclude ingredient items unless the affected store is a Distribution Center
        WHERE (((Ingredient = 0 AND ISNULL(UseLastReceivedCost, 0) = 0) AND (Item.Subteam_No = IH.Subteam_No)) OR Store.Distribution_Center = 1)
            AND IH.Adjustment_ID = 5
            AND (
                ((Inserted.ReceivedItemCost + Inserted.ReceivedItemFreight) <> (Deleted.ReceivedItemCost + Deleted.ReceivedItemFreight))
                OR Inserted.UnitsReceived <> Deleted.UnitsReceived
                )
                        
        -- For updates, keep receiving ItemHistory in synch
        -- Use a table variable and a while loop instead of a cursor
        DECLARE @ReceivedList TABLE (OrderItem_ID int PRIMARY KEY)
        DECLARE @OrderItem_ID int
        
        INSERT INTO @ReceivedList
        SELECT Inserted.OrderItem_ID
        FROM Inserted
        INNER JOIN Deleted ON Inserted.OrderItem_ID = Deleted.OrderItem_ID
        WHERE (Inserted.UnitsReceived <> Deleted.UnitsReceived)
        
        WHILE EXISTS (SELECT * FROM @ReceivedList)
        BEGIN
            SET @OrderItem_ID = (SELECT TOP 1 OrderItem_ID FROM @ReceivedList)
            
            EXEC InsertReceivingItemHistory @OrderItem_ID, 0  -- Does not matter who the user is as far as ItemHistory since there is a link to OrderItem.  The receiver should be recorded in the OrderHeader record.
            
            DELETE @ReceivedList WHERE OrderItem_ID = @OrderItem_ID
        END
                                           
    END TRY
    BEGIN CATCH
        DECLARE @err_no int, @err_sev int, @err_msg nvarchar(4000)
        SELECT @err_no = ERROR_NUMBER(), @err_sev = ERROR_SEVERITY(), @err_msg = ERROR_MESSAGE()
        IF @@TranCount > 0 
          begin
            ROLLBACK TRAN
          end
        
        RAISERROR ('OrderItemAddUpdDel trigger failed with @@ERROR: %d - %s', @err_sev, 1, @err_no, @err_msg)
    END CATCH	
    
END
GO
