IF EXISTS (SELECT name 
	   FROM   sysobjects 
	   WHERE  name = N'TaxFlagInsert' 
	   AND 	  type = 'TR')
    DROP TRIGGER TaxFlagInsert
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
