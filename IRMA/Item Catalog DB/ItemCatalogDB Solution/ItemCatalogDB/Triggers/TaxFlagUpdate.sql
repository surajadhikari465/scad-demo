set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
GO

IF EXISTS (SELECT name 
	   FROM   sysobjects 
	   WHERE  name = N'TaxFlagUpdate' 
	   AND 	  type = 'TR')
    DROP TRIGGER TaxFlagUpdate
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