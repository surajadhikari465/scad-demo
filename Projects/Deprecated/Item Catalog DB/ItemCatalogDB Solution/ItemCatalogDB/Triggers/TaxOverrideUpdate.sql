set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
GO

IF EXISTS (SELECT name 
	   FROM   sysobjects 
	   WHERE  name = N'TaxOverrideUpdate' 
	   AND 	  type = 'TR')
    DROP TRIGGER TaxOverrideUpdate
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