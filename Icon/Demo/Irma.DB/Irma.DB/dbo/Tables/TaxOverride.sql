CREATE TABLE [dbo].[TaxOverride] (
    [Store_No]     INT      NOT NULL,
    [Item_Key]     INT      NOT NULL,
    [TaxFlagKey]   CHAR (1) NOT NULL,
    [TaxFlagValue] BIT      CONSTRAINT [DF_TaxOverride_TaxFlagValue] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_TaxOverride] PRIMARY KEY CLUSTERED ([Item_Key] ASC, [Store_No] ASC, [TaxFlagKey] ASC),
    CONSTRAINT [FK_TaxOverride_Item] FOREIGN KEY ([Item_Key]) REFERENCES [dbo].[Item] ([Item_Key]),
    CONSTRAINT [FK_TaxOverride_Store] FOREIGN KEY ([Store_No]) REFERENCES [dbo].[Store] ([Store_No])
);


GO
ALTER TABLE [dbo].[TaxOverride] ENABLE CHANGE_TRACKING WITH (TRACK_COLUMNS_UPDATED = OFF);


GO
CREATE Trigger [dbo].[TaxOverrideInsert] 
ON [dbo].[TaxOverride]
FOR INSERT
AS
BEGIN
    DECLARE @error_no int
    SELECT @error_no = 0

	-- SEND DOWN PRICE BATCH DETAIL RECORDS TO ALLOW ITEM CHANGES TO BE BATCHED

        INSERT INTO PriceBatchDetail (Store_No, Item_Key, ItemChgTypeID, InsertApplication)
		SELECT DISTINCT Inserted.Store_No, Inserted.Item_Key, 2, 'TaxOverrideInsert Trigger'
		FROM	Inserted, 
				Store
		WHERE	1 in (Store.WFM_Store, Store.Mega_Store)
		  AND	Inserted.Store_No = Store.Store_No
		  AND dbo.fn_HasPendingItemChangePriceBatchDetailRecord(Inserted.Item_Key,Inserted.Store_No) = 0


        SELECT @error_no = @@ERROR

    IF @error_no <> 0
    BEGIN
        ROLLBACK TRAN
        DECLARE @Severity smallint
        SELECT @Severity = ISNULL((SELECT severity FROM master.dbo.sysmessages WHERE error = @error_no), 16)
        RAISERROR ('TaxOverrideUpdate Trigger failed with @@ERROR: %d', @Severity, 1, @error_no)
    END

END
GO
CREATE Trigger [dbo].[TaxOverrideUpdate] 
ON [dbo].[TaxOverride]
FOR UPDATE
AS
BEGIN
    DECLARE @error_no int
    SELECT @error_no = 0

-- will also need delete and insert triggers.  

	-- SEND DOWN PRICE BATCH DETAIL RECORDS TO ALLOW ITEM CHANGES TO BE BATCHED

        INSERT INTO PriceBatchDetail (Store_No, Item_Key, ItemChgTypeID, InsertApplication)
		SELECT DISTINCT Inserted.Store_No, Inserted.Item_Key, 2, 'TaxOverrideUpdate Trigger'
		FROM	Inserted, 
				Deleted, 
				Store
		WHERE	Deleted.Store_No = Inserted.Store_No
		  AND	Deleted.Item_Key = Inserted.Item_Key
		  AND	Deleted.TaxFlagKey = Inserted.TaxFlagKey
		  AND	1 in (Store.WFM_Store, Store.Mega_Store)
		  AND	Inserted.Store_No = Store.Store_No
		  AND dbo.fn_HasPendingItemChangePriceBatchDetailRecord(Inserted.Item_Key,Inserted.Store_No) = 0
/*
I tried using the "JOIN" syntax, but I'm not familiar enough with it, so I dropped it.  
-Russell

		FROM Item
        INNER JOIN
			Store
			ON Store.TaxJurisdictionID = Inserted.TaxJurisdictionID
    
		INNER JOIN 
			Inserted
			ON Inserted.TaxClassID = Item.TaxClassID
		INNER JOIN
			Deleted
			ON	Deleted.TaxClassID = Inserted.TaxClassID
		  AND	Deleted.TaxJurisdictionID = Inserted.TaxJurisdictionID
		  AND	Deleted.TaxFlagKey = Inserted.TaxFlagKey

		WHERE	1 in (Store.WFM_Store, Store.Mega_Store)
		  AND NOT EXISTS (SELECT *						-- Do not create another PBD record for the item if there is already
                            FROM PriceBatchDetail PBD		-- an Item Change PBD record not assigned to a batch or assigned to a 
                            LEFT JOIN						-- batch in the "Building" status 
                                PriceBatchHeader PBH		-- UNLESS the PBD record is for a future dated Off Promo Cost record
                                ON PBD.PriceBatchHeaderID = PBH.PriceBatchHeaderID
                            WHERE Item_Key = Item.Item_Key AND PBD.Store_No = Store.Store_No
                                AND ISNULL(PriceBatchStatusID, 0) < 2
                                AND PBD.ItemChgTypeID IS NOT NULL
                                AND NOT(PBD.ItemChgTypeID = 6 AND PBD.StartDate > GetDate()))

*/

        SELECT @error_no = @@ERROR

    IF @error_no <> 0
    BEGIN
        ROLLBACK TRAN
        DECLARE @Severity smallint
        SELECT @Severity = ISNULL((SELECT severity FROM master.dbo.sysmessages WHERE error = @error_no), 16)
        RAISERROR ('TaxOverrideUpdate Trigger failed with @@ERROR: %d', @Severity, 1, @error_no)
    END

END
GO
CREATE Trigger [dbo].[TaxOverrideDelete] 
ON [dbo].[TaxOverride]
FOR Delete
AS
BEGIN
    DECLARE @error_no int
    SELECT @error_no = 0

	-- SEND DOWN PRICE BATCH DETAIL RECORDS TO ALLOW ITEM CHANGES TO BE BATCHED

        INSERT INTO PriceBatchDetail (Store_No, Item_Key, ItemChgTypeID, InsertApplication)
		SELECT DISTINCT Deleted.Store_No, Deleted.Item_Key, 2, 'TaxOverrideDelete Trigger'
		FROM	Deleted, 
				Store
		WHERE	1 in (Store.WFM_Store, Store.Mega_Store)
		  AND	Deleted.Store_No = Store.Store_No
		  AND dbo.fn_HasPendingItemChangePriceBatchDetailRecord(Deleted.Item_Key,Deleted.Store_No) = 0

        SELECT @error_no = @@ERROR

    IF @error_no <> 0
    BEGIN
        ROLLBACK TRAN
        DECLARE @Severity smallint
        SELECT @Severity = ISNULL((SELECT severity FROM master.dbo.sysmessages WHERE error = @error_no), 16)
        RAISERROR ('TaxOverrideInsert Trigger failed with @@ERROR: %d', @Severity, 1, @error_no)
    END

END
GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[TaxOverride] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[TaxOverride] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[TaxOverride] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[TaxOverride] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[TaxOverride] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[TaxOverride] TO [IRMAReportsRole]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[TaxOverride] TO [IRMAReportsRole]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[TaxOverride] TO [IRMAReports]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[TaxOverride] TO [IRMA_Teradata]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[TaxOverride] TO [IConInterface]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[TaxOverride] TO [iCONReportingRole]
    AS [dbo];

