CREATE TABLE [dbo].[TaxFlag] (
    [TaxClassID]        INT      NOT NULL,
    [TaxJurisdictionID] INT      NOT NULL,
    [TaxFlagKey]        CHAR (1) NOT NULL,
    [TaxFlagValue]      BIT      CONSTRAINT [DF_TaxFlag_TaxFlagValue] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_TaxClassCode] PRIMARY KEY CLUSTERED ([TaxClassID] ASC, [TaxFlagKey] ASC, [TaxJurisdictionID] ASC),
    CONSTRAINT [FK_TaxClassCode_TaxClass] FOREIGN KEY ([TaxClassID]) REFERENCES [dbo].[TaxClass] ([TaxClassID]) ON DELETE CASCADE,
    CONSTRAINT [FK_TaxFlag_TaxDefinition] FOREIGN KEY ([TaxJurisdictionID]) REFERENCES [dbo].[TaxJurisdiction] ([TaxJurisdictionID]) ON DELETE CASCADE,
    CONSTRAINT [FK_TaxFlag_TaxJurisdiction] FOREIGN KEY ([TaxJurisdictionID]) REFERENCES [dbo].[TaxJurisdiction] ([TaxJurisdictionID])
);





GO
ALTER TABLE [dbo].[TaxFlag] ENABLE CHANGE_TRACKING WITH (TRACK_COLUMNS_UPDATED = OFF);


GO
CREATE TRIGGER TaxFlagInsert
ON TaxFlag
FOR INSERT
AS 
BEGIN

    DECLARE @error_no int
    SELECT @error_no = 0

	-- SEND DOWN PRICE BATCH DETAIL RECORDS TO ALLOW ITEM CHANGES TO BE BATCHED

        INSERT INTO PriceBatchDetail (Store_No, Item_Key, ItemChgTypeID, InsertApplication)
		SELECT DISTINCT Store.Store_No, Item.Item_Key, 2, 'TaxFlagInsert Trigger'
		FROM	Inserted, 
				Item, 
				Store
		WHERE	1 in (Store.WFM_Store, Store.Mega_Store)
		  AND	Inserted.TaxClassID = Item.TaxClassID
		  AND	Inserted.TaxJurisdictionID = Store.TaxJurisdictionID
		  AND dbo.fn_HasPendingItemChangePriceBatchDetailRecord(Item.Item_Key,Store.Store_No) = 0

        SELECT @error_no = @@ERROR

    IF @error_no <> 0
    BEGIN
        ROLLBACK TRAN
        DECLARE @Severity smallint
        SELECT @Severity = ISNULL((SELECT severity FROM master.dbo.sysmessages WHERE error = @error_no), 16)
        RAISERROR ('TaxFlagUpdate Trigger failed with @@ERROR: %d', @Severity, 1, @error_no)
    END

END
GO
CREATE Trigger [dbo].[TaxFlagUpdate] 
ON [dbo].[TaxFlag]
FOR UPDATE
AS
BEGIN
    DECLARE @error_no int
    SELECT @error_no = 0

	-- SEND DOWN PRICE BATCH DETAIL RECORDS TO ALLOW ITEM CHANGES TO BE BATCHED

        INSERT INTO PriceBatchDetail (Store_No, Item_Key, ItemChgTypeID, InsertApplication)
		SELECT DISTINCT Store.Store_No, Item.Item_Key, 2, 'TaxFlagUpdate Trigger'
		FROM	Inserted, 
				Deleted, 
				Item, 
				Store
		WHERE	Deleted.TaxClassID = Inserted.TaxClassID
		  AND	Deleted.TaxJurisdictionID = Inserted.TaxJurisdictionID
		  AND	Deleted.TaxFlagKey = Inserted.TaxFlagKey
		  AND	1 in (Store.WFM_Store, Store.Mega_Store)
		  AND	Inserted.TaxClassID = Item.TaxClassID
		  AND	Inserted.TaxJurisdictionID = Store.TaxJurisdictionID
		  AND dbo.fn_HasPendingItemChangePriceBatchDetailRecord(Item.Item_Key,Store.Store_No) = 0
/*
I tried using the "JOIN" syntax, but I'm not familiar enough with it, so I dropped it.  

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
        RAISERROR ('TaxFlagUpdate Trigger failed with @@ERROR: %d', @Severity, 1, @error_no)
    END

END
set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
GO
CREATE TRIGGER TaxFlagDelete
ON TaxFlag
FOR DELETE
AS 
BEGIN

    DECLARE @error_no int
    SELECT @error_no = 0

	-- SEND DOWN PRICE BATCH DETAIL RECORDS TO ALLOW ITEM CHANGES TO BE BATCHED

        INSERT INTO PriceBatchDetail (Store_No, Item_Key, ItemChgTypeID, InsertApplication)
		SELECT DISTINCT Store.Store_No, Item.Item_Key, 2, 'TaxFlagDelete Trigger'
		FROM	Deleted, 
				Item, 
				Store
		WHERE	1 in (Store.WFM_Store, Store.Mega_Store)
		  AND	Deleted.TaxClassID = Item.TaxClassID
		  AND	Deleted.TaxJurisdictionID = Store.TaxJurisdictionID
		  AND dbo.fn_HasPendingItemChangePriceBatchDetailRecord(Item.Item_Key,Store.Store_No) = 0

        SELECT @error_no = @@ERROR

    IF @error_no <> 0
    BEGIN
        ROLLBACK TRAN
        DECLARE @Severity smallint
        SELECT @Severity = ISNULL((SELECT severity FROM master.dbo.sysmessages WHERE error = @error_no), 16)
        RAISERROR ('TaxFlagUpdate Trigger failed with @@ERROR: %d', @Severity, 1, @error_no)
    END

END
GO
GRANT DELETE
    ON OBJECT::[dbo].[TaxFlag] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[TaxFlag] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[TaxFlag] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[TaxFlag] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[TaxFlag] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[TaxFlag] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[TaxFlag] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[TaxFlag] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[TaxFlag] TO [IRMAReportsRole]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[TaxFlag] TO [IRMAReportsRole]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[TaxFlag] TO [IRMAReports]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[TaxFlag] TO [IRMA_Teradata]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[TaxFlag] TO [IConInterface]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[TaxFlag] TO [iCONReportingRole]
    AS [dbo];

