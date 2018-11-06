set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
GO

IF EXISTS (SELECT name 
	   FROM   sysobjects 
	   WHERE  name = N'TaxOverrideInsert' 
	   AND 	  type = 'TR')
    DROP TRIGGER TaxOverrideInsert
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