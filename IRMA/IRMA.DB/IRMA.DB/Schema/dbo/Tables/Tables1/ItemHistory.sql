CREATE TABLE [dbo].[ItemHistory] (
    [Store_No]                   INT             NOT NULL,
    [Item_Key]                   INT             NOT NULL,
    [DateStamp]                  DATETIME        NOT NULL,
    [Quantity]                   DECIMAL (18, 4) CONSTRAINT [DF__ItemHisto__Quant__2AC11801] DEFAULT ((0)) NULL,
    [Weight]                     DECIMAL (18, 4) CONSTRAINT [DF__ItemHisto__Weigh__2BB53C3A] DEFAULT ((0)) NULL,
    [Cost]                       SMALLMONEY      CONSTRAINT [DF__ItemHistor__Cost__2CA96073] DEFAULT ((0)) NULL,
    [ExtCost]                    SMALLMONEY      CONSTRAINT [DF__ItemHisto__ExtCo__2D9D84AC] DEFAULT ((0)) NULL,
    [Retail]                     SMALLMONEY      CONSTRAINT [DF__ItemHisto__Retai__2E91A8E5] DEFAULT ((0)) NULL,
    [Adjustment_ID]              INT             NOT NULL,
    [AdjustmentReason]           VARCHAR (100)   NULL,
    [CreatedBy]                  INT             NOT NULL,
    [SubTeam_No]                 INT             NOT NULL,
    [Insert_Date]                DATETIME        CONSTRAINT [DF__itemhisto__Inser__73D73082] DEFAULT (getdate()) NULL,
    [ItemHistoryID]              INT             IDENTITY (1, 1) NOT NULL,
    [OrderItem_ID]               INT             NULL,
    [InventoryAdjustmentCode_ID] INT             NULL,
    [CorrectionRecordFlag]       BIT             NULL,
    CONSTRAINT [pk_ItemHistory_ItemHistoryId] PRIMARY KEY CLUSTERED ([ItemHistoryID] ASC, [DateStamp] ASC) WITH (FILLFACTOR = 90),
    CONSTRAINT [FK_InventoryAdjustmentCode_Id] FOREIGN KEY ([InventoryAdjustmentCode_ID]) REFERENCES [dbo].[InventoryAdjustmentCode] ([InventoryAdjustmentCode_ID]),
    CONSTRAINT [FK_ItemHistory_OrderItem] FOREIGN KEY ([OrderItem_ID]) REFERENCES [dbo].[OrderItem] ([OrderItem_ID]),
    CONSTRAINT [FK_ItemHistory_SubTeam] FOREIGN KEY ([SubTeam_No]) REFERENCES [dbo].[SubTeam] ([SubTeam_No])
);


GO
ALTER TABLE [dbo].[ItemHistory] ENABLE CHANGE_TRACKING WITH (TRACK_COLUMNS_UPDATED = OFF);


GO
CREATE UNIQUE NONCLUSTERED INDEX [idxItemHistoryItemHistoryID]
    ON [dbo].[ItemHistory]([ItemHistoryID] ASC) WITH (FILLFACTOR = 80);


GO
CREATE NONCLUSTERED INDEX [idxItemHistory]
    ON [dbo].[ItemHistory]([DateStamp] ASC, [Item_Key] ASC, [SubTeam_No] ASC, [Store_No] ASC, [Adjustment_ID] ASC) WITH (FILLFACTOR = 80);


GO
CREATE NONCLUSTERED INDEX [idxItemHistoryItemID]
    ON [dbo].[ItemHistory]([Item_Key] ASC, [SubTeam_No] ASC, [Store_No] ASC, [DateStamp] ASC, [Adjustment_ID] ASC) WITH (FILLFACTOR = 80);


GO
CREATE NONCLUSTERED INDEX [idxItemHistoryAdjID]
    ON [dbo].[ItemHistory]([Adjustment_ID] ASC, [Item_Key] ASC, [SubTeam_No] ASC, [Store_No] ASC, [DateStamp] ASC) WITH (FILLFACTOR = 80);


GO
CREATE NONCLUSTERED INDEX [idxItemHistory_Store_SubTeam_Store_Id]
    ON [dbo].[ItemHistory]([Store_No] ASC, [SubTeam_No] ASC, [Item_Key] ASC, [ItemHistoryID] ASC) WITH (FILLFACTOR = 80);


GO
CREATE NONCLUSTERED INDEX [idxItemHistoryOrderItemIDAdjID]
    ON [dbo].[ItemHistory]([OrderItem_ID] ASC, [Adjustment_ID] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_ItemHistory_OrderItemId]
    ON [dbo].[ItemHistory]([OrderItem_ID] ASC);


GO
CREATE NONCLUSTERED INDEX [idxItemHistory_InsertDate]
    ON [dbo].[ItemHistory]([Insert_Date] ASC)
    INCLUDE([Store_No], [Item_Key], [DateStamp], [SubTeam_No], [InventoryAdjustmentCode_ID]) WITH (FILLFACTOR = 90);


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
CREATE TRIGGER ItemHistoryUpd
ON [dbo].[ItemHistory]
FOR UPDATE
AS 
BEGIN
    RAISERROR('ItemHistory cannot be updated - insert and delete only', 16, 1)
    ROLLBACK TRAN
END
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
    
	IF @Error_No = 0 
	BEGIN
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
	
		SELECT @Error_No = @@ERROR
	END

	IF @Error_No = 0 AND (SELECT ISNULL(dbo.fn_InstanceDataValue('EnableAmazonEventGeneration', null), 0)) = 1
	BEGIN
		DECLARE @invAdjEvenTypeCode NVARCHAR(25) = 'INV_ADJ'
		DECLARE @invAdjMessageType NVARCHAR(50) = 'InventoryAdjustment'

		INSERT INTO amz.InventoryQueue(EventTypeCode, MessageType, KeyID, InsertDate, MessageTimestampUtc)
		SELECT 
			@invAdjEvenTypeCode,
			@invAdjMessageType,
			inserted.ItemHistoryID,
			SYSDATETIME(),
			SYSUTCDATETIME()
		FROM inserted
		WHERE inserted.Adjustment_ID = 1

		SELECT @Error_No = @@ERROR
	END

    IF @Error_No <> 0
    BEGIN
        ROLLBACK TRAN
        DECLARE @Severity smallint
        SELECT @Severity = ISNULL((SELECT severity FROM master.dbo.sysmessages WHERE error = @Error_No), 16)
        RAISERROR ('ItemHistoryAdd trigger failed with @@ERROR: %d', @Severity, 1, @Error_No)
    END
END
GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[ItemHistory] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[ItemHistory] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[ItemHistory] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[ItemHistory] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[ItemHistory] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[ItemHistory] TO [IRMAReportsRole]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[ItemHistory] TO [IRMAReportsRole]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[ItemHistory] TO [IRMA_Teradata]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[ItemHistory] TO [BizTalk]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[ItemHistory] TO [IConInterface]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[ItemHistory] TO [iCONReportingRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[ItemHistory] TO [IRMAPDXExtractRole]
    AS [dbo];

