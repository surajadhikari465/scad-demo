CREATE TABLE [dbo].[StoreItemVendor] (
    [StoreItemVendorID]     INT           IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [Store_No]              INT           NOT NULL,
    [Item_Key]              INT           NOT NULL,
    [Vendor_ID]             INT           NOT NULL,
    [AverageDelivery]       SMALLINT      NULL,
    [PrimaryVendor]         BIT           CONSTRAINT [DF_StoreItemVendor_PrimaryVendor] DEFAULT ((0)) NOT NULL,
    [DeleteDate]            SMALLDATETIME NULL,
    [DeleteWorkStation]     VARCHAR (255) NULL,
    [LastCostAddedDate]     DATETIME      NULL,
    [LastCostRefreshedDate] DATETIME      CONSTRAINT [DF_StoreItemVendor_LastCostRefreshedDate] DEFAULT (getdate()) NOT NULL,
    [DiscontinueItem]       BIT           CONSTRAINT [DF_StoreItemVendor_DiscontinueItem] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK__StoreItemVender] PRIMARY KEY CLUSTERED ([StoreItemVendorID] ASC) WITH (FILLFACTOR = 80),
    CONSTRAINT [FK_Cost_Store] FOREIGN KEY ([Store_No]) REFERENCES [dbo].[Store] ([Store_No]),
    CONSTRAINT [FK_StoreItemVendor_ItemVendor] FOREIGN KEY ([Item_Key], [Vendor_ID]) REFERENCES [dbo].[ItemVendor] ([Item_Key], [Vendor_ID]),
    CONSTRAINT [FK_StoreItemVendor_Price] FOREIGN KEY ([Item_Key], [Store_No]) REFERENCES [dbo].[Price] ([Item_Key], [Store_No])
);


GO
ALTER TABLE [dbo].[StoreItemVendor] ENABLE CHANGE_TRACKING WITH (TRACK_COLUMNS_UPDATED = OFF);


GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_StoreItemVendor_ItemKey_VendorId_StoreNo_INC_DiscoItem]
    ON [dbo].[StoreItemVendor]([Item_Key] ASC, [Vendor_ID] ASC, [Store_No] ASC)
    INCLUDE([DiscontinueItem]) WITH (FILLFACTOR = 80);


GO
CREATE NONCLUSTERED INDEX [IX_StoreItemVendorVendor_ID]
    ON [dbo].[StoreItemVendor]([Vendor_ID] ASC, [Item_Key] ASC, [Store_No] ASC, [StoreItemVendorID] ASC)
    INCLUDE([DeleteDate]) WITH (FILLFACTOR = 80);


GO
CREATE NONCLUSTERED INDEX [IX_StoreItemVendor_StoreNo_ItemKey_VendorId_INC_PrimVend_DiscoItem]
    ON [dbo].[StoreItemVendor]([Store_No] ASC, [Item_Key] ASC, [Vendor_ID] ASC)
    INCLUDE([PrimaryVendor], [DiscontinueItem]) WITH (FILLFACTOR = 80);


GO
CREATE NONCLUSTERED INDEX [IX_StoreItemVendorVendorID]
    ON [dbo].[StoreItemVendor]([Vendor_ID] ASC) WITH (FILLFACTOR = 80);


GO
CREATE NONCLUSTERED INDEX [IX_StoreItemVendorStore]
    ON [dbo].[StoreItemVendor]([Store_No] ASC) WITH (FILLFACTOR = 80);


GO
CREATE NONCLUSTERED INDEX [IX_StoreItemVendorItem]
    ON [dbo].[StoreItemVendor]([Item_Key] ASC) WITH (FILLFACTOR = 80);


GO
CREATE TRIGGER StoreItemVendorUpdate
    ON [dbo].[StoreItemVendor]
FOR UPDATE 
AS 
BEGIN
    DECLARE @Error_No int
    SELECT @Error_No = 0

	-- Update the delete date for the inserted record, if necessary.
	IF @Error_No = 0
	BEGIN
		UPDATE StoreItemVendor
		SET DeleteWorkStation = CASE WHEN Inserted.DeleteDate IS NOT NULL THEN HOST_NAME() ELSE NULL END
		FROM StoreItemVendor SIV
		INNER JOIN 
		  Inserted 
		  ON Inserted.StoreItemVendorID = SIV.StoreItemVendorID
		INNER JOIN 
		  Deleted 
		  ON Deleted.StoreItemVendorID = Inserted.StoreItemVendorID
		WHERE ISNULL(Inserted.DeleteDate, 0) <> ISNULL(Deleted.DeleteDate, 0)

	    SELECT @Error_No = @@ERROR
	END
	
	-- Unset Primary status if Vendor is deleted.  
	IF @Error_No = 0
	BEGIN
		UPDATE StoreItemVendor
		SET PrimaryVendor = 0
		FROM StoreItemVendor SIV
		INNER JOIN 
		  Inserted 
		  ON Inserted.StoreItemVendorID = SIV.StoreItemVendorID
		WHERE Inserted.DeleteDate IS NOT NULL

	    SELECT @Error_No = @@ERROR
	END
	   
    IF @Error_No = 0 
    BEGIN
        INSERT INTO PriceBatchDetail (Store_No, Item_Key, ItemChgTypeID, InsertApplication)
        SELECT Inserted.Store_No, Inserted.Item_Key, 2, 'StoreItemVendorUpdate Trigger'
        FROM Inserted
        INNER JOIN
            Deleted
            ON Deleted.Item_Key = Inserted.Item_Key	AND
               Deleted.Store_No = Inserted.Store_No AND
               Deleted.Vendor_ID = Inserted.Vendor_ID		
        WHERE Inserted.PrimaryVendor = 1	-- create records when the primary vendor changes
			AND (Inserted.PrimaryVendor <> Deleted.PrimaryVendor)
			AND (Inserted.DeleteDate IS NULL OR Inserted.DeleteDate > GetDate())
            AND dbo.fn_HasPendingItemChangePriceBatchDetailRecord(Inserted.Item_Key,Inserted.Store_No) = 0
			AND dbo.fn_InstanceDataValue ('BatchVendorChanges', Inserted.Store_No) = 1
            
        SELECT @Error_No = @@ERROR
    END
    
    	DECLARE @NewPrimaryVendor bit

	SELECT 
		@NewPrimaryVendor = CASE COUNT(*) WHEN 0 THEN 0 ELSE 1 END
	FROM Inserted
	INNER JOIN Deleted
		 ON Deleted.Item_Key = Inserted.Item_Key	AND
			   Deleted.Store_No = Inserted.Store_No AND
			   Deleted.Vendor_ID = Inserted.Vendor_ID
	WHERE
		Inserted.PrimaryVendor = 1	-- create records when the primary vendor changes
		AND (Inserted.PrimaryVendor <> Deleted.PrimaryVendor)
		AND (Inserted.DeleteDate IS NULL OR Inserted.DeleteDate > GetDate())
		
    
    IF @Error_No = 0 
    BEGIN
		IF @NewPrimaryVendor = 1
		BEGIN
			-- If there's a new primary vendor, disable the old one
			UPDATE StoreItemVendor
				SET PrimaryVendor = 0
				FROM 
					StoreItemVendor SIV
				INNER JOIN Inserted I ON 
					SIV.Store_No = I.Store_No
					AND
					SIV.Item_Key = I.Item_Key
					AND
					SIV.Vendor_ID <> I.Vendor_ID
					
			SELECT @Error_No = @@ERROR
		END
		ELSE
		BEGIN
			-- IF THERE IS NO LONGER A PRIMARY VENDOR FOR A STORE-ITEM DUE TO THIS CHANGE, THE ITEM SHOULD
			-- BE DE-AUTHORIZED FOR THE STORE
			UPDATE StoreItem
				SET	Authorized = 0
			FROM StoreItemVendor SIV
			INNER JOIN Inserted 
			  ON Inserted.StoreItemVendorID = SIV.StoreItemVendorID
			WHERE StoreItem.Item_Key = Inserted.Item_Key AND 
				  StoreItem.Store_No = Inserted.Store_No AND
				  dbo.fn_HasPrimaryVendor(Inserted.Item_Key, Inserted.Store_No) = 0

			SELECT @Error_No = @@ERROR
		END
	END

	IF @Error_No = 0
	BEGIN
		DECLARE @mammothUpdates dbo.ItemKeyAndStoreNoType

		INSERT INTO @mammothUpdates(
			Item_Key, 
			Store_No)
		SELECT 
			i.Item_Key, 
			i.Store_No
		FROM inserted i
		JOIN deleted d on i.Vendor_ID = d.Vendor_ID
		WHERE i.PrimaryVendor <> d.PrimaryVendor

		IF EXISTS (SELECT TOP 1 1 FROM @mammothUpdates)
			EXEC mammoth.GenerateEventsByItemKeyAndStoreNoType @mammothUpdates, 'ItemLocaleAddOrUpdate'

		SELECT @Error_No = @@ERROR
	END
		
    IF @Error_No <> 0
    BEGIN
        ROLLBACK TRAN
        DECLARE @Severity smallint
        SELECT @Severity = ISNULL((SELECT severity FROM master.dbo.sysmessages WHERE error = @Error_No), 16)
        RAISERROR ('StoreItemVendorUpdate trigger failed with @@ERROR: %d', @Severity, 1, @Error_No)
    END
END
GO
GRANT SELECT
    ON OBJECT::[dbo].[StoreItemVendor] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[StoreItemVendor] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[StoreItemVendor] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[StoreItemVendor] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[StoreItemVendor] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[StoreItemVendor] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[StoreItemVendor] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[StoreItemVendor] TO [IRMAReportsRole]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[StoreItemVendor] TO [IRMAReportsRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[StoreItemVendor] TO [IRMAExcelRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[StoreItemVendor] TO [IRMAExcelRole]
    AS [dbo];


GO
GRANT REFERENCES
    ON OBJECT::[dbo].[StoreItemVendor] TO [IRMAExcelRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[StoreItemVendor] TO [IRMAExcelRole]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[StoreItemVendor] TO [IRMAExcelRole]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[StoreItemVendor] TO [IRMAReports]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[StoreItemVendor] TO [IRMAAVCIRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[StoreItemVendor] TO [IMHARole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[StoreItemVendor] TO [ExtractRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[StoreItemVendor] TO [IRMASLIMRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[StoreItemVendor] TO [IRMASLIMRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[StoreItemVendor] TO [IRMASLIMRole]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[StoreItemVendor] TO [IRMASLIMRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[StoreItemVendor] TO [IRMAPromoRole]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[StoreItemVendor] TO [IRMA_Teradata]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[StoreItemVendor] TO [BizTalk]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[StoreItemVendor] TO [IConInterface]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[StoreItemVendor] TO [spice_user]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[StoreItemVendor] TO [iCONReportingRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[StoreItemVendor] TO [IRMAPDXExtractRole]
    AS [dbo];

GO
GRANT SELECT
    ON OBJECT::[dbo].[StoreItemVendor] TO [MammothRole]
    AS [dbo];