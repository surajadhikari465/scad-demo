

/****** Object:  StoredProcedure [dbo].[UpdateOrderBackdate]    Script Date: 10/04/2012 16:19:36 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UpdateOrderBackdate]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[UpdateOrderBackdate]
GO



/****** Object:  StoredProcedure [dbo].[UpdateOrderBackdate]    Script Date: 10/04/2012 16:19:36 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[UpdateOrderBackdate]
    @OrderHeaderList varchar(8000),
    @OrderHeaderListSeparator char(1),
    @DateReceived datetime
AS

   -- Modification History:
   -- Date			Init		Comment
   -- 10/03/2012	AlexB		Removed all references to ItemCaseHistory
BEGIN
    SET NOCOUNT ON

    DECLARE @Error_No int
    SELECT @Error_No = 0

    BEGIN TRAN

    DECLARE @Orders TABLE (OrderHeader_ID int)
    DECLARE @ItemHistory TABLE (
    	Store_No int NOT NULL ,
    	Item_Key int NOT NULL ,
    	DateStamp datetime NOT NULL ,
    	Quantity decimal(18, 4) NULL,
    	Weight decimal(18, 4) NULL,
    	Cost smallmoney NULL,
    	ExtCost smallmoney NULL,
    	Retail smallmoney NULL,
    	Adjustment_ID int NOT NULL ,
    	AdjustmentReason varchar (100),
    	CreatedBy int NOT NULL ,
    	SubTeam_No int NOT NULL ,
    	Insert_Date datetime NULL,
    	ItemHistoryID int NOT NULL ,
    	OrderItem_ID int NULL)

    INSERT INTO @Orders
    SELECT Key_Value 
    FROM fn_Parse_List(@OrderHeaderList, @OrderHeaderListSeparator) O
    INNER JOIN
        OrderHeader 
        ON OrderHeader.OrderHeader_ID = O.Key_Value
    WHERE UploadedDate IS NULL

    SELECT @Error_No = @@ERROR

    IF @Error_No = 0
    BEGIN
        UPDATE OrderHeader
        SET CloseDate = @DateReceived,
            -- AP Upload (SP GetAPUploads) uses RecvLogDate to determine when to upload a PO based upon
            -- a schedule in that SP.  When Accounting wants to backdate a PO, they want it to upload the same
            -- day the backdate is done.  This is why the RecvLogDate must be updated.
            -- RecvLogDate must be updated to an actual Receiving Log close date in order for it to get picked up
            -- on the Receiving Log report.
            RecvLogDate = CASE WHEN RecvLogDate IS NOT NULL
                               THEN ISNULL((SELECT MIN(OH.RecvLogDate)
                                            FROM OrderHeader OH
                                            WHERE OH.ReceiveLocation_ID = OrderHeader.ReceiveLocation_ID
                                                AND OH.RecvLogDate > @DateReceived), RecvLogDate)
                               ELSE NULL END
        FROM OrderHeader
        INNER JOIN 
            @Orders O 
            ON O.OrderHeader_ID = OrderHeader.OrderHeader_ID
    
        SELECT @Error_No = @@ERROR
    END

    IF @Error_No = 0
    BEGIN
        UPDATE OrderItem
        SET DateReceived = @DateReceived
        FROM OrderItem
        INNER JOIN 
            @Orders O 
            ON O.OrderHeader_ID = OrderItem.OrderHeader_ID

        SELECT @Error_No = @@ERROR
    END

    IF @Error_No = 0
    BEGIN
        INSERT INTO @ItemHistory (Store_No, Item_Key, DateStamp, Quantity, Weight, Cost, ExtCost, Retail, Adjustment_ID, AdjustmentReason, CreatedBy, SubTeam_No, Insert_Date, ItemHistoryID, OrderItem_ID)
        SELECT ItemHistory.Store_No, ItemHistory.Item_Key, DATEADD(minute, -1, @DateReceived), ItemHistory.Quantity, ItemHistory.Weight, ItemHistory.Cost, ItemHistory.ExtCost, ItemHistory.Retail, ItemHistory.Adjustment_ID, ItemHistory.AdjustmentReason, ItemHistory.CreatedBy, ItemHistory.SubTeam_No, ItemHistory.Insert_Date, ItemHistory.ItemHistoryID, ItemHistory.OrderItem_ID
        FROM OrderItem 
        INNER JOIN 
            @Orders O 
            ON O.OrderHeader_ID = OrderItem.OrderHeader_ID 
        INNER JOIN
            OrderHeader
            ON OrderHeader.OrderHeader_ID = OrderItem.OrderHeader_ID
        INNER JOIN 
            Vendor RL
            ON RL.Vendor_ID = OrderHeader.ReceiveLocation_ID
        INNER JOIN
            ItemHistory
            ON ItemHistory.Item_Key = OrderItem.Item_Key AND ItemHistory.Store_No = RL.Store_No AND ItemHistory.Adjustment_ID = 2
        INNER JOIN
            (SELECT Store_No, Item_Key, MIN(ItemHistoryID) As Min_ItemHistoryID
             FROM ItemHistory H
             GROUP BY Store_No, Item_Key) T
            ON T.Store_No = ItemHistory.Store_No AND T.Item_Key = ItemHistory.Item_Key AND T.Min_ItemHistoryID = ItemHistory.ItemHistoryID
        WHERE ItemHistory.DateStamp >= @DateReceived
            AND ItemHistory.Quantity = 0 AND ItemHistory.Weight = 0


        SELECT @Error_No = @@ERROR
    END

    IF @Error_No = 0
    BEGIN
        INSERT INTO @ItemHistory (Store_No, Item_Key, DateStamp, Quantity, Weight, Cost, ExtCost, Retail, Adjustment_ID, AdjustmentReason, CreatedBy, SubTeam_No, Insert_Date, ItemHistoryID, OrderItem_ID)
        SELECT ItemHistory.Store_No, ItemHistory.Item_Key, @DateReceived, ItemHistory.Quantity, ItemHistory.Weight, ItemHistory.Cost, ItemHistory.ExtCost, ItemHistory.Retail, ItemHistory.Adjustment_ID, ItemHistory.AdjustmentReason, ItemHistory.CreatedBy, ItemHistory.SubTeam_No, ItemHistory.Insert_Date, ItemHistory.ItemHistoryID, ItemHistory.OrderItem_ID
        FROM ItemHistory 
        INNER JOIN 
            OrderItem 
            ON ItemHistory.OrderItem_ID = OrderItem.OrderItem_ID
        INNER JOIN 
            @Orders O 
            ON O.OrderHeader_ID = OrderItem.OrderHeader_ID 

        SELECT @Error_No = @@ERROR
    END
    

    IF @Error_No = 0
    BEGIN
        DELETE ItemHistory
        FROM ItemHistory
        INNER JOIN @ItemHistory IH ON IH.ItemHistoryID = ItemHistory.ItemHistoryID

        SELECT @Error_No = @@ERROR
    END

    IF @Error_No = 0
    BEGIN
        INSERT INTO ItemHistory (Store_No, Item_Key, DateStamp, Quantity, Weight, Cost, ExtCost, Retail, Adjustment_ID, AdjustmentReason, CreatedBy, SubTeam_No, Insert_Date, OrderItem_ID)
        SELECT Store_No, Item_Key, DateStamp, Quantity, Weight, Cost, ExtCost, Retail, Adjustment_ID, AdjustmentReason, CreatedBy, SubTeam_No, Insert_Date, OrderItem_ID
        FROM @ItemHistory IH
        WHERE Adjustment_ID = 2

        SELECT @Error_No = @@ERROR
    END

    IF @Error_No = 0
    BEGIN
        INSERT INTO ItemHistory (Store_No, Item_Key, DateStamp, Quantity, Weight, Cost, ExtCost, Retail, Adjustment_ID, AdjustmentReason, CreatedBy, SubTeam_No, Insert_Date, OrderItem_ID)
        SELECT Store_No, Item_Key, DateStamp, Quantity, Weight, Cost, ExtCost, Retail, Adjustment_ID, AdjustmentReason, CreatedBy, SubTeam_No, Insert_Date, OrderItem_ID
        FROM @ItemHistory IH
        WHERE Adjustment_ID <> 2

        SELECT @Error_No = @@ERROR
    END

    IF @Error_No = 0
    BEGIN
        COMMIT TRAN
        SET NOCOUNT OFF
    END
    ELSE
    BEGIN
        ROLLBACK TRAN
        DECLARE @Severity smallint
        SELECT @Severity = ISNULL((SELECT severity FROM master.dbo.sysmessages WHERE error = @Error_No), 16)
        RAISERROR ('UpdateOrderBackdate failed with @@ERROR: %d', @Severity, 1, @Error_No)
    END
    
END
GO


